using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ExpressServices.CustomControls
{
    public sealed partial class ValidatingComboBox : UserControl, INotifyPropertyChanged
    {
        public ValidatingComboBox()
        {
            this.InitializeComponent();
            IsErrorMessageVisible = Visibility.Collapsed;
        }

        #region Properties

        private Visibility _isErrorMessageVisible;

        public Visibility IsErrorMessageVisible { get => _isErrorMessageVisible; set => Set(ref _isErrorMessageVisible, value); }

        private string _errorMessageText;

        public string ErrorMessageText
        {
            get => _errorMessageText;
            set => Set(ref _errorMessageText, value);
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(ValidatingComboBox), new PropertyMetadata(string.Empty));

        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRequired.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.Register("IsRequired", typeof(bool), typeof(ValidatingComboBox), new PropertyMetadata(false));

        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasErrors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasErrorsProperty =
            DependencyProperty.Register("HasErrors", typeof(bool), typeof(ValidatingComboBox), new PropertyMetadata(false));

        public bool RunValidation
        {
            get { return (bool)GetValue(RunValidationProperty); }
            set
            {
                SetValue(RunValidationProperty, value);
                if (value)
                {
                    RunValidations();
                }
            }
        }

        // Using a DependencyProperty as the backing store for RunValidation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RunValidationProperty =
            DependencyProperty.Register("RunValidation", typeof(bool), typeof(ValidatingComboBox), new PropertyMetadata(false));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ValidatingComboBox), new PropertyMetadata(DependencyProperty.UnsetValue));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(ValidatingComboBox), new PropertyMetadata(DependencyProperty.UnsetValue));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ValidatingComboBox), new PropertyMetadata(string.Empty));

        #endregion Properties

        #region Methods

        private void RunValidations()
        {
            if (SelectedItem == null && IsRequired)
            {
                HasErrors = true;
                ErrorMessageText = "Este campo é obrigatório";
                IsErrorMessageVisible = Visibility.Visible;
            }
            else
            {
                HasErrors = false;
                ErrorMessageText = string.Empty;
                IsErrorMessageVisible = Visibility.Collapsed;
            }
        }

        #endregion Methods

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion INotifyPropertyChanged

        private void Combobox_LostFocus(object sender, RoutedEventArgs e)
        {
            RunValidations();
        }
    }
}