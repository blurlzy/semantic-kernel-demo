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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string aoaiKey = configuration[SecretKeys.OpenAIKey];
            string aoaiEndpoint = configuration[SecretKeys.OpenAIEndpoint];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            string depployment = "gpt-4o";

            if(aoaiEndpoint == null || aoaiKey == null)
            {
                throw new ArgumentException($"Invalid Configuration. (AOAI Settings)");
            }

            // add Azure OpenAI
            builder.AddAzureOpenAIChatCompletion(depployment, aoaiEndpoint, aoaiKey);

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
