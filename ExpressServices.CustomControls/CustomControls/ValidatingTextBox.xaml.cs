using ExpressServices.CustomControls.Converters;
using ExpressServices.CustomControls.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace ExpressServices.CustomControls
{
    public sealed partial class ValidatingTextBox : UserControl, INotifyPropertyChanged
    {
        public ValidatingTextBox()
        {
            this.InitializeComponent();
            IsErrorMessageVisible = Visibility.Collapsed;
        }

        #region Properties

        // Negative Regex class instance for non digit characters
        private static Regex rgxNonDigits = new Regex(@"[^\d]+");

        private string FormatedText;

        public enum InputTypes { Money, CPF, CNPJ, CEP, Email, Plain, AlphaNumeric, Integer, Number, Percentage }

        private Visibility _isErrorMessageVisible;

        public Visibility IsErrorMessageVisible { get => _isErrorMessageVisible; set => Set(ref _isErrorMessageVisible, value); }

        private string _mask;

        public string Mask { get => _mask; set => Set(ref _mask, value); }

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
            DependencyProperty.Register("Header", typeof(string), typeof(ValidatingTextBox), new PropertyMetadata(string.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); OnPropertyChanged(Text); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ValidatingTextBox), new PropertyMetadata(string.Empty));

        public InputTypes InputType
        {
            get { return (InputTypes)GetValue(InputTypeProperty); }
            set { SetValue(InputTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.Register("InputType", typeof(InputTypes), typeof(ValidatingTextBox), new PropertyMetadata(InputTypes.Plain));

        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRequired.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.Register("IsRequired", typeof(bool), typeof(ValidatingTextBox), new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for Required.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequiredProperty =
            DependencyProperty.Register("Required", typeof(bool), typeof(ValidatingTextBox), new PropertyMetadata(false));

        public bool IsZeroAsValueAllowed
        {
            get { return (bool)GetValue(IsZeroAsValueAllowedProperty); }
            set { SetValue(IsZeroAsValueAllowedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsZeroAsValueAllowed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsZeroAsValueAllowedProperty =
            DependencyProperty.Register("IsZeroAsValueAllowed", typeof(bool), typeof(ValidatingTextBox), new PropertyMetadata(true));

        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasErrors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasErrorsProperty =
            DependencyProperty.Register("HasErrors", typeof(bool), typeof(ValidatingTextBox), new PropertyMetadata(false));

        public int MaxChar
        {
            get { return (int)GetValue(MaxCharProperty); }
            set { SetValue(MaxCharProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxChar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxCharProperty =
            DependencyProperty.Register("MaxChar", typeof(int), typeof(ValidatingTextBox), new PropertyMetadata(0));

        public int MinChar
        {
            get { return (int)GetValue(MinCharProperty); }
            set { SetValue(MinCharProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinChar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinCharProperty =
            DependencyProperty.Register("MinChar", typeof(int), typeof(ValidatingTextBox), new PropertyMetadata(0));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(ValidatingTextBox), new PropertyMetadata(TextWrapping.NoWrap));

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
            DependencyProperty.Register("RunValidation", typeof(bool), typeof(ValidatingTextBox), new PropertyMetadata(false));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ValidatingTextBox), new PropertyMetadata(false));

        public string PlaceHolderText
        {
            get { return (string)GetValue(PlaceHolderTextProperty); }
            set { SetValue(PlaceHolderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderTextProperty =
            DependencyProperty.Register("PlaceHolderText", typeof(string), typeof(ValidatingComboBox), new PropertyMetadata(string.Empty));

        #endregion Properties

        #region Methods

        private void MoneyNumberPercentageFormat(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if (args.NewText != FormatedText)
            {
                // It removes non digit characters from the new text
                var cleanText = rgxNonDigits.Replace(args.NewText, string.Empty);

                // It checks if a non digit character was entered
                var isMatch = rgxNonDigits.IsMatch(args.NewText);

                // If it's possible to convert the clean text to a decimal
                if (decimal.TryParse(cleanText, out decimal result))
                {
                    if (InputType.Equals(InputTypes.Money))
                    {
                        // It Formats the resulting decimal as currency
                        FormatedText = sender.Text = (result / 100).ToString("C");
                    }
                    else
                    {
                        // It Formats the resulting decimal as Number
                        FormatedText = sender.Text = (result / 100).ToString("N");
                    }

                    // It Moves the cursor far to the right
                    sender.SelectionStart = FormatedText.Length;
                }

                // The condition may only fail in two case scenarios.

                // First case scenario  - A non digit character was entered
                else if (isMatch) args.Cancel = true; // It Cancels the changes and further event propagation

                // Second case scenario - The Text is either set to null or to an empty string
                else sender.Text = string.Empty; // It Resets the Text
            }
            else
            {
                // It Moves the cursor far to the right
                sender.SelectionStart = FormatedText.Length;
            }
        }

        private void IntegerFormat(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            // It Checks if a non digit character was entered
            var isMatch = rgxNonDigits.IsMatch(args.NewText);

            if (isMatch)
            {
                args.Cancel = true;
            }
        }

        private void SetError(string errorMsg)
        {
            IsErrorMessageVisible = Visibility.Visible;
            ErrorMessageText = errorMsg;
            HasErrors = true;
        }

        private void RemoveErrors()
        {
            IsErrorMessageVisible = Visibility.Collapsed;
            ErrorMessageText = string.Empty;
            HasErrors = false;
        }

        private void RunValidations()
        {
            if (InputType.Equals(InputTypes.Plain))
            {
                //Just a regular textbox
                return;
            }

            var text = Textbox.Text;

            if (InputType.Equals(InputTypes.CNPJ) ||
                InputType.Equals(InputTypes.CPF) ||
                InputType.Equals(InputTypes.CEP))
            {
                text = rgxNonDigits.Replace(text, string.Empty);
            }

            if (IsRequired && string.IsNullOrEmpty(text))
            {
                SetError("Este campo é obrigatório");
                return;
            }
            else if (MinChar != 0 && text.Length < MinChar)
            {
                SetError($"É necessário o mínimo de {MinChar} caracteres");
                return;
            }
            else if (MaxChar != 0 && text.Length > MaxChar)
            {
                SetError($"É permitido o máximo de {MaxChar} caracteres");
                return;
            }
            else
            {
                RemoveErrors();
            }

            if (InputType.Equals(InputTypes.Money) && !string.IsNullOrEmpty(text))
            {
                if (decimal.Parse(rgxNonDigits.Replace(text, string.Empty)) == decimal.Zero && !IsZeroAsValueAllowed)
                {
                    SetError("O valor não pode ser zero");
                }
                else
                {
                    RemoveErrors();
                }
            }
            else if (InputType.Equals(InputTypes.CNPJ) && !Validators.CPFCNPJ(text, false))
            {
                SetError("O CNPJ informado não é válido");
            }
            else if (InputType.Equals(InputTypes.CPF) && !Validators.CPFCNPJ(text, false))
            {
                SetError("O CPF informado não é válido");
            }
            else if (InputType.Equals(InputTypes.Email) && !Validators.Email(text))
            {
                SetError("O e-mail informado não é válido");
            }
            else
            {
                RemoveErrors();
            }
        }

        #endregion Methods

        #region Handlers

        private void Textbox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if (InputType.Equals(InputTypes.Plain))
            {
                //Just a regular textbox
                return;
            }

            if (InputType.Equals(InputTypes.Money) || InputType.Equals(InputTypes.Number) || InputType.Equals(InputTypes.Percentage))
            {
                MoneyNumberPercentageFormat(sender, args);
            }
            else if (InputType.Equals(InputTypes.Integer))
            {
                IntegerFormat(sender, args);
            }
        }

        private void Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            RunValidations();
        }

        private void ValidatingTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            Binding binding = new Binding();

            binding.Source = this;
            binding.Path = new PropertyPath("Text");
            binding.Mode = BindingMode.TwoWay;

            if (InputType.Equals(InputTypes.CPF))
            {
                Mask = "999.999.999-99";
            }
            else if (InputType.Equals(InputTypes.CNPJ))
            {
                Mask = "99.999.999/9999-99";
            }
            else if (InputType.Equals(InputTypes.CEP))
            {
                Mask = "99999-999";
            }
            else if (InputType.Equals(InputTypes.Percentage))
            {
                Textbox.PlaceholderText = "%";
                binding.Converter = new DecimalToStringConverter();
            }
            else if (InputType.Equals(InputTypes.Money) || InputType.Equals(InputTypes.Number))
            {
                binding.Converter = new DecimalToStringConverter();
            }

            Textbox.SetBinding(TextBox.TextProperty, binding);
        }

        private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (InputType.Equals(InputTypes.Money) ||
                InputType.Equals(InputTypes.Number) ||
                InputType.Equals(InputTypes.Percentage) &&
                FormatedText != Textbox.Text)
            {
                if (!string.IsNullOrEmpty(Textbox.Text))
                {
                    Textbox.Text = FormatedText;
                }
            }
        }

        #endregion Handlers

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
    }
}