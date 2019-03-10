using System;
using System.Collections.ObjectModel;

using Caliburn.Micro;

using ExpressServices.Core.Models;
using ExpressServices.Core.Services;
using ExpressServices.Helpers;

namespace ExpressServices.ViewModels
{
    public class WorkshopViewModel : Screen
    {
        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetGridSampleData();
            }
        }
    }
}
