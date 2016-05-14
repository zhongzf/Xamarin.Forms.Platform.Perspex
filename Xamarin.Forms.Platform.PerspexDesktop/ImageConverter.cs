using Perspex.Media.Imaging;
using System;
using System.Threading.Tasks;
using System.Globalization;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class ImageConverter : Perspex.Markup.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			var source = (ImageSource)value;
			IImageSourceHandler handler;

			if (source != null && (handler = Registrar.Registered.GetHandler<IImageSourceHandler>(source.GetType())) != null)
			{
				Task<IBitmap> task = handler.LoadImageAsync(source);
				return new AsyncValue<IBitmap>(task, null);
			}

			return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
	}
}