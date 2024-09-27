using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using System.Text.Json.Serialization;

namespace ZL.SemanticKernelDemo.Host.Services
{
    public sealed class AzureAISearchService
    {
        private readonly List<string> _defaultVectorFields = ["text_vector"];
        private readonly SearchIndexClient _indexClient;

        // ctor
        public AzureAISearchService(SearchIndexClient srchIndexClient) 
        {
            _indexClient = srchIndexClient;
        }

        public async Task<List<IndexSchema>> SearchAsync(string collectionName, ReadOnlyMemory<float> vector, List<string>? searchFields = null, CancellationToken cancellationToken = default)
        {
            // Get client for search operations
            SearchClient searchClient = this._indexClient.GetSearchClient(collectionName);

            // Use search fields passed from Plugin or default fields configured in this class.
            List<string> fields = searchFields is { Count: > 0 } ? searchFields : this._defaultVectorFields;

            // Configure request parameters
            VectorizedQuery vectorQuery = new(vector);
            fields.ForEach(vectorQuery.Fields.Add);

            SearchOptions searchOptions = new() { VectorSearch = new() { Queries = { vectorQuery } } };

            // Perform search request
            var response = await searchClient.SearchAsync<IndexSchema>(searchOptions, cancellationToken);

            List<IndexSchema> results = [];

            // Collect search results
            await foreach (SearchResult<IndexSchema> result in response.Value.GetResultsAsync())
            {
                results.Add(result.Document);
            }

            return results;
            // Return text from first result.
            // In real applications, the logic can check document score, sort and return top N results
            // or aggregate all results in one text.
            // The logic and decision which text data to return should be based on business scenario. 
            //return results.FirstOrDefault()?.Chunk;
        }
    }

    // Azure AI Search index scheme
    public sealed class IndexSchema
    {
        [JsonPropertyName("chunk_id")]
        public string ChunkId { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("chunk")]
        public string Chunk { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("text_vector")]
        public ReadOnlyMemory<float> Vector { get; set; }
    }

}
