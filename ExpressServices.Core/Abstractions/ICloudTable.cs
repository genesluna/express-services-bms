using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressServices.Core.Abstractions
{
    public interface ICloudTable<T> where T : ModelBase
    {
        bool ShouldAutoSync { get; set; }

        IMobileServiceSyncTable<T> GetSyncTable();

        IMobileServiceTable<T> GetCloudTable();

        Task<T> CreateItemAsync(T item);

        Task<T> ReadItemAsync(string id);

        Task<T> ReadItemByCompanyIdAsync(string id);

        Task<T> UpdateItemAsync(T item);

        Task<T> UpsertItemAsync(T item);

        Task DeleteItemAsync(T item);

        Task<ICollection<T>> ReadAllItemsByCompanyIdAsync();

        Task<ICollection<T>> ReadAllItemsAsync();

        Task<ICollection<T>> ReadItemsByCompanyIdAsync(int start = 0, int count = 50);

        Task<ICollection<T>> ReadItemsAsync(int start = 0, int count = 50);

        Task PullAsync();

        Task PushAsync();

        Task SyncAsync();
    }
}