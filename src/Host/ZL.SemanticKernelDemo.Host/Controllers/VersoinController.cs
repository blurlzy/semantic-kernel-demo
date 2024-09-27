
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

        //[HttpGet("exp")]
        //public IActionResult TextExp()
        //{
        //    throw new NotImplementedException("Testing exp");
        //}

        //[HttpGet("Rag")]
        //public async Task<IActionResult> SearchFromYourOwnData([FromServices] Kernel kernel)
        //{
        //    string userInput = "What is the AMM Offer Navigator?";
        //    string searchIndex = "azure-workshop-001";

        //    string query2 = "{{search '" + userInput + "' collection='" + searchIndex + "'}}";
        //    // Query with index name
        //    // The final prompt will look like this "Emily and David are...(more text based on data). Who is David?".
        //    var result1 = await kernel.InvokePromptAsync(query2);

        //    return Ok(result1.ToString());
        //}
    }
}
