using System;
using System.Globalization;
using System.IO;

using Xamarin.Forms;

namespace Prolix.Xam.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        public ImageSource DefaultImage { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = value as byte[];

            if (source == null)
                return DefaultImage;

            ImageSource image = ImageSource.FromStream(() => new MemoryStream(source));

            return image;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
