using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ExpressServices.Core.Models
{
    public sealed class CurrentUser
    {
        private static readonly Lazy<CurrentUser> lazy = new Lazy<CurrentUser>(() => new CurrentUser());

        public static CurrentUser Instance { get { return lazy.Value; } }

        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public string Id
        {
            get { return localSettings.Values["UserId"]?.ToString(); }
            set { localSettings.Values["UserId"] = value; }
        }
        public string Name
        {
            get { return localSettings.Values["UserName"]?.ToString(); }
            set { localSettings.Values["UserName"] = value; }
        }
        public string Email
        {
            get { return localSettings.Values["UserEmail"]?.ToString(); }
            set { localSettings.Values["UserEmail"] = value; }
        }
        public string Role
        {
            get { return localSettings.Values["UserRole"]?.ToString() ?? "Técnico"; }
            set { localSettings.Values["UserRole"] = value; }
        }
        public string CurrentCompanyId
        {
            get { return localSettings.Values["CompanyId"]?.ToString(); }
            set { localSettings.Values["CompanyId"] = value; }
        }

        public void ResetUser()
        {
            Id = null;
            Name = null;
            Email = null;
            Role = null;
        }

        private CurrentUser()
        {

        }

    }
}
