
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
        [InlineData("New Chat 3",  "zongyi")]
        public async Task Add_ChatSession_Test(string title, string userId)
        {
            // 
            var newChatSession = new ChatSession(title,  userId, userId);

            // save
            await _chatSessionRepository.CreateAsync(newChatSession);
        }

        [Theory]
        [InlineData("zongyi")]
        public async Task List_ChatSessions_Test(string userId)
        {
            var entities = await  _chatSessionRepository.ListChatSessionsAsync(userId);

            Assert.NotNull(entities);
        }


        [Theory]
        [InlineData("fc7c961b-5913-4036-a137-941ee2daf1c0")]
        public async Task SoftDelete_ChatSession_Test(string id)
        {
            await _chatSessionRepository.SoftDeleteAsync(id);
        }

        [Theory]
        [InlineData("fc7c961b-5913-4036-a137-941ee2daf1c0")]
        public async Task Delete_ChatSession_Test(string id)
        {
            await _chatSessionRepository.DeleteAsync(id);
        }
    }
}
