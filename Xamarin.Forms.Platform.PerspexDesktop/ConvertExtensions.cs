using Perspex.Media;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
	internal static class ConvertExtensions
	{
		public static Perspex.Media.IBrush ToBrush(this Color color)
		{
			return new SolidColorBrush(color.ToWindowsColor());
		}

		public static Perspex.Media.Color ToWindowsColor(this Color color)
		{
			return new Perspex.Media.Color((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
		}
	}
}