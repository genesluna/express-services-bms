using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;

namespace ExpressServices.Core.Services
{
    public partial class CaseTableService
    {
        #region Properties

        private ICloudService CloudService = AzureCloudService.Instance;

        public ICloudTable<Case> CaseTable;
        public ICloudTable<CaseNoteModel> CaseNoteModelTable;
        public ICloudTable<CaseNote> CaseNoteTable;
        public ICloudTable<CaseProduct> CaseProductTable;
        public ICloudTable<CaseService> CaseServiceTable;
        private ICloudTable<NoteModel> NoteModelTable;
        private ICloudTable<User> UserTable;

        private CurrentUser CurrentUser = CurrentUser.Instance;

        #endregion Properties

        #region Constructor

        public CaseTableService()
        {
            InitializeTables();
        }

        #endregion Constructor

        #region Methods

        private void InitializeTables()
        {
            CaseProductTable = CloudService.GetTable<CaseProduct>();

            CaseServiceTable = CloudService.GetTable<CaseService>();

            CaseNoteTable = CloudService.GetTable<CaseNote>();

            CaseNoteModelTable = CloudService.GetTable<CaseNoteModel>();

            NoteModelTable = CloudService.GetTable<NoteModel>();

            CaseTable = CloudService.GetTable<Case>();

            UserTable = CloudService.GetTable<User>();
        }

        #endregion Methods
    }
}