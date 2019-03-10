using ExpressServices.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public partial class CaseTableService
    {
        public async Task<CaseNote> GetCaseNoteByCaseIdAndCaseNoteIdAsync(string caseId, string noteId)
        {
            return (await CaseNoteTable.GetSyncTable().Where(x => x.CaseId == caseId && x.Id == noteId).ToListAsync()).FirstOrDefault();
        }
    }
}