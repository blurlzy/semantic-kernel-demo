
namespace ZL.SemanticKernelDemo.Host.Requests
{
    public record CreateChatSessionRequest: IRequest<ChatSession>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public string UserId { get; init; }
        public string UserName { get; init; }

    }

    public class CreateChatSessionHandler: IRequestHandler<CreateChatSessionRequest, ChatSession>
    {
        private readonly ChatSessionRepository _chatSessionRepo;

        // ctor
        public CreateChatSessionHandler(ChatSessionRepository chatSessionRepo)
        {
                _chatSessionRepo = chatSessionRepo;
        }

        public async Task<ChatSession> Handle(CreateChatSessionRequest request, CancellationToken cancellationToken)
        {
            var newChatSession = new ChatSession(request.Title, request.UserId, request.UserName);

            // create
            await _chatSessionRepo.CreateAsync(newChatSession);

            return newChatSession;
        }
    }
}
