using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class Account : ModelBase
    {
        private string _name;
        private decimal _openingBalance;

        [JsonProperty(PropertyName = "name")]
        public string Name { get => _name; set => Set(ref _name, value); }

        [JsonProperty(PropertyName = "openingbalance")]
        public decimal OpeningBalance { get => _openingBalance; set => Set(ref _openingBalance, value); }
    }
}