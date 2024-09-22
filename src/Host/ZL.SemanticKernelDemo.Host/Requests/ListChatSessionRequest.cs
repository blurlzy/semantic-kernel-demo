
using ZL.SemanticKernelDemo.Host.Persistence;

namespace ZL.SemanticKernelDemo.Host.Requests
{
    public record ListChatSessionsRequest: IRequest<IEnumerable<ChatSession>>
    {
        // unique user id 
        public string UserId { get; init; }
    }

    public class ListChatSessionsHandler : IRequestHandler<ListChatSessionsRequest, IEnumerable<ChatSession>>
    {
        private readonly ChatSessionRepository _chatSessionRepo;

        // ctor
        public ListChatSessionsHandler(ChatSessionRepository chatSessionRepo)
        {
            _chatSessionRepo = chatSessionRepo;
        }

        public async Task<IEnumerable<ChatSession>> Handle(ListChatSessionsRequest request, CancellationToken cancellationToken)
        {
            var chatSessions = await _chatSessionRepo.ListChatSessionsAsync(request.UserId);

            // sorting
            return chatSessions.OrderByDescending(m => m.UpdatedOn);
        }
    }
}
