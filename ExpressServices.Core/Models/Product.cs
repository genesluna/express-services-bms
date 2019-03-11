using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class Product : ModelBase
    {
        #region Private Properties

        private bool _isKit;
        private string _name;
        private string _code;
        private string _brand;
        private string _unit;
        private string _barCode;
        private string _nCM;
        private string _grossWeight;
        private string _netWeight;
        private decimal _minimumStock;
        private decimal _quantity;
        private decimal _cost;
        private decimal _price;
        private string _size;
        private string _origin;
        private string _classification;
        private string _status;
        private string _category;
        private string _observations;
        private string _iCMS;
        private string _iPI;
        private string _pIS;
        private string _cOFINS;
        private string _taxableUnit;
        private string _cEST;
        private string _supplierId;
        private decimal _saleQuantity = 1;
        private int _saleIndexNumber = 1;
        private bool _isSaleCancelItem;
        private bool _isSaleProductCanceled;
        private bool _isSaleDiscountItem;
        private bool _isSaleService;

        #endregion Private Properties

        [JsonProperty(PropertyName = "iskit")]
        public bool IsKit { get => _isKit; set => Set(ref _isKit, value); }

        [JsonProperty(PropertyName = "name")]
        public string Name { get => _name; set => Set(ref _name, value); }

        [JsonProperty(PropertyName = "code")]
        public string Code { get => _code; set => Set(ref _code, value); }

        [JsonProperty(PropertyName = "brand")]
        public string Brand { get => _brand; set => Set(ref _brand, value); }

        [JsonProperty(PropertyName = "unit")]
        public string Unit { get => _unit; set => Set(ref _unit, value); }

        [JsonProperty(PropertyName = "barcode")]
        public string BarCode { get => _barCode; set => Set(ref _barCode, value); }

        [JsonProperty(PropertyName = "ncm")]
        public string NCM { get => _nCM; set => Set(ref _nCM, value); }

        [JsonProperty(PropertyName = "grossweight")]
        public string GrossWeight { get => _grossWeight; set => Set(ref _grossWeight, value); }

        [JsonProperty(PropertyName = "netweight")]
        public string NetWeight { get => _netWeight; set => Set(ref _netWeight, value); }

        [JsonProperty(PropertyName = "minimumstock")]
        public decimal MinimumStock { get => _minimumStock; set => Set(ref _minimumStock, value); }

        [JsonProperty(PropertyName = "quantity")]
        public decimal Quantity { get => _quantity; set => Set(ref _quantity, value); }

        [JsonProperty(PropertyName = "cost")]
        public decimal Cost { get => _cost; set => Set(ref _cost, value); }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get => _price; set => Set(ref _price, value); }

        [JsonProperty(PropertyName = "size")]
        public string Size { get => _size; set => Set(ref _size, value); }

        [JsonProperty(PropertyName = "origin")]
        public string Origin { get => _origin; set => Set(ref _origin, value); }

        [JsonProperty(PropertyName = "classification")]
        public string Classification { get => _classification; set => Set(ref _classification, value); }

        [JsonProperty(PropertyName = "status")]
        public string Status { get => _status; set => Set(ref _status, value); }

        [JsonProperty(PropertyName = "category")]
        public string Category { get => _category; set => Set(ref _category, value); }

        [JsonProperty(PropertyName = "observations")]
        public string Observations { get => _observations; set => Set(ref _observations, value); }

        //Taxes
        [JsonProperty(PropertyName = "icms")]
        public string ICMS { get => _iCMS; set => Set(ref _iCMS, value); }

        [JsonProperty(PropertyName = "ipi")]
        public string IPI { get => _iPI; set => Set(ref _iPI, value); }

        [JsonProperty(PropertyName = "pis")]
        public string PIS { get => _pIS; set => Set(ref _pIS, value); }

        [JsonProperty(PropertyName = "cofins")]
        public string COFINS { get => _cOFINS; set => Set(ref _cOFINS, value); }

        [JsonProperty(PropertyName = "taxableunit")]
        public string TaxableUnit { get => _taxableUnit; set => Set(ref _taxableUnit, value); }

        [JsonProperty(PropertyName = "cest")]
        public string CEST { get => _cEST; set => Set(ref _cEST, value); }

        [JsonProperty(PropertyName = "supplierid")]
        public string SupplierId { get => _supplierId; set => Set(ref _supplierId, value); }

        //Json Ignore
        [JsonIgnore]
        public decimal SaleQuantity { get => _saleQuantity; set => Set(ref _saleQuantity, value); }

        [JsonIgnore]
        public int SaleIndexNumber { get => _saleIndexNumber; set => Set(ref _saleIndexNumber, value); }

        [JsonIgnore]
        public bool IsSaleCancelItem { get => _isSaleCancelItem; set => Set(ref _isSaleCancelItem, value); }

        [JsonIgnore]
        public bool IsSaleDiscountItem { get => _isSaleDiscountItem; set => Set(ref _isSaleDiscountItem, value); }

        [JsonIgnore]
        public bool IsSaleProductCanceled { get => _isSaleProductCanceled; set => Set(ref _isSaleProductCanceled, value); }

        [JsonIgnore]
        public bool IsSaleService { get => _isSaleService; set => Set(ref _isSaleService, value); }

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
    }
}