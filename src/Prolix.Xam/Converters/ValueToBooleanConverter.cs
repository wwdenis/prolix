using System;
using System.Globalization;
using Xamarin.Forms;

namespace Prolix.Xam.Converters
{
    public class ValueToBooleanConverter : BindableObject, IValueConverter
	{
		public static readonly BindableProperty ExpectedValueProperty =
			BindableProperty.Create("ExpectedValue", typeof(object), typeof(ValueToBooleanConverter), null);

		public static readonly BindableProperty NotEqualProperty =
			BindableProperty.Create("NotEqual", typeof(bool), typeof(ValueToBooleanConverter), false);

		public object ExpectedValue
		{
			get { return GetValue(ExpectedValueProperty); }
			set { SetValue(ExpectedValueProperty, value); }
		}

		public bool IsNotEqual
		{
			get { return (bool)GetValue(NotEqualProperty); }
			set { SetValue(NotEqualProperty, value); }
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (IsNotEqual)
				return value != ExpectedValue;

			return value == ExpectedValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
