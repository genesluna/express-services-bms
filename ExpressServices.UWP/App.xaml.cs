using Caliburn.Micro;
using ExpressServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Xaml;

namespace ExpressServices
{
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App
    {
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        private readonly Lazy<ActivationService> _activationService;

        private ActivationService ActivationService => _activationService.Value;

        public App()
        {
            //Environment (Local, Stage, Production)
            _localSettings.Values["BackendEnvironment"] = "Stage";

            InitializeComponent();

            Initialize();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);

            FocusVisualKind = FocusVisualKind.Reveal;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                // Hide default title bar.
                var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = true;

                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private WinRTContainer _container;

        protected override void Configure()
        {
            // This configures the framework to map between MainViewModel and MainPage
            // Normally it would map between MainPageViewModel and MainPage
            var config = new TypeMappingConfiguration
            {
                IncludeViewSuffixInViewModelNames = false
            };

            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);

            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

            _container.Singleton<IEventAggregator, EventAggregator>();

            // Register the view models using reflection
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass && type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container
                .RegisterPerRequest(viewModelType, viewModelType.ToString(), viewModelType));

            //_container.PerRequest<ShellViewModel>();
            //_container.PerRequest<WorkshopViewModel>();
            //_container.PerRequest<CustomersViewModel>();
            //_container.PerRequest<SettingsViewModel>();
            //_container.PerRequest<BlankViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(_container, typeof(ViewModels.BlankViewModel), new Lazy<UIElement>(CreateShell));
        }

        private static UIElement CreateShell()
        {
            var shellPage = new Views.ShellPage();
            return shellPage;
        }
    }
}
