using ExpressServices.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public partial class CaseTableService
    {
        public async Task<List<HistoryModel>> GetHistoryByCaseIdAsync(string caseId)
        {
            List<HistoryModel> hm = new List<HistoryModel>();

            var caseNoteModels = await CaseNoteModelTable.GetSyncTable().Where(x => x.CaseId == caseId).ToListAsync();

            var caseNotes = await CaseNoteTable.GetSyncTable().Where(x => x.CaseId == caseId).ToListAsync();
            var noteModels = await NoteModelTable.ReadAllItemsAsync();

            var response = from notemodel in noteModels
                           let ca = (from c in caseNoteModels where c.CaseId == caseId select c.NoteModelId)
                           where ca.Contains(notemodel.Id)
                           select notemodel;

            var history = response.ToList<NoteModel>();

            foreach (NoteModel value in history)
            {
                var note = new HistoryModel
                {
                    Name = value.Name,
                    Type = value.Type,
                    CompanyId = value.CompanyId
                };

                var caseNoteModel = caseNoteModels.First(x => x.CaseId == caseId && x.NoteModelId == value.Id);
                note.CreatedAt = caseNoteModel.DateCreated;

                var user = await UserTable.GetSyncTable().LookupAsync(caseNoteModel.CreatedBy);
                note.CreatedBy = "Por " + user.Name;

                switch (value.Type)
                {
                    case "Diagnóstico":
                        note.ImgSource = "ms-appx:///Assets/Icons/diagnostic.png";
                        break;

                    case "Procedimento":
                        note.ImgSource = "ms-appx:///Assets/Icons/procedure.png";
                        break;

                    case "Anotação":
                        note.ImgSource = "ms-appx:///Assets/Icons/note.png";
                        break;
                }

                hm.Add(note);
            }

            foreach (CaseNote value in caseNotes)
            {
                var note = new HistoryModel
                {
                    Name = value.Name,
                    Type = value.Type,
                    CreatedAt = value.DateCreated,
                    CompanyId = value.CompanyId
                };

                var user = await UserTable.GetSyncTable().LookupAsync(value.CreatedBy);
                note.CreatedBy = "Por " + user.Name;

                switch (value.Type)
                {
                    case "Nota":
                        note.ImgSource = "ms-appx:///Assets/Icons/note.png";
                        break;

                    case "Informação":
                        note.ImgSource = "ms-appx:///Assets/Icons/info.png";
                        break;

                    case "Pagamento":
                        note.ImgSource = "ms-appx:///Assets/Icons/payment.png";
                        break;
                }

                hm.Add(note);
            }

            return hm.OrderByDescending(h => h.CreatedAt).ToList<HistoryModel>();
        }
    }
}