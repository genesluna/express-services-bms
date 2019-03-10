using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public class CompanyTableService
    {
        #region Properties

        private ICloudService CloudService = AzureCloudService.Instance;

        public ICloudTable<Company> CompanyTable;

        #endregion Properties

        #region Constructor

        public CompanyTableService()
        {
            InitializeTables();
        }

        #endregion Constructor

        #region Methods

        private void InitializeTables()
        {
            CompanyTable = CloudService.GetTable<Company>();
        }

        public async Task<List<Company>> SearchCompanysByNameOrDocumentAsync(string queryText, int start = 0, int count = 50)
        {
            return await CompanyTable.GetSyncTable()
                .OrderBy(x => x.CompanyName)
                .Where(x => x.CompanyName.Contains(queryText) || x.DocNumber.StartsWith(queryText))
                .Skip(start)
                .Take(count)
                .ToListAsync();
        }

        #endregion Methods
    }
}