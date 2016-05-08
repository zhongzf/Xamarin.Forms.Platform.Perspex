using Perspex.Media;
using System;
using System.Globalization;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public sealed class ColorConverter : Perspex.Markup.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;
            var defaultColorKey = (string)parameter;

            // TODO: Resources
            IBrush defaultBrush = defaultColorKey != null ? (IBrush)Application.Current.Resources[defaultColorKey] : (IBrush)(new SolidColorBrush(Colors.Transparent));

            return color == Color.Default ? defaultBrush : color.ToBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}