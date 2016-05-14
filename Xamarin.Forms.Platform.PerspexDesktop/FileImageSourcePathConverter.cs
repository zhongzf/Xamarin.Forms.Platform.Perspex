using Perspex.Media.Imaging;
using System;
using System.Globalization;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    internal class FileImageSourcePathConverter : Perspex.Markup.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			var source = (FileImageSource)value;
			string file = (source != null ? source.File : string.Empty);
			return new Bitmap(file);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
	}
}