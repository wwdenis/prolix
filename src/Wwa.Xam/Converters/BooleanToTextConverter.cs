using System;
using System.Globalization;
using Xamarin.Forms;

namespace Wwa.Xam.Converters
{
    public class BooleanToTextConverter : BindableObject, IValueConverter
	{
		public static readonly BindableProperty TrueTextProperty =
			BindableProperty.Create("TrueText", typeof(string), typeof(ValueToBooleanConverter), string.Empty);

		public static readonly BindableProperty FalseTextProperty =
			BindableProperty.Create("FalseText", typeof(string), typeof(ValueToBooleanConverter), string.Empty);

		public string TrueText
		{
			get { return (string)GetValue(TrueTextProperty); }
			set { SetValue(TrueTextProperty, value); }
		}

		public string FalseText
		{
			get { return (string)GetValue(FalseTextProperty); }
			set { SetValue(FalseTextProperty, value); }
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool && (bool)value)
				return TrueText;

			return FalseText;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.Equals($"{value}", TrueText, StringComparison.OrdinalIgnoreCase);
		}
	}
}
