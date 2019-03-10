using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;
using Microsoft.Toolkit.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public class CustomerTableService : IIncrementalSource<Customer>
    {
        #region Properties

        private ICloudService CloudService = AzureCloudService.Instance;

        public ICloudTable<Customer> CustomerTable;

        #endregion Properties

        #region Constructor

        public CustomerTableService()
        {
            InitializeTables();
        }

        #endregion Constructor

        #region Methods

        private void InitializeTables()
        {
            CustomerTable = CloudService.GetTable<Customer>();
        }

        public async Task<List<Customer>> SearchCustomersByNameOrDocumentAsync(string queryText, int start = 0, int count = 50)
        {
            return await CustomerTable.GetSyncTable()
                .OrderBy(x => x.Name)
                .Where(x => x.Name.Contains(queryText) || x.DocNumber.StartsWith(queryText))
                .Skip(start)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CustomerTable.ReadItemsAsync(pageIndex * pageSize, pageSize);
        }

        #endregion Methods
    }
}