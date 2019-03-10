using ExpressServices.Core.Abstractions;
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
        #region Properties

        private ICloudService CloudService = AzureCloudService.Instance;

        public ICloudTable<Account> AccountTable;
        public ICloudTable<Transaction> TransactionTable;
        public ICloudTable<TransactionCategory> TransactionCategoryTable;

        #endregion Properties

        #region Constructor

        public TransactionTableService()
        {
            InitializeTables();
        }

        #endregion Constructor

        #region Methods

        private void InitializeTables()
        {
            AccountTable = CloudService.GetTable<Account>();
            TransactionTable = CloudService.GetTable<Transaction>();
            TransactionCategoryTable = CloudService.GetTable<TransactionCategory>();
        }

        #endregion Methods
    }
}
