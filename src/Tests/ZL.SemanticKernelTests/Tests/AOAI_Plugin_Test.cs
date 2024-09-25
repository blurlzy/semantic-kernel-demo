using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.ComponentModel;
using Xunit.Abstractions;


namespace ZL.SemanticKernelTests.Tests
{
    public class AOAI_Plugin_Test
    {
        // AOAI endpoint & key
        private readonly string _endpoint = SecretManager.OpenAIEndpoint;
        private readonly string _key = SecretManager.OpenAIKey;
        private readonly string _deployment = "gpt-4o";

        //// azure search
        //private readonly string _searchEndpoint = "";
        //private readonly string _searchKey = "";
        //private readonly string _searchIndex = "";

        private readonly Kernel _kernel;

        private readonly ITestOutputHelper _output;

        public AOAI_Plugin_Test(ITestOutputHelper output)
        {
            IKernelBuilder builder = Kernel.CreateBuilder();
            // add AOAI
            builder.AddAzureOpenAIChatCompletion(_deployment, _endpoint, _key);
            // add plugin
            builder.Plugins.AddFromType<TimeInformationPlugin>();
            builder.Plugins.AddFromType<CustomerPlugin>();

            // kernal
            _kernel = builder.Build();

            _output = output;
        }

        [Theory]
        [InlineData("What time is it?")]
        [InlineData("What's  the email address for customer: zongyi?")]
        public async Task Plugin_Call_Test(string userInput)
        {
            // Get chat completion service
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

            // Enable auto function calling
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            ChatHistory chatHistory = new ChatHistory();
            chatHistory.AddUserMessage(userInput);
            var chatResult = await chatCompletionService.GetChatMessageContentAsync(chatHistory, openAIPromptExecutionSettings, _kernel);

            _output.WriteLine(chatResult.ToString());
        }
    }


    /// <summary>
    /// A plugin that returns the current time.
    /// </summary>
    public class TimeInformationPlugin
    {
        /// <summary>
        /// Retrieves the current time in UTC.
        /// </summary>
        /// <returns>The current time in UTC. </returns>
        [KernelFunction, Description("Retrieves the current time in UTC.")]
        public string GetCurrentUtcTime()
            => DateTime.UtcNow.ToString("R");
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomerPlugin
    {
        [KernelFunction("GetCustomerEmail"), Description("Retrieve customer email based on the given customer ID.")]
        public Customer GetCustomerEmail(string customerId)
        {
            if(customerId.ToLower() == "justin")
            {
                return new Customer { Email = "justin.li@testing.com" };
            }

            return new Customer { Email = "unknown@testing.com" };
        }

        public class Customer
        {
            public string Email { get; set; }
        }
    }
}
