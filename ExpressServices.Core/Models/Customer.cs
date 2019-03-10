using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class Customer : BaseModel
    {
        private bool _isCompany;
        private string _name;
        private string _corporateName;
        private string _email;
        private string _phoneMobile;
        private string _observation;
        private string _docNumber;
        private string _stateRegistration;
        private bool _isStateRegistrationExempt;
        private string _municipalRegistration;
        private string _addressStreet;
        private string _addressNumber;
        private string _addressComplement;
        private string _addressNeighborhood;
        private string _addressCity;
        private string _addressState;
        private string _addressZipcode;
        private string _fingerPrintTemplate;
        private string _fingerPrintImage;
        private string _contactName;
        private string _contactPhone;
        private string _contactEmail;

        [JsonProperty(PropertyName = "iscompany")]
        public bool IsCompany { get => _isCompany; set => Set(ref _isCompany, value); }

        [JsonProperty(PropertyName = "name")]
        public string Name { get => _name; set => Set(ref _name, value); }

        [JsonProperty(PropertyName = "corporatename")]
        public string CorporateName { get => _corporateName; set => Set(ref _corporateName, value); }

        [JsonProperty(PropertyName = "email")]
        public string Email { get => _email; set => Set(ref _email, value); }

        [JsonProperty(PropertyName = "phonemobile")]
        public string PhoneMobile { get => _phoneMobile; set => Set(ref _phoneMobile, value); }

        [JsonProperty(PropertyName = "observation")]
        public string Observation { get => _observation; set => Set(ref _observation, value); }

        [JsonProperty(PropertyName = "docnumber")]
        public string DocNumber { get => _docNumber; set => Set(ref _docNumber, value); }

        [JsonProperty(PropertyName = "stateregistration")]
        public string StateRegistration { get => _stateRegistration; set => Set(ref _stateRegistration, value); }

        [JsonProperty(PropertyName = "isstateregistrationexempt")]
        public bool IsStateRegistrationExempt { get => _isStateRegistrationExempt; set => Set(ref _isStateRegistrationExempt, value); }

        [JsonProperty(PropertyName = "municipalregistration")]
        public string MunicipalRegistration { get => _municipalRegistration; set => Set(ref _municipalRegistration, value); }

        [JsonProperty(PropertyName = "addressstreet")]
        public string AddressStreet { get => _addressStreet; set => Set(ref _addressStreet, value); }

        [JsonProperty(PropertyName = "addressnumber")]
        public string AddressNumber { get => _addressNumber; set => Set(ref _addressNumber, value); }

        [JsonProperty(PropertyName = "addresscomplement")]
        public string AddressComplement { get => _addressComplement; set => Set(ref _addressComplement, value); }

        [JsonProperty(PropertyName = "addressneighborhood")]
        public string AddressNeighborhood { get => _addressNeighborhood; set => Set(ref _addressNeighborhood, value); }

        [JsonProperty(PropertyName = "addresscity")]
        public string AddressCity { get => _addressCity; set => Set(ref _addressCity, value); }

        [JsonProperty(PropertyName = "addressstate")]
        public string AddressState { get => _addressState; set => Set(ref _addressState, value); }

        [JsonProperty(PropertyName = "addresszipcode")]
        public string AddressZipcode { get => _addressZipcode; set => Set(ref _addressZipcode, value); }

        [JsonProperty(PropertyName = "fingerprinttemplate")]
        public string FingerPrintTemplate { get => _fingerPrintTemplate; set => Set(ref _fingerPrintTemplate, value); }

        [JsonProperty(PropertyName = "fingerprintimage")]
        public string FingerPrintImage { get => _fingerPrintImage; set => Set(ref _fingerPrintImage, value); }

        [JsonProperty(PropertyName = "contactname")]
        public string ContactName { get => _contactName; set => Set(ref _contactName, value); }

        [JsonProperty(PropertyName = "contactphone")]
        public string ContactPhone { get => _contactPhone; set => Set(ref _contactPhone, value); }

        [JsonProperty(PropertyName = "contactemail")]
        public string ContactEmail { get => _contactEmail; set => Set(ref _contactEmail, value); }

        public string FormattedPhone
        {
            get
            {
                var formatted = PhoneMobile.Substring(3);
                var countryCode = PhoneMobile.Substring(0, 3);
                return countryCode == "+55" ? $"🇧🇷 {formatted}" : formatted;
            }
        }
    }
}