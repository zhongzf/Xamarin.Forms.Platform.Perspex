using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class NativeViewWrapperRenderer : ViewRenderer<NativeViewWrapper, Perspex.Controls.Control>
	{
		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (Element?.GetDesiredSizeDelegate == null)
			{
				return base.GetDesiredSize(widthConstraint, heightConstraint);
			}

			// The user has specified a different implementation of GetDesiredSize
			SizeRequest? result = Element.GetDesiredSizeDelegate(this, widthConstraint, heightConstraint);

			// If the delegate returns a SizeRequest, we use it; 
			// if it returns null, fall back to the default implementation
			return result ?? base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		protected override Perspex.Size ArrangeOverride(Perspex.Size finalSize)
		{
			if (Element?.ArrangeOverrideDelegate == null)
			{
				return base.ArrangeOverride(finalSize);
			}

            // The user has specified a different implementation of ArrangeOverride
            Perspex.Size? result = Element.ArrangeOverrideDelegate(this, finalSize);

			// If the delegate returns a Size, we use it; 
			// if it returns null, fall back to the default implementation
			return result ?? base.ArrangeOverride(finalSize);
		}

		protected Perspex.Size MeasureOverride()
		{
			return MeasureOverride(new Perspex.Size());
		}

		protected override Perspex.Size MeasureOverride(Perspex.Size availableSize)
		{
			if (Element?.MeasureOverrideDelegate == null)
			{
				return base.MeasureOverride(availableSize);
			}

			// The user has specified a different implementation of MeasureOverride
			Perspex.Size? result = Element.MeasureOverrideDelegate(this, availableSize);

			// If the delegate returns a Size, we use it; 
			// if it returns null, fall back to the default implementation
			return result ?? base.MeasureOverride(availableSize);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<NativeViewWrapper> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
			{
				SetNativeControl(Element.NativeElement);
                // TODO: LayoutUpdated
				//Control.LayoutUpdated += (sender, args) => { ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.MeasureChanged); };
			}
		}
	}
}