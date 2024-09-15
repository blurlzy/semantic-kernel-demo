

namespace ZL.SemanticKernelDemo.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;

        //ctor
        public ChatController(ILogger<ChatController> logger)
        {
            this._logger = logger;
        }

        [Route("messages")]
        [HttpPost]
        public async Task<IActionResult> ChatAsync([FromServices] Kernel kernel,
                                                   [FromBody] Ask ask)
        {
            // Create a chat history object
            ChatHistory chatHistory = [];

            // ask for the first option - pizza
            chatHistory.AddSystemMessage("You are a helpful assistant.");
            // chatHistory.AddUserMessage("What's available to order?");
            // chatHistory.AddAssistantMessage("We have pizza, pasta, and salad available to order. What would you like to order?");
            chatHistory.AddUserMessage(ask.Input);

            // Once the services have been added, we then build the kernel and retrieve the chat completion service for later use.
            // Retrieving chat completion services
            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            // Get the chat message content
            ChatMessageContent result = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory,
                kernel: kernel
            );

            AskResult chatAskResult = new()
            {
                Value = result.ToString() ?? string.Empty,
                //Variables = contextVariables.Select(v => new KeyValuePair<string, object?>(v.Key, v.Value))
            };

            return this.Ok(chatAskResult);
        }
    }
}
