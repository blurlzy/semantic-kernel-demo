using Azure;
using Azure.Search.Documents.Indexes;

namespace ZL.SemanticKernelDemo.Host.Services
{
    public static class Startup
    {
        // register / configure services
        public static void ConfigureSemanticKernel(this IServiceCollection services, IConfiguration configuration)
        {
            // init kernal provider
            services.AddSingleton(sp => new SemanticKernelProvider(sp, configuration));

            // semantic kernel service
            services.AddScoped<Kernel>(
                sp => {

                    var provider = sp.GetRequiredService<SemanticKernelProvider>();
                    var kernel = provider.GetCompletionKernel();

                    // register plugins
                    kernel.ImportPluginFromType<AzureAISearchPlugin>();
                    // kernel.ImportPluginFromObject(new CustomerPlugin(), nameof(CustomerPlugin));
                    // kernel.ImportPluginFromType<CustomerPlugin>();
                    return kernel;
                });

        }

        //// register / configure Azure AI search client
        //public static void ConfigureAzureAISearchClient(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var searchEndpoint = configuration[SecretKeys.SearchEndpoint];
        //    var searchKey = configuration[SecretKeys.SearchKey];

        //    // Azure AI Search configuration
        //    Uri endpoint = new(searchEndpoint);
        //    AzureKeyCredential keyCredential = new(searchKey);


        //    services.AddSingleton<SearchIndexClient>((_) => new SearchIndexClient(endpoint, keyCredential));
        //    // Custom AzureAISearchService to configure request parameters and make a request.
        //    services.AddSingleton<AzureAISearchService>();
        //}

    }
}
