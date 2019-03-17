using Caliburn.Micro;
using ExpressServices.Core.Models;

namespace ExpressServices.Abstractions
{
    public abstract class ViewModelBase : Screen
    {
        private string _title = string.Empty;
        private bool _isBusy;
        private string _userRole = CurrentUser.Instance.Role;

        public bool IsAdmin => "Administrador".Equals(_userRole);

        public bool IsAdminOrManager => "Administrador".Equals(_userRole) ||
                                        "Gerente".Equals(_userRole);

        public bool IsAdminOrManagerOrTechnician => "Administrador".Equals(_userRole) ||
                                                    "Gerente".Equals(_userRole) ||
                                                    "Técnico".Equals(_userRole);

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }
    }
}
