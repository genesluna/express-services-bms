namespace ExpressServices.Core.Helpers
{
    public static class Constants
    {
        // Azure AD B2C Coordinates
        public static readonly string Tenant = "manuex.onmicrosoft.com";

        public static readonly string AzureADB2CHostname = "manuex.b2clogin.com";
        public static readonly string ClientID = "7719fe0f-1f0b-4c01-b1d3-51b0b41b536f";
        public static readonly string PolicySignUpSignIn = "B2C_1_MEB2CSingInV2";
        public static readonly string PolicyEditProfile = "B2C_1_MEB2CProfileEdit";
        public static readonly string PolicyResetPassword = "B2C_1_MEB2CPassworrdResetV2";

        public static readonly string[] Scopes = { "" };

        public static readonly string AuthorityBase = $"https://{AzureADB2CHostname}/tfp/{Tenant}/";
        public static readonly string Authority = $"{AuthorityBase}{PolicySignUpSignIn}";
        public static readonly string AuthorityEditProfile = $"{AuthorityBase}{PolicyEditProfile}";
        public static readonly string AuthorityPasswordReset = $"{AuthorityBase}{PolicyResetPassword}";

        public static readonly string RedirectUri = $"msal{ClientID}://auth";

        // B2C GRAPH
        public static readonly string aadInstance = "https://login.microsoftonline.com/";

        public static readonly string aadGraphResourceId = "https://graph.windows.net/";
        public static readonly string aadGraphEndpoint = "https://graph.windows.net/";
        public static readonly string aadGraphSuffix = "";
        public static readonly string aadGraphVersion = "api-version=1.6";

        public static readonly string aadGraphClientId = "3d68f875-78f7-4f28-8e1f-fe9ceac81bda";
    }
}