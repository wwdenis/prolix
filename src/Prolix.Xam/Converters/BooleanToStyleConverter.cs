using System;
using System.Globalization;
using Xamarin.Forms;

namespace Prolix.Xam.Converters
{
    public class BooleanToStyleConverter : IValueConverter
    {
        public Style TrueStyle { get; set; }

        public Style FalseStyle { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return FalseStyle;

            return (bool)value ? TrueStyle : FalseStyle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
