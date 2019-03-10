using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class SaleTransaction : BaseModel
    {
        private string _saleId;
        private string _transactionId;

        [JsonProperty(PropertyName = "saleid")]
        public string SaleId { get => _saleId; set => Set(ref _saleId, value); }

        [JsonProperty(PropertyName = "transactionid")]
        public string TransactionId { get => _transactionId; set => Set(ref _transactionId, value); }
    }
}