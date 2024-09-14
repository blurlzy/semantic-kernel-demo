using Microsoft.SemanticKernel;
using Xunit.Abstractions;

namespace ZL.SemanticKernelTests
{
    public class AOAI_Tests
    {
        // endpoint & key
        private readonly string _endpoint = SecretManager.OpenAIEndpoint;
        private readonly string _key = SecretManager.OpenAIKey;
        private readonly string _deployment = "gpt-4o";

        private readonly Kernel _kernel;

        private readonly ITestOutputHelper _output;
        
        // ctor
        public AOAI_Tests(ITestOutputHelper output)
        {
            // for Azure OpenAI
            _kernel = Kernel.CreateBuilder()
                            .AddAzureOpenAIChatCompletion(_deployment, _endpoint, _key)
                            .Build();

            _output = output;
            //// for OpenAI
            //_kernel = Kernel.CreateBuilder().AddOpenAIChatCompletion(model, apiKey, orgId);
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

        //// Extract chat history within token limit as a formatted string and optionally update the ChatHistory object with the allotted messages
        //private async Task<string> GetAllowedChatHistoryAsync()
        //{

        //}
    }
}
