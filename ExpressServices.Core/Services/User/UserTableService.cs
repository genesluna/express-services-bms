using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public class UserTableService
    {
        #region Properties

        private ICloudService CloudService = AzureCloudService.Instance;

        public ICloudTable<User> UserTable;

        #endregion Properties

        #region Constructor

        public UserTableService()
        {
            InitializeTables();
        }

        #endregion Constructor

        #region Methods

        private void InitializeTables()
        {
            UserTable = CloudService.GetTable<User>();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return (await UserTable.GetSyncTable().Where(x => x.Email == email).ToListAsync()).FirstOrDefault();
        }

        public async Task<List<User>> SearchUsersByNameOrDocumentAsync(string queryText, int start = 0, int count = 50)
        {
            return await UserTable.GetSyncTable()
                .OrderBy(x => x.Name)
                .Where(x => x.Name.Contains(queryText) || x.DocNumber.StartsWith(queryText))
                .Skip(start)
                .Take(count)
                .ToListAsync();
        }

        #endregion Methods
    }
}