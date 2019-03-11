using Caliburn.Micro;
using System;

namespace ExpressServices.Core.Models
{
    public class HistoryModel : PropertyChangedBase
    {
        private string _id;
        private string _companyId;
        private string _name;
        private string _type;
        private DateTimeOffset? _createdAt;
        private string _createdBy;
        private string _imgSource;

        public string Id { get => _id; set => Set(ref _id, value); }
        public string CompanyId { get => _companyId; set => Set(ref _companyId, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public string Type { get => _type; set => Set(ref _type, value); }
        public DateTimeOffset? CreatedAt { get => _createdAt; set => Set(ref _createdAt, value); }
        public string CreatedBy { get => _createdBy; set => Set(ref _createdBy, value); }
        public string ImgSource { get => _imgSource; set => Set(ref _imgSource, value); }

        public string CreatedAtFormated { get => string.Format("em {0:dd/M/yyyy} às {0:HH:mm}", CreatedAt); }
    }
}