using ExpressServices.Core.Helpers;
using ExpressServices.Core.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ExpressServices.Core.Credentials;

namespace ExpressServices.Core.Services
{
    public class B2CGraphService
    {
        private string ClientId { get; }
        private string ClientSecret { get; }
        private string Tenant { get; }

        private AuthenticationContext authContext;
        private ClientCredential credential;

        public B2CGraphService()
        {
            // The client_id, client_secret, and tenant are pulled in from the App.config file
            this.ClientId = Constants.aadGraphClientId;
            this.ClientSecret = ApiKeys.AadGraphClientSecret;
            this.Tenant = Constants.Tenant;

            // The AuthenticationContext is ADAL's primary class, in which you indicate the direcotry to use.
            this.authContext = new AuthenticationContext("https://login.microsoftonline.com/" + this.Tenant);

            // The ClientCredential is where you pass in your client_id and client_secret, which are
            // provided to Azure AD in order to receive an access_token using the app's identity.
            this.credential = new ClientCredential(this.ClientId, this.ClientSecret);
        }

        public async Task<GraphUser> GetUserByObjectId(string objectId)
        {
            var result = await SendGraphGetRequest("/users/" + objectId, null);

            var user = JsonConvert.DeserializeObject<GraphUser>(result);

            return user;
        }

        public async Task<List<GraphUser>> GetAllUsers(string query)
        {
            List<GraphUser> graphUsers = new List<GraphUser>();

            var result = await SendGraphGetRequest("/users", query);

            JObject deserializedResult = JsonConvert.DeserializeObject(result) as JObject;

            var deserializedUsers = deserializedResult.Last.First.Values<object>().ToList();
            foreach (var item in deserializedUsers)
            {
                graphUsers.Add(JsonConvert.DeserializeObject<GraphUser>(JsonConvert.SerializeObject(item)));
            }

            return graphUsers;
        }

        public async Task<string> CreateUser(string json)
        {
            return await SendGraphPostRequest("/users", json);
        }

        public async Task<string> UpdateUser(string objectId, string json)
        {
            return await SendGraphPatchRequest("/users/" + objectId, json);
        }

        public async Task<string> DeleteUser(string objectId)
        {
            return await SendGraphDeleteRequest("/users/" + objectId);
        }

        public async Task<string> RegisterExtension(string objectId, string body)
        {
            return await SendGraphPostRequest("/applications/" + objectId + "/extensionProperties", body);
        }

        public async Task<string> UnregisterExtension(string appObjectId, string extensionObjectId)
        {
            return await SendGraphDeleteRequest("/applications/" + appObjectId + "/extensionProperties/" + extensionObjectId);
        }

        public async Task<string> GetExtensions(string appObjectId)
        {
            return await SendGraphGetRequest("/applications/" + appObjectId + "/extensionProperties", null);
        }

        public async Task<string> GetApplications(string query)
        {
            return await SendGraphGetRequest("/applications", query);
        }

        private async Task<string> SendGraphDeleteRequest(string api)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Constants.aadGraphResourceId, credential);
            HttpClient http = new HttpClient();
            string url = Constants.aadGraphEndpoint + Tenant + api + "?" + Constants.aadGraphVersion;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage response = await http.SendAsync(request);

            Debug.WriteLine("DELETE " + url);
            Debug.WriteLine("Authorization: Bearer " + result.AccessToken.Substring(0, 80) + "...");
            Debug.WriteLine("");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            Debug.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            Debug.WriteLine("");

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> SendGraphPatchRequest(string api, string json)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Constants.aadGraphResourceId, credential);
            HttpClient http = new HttpClient();
            string url = Constants.aadGraphEndpoint + Tenant + api + "?" + Constants.aadGraphVersion;

            Debug.WriteLine("PATCH " + url);
            Debug.WriteLine("Authorization: Bearer " + result.AccessToken.Substring(0, 80) + "...");
            Debug.WriteLine("Content-Type: application/json");
            Debug.WriteLine("");
            Debug.WriteLine(json);
            Debug.WriteLine("");

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            Debug.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            Debug.WriteLine("");

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> SendGraphPostRequest(string api, string json)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Constants.aadGraphResourceId, credential);
            HttpClient http = new HttpClient();
            string url = Constants.aadGraphEndpoint + Tenant + api + "?" + Constants.aadGraphVersion;

            Debug.WriteLine("POST " + url);
            Debug.WriteLine("Authorization: Bearer " + result.AccessToken.Substring(0, 80) + "...");
            Debug.WriteLine("Content-Type: application/json");
            Debug.WriteLine("");
            Debug.WriteLine(json);
            Debug.WriteLine("");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            Debug.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            Debug.WriteLine("");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendGraphGetRequest(string api, string query)
        {
            // First, use ADAL to acquire a token using the app's identity (the credential)
            // The first parameter is the resource we want an access_token for; in this case, the Graph API.
            AuthenticationResult result = await authContext.AcquireTokenAsync("https://graph.windows.net", credential);

            // For B2C user managment, be sure to use the 1.6 Graph API version.
            HttpClient http = new HttpClient();
            string url = "https://graph.windows.net/" + Tenant + api + "?" + Constants.aadGraphVersion;
            if (!string.IsNullOrEmpty(query))
            {
                url += "&" + query;
            }

            Debug.WriteLine("GET " + url);
            Debug.WriteLine("Authorization: Bearer " + result.AccessToken.Substring(0, 80) + "...");
            Debug.WriteLine("");

            // Append the access token for the Graph API to the Authorization header of the request, using the Bearer scheme.
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            Debug.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            Debug.WriteLine("");

            return await response.Content.ReadAsStringAsync();
        }
    }
}