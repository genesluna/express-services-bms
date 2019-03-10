using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Diagnostics;
using Windows.Storage;

namespace ExpressServices.Core.Services
{
    public partial class AzureCloudService : ICloudService
    {
        private static readonly Lazy<AzureCloudService> lazy = new Lazy<AzureCloudService>(() => new AzureCloudService());

        public static AzureCloudService Instance { get { return lazy.Value; } }

        private MobileServiceClient Client { get; }

        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        private int NumberOfInstances = 0;

        public AzureCloudService()
        {
            switch (localSettings.Values["BackendEnvironment"].ToString())
            {
                case "Local":
                    {
                        Client = new MobileServiceClient(
                        Locations.AppServiceLocalUrl, new AuthenticationDelegatingHandler())
                        {
                            AlternateLoginHost = new Uri(Locations.AppServiceStageUrl)
                        };
                        break;
                    }

                case "Stage":
                    {
                        Client = new MobileServiceClient(Locations.AppServiceStageUrl, new AuthenticationDelegatingHandler());
                        break;
                    }

                default:
                    {
                        Client = new MobileServiceClient(Locations.AppServiceUrl, new AuthenticationDelegatingHandler());
                        break;
                    }
            }

            NumberOfInstances++;
            Debug.WriteLine($"Azure Cloud Service instanciated. Number of instances: {NumberOfInstances}");
        }

        public ICloudTable<T> GetTable<T>() where T : BaseModel => new AzureCloudTable<T>(Client);
    }
}