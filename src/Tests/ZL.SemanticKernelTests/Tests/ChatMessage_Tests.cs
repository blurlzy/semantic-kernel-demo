using Microsoft.Identity.Client;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI.Chat;
using System.Globalization;
using System.Threading;
using Xunit.Abstractions;
using ZL.SemanticKernelDemo.Host.Models;
using ZL.SemanticKernelDemo.Host.Persistence;

namespace ZL.SemanticKernelTests.Tests
{
    public class ChatMessage_Tests
    {
        // cosmos settings
        private readonly string _cosmosConnection = SecretManager.GetSecret(SecretKeys.CosmosConnection);
        private readonly string _cosmosDb = SecretManager.GetSecret(SecretKeys.CosmosDb);
        private readonly string _chatSessionContainer = SecretManager.GetSecret(SecretKeys.ChatSessionContainer);
        private readonly string _chatMessageContainer = SecretManager.GetSecret(SecretKeys.ChatHistoryContainer);


        private readonly CosmosDbContext<ChatSession> _chatSessionContext;
        private readonly CosmosDbChatMessageContext _chatMessageContext;

        private readonly ChatSessionRepository _chatSessionRepository;
        private readonly ChatMessageRepository _chatMessageRepository;

        // AOAI endpoint & key
        private readonly string _endpoint = SecretManager.OpenAIEndpoint;
        private readonly string _key = SecretManager.OpenAIKey;
        private readonly string _deployment = "gpt-4o";

        private readonly Kernel _kernel;

        private readonly ITestOutputHelper _output;

        // ctor
        public ChatMessage_Tests(ITestOutputHelper output)
        {
            IKernelBuilder builder = Kernel.CreateBuilder();
            // add AOAI
            builder.AddAzureOpenAIChatCompletion(_deployment, _endpoint, _key);
            // kernal
            _kernel = builder.Build();

            // cosmos db context
            _chatSessionContext = new CosmosDbContext<ChatSession>(_cosmosConnection, _cosmosDb, _chatSessionContainer);
            _chatMessageContext = new CosmosDbChatMessageContext(_cosmosConnection, _cosmosDb, _chatMessageContainer);

            // repos
            _chatSessionRepository = new ChatSessionRepository(_chatSessionContext);
            _chatMessageRepository = new ChatMessageRepository(_chatMessageContext);

            _output = output;
        }

        [Theory]
        [InlineData("7aea25e7-a54f-49df-b0ae-9b01de6e1c6e", "Can you write a C# function which can compare 2 dates?")]
        public async Task Save_Chat_Messages_Test(string chatSessionId, string userInput)
        {
            string userId = "fa1148a6-f974-463e-b453-aba9b1466392";
            string userName = "blurlzy@hotmail.com";

            // Create a chat history object
            ChatHistory chatHistory = [];

            // ask for the first option - pizza
            chatHistory.AddSystemMessage("You are a helpful assistant.");
            chatHistory.AddUserMessage(userInput);

            // Retrieving chat completion services
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

            // Get the chat message content
            ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory,
                kernel: _kernel
            );

            // save user content
            var newUserMessage = await _chatMessageRepository.SaveMessageAsync(chatSessionId, userId, userName, userInput, AuthorRoles.User, ChatMessageType.Message);
            // save assistant content
            var newAssistantMessage = await _chatMessageRepository.SaveMessageAsync(chatSessionId, userId, userName, result.ToString(), AuthorRoles.Bot, ChatMessageType.Message);

        }

        [Theory]
        [InlineData("7aea25e7-a54f-49df-b0ae-9b01de6e1c6e", "Can you write the same function in Python?")]
        public async Task Chat_History_Intent_Test(string chatSessionId, string userInput)
        {
            // get messages
            var chatMessages = await _chatMessageRepository.GetHistoryMessagesAsync(chatSessionId,0, 10);

            chatMessages = chatMessages.Reverse();
            // Create a chat history object
            ChatHistory chatHistory = [];

            // ask for the first option - pizza
            chatHistory.AddSystemMessage("You are a helpful assistant.");

            // add
            foreach (var chatMessage in chatMessages)
            {
                if (chatMessage.AuthorRole == AuthorRoles.Bot)
                {
                    chatHistory.AddAssistantMessage(chatMessage.Content);
                }
                else
                {
                    chatHistory.AddUserMessage(chatMessage.Content);
                }
            }

            chatHistory.AddUserMessage(userInput);
            // Retrieving chat completion services
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            // Get the chat message content
            ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory,
                kernel: _kernel
            );

