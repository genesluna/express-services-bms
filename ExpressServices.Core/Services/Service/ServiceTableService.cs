using GLunaLibrary.Helpers;
using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public class ServiceTableService
    {
        #region Properties

        private ICloudService CloudService = AzureCloudService.Instance;

        public ICloudTable<Service> ServiceTable;

        #endregion Properties

        #region Constructor

        public ServiceTableService()
        {
            InitializeTables();
        }

        #endregion Constructor

        #region Methods

        private void InitializeTables()
        {
            ServiceTable = CloudService.GetTable<Service>();
        }

        public async Task<List<Service>> SearchServicesByNameAsync(string queryText, int start = 0, int count = 50)
        {
            return await ServiceTable.GetSyncTable()
                .OrderBy(x => x.Name)
                .Where(x => x.Name.Contains(queryText))
                .Skip(start)
                .Take(count)
                .ToListAsync();
        }

        #endregion Methods
    }
}
