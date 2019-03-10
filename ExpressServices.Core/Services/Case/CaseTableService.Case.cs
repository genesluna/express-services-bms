using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public partial class CaseTableService
    {
        public async Task<List<Case>> SearchCasesByNumberOrCustomerDocAsync(string queryText)
        {
            return await CaseTable.GetSyncTable()
                .OrderByDescending(x => x.DateCreated)
                .Where(x => (x.CaseNumber.ToString().EndsWith(queryText) || x.CustomerDoc.StartsWith(queryText)) && x.CompanyId == CurrentUser.CurrentCompanyId)
                .ToListAsync();
        }

        public async Task<List<Case>> SearchCasesByCustomerNameAsync(string name)
        {
            return await CaseTable.GetSyncTable()
                .OrderByDescending(x => x.DateCreated)
                .Where(x => x.CustomerName.StartsWith(name) && x.CompanyId == CurrentUser.CurrentCompanyId).ToListAsync();
        }
    }
}