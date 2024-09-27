namespace ZL.SemanticKernelDemo.Host.Auth
{
    internal static class SecretKeys
    {
        // AOAI
        public const string OpenAIEndpoint = "OpenAIEndpoint";
        public const string OpenAIKey = "OpenAIKey";

        // AI Search
        public const string SearchEndpoint = "SearchEndpoint";
        public const string SearchKey = "SearchKey";
        public const string SearchIndex = "SearchIndex";

        // Cosmos
        public const string CosmosConnection = "CosmosConnection";
        public const string CosmosDb = "CosmosDb";
        public const string ChatSessionContainer = "ChatSessionContainer";
        public const string ChatHistoryContainer = "ChatMessageContainer";

        // application insights
        public const string AppInsightsConnection = "AppInsightsConnection";
    }
}
