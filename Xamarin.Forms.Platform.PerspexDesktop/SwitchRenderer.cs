using System;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class SwitchRenderer : ViewRenderer<Switch, Perspex.Controls.CheckBox>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					var control = new Perspex.Controls.CheckBox();
                    //control.Toggled += OnNativeToggled;
                    control.PropertyChanged += OnPropertyChanged;
					//control.ClearValue(ToggleSwitch.OnContentProperty);
					//control.ClearValue(ToggleSwitch.OffContentProperty);

					SetNativeControl(control);
				}

                Control.IsChecked = Element.IsToggled;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Switch.IsToggledProperty.PropertyName)
			{
				Control.IsChecked = Element.IsToggled;
			}
		}
        
        private void OnPropertyChanged(object sender, Perspex.PerspexPropertyChangedEventArgs e)
        {
            if (e.Property == Perspex.Controls.CheckBox.IsCheckedProperty)
            {
                OnNativeToggled(sender, e);
            }
        }


        void OnNativeToggled(object sender, EventArgs routedEventArgs)
		{
			((IElementController)Element).SetValueFromRenderer(Switch.IsToggledProperty, Control.IsChecked);
		}
	}
}