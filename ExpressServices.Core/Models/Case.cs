using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ExpressServices.Core.Models
{
    public class Case : BaseModel
    {
        #region Fields

        private long _caseNumber;
        private string _description;
        private string _type;
        private string _priority;
        private string _dueTo;
        private string _status;
        private bool _isDigitallySignedIn;
        private bool _isDigitallySignedOut;
        private bool _isPayed;
        private string _equipamentType;
        private string _equipamentSerialNumber;
        private string _equipamentBrandName;
        private string _equipamentModel;
        private string _equipamentHD;
        private string _equipamentCPU;
        private string _equipamentMEM;
        private string _equipamentAccessories;
        private string _equipamentObservations;
        private string _customerId;
        private string _customerName;
        private string _customerDoc;
        private string _customerPhone;
        private string _saleId;
        private Customer _customer;
        private ObservableCollection<Service> _services;
        private ObservableCollection<Product> _products;
        private ObservableCollection<HistoryModel> _history;
        private bool _isCanceled;
        private bool _isClosed;

        #endregion Fields

        //Case

        [JsonProperty(PropertyName = "casenumber")]
        public long CaseNumber { get => _caseNumber; set => Set(ref _caseNumber, value); }

        [JsonProperty(PropertyName = "description")]
        public string Description { get => _description; set => Set(ref _description, value); }

        [JsonProperty(PropertyName = "type")]
        public string Type { get => _type; set => Set(ref _type, value); }

        [JsonProperty(PropertyName = "priority")]
        public string Priority { get => _priority; set => Set(ref _priority, value); }

        [JsonProperty(PropertyName = "dueto")]
        public string DueTo { get => _dueTo; set => Set(ref _dueTo, value); }

        [JsonProperty(PropertyName = "status")]
        public string Status
        {
            get => _status;

            set
            {
                Set(ref _status, value);

                if ("Cancelada".Equals(value))
                {
                    IsCanceled = true;
                }
                else if ("Encerrada".Equals(value))
                {
                    IsClosed = true;
                }
                else
                {
                    IsClosed = false;
                    IsCanceled = false;
                }
            }
        }

        [JsonProperty(PropertyName = "isdigitallysignedin")]
        public bool IsDigitallySignedIn { get => _isDigitallySignedIn; set => Set(ref _isDigitallySignedIn, value); }

        [JsonProperty(PropertyName = "isdigitallysignedout")]
        public bool IsDigitallySignedOut { get => _isDigitallySignedOut; set => Set(ref _isDigitallySignedOut, value); }

        [JsonProperty(PropertyName = "ispayed")]
        public bool IsPayed { get => _isPayed; set => Set(ref _isPayed, value); }

        //Equipament
        [JsonProperty(PropertyName = "equipamenttype")]
        public string EquipamentType { get => _equipamentType; set => Set(ref _equipamentType, value); }

        [JsonProperty(PropertyName = "equipamentserialnumber")]
        public string EquipamentSerialNumber { get => _equipamentSerialNumber; set => Set(ref _equipamentSerialNumber, value); }

        [JsonProperty(PropertyName = "equipamentbrandname")]
        public string EquipamentBrandName { get => _equipamentBrandName; set => Set(ref _equipamentBrandName, value); }

        [JsonProperty(PropertyName = "equipamentmodel")]
        public string EquipamentModel { get => _equipamentModel; set => Set(ref _equipamentModel, value); }

        [JsonProperty(PropertyName = "equipamenthd")]
        public string EquipamentHD { get => _equipamentHD; set => Set(ref _equipamentHD, value); }

        [JsonProperty(PropertyName = "equipamentcpu")]
        public string EquipamentCPU { get => _equipamentCPU; set => Set(ref _equipamentCPU, value); }

        [JsonProperty(PropertyName = "equipamentmem")]
        public string EquipamentMEM { get => _equipamentMEM; set => Set(ref _equipamentMEM, value); }

        [JsonProperty(PropertyName = "equipamentaccessories")]
        public string EquipamentAccessories { get => _equipamentAccessories; set => Set(ref _equipamentAccessories, value); }

        [JsonProperty(PropertyName = "equipamentobservations")]
        public string EquipamentObservations { get => _equipamentObservations; set => Set(ref _equipamentObservations, value); }

        [JsonProperty(PropertyName = "customerid")]
        public string CustomerId { get => _customerId; set => Set(ref _customerId, value); }

        [JsonProperty(PropertyName = "customername")]
        public string CustomerName { get => _customerName; set => Set(ref _customerName, value); }

        [JsonProperty(PropertyName = "customerdoc")]
        public string CustomerDoc { get => _customerDoc; set => Set(ref _customerDoc, value); }

        [JsonProperty(PropertyName = "customerphone")]
        public string CustomerPhone { get => _customerPhone; set => Set(ref _customerPhone, value); }

        [JsonProperty(PropertyName = "saleid")]
        public string SaleId { get => _saleId; set => Set(ref _saleId, value); }

        [JsonIgnore]
        public ObservableCollection<Service> Services { get => _services; set => Set(ref _services, value); }

        [JsonIgnore]
        public ObservableCollection<Product> Products { get => _products; set => Set(ref _products, value); }

        [JsonIgnore]
        public Customer Customer { get => _customer; set => Set(ref _customer, value); }

        [JsonIgnore]
        public ObservableCollection<HistoryModel> History { get => _history; set => Set(ref _history, value); }

        [JsonIgnore]
        public bool IsCanceled { get => _isCanceled; set => Set(ref _isCanceled, value); }

        [JsonIgnore]
        public bool IsClosed { get => _isClosed; set => Set(ref _isClosed, value); }

        [JsonIgnore]
        public string MaskedCaseNumber { get => CaseNumber.ToString().Insert(CaseNumber.ToString().Length - 6, "."); }

        [JsonIgnore]
        public string FormatedDate { get => DateCreated.ToString("dd/MM/yyyy HH:mm"); }
    }
}