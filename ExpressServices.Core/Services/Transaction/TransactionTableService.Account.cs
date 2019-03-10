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
        public async Task<Account> GetAccountByNameAsync(string name)
        {
            return (await AccountTable.GetSyncTable()
                .Where(x => x.Name == name)
                .ToListAsync())
                .FirstOrDefault();
        }
    }
}
