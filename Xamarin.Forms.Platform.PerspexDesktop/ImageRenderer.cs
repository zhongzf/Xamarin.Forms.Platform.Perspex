using System;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class ImageRenderer : ViewRenderer<Image, Perspex.Controls.Image>
	{
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var image = new Perspex.Controls.Image();
                    SetNativeControl(image);
                }
            }
        }

        // TODO: ImageRenderer
    }
}