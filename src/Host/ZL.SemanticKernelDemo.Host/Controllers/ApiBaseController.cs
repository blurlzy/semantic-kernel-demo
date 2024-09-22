
using System.Security.Claims;

namespace ZL.SemanticKernelDemo.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        // ! (null-forgiving) operator
        // By using the null-forgiving operator, you inform the compiler that passing null is expected and shouldn't be warned about.
        private ISender _mediator = null!;

        // return the instance of MediatR
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        // return identity name
        protected string IdentityName => (User.Identity != null && User.Identity.IsAuthenticated) ? User.Identity.Name ?? string.Empty : string.Empty;

        // upn 
        // ** upn is not available for guest user (Azure AD)
        protected string Upn
        {
            get
            {
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return string.Empty;
                }

                // find upn claim, if upn is not available, then return email
                return User.Claims.FirstOrDefault(c => c.Type == AzureAdClaimTypes.Upn)?.Value ?? this.Email;
            }
        }

        // unique object (user) id
        protected string ObjectId
        {
            get
            {
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return string.Empty;
                }

                // find upn claim
                return User.Claims.FirstOrDefault(c => c.Type == AzureAdClaimTypes.ObjectId)?.Value ?? string.Empty;
            }
        }

        // email
        protected string Email
        {
            get
            {
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return string.Empty;
                }

                return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            }
        }
    }
}
