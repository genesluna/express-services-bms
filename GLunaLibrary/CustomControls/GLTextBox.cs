using GLunaLibrary.Converters;
using GLunaLibrary.Helpers;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using static GLunaLibrary.Helpers.Enums;

namespace GLunaLibrary.CustomControls
{
    public class GLTextBox : TextBox
    {
        public GLTextBox() : base()
        {
            this.DefaultStyleKey = typeof(GLTextBox);

            this.TextChanged += GLTextBox_TextChanged;

            this.BeforeTextChanging += GLTextBox_BeforeTextChanging;

            this.LostFocus += GLTextBox_LostFocus;
        }

        #region Fields

        // Negative Regex class instance for non digit characters
        private static readonly Regex _rgxNonDigits = new Regex(@"[^\d]+");

        private static readonly Regex _rgxNonDigitsPlusComma = new Regex(@"[^\d,]+");

        private string _formatedText;

        #endregion Fields

        #region DependencyProperties

        new public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        new public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(GLTextBox), new PropertyMetadata(string.Empty));

        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register(nameof(ErrorMessage), typeof(string), typeof(GLTextBox), new PropertyMetadata(string.Empty));

        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }

        public static readonly DependencyProperty HasErrorsProperty =
            DependencyProperty.Register(nameof(HasErrors), typeof(bool), typeof(GLTextBox), new PropertyMetadata(false));

        public string Mask
        {
            get { return (string)GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); TextBoxMask.SetMask(this, value); }
        }

        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register(nameof(Mask), typeof(string), typeof(GLTextBox), new PropertyMetadata(string.Empty));

        public string MaskPlaceHolder
        {
            get { return (string)GetValue(MaskPlaceHolderProperty); }
            set { SetValue(MaskPlaceHolderProperty, value); TextBoxMask.SetPlaceHolder(this, value); }
        }

        public static readonly DependencyProperty MaskPlaceHolderProperty =
            DependencyProperty.Register(nameof(MaskPlaceHolder), typeof(string), typeof(GLTextBox), new PropertyMetadata(string.Empty));

        public int MaxChar
        {
            get { return (int)GetValue(MaxCharProperty); }
            set { SetValue(MaxCharProperty, value); }
        }

        public static readonly DependencyProperty MaxCharProperty =
            DependencyProperty.Register(nameof(MaxChar), typeof(int), typeof(GLTextBox), new PropertyMetadata(0));

        public int MinChar
        {
            get { return (int)GetValue(MinCharProperty); }
            set { SetValue(MinCharProperty, value); }
        }

        public static readonly DependencyProperty MinCharProperty =
            DependencyProperty.Register(nameof(MinChar), typeof(int), typeof(GLTextBox), new PropertyMetadata(0));

        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }

        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(GLTextBox), new PropertyMetadata(false));

        public bool IsZeroAsValueAllowed
        {
            get { return (bool)GetValue(IsZeroAsValueAllowedProperty); }
            set { SetValue(IsZeroAsValueAllowedProperty, value); }
        }

        public static readonly DependencyProperty IsZeroAsValueAllowedProperty =
            DependencyProperty.Register(nameof(IsZeroAsValueAllowed), typeof(bool), typeof(GLTextBox), new PropertyMetadata(true));

        public InputTypes InputType
        {
            get { return (InputTypes)GetValue(InputTypeProperty); }
            set { SetValue(InputTypeProperty, value); }
        }

        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.Register(nameof(InputType), typeof(InputTypes), typeof(GLTextBox), new PropertyMetadata(InputTypes.Plain));

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

        public static readonly DependencyProperty RunValidationProperty =
            DependencyProperty.Register(nameof(RunValidation), typeof(bool), typeof(GLTextBox), new PropertyMetadata(false));

        #endregion DependencyProperties

        #region Methods

        private T GetTemplateChild<T>(string name) where T : DependencyObject
        {
            var child = GetTemplateChild(name) as T;
            if (child == null)
            {
                throw new NullReferenceException(name);
            }

            return child;
        }

        private void MoneyNumberPercentageFormat(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if (args.NewText != _formatedText)
            {
                // It removes non digit characters from the new text
                var cleanText = _rgxNonDigits.Replace(args.NewText, string.Empty);

                // It checks if a non digit character was entered
                var isMatch = _rgxNonDigits.IsMatch(args.NewText);

                // If it's possible to convert the clean text to a decimal
                if (decimal.TryParse(cleanText, out decimal result))
                {
                    if (InputType.Equals(InputTypes.Money))
                    {
                        // It Formats the resulting decimal as currency
                        _formatedText = sender.Text = (result / 100).ToString("C");
                    }
                    else
                    {
                        // It Formats the resulting decimal as Number
                        _formatedText = sender.Text = (result / 100).ToString("N");
                    }

                    // It Moves the cursor far to the right
                    sender.SelectionStart = _formatedText.Length;
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
                sender.SelectionStart = _formatedText.Length;
            }
        }

        private void IntegerFormat(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            // It Checks if a non digit character was entered
            var isMatch = _rgxNonDigits.IsMatch(args.NewText);

            if (isMatch)
            {
                args.Cancel = true;
            }
        }

        private void DecimalFormat(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            // It Checks if a non digit character was entered
            var isMatch = _rgxNonDigitsPlusComma.IsMatch(args.NewText);

            if (isMatch)
            {
                args.Cancel = true;
            }
        }

        private void SetError(string errorMsg)
        {
            ErrorMessage = errorMsg;
            HasErrors = true;
        }

        private void RemoveErrors()
        {
            ErrorMessage = string.Empty;
            HasErrors = false;
        }

        private void RunValidations()
        {
            if (InputType.Equals(InputTypes.Plain))
            {
                //Just a regular textbox
                return;
            }

            var text = this.Text;

            if (InputType.Equals(InputTypes.CNPJ) ||
                InputType.Equals(InputTypes.CPF) ||
                InputType.Equals(InputTypes.CEP))
            {
                text = _rgxNonDigits.Replace(text, string.Empty);
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
                if (decimal.Parse(_rgxNonDigits.Replace(text, string.Empty)) == decimal.Zero && !IsZeroAsValueAllowed)
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

        protected override void OnApplyTemplate()
        {
            //_errorMessageTextBox = GetTemplateChild<TextBlock>("ErrorMessageTextBox");

            Binding binding = new Binding();

            binding.Source = this;
            binding.Path = new PropertyPath("Text");
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

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
                this.PlaceholderText = "%";
                binding.Converter = new DecimalToStringConverter();
            }
            else if (InputType.Equals(InputTypes.Money) || InputType.Equals(InputTypes.Number))
            {
                binding.Converter = new DecimalToStringConverter();
            }

            this.SetBinding(TextBox.TextProperty, binding);

            base.OnApplyTemplate();
        }

        private void GLTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            RunValidations();
        }

        private void GLTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
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
            else if (InputType.Equals(InputTypes.Decimal))
            {
                DecimalFormat(sender, args);
            }
        }

        private void GLTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (InputType.Equals(InputTypes.Money) ||
                InputType.Equals(InputTypes.Number) ||
                InputType.Equals(InputTypes.Percentage) &&
                _formatedText != textBox.Text)
            {
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = _formatedText;
                }
            }
        }

        #endregion Handlers
    }
}