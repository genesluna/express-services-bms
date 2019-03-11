using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class User : ModelBase
    {
        private string _providerId;
        private string _name;
        private string _email;
        private string _docNumber;
        private string _observation;
        private string _phoneMobile;
        private string _phoneHome;
        private string _addressStreet;
        private string _addressNumber;
        private string _addressComplement;
        private string _addressNeighborhood;
        private string _addressCity;
        private string _addressState;
        private string _addressZipcode;
        private string _emergencyContactName;
        private string _emergencyContactPhone;
        private string _localPassword;
        private string _avatar;
        private bool _isActive;
        private string _role;

        [JsonProperty(PropertyName = "providerid")]
        public string ProviderId { get => _providerId; set => Set(ref _providerId, value); }

        [JsonProperty(PropertyName = "name")]
        public string Name { get => _name; set => Set(ref _name, value); }

        [JsonProperty(PropertyName = "email")]
        public string Email { get => _email; set => Set(ref _email, value); }

        [JsonProperty(PropertyName = "docnumber")]
        public string DocNumber { get => _docNumber; set => Set(ref _docNumber, value); }

        [JsonProperty(PropertyName = "observation")]
        public string Observation { get => _observation; set => Set(ref _observation, value); }

        [JsonProperty(PropertyName = "phonemobile")]
        public string PhoneMobile { get => _phoneMobile; set => Set(ref _phoneMobile, value); }

        [JsonProperty(PropertyName = "phonehome")]
        public string PhoneHome { get => _phoneHome; set => Set(ref _phoneHome, value); }

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

        [JsonProperty(PropertyName = "emergencycontactname")]
        public string EmergencyContactName { get => _emergencyContactName; set => Set(ref _emergencyContactName, value); }

        [JsonProperty(PropertyName = "emergencycontactphone")]
        public string EmergencyContactPhone { get => _emergencyContactPhone; set => Set(ref _emergencyContactPhone, value); }

        [JsonProperty(PropertyName = "localpassword")]
        public string LocalPassword { get => _localPassword; set => Set(ref _localPassword, value); }

        [JsonProperty(PropertyName = "role")]
        public string Role { get => _role; set => Set(ref _role, value); }

        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get => _avatar; set => Set(ref _avatar, value); }

        [JsonProperty(PropertyName = "isactive")]
        public bool IsActive { get => _isActive; set => Set(ref _isActive, value); }

    }
}
