using ZL.SemanticKernelDemo.Host.Models.DtoModels;

namespace ZL.SemanticKernelDemo.Host.Requests
{
    public record UpdateChatSessionRequest: IRequest<ChatSession>
    {
        public string ChatSessionId { get; init; }
        public UpdateChatSessionDto UpdateChatSessionDto { get; init; }

    }

    public class UpdateChatSessionHander : IRequestHandler<UpdateChatSessionRequest, ChatSession>
    {
        private readonly ChatSessionRepository _chatSessionRepo;
        
        // ctor
        public UpdateChatSessionHander(ChatSessionRepository chatSessionRepo)
        {
                _chatSessionRepo = chatSessionRepo;
        }

        public async Task<ChatSession> Handle(UpdateChatSessionRequest request, CancellationToken cancellationToken)
        {
            var chatSession = await _chatSessionRepo.UpdateAsync(request.ChatSessionId, request.UpdateChatSessionDto.Title);

            return chatSession;
        }
    }
}
