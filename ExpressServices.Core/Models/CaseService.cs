using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class CaseService : ModelBase
    {
        private string _caseId;
        private string _serviceId;
        private string _name;
        private int _quantity;
        private decimal _price;

        [JsonProperty(PropertyName = "caseid")]
        public string CaseId { get => _caseId; set => Set(ref _caseId, value); }

        [JsonProperty(PropertyName = "serviceid")]
        public string ServiceId { get => _serviceId; set => Set(ref _serviceId, value); }

        [JsonProperty(PropertyName = "name")]
        public string Name { get => _name; set => Set(ref _name, value); }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get => _quantity; set => Set(ref _quantity, value); }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get => _price; set => Set(ref _price, value); }
    }
}