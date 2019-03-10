using ExpressServices.Core.Helpers;
using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ExpressServices.Core.Services
{
    public class AzureCloudTable<T> : ICloudTable<T> where T : BaseModel
    {
        private MobileServiceClient _client;

        private IMobileServiceSyncTable<T> table;

        private IMobileServiceTable<T> cloudTable;

        private CurrentUser currentUser = CurrentUser.Instance;

        #region Constructor

        public AzureCloudTable(MobileServiceClient client)
        {
            _client = client;
            table = client.GetSyncTable<T>();
            cloudTable = client.GetTable<T>();
        }

        #endregion Constructor

        #region ICloudTable interface

        public bool ShouldAutoSync { get; set; } = true;

        public async Task<T> CreateItemAsync(T item)
        {
            item.CompanyId = currentUser.CurrentCompanyId;
            item.CreatedBy = currentUser.Id;
            item.DateCreated = DateTime.UtcNow;

            await table.InsertAsync(item);

            await SyncAsync();

            return item;
        }

        public async Task DeleteItemAsync(T item)
        {
            await table.DeleteAsync(item);

            await SyncAsync();
        }

        public IMobileServiceSyncTable<T> GetSyncTable()
        {
            return table;
        }

        public IMobileServiceTable<T> GetCloudTable()
        {
            return cloudTable;
        }

        public async Task<ICollection<T>> ReadAllItemsAsync()
        {
            List<T> allItems = new List<T>();

            var pageSize = 50;
            var hasMore = true;
            while (hasMore)
            {
                var pageOfItems = await table.OrderByDescending(x => x.DateCreated)
                        .Skip(allItems.Count)
                        .Take(pageSize)
                        .ToListAsync();

                if (pageOfItems.Count > 0)
                {
                    allItems.AddRange(pageOfItems);
                }
                else
                {
                    hasMore = false;
                }
            }

            return allItems;
        }

        public async Task<ICollection<T>> ReadAllItemsByCompanyIdAsync()
        {
            List<T> allItems = new List<T>();

            var pageSize = 50;
            var hasMore = true;
            while (hasMore)
            {
                var pageOfItems = await table.OrderByDescending(x => x.DateCreated)
                        .Where(x => x.CompanyId.Contains(currentUser.CurrentCompanyId))
                        .Skip(allItems.Count)
                        .Take(pageSize)
                        .ToListAsync();

                if (pageOfItems.Count > 0)
                {
                    allItems.AddRange(pageOfItems);
                }
                else
                {
                    hasMore = false;
                }
            }

            return allItems;
        }

        public async Task<ICollection<T>> ReadItemsAsync(int start = 0, int count = 50)
        {
            return await table.OrderByDescending(x => x.DateCreated).Skip(start).Take(count).ToListAsync();
        }

        public async Task<T> ReadItemAsync(string id)
            => await table.LookupAsync(id);

        public async Task<T> ReadItemByCompanyIdAsync(string id)
            => (await table.Where(x => x.Id.Contains(id) && x.CompanyId.Contains(currentUser.CurrentCompanyId)).ToListAsync())[0];

        public async Task<ICollection<T>> ReadItemsByCompanyIdAsync(int start = 0, int count = 50)
        {
            return await table.OrderByDescending(x => x.DateCreated).Where(x => x.CompanyId.Contains(currentUser.CurrentCompanyId)).Skip(start).Take(count).ToListAsync();
        }

        public async Task SyncAsync()
        {
            if (ShouldAutoSync)
            {
                await PushAsync();
                await PullAsync();
            }
        }

        public async Task PullAsync()
        {
            // If we're online, then sync
            if (Network.IsConnected)
            {
                try
                {
                    Debug.WriteLine($"{typeof(T).Name} Sync started");
                    string queryName = $"incsync_{typeof(T).Name}";
                    await table.PullAsync(queryName, table.CreateQuery());
                    Debug.WriteLine($"{typeof(T).Name} Sync ended");
                }
                catch (MobileServiceInvalidOperationException ex)
                {
                    Debug.WriteLine($"Erro(Pull) ao sincronizar a tabela {typeof(T).Name}. Mensagem: { ex.Message}");
                    throw new Exception($"Erro (Pull) ao sincronizar. \n\nTabela: {typeof(T).Name}. \n\nMensagem: { ex.Message }", ex);
                }
            }

        }

        public async Task PushAsync()
        {
            if (Network.IsConnected)
            {
                try
                {
                    // Push the Operations Queue to the mobile backend
                    await _client.SyncContext.PushAsync();
                }
                catch (MobileServicePushFailedException ex)
                {
                    if (ex.PushResult != null)
                    {
                        foreach (var error in ex.PushResult.Errors)
                        {
                            await ResolveConflictAsync(error);
                        }
                    }
                    if (ex.PushResult.Errors.Count < 1)
                    {
                        //Singleton<ToastNotificationsService>.Instance.ShowErrorToast("Não foi possível sincronizar. Não utilize mais o sistema. Entre em contato imediatamente com o suporte.", "Erro de sincronização. Ligar para o Genes imediatamente!");
                        Debug.WriteLine($"Erro (Push) ao sincronizar a tabela {typeof(T).Name}. Mensagem: { ex.Message }");
                        throw new Exception($"Erro (Push) ao sincronizar. \n\nTabela: {typeof(T).Name}. \n\nMensagem: { ex.Message }", ex);
                    }
                }
            }
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            item.UpdatedBy = currentUser.Id;

            await table.UpdateAsync(item);

            await SyncAsync();

            return item;
        }

        public async Task<T> UpsertItemAsync(T item)
        {
            return (item.Id == null) ?
                await CreateItemAsync(item) :
                await UpdateItemAsync(item);
        }

        #endregion ICloudTable interface

        private async Task ResolveConflictAsync(MobileServiceTableOperationError error)
        {
            if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
            {
                //Update failed, reverting to server's copy.
                await error.CancelAndUpdateItemAsync(error.Result);
            }
            else
            {
                // Discard local change.
                await error.CancelAndDiscardItemAsync();
            }
            Debug.WriteLine($"Não foi possível sincronizar. Operação {error.TableName} ({error.Item["id"]}) descartada.");
            await new MessageDialog("\nFaça um printscreen ou tire uma foto e envie ao suporte." +
                                    $"\n\nId: {error.Item["id"]}" +
                                    $"\nOperação: {error.OperationKind}" +
                                    $"\nTabela: {error.TableName}" +
                                    $"\n\n{error.Result.First}",
                                    "🔂 Erro de sincronização! Operação descartada.").ShowAsync();
        }
    }
}