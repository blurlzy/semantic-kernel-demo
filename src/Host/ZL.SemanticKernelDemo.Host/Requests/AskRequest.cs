
namespace ZL.SemanticKernelDemo.Host.Requests
{
    public record AskRequest: IRequest<CopilotChatMessage>
    {
        public string ChatSessionId { get; init; }

        public string UserInput { get; init; }

        public string UserId { get; init; }

        public string UserName { get; init; }
    }

    public class AskHandler: IRequestHandler<AskRequest, CopilotChatMessage>
    {
        private readonly ChatMessageRepository _chatMessageRepo;
        private readonly ChatSessionRepository _chatSessionRepo;
        private readonly Kernel _kernel;

        // ctor
        public AskHandler(ChatSessionRepository chatSessionRepo, ChatMessageRepository chatMessageRepo, Kernel kernel)
        {
            _kernel = kernel;
            _chatMessageRepo = chatMessageRepo;
            _chatSessionRepo = chatSessionRepo;
        }

        public async Task<CopilotChatMessage> Handle(AskRequest request, CancellationToken cancellationToken)
        {
            // verify the chat session ownership
            var chatSession = await _chatSessionRepo.GetChatSessionAsync(request.ChatSessionId);

            if (chatSession.UserId.ToLower() != request.UserId.ToLower())
            {
                throw new ArgumentException("Invalid chat session id");
            }


            // get history messages (load last 10 messages)
            var chatMessages = await _chatMessageRepo.GetHistoryMessagesAsync(request.ChatSessionId, 0, 10);

            ChatHistory chatHistory = new();
            // allowed chat history (based on the token limitation)
            ChatHistory allottedChatHistory = new();
            var remainingToken = PromptsOptions.CompletionTokenLimit;

            // add history messages into allowed history
            foreach (var chatMessage in chatMessages)
            {
                // formatting
                var formattedMessage = chatMessage.ToFormattedString();

                // count the token of  the message
                int tokenCount = TokenUtils.TokenCount(formattedMessage);

                if (remainingToken - tokenCount >= 0)
                {
                    //historyText = $"{formattedMessage}\n{historyText}";
                    if (chatMessage.AuthorRole == AuthorRoles.Bot)
                    {
                        // Message doesn't have to be formatted for bot. This helps with asserting a natural language response from the LLM (no date or author preamble).
                        allottedChatHistory.AddAssistantMessage(chatMessage.Content.Trim());
                    }
                    else
                    {
                        // user message 
                        allottedChatHistory.AddUserMessage(formattedMessage);
                    }

                    remainingToken -= tokenCount;
                }
                else
                {
                    break;
                }
            }

            // sort the history messags asc before adding to chat history
            chatHistory.AddRange(allottedChatHistory.Reverse());
            //  user input
            chatHistory.AddUserMessage(request.UserInput);

            // Retrieving chat completion services
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            // Get the chat message content
            ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(
            chatHistory,
                kernel: _kernel
            );

            // save the new user & assistant message into db
            // save user content
            var newUserMessage = await _chatMessageRepo.SaveMessageAsync(request.ChatSessionId, request.UserId, request.UserName, request.UserInput, AuthorRoles.User, ChatMessageType.Message);
            // save assistant content
            var newAssistantMessage = await _chatMessageRepo.SaveMessageAsync(request.ChatSessionId, request.UserId, request.UserName, result.ToString(), AuthorRoles.Bot, ChatMessageType.Message);

            return newAssistantMessage;

        }
    }
}
