using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class Sale : ModelBase
    {
        private string _customerId;
        private long _saleNumber;
        private string _type;
        private decimal _amount;
        private decimal _subtotal;
        private string _caseId;
        private decimal _discount;

        [JsonProperty(PropertyName = "customerid")]
        public string CustomerId { get => _customerId; set => Set(ref _customerId, value); }

        [JsonProperty(PropertyName = "salenumber")]
        public long SaleNumber { get => _saleNumber; set => Set(ref _saleNumber, value); }

        [JsonProperty(PropertyName = "type")]
        public string Type { get => _type; set => Set(ref _type, value); }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get => _amount; set => Set(ref _amount, value); }

        [JsonProperty(PropertyName = "subtotal")]
        public decimal SubTotal { get => _subtotal; set => Set(ref _subtotal, value); }

        [JsonProperty(PropertyName = "discount")]
        public decimal Discount { get => _discount; set => Set(ref _discount, value); }

        [JsonProperty(PropertyName = "caseid")]
        public string CaseId { get => _caseId; set => Set(ref _caseId, value); }

        [JsonIgnore]
        public decimal Balance { get; set; }
    }
}