

namespace ZL.SemanticKernelDemo.Host.Persistence
{
    public class ChatSessionRepository
    {
        private readonly CosmosDbContext<ChatSession> _context;

        // ctor
        public ChatSessionRepository(CosmosDbContext<ChatSession> context)
        {
            _context = context;
        }

        // get chat sessions for the user
        public async Task<IEnumerable<ChatSession>> ListChatSessionsAsync(string userId)
        {
            return await _context.ListItemsAsync(m => m.UserId == userId);
        }

        // get the chat session by id
        public async Task<ChatSession> GetChatSessionAsync(string chatSessionId)
        {
            return await _context.ReadAsync(chatSessionId, chatSessionId);
        }

        // create a new chat session
        public async Task<ChatSession> CreateAsync(ChatSession chatSession)
        {
            return await _context.CreateAsync(chatSession, chatSession.Id);
        }

        public async Task<ChatSession> UpdateAsync(string chatSessionId)
        {
            // read
            var chatSession = await _context.ReadAsync(chatSessionId, chatSessionId);

            // last updated
            chatSession.UpdatedOn = DateTimeOffset.UtcNow;

            return await _context.UpsertAsync(chatSession, chatSessionId);
        }

        // update chat session
        public async Task<ChatSession> UpdateAsync(string chatSessionId, string title)
        {
            // read
            var chatSession = await _context.ReadAsync(chatSessionId, chatSessionId);

            // update title
            chatSession.Title = title;
            chatSession.UpdatedOn = DateTimeOffset.UtcNow;

            return await _context.UpsertAsync(chatSession, chatSessionId);
        }
        
        // soft delete
        public async Task SoftDeleteAsync(string id)
        {
            // get the entity
            var entity = await _context.ReadAsync(id, id);

            entity.IsDeleted = true;
            entity.UpdatedOn = DateTimeOffset.Now;
            // update
            await _context.UpsertAsync(entity, entity.Id);
        }

        // delete
        public async Task DeleteAsync(string id)
        {
            await _context.DeleteAsync(id, id);  
        }
    }
}
