using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Windows.Security.Credentials;

namespace ExpressServices.Core.Services
{
    public partial class AzureCloudService
    {
        private PasswordVault vault = new PasswordVault();

        private string provider = MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory.ToString();

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

        private void RemoveTokenFromSecureStore()
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
    }
}