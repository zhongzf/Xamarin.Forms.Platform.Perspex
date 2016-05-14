using System;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class SliderRenderer : ViewRenderer<Slider, Perspex.Controls.Slider>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					var slider = new Perspex.Controls.Slider();
					SetNativeControl(slider);

                    slider.PropertyChanged += OnPropertyChanged;
                }

                double stepping = Math.Min((e.NewElement.Maximum - e.NewElement.Minimum) / 10, 1);
				Control.TickFrequency = stepping;

				Control.Minimum = e.NewElement.Minimum;
				Control.Maximum = e.NewElement.Maximum;
				Control.Value = e.NewElement.Value;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Slider.MinimumProperty.PropertyName)
				Control.Minimum = Element.Minimum;
			else if (e.PropertyName == Slider.MaximumProperty.PropertyName)
				Control.Maximum = Element.Maximum;
			else if (e.PropertyName == Slider.ValueProperty.PropertyName)
			{
				if (Control.Value != Element.Value)
					Control.Value = Element.Value;
			}
		}


        private void OnPropertyChanged(object sender, Perspex.PerspexPropertyChangedEventArgs e)
        {
            if (e.Property == Perspex.Controls.ProgressBar.ValueProperty)
            {
                OnNativeValueCHanged(sender, e);
            }
        }


        void OnNativeValueCHanged(object sender, Perspex.PerspexPropertyChangedEventArgs e)
		{
			((IElementController)Element).SetValueFromRenderer(Slider.ValueProperty, e.NewValue);
		}
	}
}