using Perspex.Media;
using System;
using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class TimePickerRenderer : ViewRenderer<TimePicker, Perspex.Controls.TextBox>, IWrapperAware
	{
		IBrush _defaultBrush;

		public void NotifyWrapped()
		{
			if (Control != null)
			{
				//Control.ForceInvalidate += PickerOnForceInvalidate;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && Control != null)
			{
				//Control.ForceInvalidate -= PickerOnForceInvalidate;
				//Control.TimeChanged -= OnControlTimeChanged;
				//Control.Loaded -= ControlOnLoaded;
			}

			base.Dispose(disposing);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					var picker = new Perspex.Controls.TextBox();
					SetNativeControl(picker);

					//Control.TimeChanged += OnControlTimeChanged;
					//Control.Loaded += ControlOnLoaded;
				}

				UpdateTime();
			}
		}

		void ControlOnLoaded(object sender, EventArgs routedEventArgs)
		{
			// The defaults from the control template won't be available
			// right away; we have to wait until after the template has been applied
			_defaultBrush = Control.Foreground;
			UpdateTextColor();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
				UpdateTime();

			if (e.PropertyName == TimePicker.TextColorProperty.PropertyName)
				UpdateTextColor();
		}

		//void OnControlTimeChanged(object sender, TimePickerValueChangedEventArgs e)
		//{
		//	Element.Time = e.NewTime;
		//	((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.SizeRequestChanged);
		//}

		void PickerOnForceInvalidate(object sender, EventArgs eventArgs)
		{
			((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.SizeRequestChanged);
		}

		void UpdateTime()
		{
            Control.Text = Element.Time.ToString();
        }

        void UpdateTextColor()
		{
			Color color = Element.TextColor;
			Control.Foreground = color.IsDefault ? (_defaultBrush ?? color.ToBrush()) : color.ToBrush();
		}
	}
}