using ExpressServices.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public partial class CaseTableService
    {
        public async Task<List<NoteModel>> GetNoteModelsByCaseIdAsync(string queryText, string byType = "")
        {
            var caseNoteModels = await CaseNoteModelTable.GetSyncTable().ToCollectionAsync();

            var noteModels = await NoteModelTable.GetSyncTable().ToCollectionAsync();

            List<NoteModel> NoteModelList = new List<NoteModel>();

            var response = from notemodel in noteModels
                           let ca = (from c in caseNoteModels where c.CaseId == queryText select c.NoteModelId)
                           where ca.Contains(notemodel.Id)
                           select notemodel;

            if (byType != "")
            {
                var list = response.ToList<NoteModel>();

                list.ForEach(delegate (NoteModel notemodel)
                {
                    if (notemodel.Type == byType) NoteModelList.Add(notemodel);
                });

                return NoteModelList;
            }

            return response.ToList<NoteModel>();
        }

        public async Task<CaseNoteModel> GetCaseNoteModelByCaseIdAndNoteModelIdAsync(string caseId, string noteId)
        {
            return (await CaseNoteModelTable.GetSyncTable().Where(x => x.CaseId == caseId && x.Id == noteId).ToListAsync()).FirstOrDefault();
        }

        public async Task<List<NoteModel>> GetNoteModelsByCaseIdWithCaseNoteAsync(string queryText)
        {
            var caseNoteModels = await CaseNoteModelTable.GetSyncTable().ToCollectionAsync();

            var noteModels = await NoteModelTable.GetSyncTable().ToCollectionAsync();

            var caseNotes = await CaseNoteTable.GetSyncTable()
                .Where(casenote => casenote.Type == "Nota" && casenote.CaseId == queryText)
                .ToCollectionAsync();

            var response = from notemodel in noteModels
                           let ca = (from c in caseNoteModels where c.CaseId == queryText select c.NoteModelId)
                           where ca.Contains(notemodel.Id)
                           select notemodel;

            var history = response.ToList<NoteModel>().FindAll(x => x.Type == "Anotação");

            foreach (CaseNote value in caseNotes)
            {
                NoteModel note = new NoteModel
                {
                    Id = value.Id,
                    Name = value.Name,
                    Type = value.Type,
                    CreatedAt = value.DateCreated,
                    CompanyId = value.CompanyId,
                    CreatedBy = value.CreatedBy
                };

                history.Add(note);
            }

            return history.OrderByDescending(h => h.CreatedAt).ToList<NoteModel>();
        }
    }
}