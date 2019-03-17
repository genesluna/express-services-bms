using ExpressServices.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;

namespace ExpressServices.Core.Abstractions
{
    public interface ICloudService
    {
        Task<MobileServiceUser> AuthenticateAsync();

        Task<User> GetAuthenticatedUserAsync();

        ICloudTable<T> GetTable<T>() where T : ModelBase;

        Task InitializeAsync();

        Task LogoutAsync();

        Task SyncOfflineCacheAsync(IProgress<ProgressReportModel> progress);

        Task SyncOfflineCacheAsyncV2();

        Task SyncOffLineTableAsync<T>() where T : ModelBase;
    }
}