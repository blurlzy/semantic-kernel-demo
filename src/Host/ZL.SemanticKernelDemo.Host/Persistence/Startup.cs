

namespace ZL.SemanticKernelDemo.Host.Persistence
{
    public static class Startup
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var cosmosConnection = configuration[SecretKeys.CosmosConnection];
            var cosmosDb = configuration[SecretKeys.CosmosDb];
            var chatSessionContainer = configuration[SecretKeys.ChatSessionContainer];

            // 
#pragma warning disable CS8604 // Possible null reference argument.
            var chatSessionDbContext = new CosmosDbContext<ChatSession>(cosmosConnection, cosmosDb, chatSessionContainer);
#pragma warning restore CS8604 // Possible null reference argument.

            // register
            // chat session context
            services.AddSingleton<ChatSessionRepository>(m => new ChatSessionRepository(chatSessionDbContext));
            // chat history context
        }
    }
}
