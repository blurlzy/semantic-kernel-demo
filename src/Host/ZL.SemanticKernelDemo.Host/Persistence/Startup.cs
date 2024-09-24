namespace ZL.SemanticKernelDemo.Host.Persistence
{
    public static class Startup
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var cosmosConnection = configuration[SecretKeys.CosmosConnection];
            var cosmosDb = configuration[SecretKeys.CosmosDb];
            var chatSessionContainer = configuration[SecretKeys.ChatSessionContainer];
            var chatMessageContainer = configuration[SecretKeys.ChatHistoryContainer];

            // 
#pragma warning disable CS8604 // Possible null reference argument.
            var chatSessionDbContext = new CosmosDbContext<ChatSession>(cosmosConnection, cosmosDb, chatSessionContainer);
            var chatMessageDbContext = new CosmosDbChatMessageContext(cosmosConnection, cosmosDb, chatMessageContainer);

#pragma warning restore CS8604 // Possible null reference argument.

            // register
            // chat session context
            services.AddSingleton<ChatSessionRepository>(m => new ChatSessionRepository(chatSessionDbContext));
            // chat history context
            services.AddSingleton<ChatMessageRepository>(m => new ChatMessageRepository(chatMessageDbContext));
        }
    }
}
