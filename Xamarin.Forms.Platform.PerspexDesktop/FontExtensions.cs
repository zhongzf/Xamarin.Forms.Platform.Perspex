using Perspex.Controls;
using Perspex.Controls.Primitives;
using Perspex.Media;
using System;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public static class FontExtensions
	{
		public static void ApplyFont(this TemplatedControl self, Font font)
		{
			self.FontSize = font.UseNamedSize ? font.NamedSize.GetFontSize() : font.FontSize;
			self.FontFamily = !string.IsNullOrEmpty(font.FontFamily) ? font.FontFamily : (string)DesktopResources.GetDefault("ContentControlThemeFontFamily");
			self.FontStyle = font.FontAttributes.HasFlag(FontAttributes.Italic) ? FontStyle.Italic : FontStyle.Normal;
			self.FontWeight = font.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeight.Bold : FontWeight.Normal;
		}

		public static void ApplyFont(this TextBlock self, Font font)
		{
			self.FontSize = font.UseNamedSize ? font.NamedSize.GetFontSize() : font.FontSize;
            self.FontFamily = !string.IsNullOrEmpty(font.FontFamily) ? font.FontFamily : (string)DesktopResources.GetDefault("ContentControlThemeFontFamily");
            self.FontStyle = font.FontAttributes.HasFlag(FontAttributes.Italic) ? FontStyle.Italic : FontStyle.Normal;
            self.FontWeight = font.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeight.Bold : FontWeight.Normal;
        }

        internal static void ApplyFont(this TemplatedControl self, IFontElement element)
		{
			self.FontSize = element.FontSize;
			self.FontFamily = !string.IsNullOrEmpty(element.FontFamily) ? element.FontFamily : (string)DesktopResources.GetDefault("ContentControlThemeFontFamily");
			self.FontStyle = element.FontAttributes.HasFlag(FontAttributes.Italic) ? FontStyle.Italic : FontStyle.Normal;
			self.FontWeight = element.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeight.Bold : FontWeight.Normal;
		}
        
        internal static void ApplyFont(this TextBlock self, IFontElement element)
        {
            self.FontSize = element.FontSize;
            self.FontFamily = !string.IsNullOrEmpty(element.FontFamily) ? element.FontFamily : (string)DesktopResources.GetDefault("ContentControlThemeFontFamily");
            self.FontStyle = element.FontAttributes.HasFlag(FontAttributes.Italic) ? FontStyle.Italic : FontStyle.Normal;
            self.FontWeight = element.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeight.Bold : FontWeight.Normal;
        }

        internal static double GetFontSize(this NamedSize size)
		{
			// These are values pulled from the mapped sizes on Windows Phone, WinRT has no equivalent sizes, only intents.
			switch (size)
			{
				case NamedSize.Default:
                    return (double)DesktopResources.GetDefault("ControlContentThemeFontSize");
				case NamedSize.Micro:
					return 18.667 - 3;
				case NamedSize.Small:
					return 18.667;
				case NamedSize.Medium:
					return 22.667;
				case NamedSize.Large:
					return 32;
				default:
					throw new ArgumentOutOfRangeException("size");
			}
		}

		internal static bool IsDefault(this IFontElement self)
		{
			return self.FontFamily == null && self.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Label), true) && self.FontAttributes == FontAttributes.None;
		}
	}
}