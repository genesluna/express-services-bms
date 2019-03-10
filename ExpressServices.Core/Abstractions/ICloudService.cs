using ExpressServices.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace ExpressServices.Core.Abstractions
{
    public interface ICloudService
    {
        Task<MobileServiceUser> AuthenticateAsync();

        Task<User> GetAuthenticatedUserAsync();

        ICloudTable<T> GetTable<T>() where T : BaseModel;

        Task InitializeAsync();

        Task LogoutAsync();

        Task SyncOfflineCacheAsync();

        Task SyncOffLineTableAsync<T>() where T : BaseModel;
    }
}