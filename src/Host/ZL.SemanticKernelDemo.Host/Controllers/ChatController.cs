

namespace ZL.SemanticKernelDemo.Host.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ApiBaseController
    {
        private readonly ILogger<ChatController> _logger;

        //ctor
        public ChatController(ILogger<ChatController> logger)
        {
            this._logger = logger;
        }

        [Route("{chatSessionId}/history-messages")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IEnumerable<CopilotChatMessage>> GetHistoryMessages(string chatSessionId)
        {
            var req = new ListChatHistoryRequest { ChatSessionId = chatSessionId, UserId = base.ObjectId };

            var historyMessages = await base.Mediator.Send(req);

            return historyMessages;
        }



        [Route("messages")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChatAsync([FromBody] Ask ask)
        {
            var req = new AskRequest
            {
                ChatSessionId = ask.ChatSessionId,
                UserInput = ask.Input,
                UserId = base.ObjectId,
                UserName = base.Upn,
            };

            var result = await base.Mediator.Send(req);

            AskResult chatAskResult = new()
            {
                Value = result.Content,
            };

            return this.Ok(chatAskResult);
        }

    }
}
