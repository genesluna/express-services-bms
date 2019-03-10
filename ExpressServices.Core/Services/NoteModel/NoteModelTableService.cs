using GLunaLibrary.Helpers;
using ExpressServices.Core.Abstractions;
using ExpressServices.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public class NoteModelTableService
    {
        #region Properties

        private ICloudService CloudService = AzureCloudService.Instance;

        public ICloudTable<NoteModel> NoteModelTable;

        #endregion Properties

        #region Constructor

        public NoteModelTableService()
        {
            InitializeTables();
        }

        #endregion Constructor

        #region Methods

        private void InitializeTables()
        {
            NoteModelTable = CloudService.GetTable<NoteModel>();
        }

        public async Task<List<NoteModel>> GetAllNoteModelsByTypeAsync(string type)
        {
            List<NoteModel> allItems = new List<NoteModel>();

            const int pageSize = 50;
            var hasMore = true;

            while (hasMore)
            {
                var pageOfItems = await NoteModelTable.GetSyncTable()
                        .Where(x => x.Type.Contains(type))
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

        public async Task<List<NoteModel>> SearchNoteModelsByNameAsync(string name, int start = 0, int count = 50)
        {
            return await NoteModelTable.GetSyncTable()
                        .OrderBy(x => x.Name)
                        .Where(x => x.Name.Contains(name))
                        .Skip(start)
                        .Take(count)
                        .ToListAsync();
        }

        public async Task<List<NoteModel>> SearchNoteModelsByTypeAndNameAsync(string type, string name, int start = 0, int count = 50)
        {
            return await NoteModelTable.GetSyncTable()
                        .OrderBy(notemodel => notemodel.Name)
                        .Where(notemodel => notemodel.Name.Contains(name) && notemodel.Type.Contains(type))
                        .Skip(start)
                        .Take(count)
                        .ToListAsync();
        }

        #endregion Methods
    }
}