using Azure.Search.Documents.Indexes;
using Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.ComponentModel;
using Xunit.Abstractions;
using ZL.SemanticKernelDemo.Host.Services;
using ZL.SemanticKernelDemo.Host.Services.Plugins;


namespace ZL.SemanticKernelTests.Tests
{
    public class AOAI_Plugin_Test
    {
        // AOAI endpoint & key
        private readonly string _endpoint = SecretManager.OpenAIEndpoint;
        private readonly string _key = SecretManager.OpenAIKey;
        private readonly string _deployment = "gpt-4o";

        // azure search
        private readonly string _searchEndpoint = SecretManager.SearchEndpoint;
        private readonly string _searchKey = SecretManager.SearchKey;
        private readonly string _searchIndex = "azure-workshop-001";

        private readonly Kernel _kernel;

        private readonly ITestOutputHelper _output;

        public AOAI_Plugin_Test(ITestOutputHelper output)
        {
            IKernelBuilder builder = Kernel.CreateBuilder();
            // add AOAI
            builder.AddAzureOpenAIChatCompletion(_deployment, _endpoint, _key);
            
            // Embedding generation service to convert string query to vector
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            builder.AddAzureOpenAITextEmbeddingGeneration("text-embedding-ada-002", _endpoint, _key);
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            // SearchIndexClient from Azure .NET SDK to perform search operations.
            // Azure AI Search configuration
            Uri endpoint = new(_searchEndpoint);
            AzureKeyCredential keyCredential = new(_searchKey);

            builder.Services.AddSingleton<SearchIndexClient>((_) => new SearchIndexClient(endpoint, keyCredential));
            // Custom AzureAISearchService to configure request parameters and make a request.
            builder.Services.AddSingleton<AzureAISearchService>();

            // add plugin
            builder.Plugins.AddFromType<TimeInformationPlugin>();
            builder.Plugins.AddFromType<CustomerPlugin>();
            builder.Plugins.AddFromType<AzureAISearchPlugin>();


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

        [Theory]
        [InlineData("What is the AMM Offer Navigator?")]
        public async Task Azure_AI_Search_Test(string userInput)
        {

            string query2 = "{{search '" + userInput + "' collection='" + _searchIndex + "'}}";
            // Query with index name
            // The final prompt will look like this "Emily and David are...(more text based on data). Who is David?".
            var result1 = await _kernel.InvokePromptAsync(query2);

            _output.WriteLine(result1.ToString());
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