            _output.WriteLine( result.ToString() );
        }

        [Theory]
        [InlineData("7aea25e7-a54f-49df-b0ae-9b01de6e1c6e", "Can you write the same function in Python?")]
        public async Task Count_Token_Test(string chatSessionId, string userInput)
        {
            // get history messages (load last 10 messages)
            var chatMessages = await _chatMessageRepository.GetHistoryMessagesAsync(chatSessionId, 0, 10);

            ChatHistory chatHistory = new();
            ChatHistory allottedChatHistory = new();
            var remainingToken = PromptsOptions.CompletionTokenLimit;
            string historyText = string.Empty;


            foreach (var chatMessage in chatMessages)
            {
                // formatting
                var formattedMessage = chatMessage.ToFormattedString();

                //var promptRole = chatMessage.AuthorRole == AuthorRoles.Bot ? MessageAuthorRole.System : MessageAuthorRole.User;
                int tokenCount = TokenUtils.TokenCount(formattedMessage);

                if (remainingToken - tokenCount >= 0)
                {
                    historyText = $"{formattedMessage}\n{historyText}";
                    if (chatMessage.AuthorRole == AuthorRoles.Bot)
                    {
                        // Message doesn't have to be formatted for bot. This helps with asserting a natural language response from the LLM (no date or author preamble).
                        allottedChatHistory.AddAssistantMessage(chatMessage.Content.Trim());
                    }
                    else
                    {
                        //// Omit user name if Auth is disabled.
                        //var userMessage = PassThroughAuthenticationHandler.IsDefaultUser(chatMessage.UserId)
                        //        ? $"[{chatMessage.Timestamp.ToString("G", CultureInfo.CurrentCulture)}] {chatMessage.Content}"
                        //        : formattedMessage;
                        allottedChatHistory.AddUserMessage(formattedMessage);
                    }

                    remainingToken -= tokenCount;
                }
                else
                {
                    break;
                }
            }

            chatHistory.AddRange(allottedChatHistory.Reverse());
            chatHistory.AddUserMessage(userInput);

            // Retrieving chat completion services
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            // Get the chat message content
            ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory,
                kernel: _kernel
            );

            _output.WriteLine(result.ToString());
        }


        //[Theory]
        //[InlineData("")]
        //public async Task Get_UserIntent_Test(string chatSessionId)
        //{
        //    //KernelArguments chatContext = new(context);

        //    var completionFunction = this._kernel.CreateFunctionFromPrompt(
        //             PromptsOptions.SystemIntentExtraction,
        //             this.CreateIntentCompletionSettings(),
        //             functionName: "UserIntentExtraction",
        //             description: "Extract user intent");


        //    var result = await completionFunction.InvokeAsync(this._kernel);

        //    _output.WriteLine(result.ToString());
        //}


        /// <summary>
        /// Create `OpenAIPromptExecutionSettings` for intent response. Parameters are read from the PromptSettings class.
        /// </summary>
        private OpenAIPromptExecutionSettings CreateIntentCompletionSettings()
        {
            return new OpenAIPromptExecutionSettings
            {
                MaxTokens = PromptsOptions.ResponseTokenLimit,
                Temperature = PromptsOptions.IntentTemperature,
                TopP = PromptsOptions.IntentTopP,
                FrequencyPenalty = PromptsOptions.IntentFrequencyPenalty,
                PresencePenalty = PromptsOptions.IntentPresencePenalty,
                StopSequences = new string[] { "] bot:" }
            };
        }
    }
}
