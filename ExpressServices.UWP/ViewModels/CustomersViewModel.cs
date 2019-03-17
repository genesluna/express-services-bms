using System;
using System.Collections.ObjectModel;

using Caliburn.Micro;
using ExpressServices.Abstractions;
using ExpressServices.Core.Models;
using ExpressServices.Core.Services;
using ExpressServices.Helpers;

namespace ExpressServices.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public ObservableCollection<SampleOrder> Source => SampleDataService.GetGridSampleData();
    }
}
