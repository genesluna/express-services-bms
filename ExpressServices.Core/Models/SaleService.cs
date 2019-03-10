using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class SaleService : BaseModel
    {
        private string _saleId;
        private string _serviceId;
        private string _name;
        private int _quantity;
        private decimal _price;

        [JsonProperty(PropertyName = "saleid")]
        public string SaleId { get => _saleId; set => Set(ref _saleId, value); }

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