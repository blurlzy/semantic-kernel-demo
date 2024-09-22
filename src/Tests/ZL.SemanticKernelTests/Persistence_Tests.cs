
using Microsoft.Azure.Cosmos;
using ZL.SemanticKernelDemo.Host.Models;
using ZL.SemanticKernelDemo.Host.Persistence;

namespace ZL.SemanticKernelTests
{
    public class Persistence_Tests
    {
        // cosmos settings
        private readonly string _cosmosConnection = SecretManager.GetSecret(SecretKeys.CosmosConnection);
        private readonly string _cosmosDb = SecretManager.GetSecret(SecretKeys.CosmosDb);
        private readonly string _chatSessionContainer = SecretManager.GetSecret(SecretKeys.ChatSessionContainer);

        private readonly CosmosDbContext<ChatSession> _chatSessionContext;
        private readonly ChatSessionRepository _chatSessionRepository;

        // ctor
        public Persistence_Tests()
        {
            _chatSessionContext = new CosmosDbContext<ChatSession>(_cosmosConnection, _cosmosDb, _chatSessionContainer);
            _chatSessionRepository = new ChatSessionRepository(_chatSessionContext);
        }

        [Theory]
        [InlineData("New Chat 1", "This is a testing chat", "zongyi")]
        public async Task Add_ChatSession_Test(string title,string desc, string userId)
        {
            // 
            var newChatSession = new ChatSession(title, desc, userId, userId);

            // save
            await _chatSessionRepository.CreateAsync(newChatSession);
        }
    }
}
