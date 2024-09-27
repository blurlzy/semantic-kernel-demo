using Azure.Search.Documents.Indexes;
using Azure;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ZL.SemanticKernelDemo.Host.Services
{
    public class SemanticKernelProvider
    {
        private readonly Kernel _kernel;

        // ctor
        public SemanticKernelProvider(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this._kernel = InitializeCompletionKernel(serviceProvider, configuration);
        }

        /// <summary>
        /// Produce semantic-kernel with only completion services for chat.
        /// </summary>
        public Kernel GetCompletionKernel() => this._kernel.Clone();

        private static Kernel InitializeCompletionKernel(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var builder = Kernel.CreateBuilder();

            builder.Services.AddLogging();

            // AOAI
            string aoaiKey = configuration[SecretKeys.OpenAIKey]!;
            string aoaiEndpoint = configuration[SecretKeys.OpenAIEndpoint]!;
            string depployment = "gpt-4o";

            if(aoaiEndpoint == null || aoaiKey == null)
            {
                throw new ArgumentException($"Invalid Configuration. (AOAI Settings)");
            }

            // add AOAI chat completion service
            builder.AddAzureOpenAIChatCompletion(depployment, aoaiEndpoint, aoaiKey);

            // add AOAI text embedding generation service
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            builder.AddAzureOpenAITextEmbeddingGeneration("text-embedding-ada-002", aoaiEndpoint, aoaiKey);
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            string searchEndpoint = configuration[SecretKeys.SearchEndpoint]!;
            string searchKey = configuration[SecretKeys.SearchKey]!;

            // Azure AI Search configuration
            Uri endpoint = new(searchEndpoint);
            AzureKeyCredential keyCredential = new(searchKey);

            builder.Services.AddSingleton<SearchIndexClient>((_) => new SearchIndexClient(endpoint, keyCredential));
            // Custom AzureAISearchService to configure request parameters and make a request.
            builder.Services.AddSingleton<AzureAISearchService>();



            //            var memoryOptions = serviceProvider.GetRequiredService<IOptions<KernelMemoryConfig>>().Value;

            //            switch (memoryOptions.TextGeneratorType)
            //            {
            //                case string x when x.Equals("AzureOpenAI", StringComparison.OrdinalIgnoreCase):
            //                case string y when y.Equals("AzureOpenAIText", StringComparison.OrdinalIgnoreCase):
            //                    var azureAIOptions = memoryOptions.GetServiceConfig<AzureOpenAIConfig>(configuration, "AzureOpenAIText");
            //#pragma warning disable CA2000 // No need to dispose of HttpClient instances from IHttpClientFactory
            //                    builder.AddAzureOpenAIChatCompletion(
            //                        azureAIOptions.Deployment,
            //                        azureAIOptions.Endpoint,
            //                        azureAIOptions.APIKey,
            //                        httpClient: httpClientFactory.CreateClient());
            //                    break;

            //                case string x when x.Equals("OpenAI", StringComparison.OrdinalIgnoreCase):
            //                    var openAIOptions = memoryOptions.GetServiceConfig<OpenAIConfig>(configuration, "OpenAI");
            //                    builder.AddOpenAIChatCompletion(
            //                        openAIOptions.TextModel,
            //                        openAIOptions.APIKey,
            //                        httpClient: httpClientFactory.CreateClient());
            //#pragma warning restore CA2000
            //                    break;

            //                default:
            //                    throw new ArgumentException($"Invalid {nameof(memoryOptions.TextGeneratorType)} value in 'KernelMemory' settings.");
            //            }

            return builder.Build();
        }
    }
}
