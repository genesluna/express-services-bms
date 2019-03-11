using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;
using System;

namespace ExpressServices.Core.Models
{
    public class Transaction : ModelBase
    {
        private string _operationType;
        private string _paymentMethod;
        private int _numberOfInstallments;
        private string _salesReceiptNumber;
        private string _creditCardReceiptNumber;
        private string _description;
        private decimal _amount;
        private string _accountId;
        private string _categoryId;
        private DateTimeOffset _dueTo;
        private string _transferCounterPartnerId;
        private int _saleIndexNumber = 1;
        private bool _isBalanceTransaction;

        [JsonProperty(PropertyName = "isbalancetransaction")]
        public bool IsBalanceTransaction { get => _isBalanceTransaction; set => Set(ref _isBalanceTransaction, value); }

        [JsonProperty(PropertyName = "operationtype")]
        public string OperationType { get => _operationType; set => Set(ref _operationType, value); }

        [JsonProperty(PropertyName = "paymentmethod")]
        public string PaymentMethod { get => _paymentMethod; set => Set(ref _paymentMethod, value); }

        [JsonProperty(PropertyName = "dueto")]
        public DateTimeOffset DueTo { get => _dueTo; set => Set(ref _dueTo, value); }

        [JsonProperty(PropertyName = "numberofinstallments")]
        public int NumberOfInstallments { get => _numberOfInstallments; set => Set(ref _numberOfInstallments, value); }

        [JsonProperty(PropertyName = "salesreceiptnumber")]
        public string SalesReceiptNumber { get => _salesReceiptNumber; set => Set(ref _salesReceiptNumber, value); }

        [JsonProperty(PropertyName = "creditcardreceiptnumber")]
        public string CreditCardReceiptNumber { get => _creditCardReceiptNumber; set => Set(ref _creditCardReceiptNumber, value); }

        [JsonProperty(PropertyName = "description")]
        public string Description { get => _description; set => Set(ref _description, value); }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get => _amount; set => Set(ref _amount, value); }

        [JsonProperty(PropertyName = "accountid")]
        public string AccountId { get => _accountId; set => Set(ref _accountId, value); }

        [JsonProperty(PropertyName = "categoryid")]
        public string CategoryId { get => _categoryId; set => Set(ref _categoryId, value); }

        [JsonProperty(PropertyName = "transfercounterpartnerid")]
        public string TransferCounterPartnerId { get => _transferCounterPartnerId; set => Set(ref _transferCounterPartnerId, value); }

        [JsonIgnore]
        public string CategoryName { get; set; }

        [JsonIgnore]
        public decimal Balance { get; set; }

        [JsonIgnore]
        public int SaleIndexNumber { get => _saleIndexNumber; set => Set(ref _saleIndexNumber, value); }

        [JsonIgnore]
        public DateTime Date { get => DateCreated.Date; }

        [JsonIgnore]
        public string FormatedAmount
        {
            get => $"{Amount:C}";
        }

        public object Clone()
        {
            return (Transaction)this.MemberwiseClone();
        }
    }
}