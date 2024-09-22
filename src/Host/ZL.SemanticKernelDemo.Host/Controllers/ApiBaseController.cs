
using System.Security.Claims;

namespace ZL.SemanticKernelDemo.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        // return identity name
        // * you will need include preferred_username as optionl claims when using Azure AD token v2.0
        // The v1.0 tokens include preferred_username by default, but the v2.0 tokens do not.
        protected string IdentityName => (User.Identity != null && User.Identity.IsAuthenticated) ? User.Identity.Name ?? string.Empty : string.Empty;

        // upn 
        // upn is not available for guest user
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
