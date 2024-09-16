
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI.Chat;
using System.Globalization;
using System.Net;
using System.Text.Json;
using Xunit.Abstractions;
using ZL.SemanticKernelTests.Plugins;
using static ZL.SemanticKernelTests.CopilotChatMessage;
using static ZL.SemanticKernelTests.Plugins.CustomerPlugin;

namespace ZL.SemanticKernelTests
{
    public class AOAI_Tests
    {
        // AOAI endpoint & key
        private readonly string _endpoint = SecretManager.OpenAIEndpoint;
        private readonly string _key = SecretManager.OpenAIKey;
        private readonly string _deployment = "gpt-4o";

        // azure search
        private readonly string _searchEndpoint = "";
        private readonly string _searchKey = "";
        private readonly string _searchIndex = "";

        private readonly Kernel _kernel;

        private readonly ITestOutputHelper _output;
        
        // ctor
        public AOAI_Tests(ITestOutputHelper output)
        {
            IKernelBuilder builder = Kernel.CreateBuilder();
            // add AOAI
            builder.AddAzureOpenAIChatCompletion(_deployment, _endpoint, _key);
            // add plugin
            builder.Plugins.AddFromType<CustomerPlugin>();

            // kernal
            _kernel = builder.Build();

            //// for Azure OpenAI
            //_kernel = Kernel.CreateBuilder()
            //                .AddAzureOpenAIChatCompletion(_deployment, _endpoint, _key)
            //                .Build();

            // add plugin

            _output = output;
            //// for OpenAI
            //_kernel = Kernel.CreateBuilder().AddOpenAIChatCompletion(model, apiKey, orgId);
        }

        [Fact]
        public async Task Chat_Own_Data_Test()
        {
            // Create a chat history object
            ChatHistory chatHistory = [];

            // First question without previous context based on uploaded content.
            chatHistory.AddUserMessage("How did Emily and David meet?");

            var chatCompletion = _kernel.GetRequiredService<IChatCompletionService>();

            var dataSource =  new AzureSearchChatDataSource
            {
                Endpoint = new Uri(_searchEndpoint),
                Authentication = DataSourceAuthentication.FromApiKey(_searchKey),
                IndexName = "" // index name
            };

            #pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var promptExecutionSettings = new AzureOpenAIPromptExecutionSettings { AzureChatDataSource = dataSource };
            #pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            var chatMessage = await chatCompletion.GetChatMessageContentAsync(chatHistory, promptExecutionSettings);
            var response = chatMessage.Content!;
        }

        // chat history
        // The chat history object is used to maintain a record of messages in a chat session.
        // It is used to store messages from different authors, such as users, assistants, tools, or the system.
        // As the primary mechanism for sending and receiving messages, the chat history object is essential for maintaining context and continuity in a conversation.
        [Fact]
        public async Task Chat_History_Test()
        {
            // Create a chat history object
            ChatHistory chatHistory = [];

            // ask for the first option - pizza
            chatHistory.AddSystemMessage("You are a helpful assistant.");
            chatHistory.AddUserMessage("What's available to order?");
            chatHistory.AddAssistantMessage("We have pizza, pasta, and salad available to order. What would you like to order?");
            chatHistory.AddUserMessage("I'd like to have the first option, please.");

            // Get the current length of the chat history object
            int currentChatHistoryLength = chatHistory.Count;

            // Once the services have been added, we then build the kernel and retrieve the chat completion service for later use.
            // Retrieving chat completion services
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

            // Get the chat message content
            ChatMessageContent results = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory,
                kernel: _kernel
            );

            // Get the new messages added to the chat history object
            for (int i = 0; i < chatHistory.Count; i++)
            {
                _output.WriteLine($"Message type: {chatHistory[i].Role}: " + chatHistory[i].ToString());
            }

            // Print the final message
            _output.WriteLine(results.ToString());
        }

        // test allowed chat history based on token limits
        [Fact]
        public async Task Allowed_Chat_History_Test()
        {
            // Retrieving chat completion services
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            ChatHistory chatHistory = new ChatHistory();

            // system prompt
            chatHistory.AddSystemMessage("You are a helpful coding assistant.");

            // first user prompt
            chatHistory.AddUserMessage("Can you write a C# function which can compare 2 dates?");
            // Get the chat message content
            ChatMessageContent result1 = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory,
                kernel: _kernel
            );
            // add assistant message
            chatHistory.AddAssistantMessage(result1.ToString());
            _output.WriteLine(result1.ToString());
            _output.WriteLine("______________________________");

            // add second user prompt
            chatHistory.AddUserMessage("Can you convert it into python?");
            ChatMessageContent result2 = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory,
                kernel: _kernel
            );
            chatHistory.AddAssistantMessage(result2.ToString());
            _output.WriteLine(result2.ToString());
            _output.WriteLine("______________________________");
            
            // add third user prompy
            chatHistory.AddUserMessage("Can you convert it into Typescript?");
            ChatMessageContent result3 = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory,
                kernel: _kernel
            );
            chatHistory.AddAssistantMessage(result3.ToString());
            _output.WriteLine(result3.ToString());


            var remainingToken = PromptsOptions.CompletionTokenLimit;
            //string historyText = string.Empty;

            // count the tokens
            foreach (var message in chatHistory)
            {
                //var formattedMessage = message.ToFormattedString();
                var formattedMessage = message.ToString();

                // check the total tokens
                int tokenCount = chatHistory is not null ? TokenUtils.GetContextMessageTokenCount(message.Role, formattedMessage) : TokenUtils.TokenCount(formattedMessage);

                remainingToken -= tokenCount;
            }

            _output.WriteLine($"############### Remaining (Allowed) Token: {remainingToken}");
        }

        [Fact]
        public async Task Allowed_Chat_Histroy_Test2()
        {
            // Retrieving chat completion services
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            ChatHistory chatHistory = new ChatHistory();

            // system prompt
            chatHistory.AddSystemMessage("You are a helpful coding assistant.");

            // first user prompt
            chatHistory.AddUserMessage("Can you write a C# function which can compare 2 dates?");
            // chatHistory.AddUserMessage("Can you convert it into python?");
            // chatHistory.AddUserMessage("Can you convert it into Typescript?");

            ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(
                  chatHistory,
                  kernel: _kernel
              );
            _output.WriteLine(result.ToString());
        }

        [Theory]
        [InlineData("JL")]
        public async Task Customer_Plugin_Test(string customerId)
        {
            // init a plugin
            KernelPlugin myPlugin = _kernel.CreatePluginFromType<CustomerPlugin>();

            // Invoke function through kernel
            FunctionResult customer = await _kernel.InvokeAsync(myPlugin["GetCustomerInfo"], new() { ["customerId"] = customerId });

            // Initialize the chat history with the weather
            ChatHistory chatHistory = new ChatHistory();

            // _output.WriteLine($"The JL's email is:  + {customer.GetValue<Customer>().Email}");
            // Simulate a user message
            chatHistory.AddSystemMessage("You are a helpful assistan. Please note: Justin's email address is: " + customer.GetValue<Customer>().Email);
            chatHistory.AddUserMessage("Can you please tell me what's Justin's email address?");

            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            var result = await chatCompletionService.GetChatMessageContentAsync(chatHistory);

            _output.WriteLine(result.ToString());
        }

        //// Extract chat history within token limit as a formatted string and optionally update the ChatHistory object with the allotted messages
        //private async Task<string> GetAllowedChatHistoryAsync()
        //{

        //}


    }
}
