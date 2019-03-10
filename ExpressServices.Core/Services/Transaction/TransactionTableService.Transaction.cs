using ExpressServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public partial class TransactionTableService
    {
        public async Task<List<Transaction>> GetAllCashierTransactionsAsync()
        {
            List<Transaction> allItems = new List<Transaction>();

            const int pageSize = 50;

            var hasMore = true;

            var accountId = (await AccountTable.GetSyncTable().Where(x => x.Name == "Caixa")
                .ToListAsync())
                .FirstOrDefault().Id;

            while (hasMore)
            {
                var pageOfItems = await TransactionTable.GetSyncTable()
                    .Where(x => x.CompanyId.Contains(CurrentUser.Instance.CurrentCompanyId) && x.AccountId == accountId && x.IsBalanceTransaction == false)
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

        public async Task<Transaction> GetCounterPartnerTransactionAsync(Transaction transaction)
        {
            return (await TransactionTable.GetSyncTable().Where(x => x.Id == transaction.TransferCounterPartnerId)
                .ToListAsync())
                .FirstOrDefault();
        }

        public async Task<List<Transaction>> GetCurrentMonthCashierTransactionsAsync()
        {
            DateTime now = DateTime.UtcNow;

            var firstDayOfTheMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var accountId = (await AccountTable.GetSyncTable().Where(x => x.Name == "Caixa")
                .ToListAsync())
                .FirstOrDefault().Id;

            return await TransactionTable.GetSyncTable()
                .Where(x => x.CompanyId.Contains(CurrentUser.Instance.CurrentCompanyId)
                && x.AccountId == accountId
                && x.DateCreated >= firstDayOfTheMonth)
                .ToListAsync();
        }

        public async Task<List<Transaction>> SearchTransactionsByDescriptionAsync(string queryText)
        {
            return await TransactionTable.GetSyncTable().Where(x => x.Description.Contains(queryText)).ToListAsync();
        }
    }
}
