using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class Service : ModelBase
    {
        private string _name;
        private string _description;
        private decimal _price;
        private int _saleQuantity = 1;
        private int _saleIndexNumber = 1;

        [JsonProperty(PropertyName = "name")]
        public string Name { get => _name; set => Set(ref _name, value); }

        [JsonProperty(PropertyName = "description")]
        public string Description { get => _description; set => Set(ref _description, value); }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get => _price; set => Set(ref _price, value); }

        [JsonIgnore]
        public int SaleQuantity { get => _saleQuantity; set => Set(ref _saleQuantity, value); }

        [JsonIgnore]
        public int SaleIndexNumber { get => _saleIndexNumber; set => Set(ref _saleIndexNumber, value); }

        [JsonIgnore]
        public string TotalValue
        {
            get => $"{GetTotalValue():C}";
        }

        [JsonIgnore]
        public string SaleQuantityFormated
        {
            get => $"{SaleQuantity} x {Price:C}";
        }

        public decimal GetTotalValue()
        {
            return SaleQuantity * Price;
        }

        public object Clone()
        {
            return (Service)this.MemberwiseClone();
        }
    }
}