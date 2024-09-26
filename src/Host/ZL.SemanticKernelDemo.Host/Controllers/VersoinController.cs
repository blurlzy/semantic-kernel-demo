
namespace ZL.SemanticKernelDemo.Host.Controllers
{
    [Route("")]
    [ApiController]
    public class VersoinController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetVersion()
        {
            return Ok(new { version = "ZL Copilot API beta 1.0 20240926" });
        }
    }
}
