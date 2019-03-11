using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class Supplier : ModelBase
    {
        [JsonProperty(PropertyName = "iscompany")]
        public bool IsCompany { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "corporatename")]
        public string CorporateName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phonemobile")]
        public string PhoneMobile { get; set; }

        [JsonProperty(PropertyName = "observation")]
        public string Observation { get; set; }

        [JsonProperty(PropertyName = "docnumber")]
        public string DocNumber { get; set; }

        [JsonProperty(PropertyName = "stateregistration")]
        public string StateRegistration { get; set; }

        [JsonProperty(PropertyName = "isstateregistrationexempt")]
        public bool IsStateRegistrationExempt { get; set; }

        [JsonProperty(PropertyName = "municipalregistration")]
        public string MunicipalRegistration { get; set; }

        [JsonProperty(PropertyName = "addressstreet")]
        public string AddressStreet { get; set; }

        [JsonProperty(PropertyName = "addressnumber")]
        public string AddressNumber { get; set; }

        [JsonProperty(PropertyName = "addresscomplement")]
        public string AddressComplement { get; set; }

        [JsonProperty(PropertyName = "addressneighborhood")]
        public string AddressNeighborhood { get; set; }

        [JsonProperty(PropertyName = "addresscity")]
        public string AddressCity { get; set; }

        [JsonProperty(PropertyName = "addressstate")]
        public string AddressState { get; set; }

        [JsonProperty(PropertyName = "addresszipcode")]
        public string AddressZipcode { get; set; }

        [JsonProperty(PropertyName = "contactname")]
        public string ContactName { get; set; }

        [JsonProperty(PropertyName = "contactphone")]
        public string ContactPhone { get; set; }

        [JsonProperty(PropertyName = "contactemail")]
        public string ContactEmail { get; set; }
    }
}