
namespace ZL.SemanticKernelDemo.Host.Requests
{
    public record ListChatHistoryRequest: IRequest<IEnumerable<CopilotChatMessage>>
    {
        public string ChatSessionId { get; set; }
        public string UserId { get; set; }
        
    }

    public class ListChatHistoryHandler : IRequestHandler<ListChatHistoryRequest, IEnumerable<CopilotChatMessage>>
    {
        private readonly ChatMessageRepository _chatMessageRepo;
        private readonly ChatSessionRepository _chatSessionRepo;
        
        // ctor
        public ListChatHistoryHandler(ChatSessionRepository chatSessionRepo, ChatMessageRepository chatMessageRepo)
        {
            _chatMessageRepo = chatMessageRepo;
            _chatSessionRepo = chatSessionRepo;
        }

        public async Task<IEnumerable<CopilotChatMessage>> Handle(ListChatHistoryRequest request, CancellationToken cancellationToken)
        {
            // verify the chat session ownership
            var chatSession = await _chatSessionRepo.GetChatSessionAsync(request.ChatSessionId);

            if(chatSession.UserId.ToLower() != request.UserId.ToLower())
            {
                throw new ArgumentException("Invalid chat session id");
            }

            // return latest 100 chat messages
            var historyMessages = await _chatMessageRepo.GetHistoryMessagesAsync(request.ChatSessionId, 0, 100);
            return historyMessages.Reverse();

        }
    }
}
