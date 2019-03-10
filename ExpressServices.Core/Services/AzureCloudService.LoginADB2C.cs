using ExpressServices.Core.Helpers;
using ExpressServices.Core.Models;
using Microsoft.Identity.Client;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public partial class AzureCloudService
    {
        private PublicClientApplication PCA = new PublicClientApplication(Constants.ClientID, Constants.Authority);

        private async Task<User> LoginADB2CAsync()
        {
            PCA.RedirectUri = Constants.RedirectUri;

            IEnumerable<IAccount> accounts = await PCA.GetAccountsAsync();

            User user = null;

            try
            {
                AuthenticationResult ar = await PCA.AcquireTokenAsync(Constants.Scopes, GetAccountByPolicy(accounts, Constants.PolicySignUpSignIn));

                await GetAzureMobileAuthAsync(ar.IdToken);

                user = await UpdateUserInfo(ar.IdToken);
            }
            catch (Exception ex)
            {
                // Checking the exception message
                // should ONLY be done for B2C
                // reset and not any other error.
                if (ex.Message.Contains("AADB2C90118"))
                    user = await OnPasswordReset();
                // Alert if any exception excludig user cancelling sign-in dialog
                else if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                    throw new Exception($"Exception: {ex}");
            }

            return user;
        }

        private async Task<User> OnPasswordReset()
        {
            User user = null;

            try
            {
                AuthenticationResult ar = await PCA.AcquireTokenAsync(Constants.Scopes, (IAccount)null, UIBehavior.NoPrompt, string.Empty, null, Constants.AuthorityPasswordReset);

                await GetAzureMobileAuthAsync(ar.IdToken);

                user = await UpdateUserInfo(ar.IdToken);
            }
            catch (Exception ex)
            {
                // Alert if any exception excludig user cancelling sign-in dialog
                if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                    throw new Exception($"Exception: {ex}");
            }

            return user;
        }

        private IAccount GetAccountByPolicy(IEnumerable<IAccount> accounts, string policy)
        {
            foreach (var account in accounts)
            {
                string userIdentifier = account.HomeAccountId.ObjectId.Split('.')[0];
                if (userIdentifier.EndsWith(policy.ToLower())) return account;
            }

            return null;
        }

        private async Task GetAzureMobileAuthAsync(string idToken)
        {
            var payload = new JObject();
            payload["access_token"] = idToken;

            await Client.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, payload);
        }

        private string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }

        private JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }

        private async Task<User> UpdateUserInfo(string idToken)
        {
            JObject account = ParseIdToken(idToken);

            var userTable = GetTable<User>().GetCloudTable();

            User user = new User
            {
                ProviderId = account["oid"]?.ToString(),
                Name = account["name"]?.ToString(),
                Email = account["emails"]?[0]?.ToString(),
                Role = account["jobTitle"]?.ToString()
            };

            // checks if it's the user's first access          
            var loggedUser = (await userTable.Where(x => x.Email == user.Email).ToListAsync()).FirstOrDefault();

            // if it is, add him to the app's database
            if (loggedUser != null)
            {
                user = loggedUser;
            }
            else
            {
                user.DateCreated = DateTime.UtcNow;
                await userTable.InsertAsync(user);
            }

            // Update current user info
            CurrentUser currentUser = CurrentUser.Instance;

            currentUser = user.Cast<CurrentUser>();

            return user;
        }
    }
}
