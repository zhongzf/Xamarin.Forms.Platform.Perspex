﻿using Perspex;
using Perspex.Controls;
using Perspex.Animation;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
	public class ViewRenderer<TElement, TNativeElement> : VisualElementRenderer<TElement, TNativeElement> where TElement : View where TNativeElement : Control
	{
        protected override void OnElementChanged(ElementChangedEventArgs<TElement> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                UpdateBackgroundColor();
            }
        }
    }
}