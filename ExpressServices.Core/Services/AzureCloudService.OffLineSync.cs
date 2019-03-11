using ExpressServices.Core.Helpers;
using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ExpressServices.Core.Services
{
    public partial class AzureCloudService
    {
        public async Task InitializeAsync()
        {
            // Short circuit - local database is already initialized
            if (Client.SyncContext.IsInitialized)
                return;

            // Create a reference to the local sqlite store
            MobileServiceSQLiteStore store;

            switch (localSettings.Values["BackendEnvironment"].ToString())
            {
                case "Local":
                    {
                        store = new MobileServiceSQLiteStore("localstore-debug.db");
                        break;
                    }

                case "Stage":
                    {
                        store = new MobileServiceSQLiteStore("localstore-stage.db");
                        break;
                    }

                default:
                    {
                        store = new MobileServiceSQLiteStore("localstore.db");
                        break;
                    }
            }

            // Define the database schema
            store.DefineTable<Customer>();
            store.DefineTable<Service>();
            store.DefineTable<Case>();
            store.DefineTable<CaseService>();
            store.DefineTable<CaseProduct>();
            store.DefineTable<CaseNote>();
            store.DefineTable<CaseNoteModel>();
            store.DefineTable<NoteModel>();
            store.DefineTable<Company>();
            store.DefineTable<User>();
            store.DefineTable<Supplier>();
            store.DefineTable<Product>();
            store.DefineTable<Account>();
            store.DefineTable<TransactionCategory>();
            store.DefineTable<Transaction>();
            store.DefineTable<Sale>();
            store.DefineTable<SaleProduct>();
            store.DefineTable<SaleService>();
            store.DefineTable<SaleTransaction>();
            //store.DefineTable<FingerPrint>();

            // Actually create the store and update the schema
            await Client.SyncContext.InitializeAsync(store);
            Debug.WriteLine("Initialized Sync Context");
        }

        public async Task SyncOfflineCacheAsync(IProgress<ProgressReportModel> progress)
        {
            await InitializeAsync();

            // If we're online, then sync
            if (Network.IsConnected)
            {
                ProgressReportModel report = new ProgressReportModel();

                // Push the Operations Queue to the mobile backend
                try
                {
                    var tableList = new List<object>();

                    tableList.Add(GetTable<Customer>());
                    tableList.Add(GetTable<Service>());
                    tableList.Add(GetTable<Case>());
                    tableList.Add(GetTable<CaseService>());
                    tableList.Add(GetTable<CaseProduct>());
                    tableList.Add(GetTable<CaseNote>());
                    tableList.Add(GetTable<CaseNoteModel>());
                    tableList.Add(GetTable<NoteModel>());
                    tableList.Add(GetTable<Company>());
                    tableList.Add(GetTable<User>());
                    tableList.Add(GetTable<Product>());
                    tableList.Add(GetTable<Supplier>());
                    tableList.Add(GetTable<Account>());
                    tableList.Add(GetTable<TransactionCategory>());
                    tableList.Add(GetTable<Transaction>());
                    tableList.Add(GetTable<Sale>());
                    tableList.Add(GetTable<SaleProduct>());
                    tableList.Add(GetTable<SaleService>());
                    tableList.Add(GetTable<SaleTransaction>());
                    //tableList.Add(GetTable<FingerPrint>());

                    // Pull each sync table
                    await Task.Run(async () =>
                    {
                        await Client.SyncContext.PushAsync();

                        int i = 0;

                        Parallel.ForEach<dynamic>(tableList, async (table) =>
                        {
                            await table.PullAsync();
                            ++i;
                            report.PercentageComplete = (i * 100) / tableList.Count;
                            report.CurrentTask = table.ToString();
                            progress.Report(report);
                        });
                    });
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
                        Debug.WriteLine($"Erro (Full Sync) ao sincronizar as tabelas. Mensagem: { ex.Message}");
                        throw new Exception($"Erro (Full Sync) ao sincronizar. \n\nMensagem: { ex.Message } {ex.InnerException?.Message}", ex);
                    }
                }
            }
        }

        public async Task SyncOfflineCacheAsyncV2()
        {
            await InitializeAsync();

            // If we're online, then sync
            if (Network.IsConnected)
            {
                List<Task> tasks = new List<Task>();

                // Push the Operations Queue to the mobile backend
                try
                {
                    await Client.SyncContext.PushAsync();

                    tasks.Add(GetTable<Customer>().PullAsync());
                    tasks.Add(GetTable<Service>().PullAsync());
                    tasks.Add(GetTable<Case>().PullAsync());
                    tasks.Add(GetTable<CaseService>().PullAsync());
                    tasks.Add(GetTable<CaseProduct>().PullAsync());
                    tasks.Add(GetTable<CaseNote>().PullAsync());
                    tasks.Add(GetTable<CaseNoteModel>().PullAsync());
                    tasks.Add(GetTable<NoteModel>().PullAsync());
                    tasks.Add(GetTable<Company>().PullAsync());
                    tasks.Add(GetTable<User>().PullAsync());
                    tasks.Add(GetTable<Product>().PullAsync());
                    tasks.Add(GetTable<Supplier>().PullAsync());
                    tasks.Add(GetTable<Account>().PullAsync());
                    tasks.Add(GetTable<TransactionCategory>().PullAsync());
                    tasks.Add(GetTable<Transaction>().PullAsync());
                    tasks.Add(GetTable<Sale>().PullAsync());
                    tasks.Add(GetTable<SaleProduct>().PullAsync());
                    tasks.Add(GetTable<SaleService>().PullAsync());
                    tasks.Add(GetTable<SaleTransaction>().PullAsync());
                    //tasks.Add(GetTable<FingerPrint>().PullAsync());

                    // Pull each sync table
                    await Task.WhenAll(tasks);
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
                        Debug.WriteLine($"Erro (Full Sync) ao sincronizar as tabelas. Mensagem: { ex.Message}");
                        throw new Exception($"Erro (Full Sync) ao sincronizar. \n\nMensagem: { ex.Message } {ex.InnerException?.Message}", ex);
                    }
                }
            }
        }

        public async Task SyncOffLineTableAsync<T>() where T : ModelBase
        {
            await InitializeAsync();

            // If we're online, then sync
            if (Network.IsConnected)
            {
                try
                {
                    // Push the Operations Queue to the mobile backend
                    await Client.SyncContext.PushAsync();

                    // Pull sync table
                    var table = GetTable<T>(); await table.PullAsync();
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
                        Debug.WriteLine($"Erro (Push) ao sincronizar a tabela {typeof(T).Name}. Mensagem: { ex.Message}");
                        throw new Exception($"Erro (Push) ao sincronizar. \n\nTabela: {typeof(T).Name}. \n\nMensagem: { ex.Message }", ex);
                    }
                }
            }
        }

        private async Task ResolveConflictAsync(MobileServiceTableOperationError error)
        {
            if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
            {
                //Update failed, reverting to server's copy.
                await error.CancelAndUpdateItemAsync(error.Result).ConfigureAwait(false);
            }
            else
            {
                // Discard local change.
                await error.CancelAndDiscardItemAsync().ConfigureAwait(false);
            }

            await new MessageDialog("\nFaça um printscreen ou tire uma foto e envie ao suporte." +
                                    $"\n\nId: {error.Item["id"]}" +
                                    $"\nOperação: {error.OperationKind}" +
                                    $"\nTabela: {error.TableName}" +
                                    $"\n\n{error.Result.First}",
                                    "🔂 Erro de sincronização! Operação descartada.").ShowAsync();
        }
    }
}