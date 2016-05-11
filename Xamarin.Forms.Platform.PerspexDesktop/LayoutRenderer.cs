using Perspex;
using System;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class LayoutRenderer : ViewRenderer<Layout, Perspex.Controls.Panel>
	{
        protected override void OnElementChanged(ElementChangedEventArgs<Layout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //SizeChanged -= OnSizeChanged;
                PropertyChanged -= LayoutRenderer_PropertyChanged;
            }

            if (e.NewElement != null)
            {
                //SizeChanged += OnSizeChanged;
                PropertyChanged += LayoutRenderer_PropertyChanged;

                UpdateClipToBounds();
            }
        }

        private void LayoutRenderer_PropertyChanged(object sender, PerspexPropertyChangedEventArgs e)
        {
            if(e.Property == Perspex.Layout.Layoutable.WidthProperty || e.Property == Perspex.Layout.Layoutable.HeightProperty)
            {
                OnSizeChanged(sender, e);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Layout.IsClippedToBoundsProperty.PropertyName)
                UpdateClipToBounds();
        }

        void OnSizeChanged(object sender, EventArgs e)
        {
            UpdateClipToBounds();
        }


        void UpdateClipToBounds()
        {
            Bounds = new Rect();
            if (Element.IsClippedToBounds)
            {
                Bounds = new Rect(0, 0, Width, Height);
            }
        }
    }
}