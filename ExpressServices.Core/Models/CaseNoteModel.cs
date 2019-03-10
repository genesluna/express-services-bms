using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class CaseNoteModel : BaseModel
    {
        [JsonProperty(PropertyName = "caseid")]
        public string CaseId { get; set; }

        [JsonProperty(PropertyName = "notemodelid")]
        public string NoteModelId { get; set; }
    }
}