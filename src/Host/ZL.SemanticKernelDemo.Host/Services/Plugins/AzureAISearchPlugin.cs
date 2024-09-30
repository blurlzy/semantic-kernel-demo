using Microsoft.SemanticKernel.Embeddings;
using System.ComponentModel;

namespace ZL.SemanticKernelDemo.Host.Services.Plugins
{
    /// <summary>
    /// Azure AI Search SK Plugin.
    /// It uses <see cref="ITextEmbeddingGenerationService"/> to convert string query to vector.
    /// It uses <see cref="IAzureAISearchService"/> to perform a request to Azure AI Search.
    /// </summary>
    public sealed class AzureAISearchPlugin(
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        ITextEmbeddingGenerationService textEmbeddingGenerationService,
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        AzureAISearchService searchService)
    {
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        private readonly ITextEmbeddingGenerationService _textEmbeddingGenerationService = textEmbeddingGenerationService;
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        private readonly AzureAISearchService _searchService = searchService;

        [KernelFunction("Search"), Description("Search results from Azure AI search")]
        public async Task<string> SearchAsync(
            string query,
            string collection,
            List<string>? searchFields = null,
            CancellationToken cancellationToken = default)
        {
            // Convert string query to vector
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            ReadOnlyMemory<float> embedding = await _textEmbeddingGenerationService.GenerateEmbeddingAsync(query, cancellationToken: cancellationToken);
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            // Perform search
            var results = await this._searchService.SearchAsync(collection, embedding, searchFields, cancellationToken);

            //foreach (var result in results)
            //{
            //    if(result.Title.Contains("SMB") || result.Title.Contains("SMB + AMM readiness call"))
            //    {
            //        return result.Chunk;
            //    }
            //}

            // Return text from first result.
            // In real applications, the logic can check document score, sort and return top N results
            // or aggregate all results in one text.
            // The logic and decision which text data to return should be based on business scenario. 
            return results.FirstOrDefault()?.Chunk ?? "No results found.";
        }
    }
}