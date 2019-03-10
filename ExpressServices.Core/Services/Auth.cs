using ExpressServices.Core.Helpers;
using ExpressServices.Core.Models;
using Microsoft.Identity.Client;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace ExpressServices.Core.Services
{
    public class Auth
    {
        #region Properties

        private PasswordVault vault = new PasswordVault();

        private string provider = MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory.ToString();

        private MobileServiceClient client;

        private UserTableService UserTableService = new UserTableService();

        private B2CGraphService b2CGraphService = new B2CGraphService();

        private PublicClientApplication PCA = new PublicClientApplication(Constants.ClientID, Constants.Authority);

        #endregion Properties

        #region Methods

        public async Task<MobileServiceUser> AuthenticateAsync()
        {
            //var users = await b2CGraphService.GetAllUsers(query: null);
            //var user = await b2CGraphService.GetUserByObjectId("18a4adbb-31eb-4af2-95f4-ee94865566d7");

            //GraphUser graphUser = new GraphUser();

            //graphUser.SingInNames.Add(new SingInNames { Value = "genesluna@manutencaoexpress.com.br" });
            //graphUser.AccountEnabled = true;
            //graphUser.DisplayName = "Genética Houston";
            //graphUser.Role = "Técnico";

            //string payload = JsonConvert.SerializeObject(graphUser);

            //await b2CGraphService.CreateUser(payload);

            client.CurrentUser = RetrieveTokenFromSecureStore();

            if (client.CurrentUser != null && !IsTokenExpired(client.CurrentUser.MobileServiceAuthenticationToken))
            {
                // User has previously been authenticated, no refresh is required
                return client.CurrentUser;
            }

            if (client.CurrentUser != null)
            {
                // User has previously been authenticated - try to Refresh the token
                try
                {
                    var refreshed = await client.RefreshUserAsync();
                    if (refreshed != null)
                    {
                        StoreTokenInSecureStore(refreshed);
                        return refreshed;
                    }
                }
                catch (Exception refreshException)
                {
                    Debug.WriteLine($"Could not refresh token: {refreshException.Message}");
                }
            }

            // We need to ask for credentials at this point
            client.CurrentUser = null;

        TryAgain:

            try
            {
                // Client Flow
                await LoginADB2CAsync();

                // Server-Flow Version
                //await client.LoginAsync(provider);

                // We were able to successfully log in
                if (client.CurrentUser != null)
                {
                    StoreTokenInSecureStore(client.CurrentUser);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error authenticating: {ex.Message}");
                var dialog = new MessageDialog("Falha na autenticação. Você deve fazer login para ter acesso ao sistema.");
                dialog.Commands.Add(new UICommand("Tentar novamente"));
                await dialog.ShowAsync();

                goto TryAgain;
            }

            return client.CurrentUser;
        }

        public async Task<bool> LogoutAsync()
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Deseja mesmo fazer logout?",
                Content = "Caso prossiga, o aplicativo será reinicializado e novas credenciais serão solicitadas.",
                PrimaryButtonText = "Logout",
                CloseButtonText = "Cancelar"
            };

            if (client.CurrentUser == null || client.CurrentUser.MobileServiceAuthenticationToken == null)
                return false;

            if (Network.IsConnected)
            {
                ContentDialogResult result = await deleteFileDialog.ShowAsync();

                // Delete the customer if the user clicked the primary button.
                // Otherwise, do nothing.
                if (result == ContentDialogResult.Primary)
                {
                    // Invalidate the token on the mobile backend
                    var authUri = new Uri($"{client.MobileAppUri}/.auth/logout");
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", client.CurrentUser.MobileServiceAuthenticationToken);
                        await httpClient.GetAsync(authUri);
                    }

                    // Remove the token from the cache
                    RemoveTokenFromSecureStore();

                    // Remove the token from the MobileServiceClient
                    await client.LogoutAsync();

                    return true;

                    //Reilitialize
                    //Util util = new Util();
                    //util.DoMajorAppReconfiguration();
                }
            }
            else
            {
                var dialog = new MessageDialog("O equipamento está offline. Não é possivel efetuar o logout.");
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
                return false;
            }

            return false;
        }

        public void UpdateUserInfo(AuthenticationResult ar)
        {
            JObject account = ParseIdToken(ar.IdToken);

            // checks if it's the user's first access
            var loggedUser = UserTableService.UserTable.ReadItemAsync(account["oid"]?.ToString());

            // if it is, add him to the app's database
            if (loggedUser == null)
            {
                User user = new User
                {
                    Id = account["oid"]?.ToString(),
                    Name = account["name"]?.ToString(),
                };
            }
        }

        public async Task GetUserProfileInfoAsync()
        {
            // If we're online, then sync
            if (Network.IsConnected)
            {
                try
                {
                    var user = CurrentUser.Instance;

                    var response = await client.InvokeApiAsync("AzureAD", HttpMethod.Get, null);

                    user = response.ToObject<CurrentUser>();

                    //localSettings.Values["CompanyId"] = App.CompanyID = user.CompanyId;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error Getting UserProfileInfo: {ex.Message}");
                }
            }
        }

        private async Task LoginADALAsync()
        {
            //MobileServiceUser user = null;

            //string authority = Locations.AadAuthority;
            //string resourceId = Locations.AadResourceId;
            //string clientId = Locations.AadClientId;
            //string redirectUri = Locations.AadRedirectUri;

            //while (user == null)
            //{
            //    string message;
            //    try
            //    {
            //        AuthenticationContext ac = new AuthenticationContext(authority);
            //        AuthenticationResult ar = await ac.AcquireTokenAsync(resourceId, clientId,
            //            new Uri(redirectUri), new PlatformParameters(PromptBehavior.Auto, false));
            //        JObject payload = new JObject();
            //        payload["access_token"] = ar.AccessToken;
            //        user = await client.LoginAsync(
            //            MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, payload);
            //        message = string.Format("You are now logged in - {0}", user.UserId);
            //    }
            //    catch (InvalidOperationException)
            //    {
            //        message = "You must log in. Login Required";
            //    }
            //    var dialog = new MessageDialog(message);
            //    dialog.Commands.Add(new UICommand("OK"));
            //    await dialog.ShowAsync();
            //}
        }

        private async Task LoginADB2CAsync(bool useSilent = false)
        {
            PCA.RedirectUri = Constants.RedirectUri;

            IEnumerable<IAccount> accounts = await PCA.GetAccountsAsync();

            try
            {
                AuthenticationResult ar = await PCA.AcquireTokenAsync(Constants.Scopes, GetAccountByPolicy(accounts, Constants.PolicySignUpSignIn));

                //UpdateUserInfo(ar);

                var payload = new JObject();
                payload["access_token"] = ar.IdToken;

                await client.LoginAsync(
                    MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, payload);
            }
            catch (Exception ex)
            {
                // Checking the exception message
                // should ONLY be done for B2C
                // reset and not any other error.
                if (ex.Message.Contains("AADB2C90118"))
                    await OnPasswordReset();
                // Alert if any exception excludig user cancelling sign-in dialog
                else if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                    throw new Exception($"Exception: {ex.ToString()}");
            }
        }

        private async Task OnPasswordReset()
        {
            try
            {
                AuthenticationResult ar = await PCA.AcquireTokenAsync(Constants.Scopes, (IAccount)null, UIBehavior.NoPrompt, string.Empty, null, Constants.AuthorityPasswordReset);
                UpdateUserInfo(ar);
            }
            catch (Exception ex)
            {
                // Alert if any exception excludig user cancelling sign-in dialog
                if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                    throw new Exception($"Exception: {ex.ToString()}");
            }
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

        #region TOKEN

        private MobileServiceUser RetrieveTokenFromSecureStore()
        {
            try
            {
                // Check if the token is available within the password vault

                var credential = vault.FindAllByResource(provider).FirstOrDefault();
                if (credential != null)
                {
                    var token = vault.Retrieve(provider, credential.UserName).Password;
                    if (token != null && token.Length > 0)
                    {
                        return new MobileServiceUser(credential.UserName)
                        {
                            MobileServiceAuthenticationToken = token
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving existing token: {ex.Message}");
            }
            return null;
        }

        private void StoreTokenInSecureStore(MobileServiceUser user)
        {
            vault.Add(new PasswordCredential(provider, user.UserId, user.MobileServiceAuthenticationToken));
        }

        private bool IsTokenExpired(string token)
        {
            // Get just the JWT part of the token (without the signature).
            var jwt = token.Split(new Char[] { '.' })[1];

            // Undo the URL encoding.
            jwt = jwt.Replace('-', '+').Replace('_', '/');
            switch (jwt.Length % 4)
            {
                case 0: break;
                case 2: jwt += "=="; break;
                case 3: jwt += "="; break;
                default:
                    throw new ArgumentException("The token is not a valid Base64 string.");
            }

            // Convert to a JSON String
            var bytes = Convert.FromBase64String(jwt);
            string jsonString = UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            // Parse as JSON object and get the exp field value,
            // which is the expiration date as a JavaScript primative date.
            JObject jsonObj = JObject.Parse(jsonString);
            var exp = Convert.ToDouble(jsonObj["exp"].ToString());

            // Calculate the expiration by adding the exp value (in seconds) to the
            // base date of 1/1/1970.
            DateTime minTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var expire = minTime.AddSeconds(exp);
            return (expire < DateTime.UtcNow);
        }

        public void RemoveTokenFromSecureStore()
        {
            try
            {
                // Check if the token is available within the password vault
                var credential = vault.FindAllByResource(provider).FirstOrDefault();
                if (credential != null)
                {
                    vault.Remove(credential);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving existing token: {ex.Message}");
            }
        }

        #endregion TOKEN

        #endregion Methods
    }
}