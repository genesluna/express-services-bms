using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public partial class SaleTableService
    {
        #region Properties

        private ICloudService CloudService = AzureCloudService.Instance;

        public ICloudTable<Sale> SaleTable;
        public ICloudTable<SaleProduct> SaleProductTable;
        public ICloudTable<SaleService> SaleServiceTable;
        public ICloudTable<SaleTransaction> SaleTransactionTable;

        #endregion Properties

        #region Constructor

        public SaleTableService()
        {
            InitializeTables();
        }

        #endregion Constructor

        #region Methods

        private void InitializeTables()
        {
            SaleTable = CloudService.GetTable<Sale>();

            SaleProductTable = CloudService.GetTable<SaleProduct>();

            SaleTransactionTable = CloudService.GetTable<SaleTransaction>();

            SaleServiceTable = CloudService.GetTable<SaleService>();
        }

        #endregion Methods
    }
}
