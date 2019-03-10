using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;

namespace ExpressServices.Core.Abstractions
{
    public abstract class BaseModel : ObservableObject
    {
        private string _id;
        private string _companyId;
        private string _createdBy;
        private string _updatedBy;
        private DateTimeOffset? _createdAt;
        private DateTimeOffset _dateCreated;
        private DateTimeOffset? _updatedAt;

        //Default Properties
        public string Id { get => _id; set => Set(ref _id, value); }

        [JsonProperty(PropertyName = "companyid")]
        public string CompanyId { get => _companyId; set => Set(ref _companyId, value); }

        [JsonProperty(PropertyName = "createdby")]
        public string CreatedBy { get => _createdBy; set => Set(ref _createdBy, value); }

        [JsonProperty(PropertyName = "updatedby")]
        public string UpdatedBy { get => _updatedBy; set => Set(ref _updatedBy, value); }

        [JsonProperty(PropertyName = "createdat")]
        public DateTimeOffset? CreatedAt { get => _createdAt; set => Set(ref _createdAt, value); }

        //for client purposes, createdAt not stored when offline
        [JsonProperty(PropertyName = "datecreated")]
        public DateTimeOffset DateCreated { get => _dateCreated; set => Set(ref _dateCreated, value); }

        [JsonProperty(PropertyName = "updatedat")]
        public DateTimeOffset? UpdatedAt { get => _updatedAt; set => Set(ref _updatedAt, value); }

        public T Cast<T>()
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(this));
        }
    }
}