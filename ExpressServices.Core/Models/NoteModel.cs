using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class NoteModel : BaseModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}