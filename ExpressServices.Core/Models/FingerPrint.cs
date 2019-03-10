using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class FingerPrint : BaseModel
    {
        [JsonProperty(PropertyName = "ownerid")]
        public string OwnerId { get; set; }

        [JsonProperty(PropertyName = "image")]
        public byte[] Image { get; set; }

        [JsonProperty(PropertyName = "template")]
        public byte[] Template { get; set; }
    }
}