using ExpressServices.Core.Abstractions;
using Newtonsoft.Json;

namespace ExpressServices.Core.Models
{
    public class Company : BaseModel
    {
        [JsonProperty(PropertyName = "companyname")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "corporatename")]
        public string CorporateName { get; set; }

        [JsonProperty(PropertyName = "docnumber")]
        public string DocNumber { get; set; }

        [JsonProperty(PropertyName = "site")]
        public string Site { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "logo")]
        public string Logo { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

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

        [JsonProperty(PropertyName = "isactive")]
        public bool IsActive { get; set; }
    }
}