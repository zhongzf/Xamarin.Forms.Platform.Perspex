using System.Collections.Generic;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public delegate SizeRequest? GetDesiredSizeDelegate(NativeViewWrapperRenderer renderer, double widthConstraint, double heightConstraint);

	public delegate Perspex.Size? ArrangeOverrideDelegate(NativeViewWrapperRenderer renderer, Perspex.Size finalSize);

	public delegate Perspex.Size? MeasureOverrideDelegate(NativeViewWrapperRenderer renderer, Perspex.Size availableSize);

	public static class LayoutExtensions
	{
		public static void Add(this IList<View> children, Perspex.Controls.Control view, GetDesiredSizeDelegate getDesiredSizeDelegate = null, ArrangeOverrideDelegate arrangeOverrideDelegate = null,
							   MeasureOverrideDelegate measureOverrideDelegate = null)
		{
			children.Add(view.ToView(getDesiredSizeDelegate, arrangeOverrideDelegate, measureOverrideDelegate));
		}

		public static View ToView(this Perspex.Controls.Control view, GetDesiredSizeDelegate getDesiredSizeDelegate = null, ArrangeOverrideDelegate arrangeOverrideDelegate = null,
								  MeasureOverrideDelegate measureOverrideDelegate = null)
		{
			return new NativeViewWrapper(view, getDesiredSizeDelegate, arrangeOverrideDelegate, measureOverrideDelegate);
		}
	}
}