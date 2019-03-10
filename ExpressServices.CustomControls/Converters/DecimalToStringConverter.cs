using System;
using Windows.UI.Xaml.Data;

namespace ExpressServices.CustomControls.Converters
{
    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (decimal.TryParse(value as string, out decimal result))
            {
                if (result % 1 == 0)
                {
                    return (result * 100).ToString();
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}