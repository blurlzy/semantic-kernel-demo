

namespace ZL.SemanticKernelDemo.Host.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatSessionsController : ApiBaseController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IEnumerable<ChatSession>> ListChatSessions()
        {
            var req = new ListChatSessionsRequest { UserId = base.ObjectId };

            return await base.Mediator.Send(req);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateChatSessionDto model)
        {
            var req = new CreateChatSessionRequest { Title = model.Title, Description = model.Description ?? string.Empty, UserId = base.ObjectId, UserName = base.Upn };

            var result = await base.Mediator.Send(req);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateChatSessionDto model)
        {
            var req = new UpdateChatSessionRequest { ChatSessionId = id, UpdateChatSessionDto = model };
            var respnse = await base.Mediator.Send(req);
            return Ok(respnse);
        }
    }
}
