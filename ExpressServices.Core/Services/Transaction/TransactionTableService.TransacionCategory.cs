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
        public async Task<TransactionCategory> GetTransactionCategoryByNameAsync(string name)
        {
            return (await TransactionCategoryTable.GetSyncTable()
                .Where(x => x.Name == name)
                .ToListAsync())
                .FirstOrDefault();
        }
    }
}
