using System;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class StepperRenderer : ViewRenderer<Stepper, Perspex.Controls.ProgressBar>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Stepper> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					SetNativeControl(new Perspex.Controls.ProgressBar());
                    Control.PropertyChanged += OnPropertyChanged;
                }

                UpdateMaximum();
				UpdateMinimum();
				UpdateValue();
				UpdateIncrement();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Stepper.ValueProperty.PropertyName)
				UpdateValue();
			else if (e.PropertyName == Stepper.MaximumProperty.PropertyName)
				UpdateMaximum();
			else if (e.PropertyName == Stepper.MinimumProperty.PropertyName)
				UpdateMinimum();
			else if (e.PropertyName == Stepper.IncrementProperty.PropertyName)
				UpdateIncrement();
		}


        private void OnPropertyChanged(object sender, Perspex.PerspexPropertyChangedEventArgs e)
        {
            if (e.Property == Perspex.Controls.ProgressBar.ValueProperty)
            {
                //ProgressBarOnValueChanged(sender, e);
            }
        }

        void OnControlValue(object sender, EventArgs e)
		{
			Element.SetValueCore(Stepper.ValueProperty, Control.Value);
		}

		void UpdateIncrement()
		{
			//Control.Increment = Element.Increment;
		}

		void UpdateMaximum()
		{
			Control.Maximum = Element.Maximum;
		}

		void UpdateMinimum()
		{
			Control.Minimum = Element.Minimum;
		}

		void UpdateValue()
		{
			Control.Value = Element.Value;
		}
	}
}