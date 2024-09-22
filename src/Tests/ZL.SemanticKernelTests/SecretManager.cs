
namespace ZL.SemanticKernelTests
{
    /// <summary>
    /// load secrets (open ai endpoint, key..etc) from azure key vault 
    /// </summary>
    internal static class SecretManager
    {
        // azure key vault name
        private const string _kv = "kv-aianywhere-dev";

        private static SecretClient Client { get; } = new SecretClient(
                       new Uri($"https://{_kv}.vault.azure.net/"),
                                   new DefaultAzureCredential(new DefaultAzureCredentialOptions
                                   {
                                       ExcludeEnvironmentCredential = true,
                                       ExcludeVisualStudioCredential = true,
                                       ExcludeVisualStudioCodeCredential = true,
                                       ExcludeSharedTokenCacheCredential = true
                                   }));

        //
        internal static string OpenAIEndpoint => GetSecret(SecretKeys.OpenAIEndpoint);
        internal static string OpenAIKey => GetSecret(SecretKeys.OpenAIKey);
        //internal static string SearchEndpoint => GetSecret(SecretKeys.SearchEndpoint);
        //internal static string SearchKey => GetSecret(SecretKeys.SearchKey);

        // get secret from azure key vault
        internal static string GetSecret(string secretName) => Client.GetSecret(secretName).Value.Value;
    }

    internal static class SecretKeys
    {
        // Open AI 
        public const string OpenAIEndpoint = "OpenAIEndpoint";
        public const string OpenAIKey = "OpenAIKey";

        //// AI Search
        //public const string SearchEndpoint = "SearchEndpoint";
        //public const string SearchKey = "SearchKey";

        // Cosmos
        public const string CosmosConnection = "CosmosConnection";
        public const string CosmosDb = "CosmosDb";
        public const string ChatSessionContainer = "ChatSessionContainer";
        public const string ChatHistoryContainer = "ChatMessageContainer";
    }
}
