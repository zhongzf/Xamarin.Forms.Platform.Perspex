
namespace Xamarin.Forms.Platform.PerspexDesktop
{
	internal static class AlignmentExtensions
	{
		internal static Perspex.Media.TextAlignment ToNativeTextAlignment(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Center:
					return Perspex.Media.TextAlignment.Center;
				case TextAlignment.End:
					return Perspex.Media.TextAlignment.Right;
				default:
					return Perspex.Media.TextAlignment.Left;
			}
		}

        internal static Perspex.Layout.VerticalAlignment ToNativeVerticalAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return Perspex.Layout.VerticalAlignment.Center;
                case TextAlignment.End:
                    return Perspex.Layout.VerticalAlignment.Bottom;
                default:
                    return Perspex.Layout.VerticalAlignment.Top;
            }
        }
    }
}