using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class CaseProduct : ModelBase
    {
        private string _caseId;
        private string _productId;
        private string _name;
        private decimal _quantity;
        private decimal _price;

        [JsonProperty(PropertyName = "caseid")]
        public string CaseId { get => _caseId; set => Set(ref _caseId, value); }

        [JsonProperty(PropertyName = "productid")]
        public string ProductId { get => _productId; set => Set(ref _productId, value); }

        [JsonProperty(PropertyName = "name")]
        public string Name { get => _name; set => Set(ref _name, value); }

        [JsonProperty(PropertyName = "quantity")]
        public decimal Quantity { get => _quantity; set => Set(ref _quantity, value); }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get => _price; set => Set(ref _price, value); }
    }
}