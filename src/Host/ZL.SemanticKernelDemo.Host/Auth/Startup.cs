using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace ZL.SemanticKernelDemo.Host.Auth
{
    public static class Startup
    {
        public static void ConfigureAzureEntraIDAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddMicrosoftIdentityWebApi(configuration, "AzureEntraId"); // specify the configuration section name, the default value is "AzureAd"
        }
    }
}
