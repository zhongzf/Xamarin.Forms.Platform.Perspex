using Perspex.Controls;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class NativeViewWrapper : View
	{
		public NativeViewWrapper(Control nativeElement, GetDesiredSizeDelegate getDesiredSizeDelegate = null, ArrangeOverrideDelegate arrangeOverrideDelegate = null,
								 MeasureOverrideDelegate measureOverrideDelegate = null)
		{
			GetDesiredSizeDelegate = getDesiredSizeDelegate;
			ArrangeOverrideDelegate = arrangeOverrideDelegate;
			MeasureOverrideDelegate = measureOverrideDelegate;
			NativeElement = nativeElement;
		}

		public ArrangeOverrideDelegate ArrangeOverrideDelegate { get; set; }

		public GetDesiredSizeDelegate GetDesiredSizeDelegate { get; }

		public MeasureOverrideDelegate MeasureOverrideDelegate { get; set; }

		public Control NativeElement { get; }
	}
}