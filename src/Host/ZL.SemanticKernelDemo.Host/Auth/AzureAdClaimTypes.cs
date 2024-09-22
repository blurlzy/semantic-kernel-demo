namespace ZL.SemanticKernelDemo.Host.Auth
{
    public static class AzureAdClaimTypes
    {
        // User principal name
        public const string Upn = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";
        // Azure Object ID
        public const string ObjectId = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        // Azure Ad tenant id
        public const string TenantId = "http://schemas.microsoft.com/identity/claims/tenantid";

        public const string Tid = "tid";
        // auth_time
        public const string AuthTime = "auth_time";
    }
}
