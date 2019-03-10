using ExpressServices.Core.Helpers;
using ExpressServices.Core.Models;
using Microsoft.Identity.Client;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ExpressServices.Core.Services
{
    public partial class AzureCloudService
    {
        public async Task<MobileServiceUser> AuthenticateAsync()
        {
            Client.CurrentUser = RetrieveTokenFromSecureStore();

            if (Client.CurrentUser != null && !IsTokenExpired(Client.CurrentUser.MobileServiceAuthenticationToken))
            {
                // User has previously been authenticated, no refresh is required
                return Client.CurrentUser;
            }

            if (Client.CurrentUser != null)
            {
                // User has previously been authenticated - try to Refresh the token
                try
                {
                    var refreshed = await Client.RefreshUserAsync();
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
            // so we make sure the current user is null
            // and go after the credentials
            Client.CurrentUser = null;

        TryAgain:

            try
            {
                // Client Login Flow Azure AD B2C
                await LoginADB2CAsync();

                // If we were able to successfully log in
                if (Client.CurrentUser != null)
                {
                    // Store the token for future use
                    StoreTokenInSecureStore(Client.CurrentUser);
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

            return Client.CurrentUser;
        }

        public async Task LogoutAsync()
        {
            // Remove the token from the cache
            RemoveTokenFromSecureStore();

            // Remove the accounts in the PCA (Azure AD B2C related)
            IEnumerable<IAccount> accounts = await PCA.GetAccountsAsync();

            foreach (var item in accounts)
            {
                await PCA.RemoveAsync(item);
            }

            // Remove Current user from local settings
            CurrentUser.Instance.ResetUser();

            if (Client.CurrentUser == null || Client.CurrentUser.MobileServiceAuthenticationToken == null)
            {
                return;
            }

            // Invalidate the token on the backend
            if (Network.IsConnected)
            {
                var authUri = new Uri($"{Client.MobileAppUri}/.auth/logout");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", Client.CurrentUser.MobileServiceAuthenticationToken);
                    HttpResponseMessage response = await httpClient.GetAsync(authUri);
                    Debug.WriteLine($"Logging off\nSuccess: {response.IsSuccessStatusCode}\nCode: {(int)response.StatusCode}\nMessage: {response.ReasonPhrase}");
                }
            }

            // Remove the token from the MobileServiceClient
            await Client.LogoutAsync();
        }

        public async Task<User> GetAuthenticatedUserAsync()
        {
            return await GetTable<User>().ReadItemAsync(CurrentUser.Instance.Id);
        }
    }
}