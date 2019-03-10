using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExpressServices.Core.Models
{
    public class GraphUser
    {
        [JsonProperty(PropertyName = "accountEnabled")]
        public bool AccountEnabled { get; set; }

        [JsonProperty(PropertyName = "signInNames")]
        public List<SingInNames> SingInNames { get; set; } = new List<SingInNames>();

        [JsonProperty(PropertyName = "creationType")]
        public string CreationType { get; set; } = "LocalAccount";

        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "mailNickName")]
        public string MailNickName { get; set; }

        [JsonProperty(PropertyName = "passwordProfile")]
        public PasswordProfile PasswordProfile { get; set; } = new PasswordProfile();

        [JsonProperty(PropertyName = "passwordPolicies")]
        public string PasswordPolicies { get; set; } = "DisablePasswordExpiration";

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; } = null;

        [JsonProperty(PropertyName = "facsimileTelephoneNumber")]
        public string FacsimileTelephoneNumber { get; set; } = null;

        [JsonProperty(PropertyName = "givenName")]
        public string GivenName { get; set; }

        [JsonProperty(PropertyName = "jobTitle")]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "mail")]
        public string Mail { get; set; } = null;

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; } = null;

        [JsonProperty(PropertyName = "otherMails")]
        public string[] OtherMails { get; set; } = new string[] { };

        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "preferredLanguage")]
        public string PreferredLanguage { get; set; } = null;

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "streetAddress")]
        public string StreetAddress { get; set; } = null;

        [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }

        [JsonProperty(PropertyName = "telephoneNumber")]
        public string TelephoneNumber { get; set; } = null;

    }

    public class SingInNames
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = "emailAddress";

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class PasswordProfile
    {
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; } = "Senha1234";

        [JsonProperty(PropertyName = "forceChangePasswordNextLogin")]
        public bool ForceChangePasswordNextLogin { get; set; } = false;
    }
}