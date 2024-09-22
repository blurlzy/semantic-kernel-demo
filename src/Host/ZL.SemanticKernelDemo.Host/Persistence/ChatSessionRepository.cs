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

        // create a new chat session
        public async Task<ChatSession> CreateAsync(ChatSession chatSession)
        {
            return await _context.CreateAsync(chatSession, chatSession.Id);
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
