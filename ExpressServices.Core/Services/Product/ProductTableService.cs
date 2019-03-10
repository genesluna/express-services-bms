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
    public class ProductTableService
    {
        #region Properties

        private ICloudService CloudService = AzureCloudService.Instance;

        public ICloudTable<Product> ProductTable;

        #endregion Properties

        #region Constructor

        public ProductTableService()
        {
            InitializeTables();
        }

        #endregion Constructor

        #region Methods

        private void InitializeTables()
        {
            ProductTable = CloudService.GetTable<Product>();
        }

        public async Task<List<Product>> GetAllProductsInStockAsync()
        {
            List<Product> allItems = new List<Product>();

            const int pageSize = 50;
            var hasMore = true;

            while (hasMore)
            {
                var pageOfItems = await ProductTable.GetSyncTable()
                    .Where(x => x.Quantity > 0)
                    .OrderBy(x => x.Name)
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

        public async Task<List<Product>> SearchProductsByNameOrCodeAsync(string queryText, int start = 0, int count = 50)
        {
            return await ProductTable.GetSyncTable()
                .OrderBy(x => x.Name)
                .Where(x =>
                 x.Name.Contains(queryText)
                 || x.Code.Contains(queryText)
                 || x.BarCode.Contains(queryText))
                .Skip(start)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Product>> SearchProductsInStockByNameOrCodeAsync(string queryText, int start = 0, int count = 50)
        {
            return await ProductTable.GetSyncTable()
                .OrderBy(x => x.Name)
                .Where(x =>
                 (x.Name.Contains(queryText)
                 || x.Code.Contains(queryText)
                 || x.BarCode.Contains(queryText))
                 && x.Quantity > 0)
                .Skip(start)
                .Take(count)
                .ToListAsync();
        }

        #endregion Methods
    }
}
