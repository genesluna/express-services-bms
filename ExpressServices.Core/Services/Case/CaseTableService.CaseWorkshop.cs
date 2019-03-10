using ExpressServices.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public partial class CaseTableService
    {
        public async Task<ICollection<Case>> GetAllWorkShopRelatedCasesAsync()
        {
            List<Case> allItems = new List<Case>();

            const int pageSize = 50;
            bool hasMore = true;

            while (hasMore)
            {
                var pageOfItems = await CaseTable.GetSyncTable()
                    .OrderByDescending(x => x.DateCreated)
                    .Where(x => x.CompanyId.Contains(CurrentUser.CurrentCompanyId)
                        && x.Status != "Encerrada"
                        && x.Status != "Cancelada")
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

        public async Task<List<Case>> SearchWorkShopRelatedCasesByCaseNumberAsync(int caseNumber)
        {
            List<Case> allItems = new List<Case>();

            const int pageSize = 50;
            var hasMore = true;

            while (hasMore)
            {
                var pageOfItems = await CaseTable.GetSyncTable()
                    .OrderByDescending(x => x.DateCreated)
                    .Where(x => x.CompanyId.Contains(CurrentUser.CurrentCompanyId)
                        && x.Status != "Encerrada"
                        && x.Status != "Cancelada"
                        && x.CaseNumber.ToString().Contains(caseNumber.ToString()))
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
    }
}