using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class TransactionCategory : BaseModel
    {
        private string _name;

        [JsonProperty(PropertyName = "name")]
        public string Name { get => _name; set => Set(ref _name, value); }
    }
}