namespace ZL.SemanticKernelDemo.Host.Persistence
{
    public class ChatMessageRepository
    {
        private readonly CosmosDbChatMessageContext _context;

        // ctor
        public ChatMessageRepository(CosmosDbChatMessageContext context)
        {
            _context = context;
        }

        // get chat messages (only retain the latest 10 messages )
        public async Task<IEnumerable<CopilotChatMessage>> GetHistoryMessagesAsync(string chatSessionId, int skip = 0, int count = -1)
        {
            return await _context.QueryEntitiesAsync(m => m.ChatId == chatSessionId, skip, count);
        }

        // save message
        public async Task<CopilotChatMessage> SaveMessageAsync(string chatSessionId, string userId, string userName, string content, AuthorRoles role, ChatMessageType messageType)
        {
            var newMessage = new CopilotChatMessage();
            newMessage.ChatId = chatSessionId;
            newMessage.UserId = userId;
            newMessage.UserName = userName;
            newMessage.Content = content;
            newMessage.AuthorRole = role;
            newMessage.Type = messageType;

            return await _context.CreateAsync(newMessage, chatSessionId);
        }
    }
}
