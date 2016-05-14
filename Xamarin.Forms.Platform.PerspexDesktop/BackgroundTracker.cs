using Perspex;
using Perspex.Controls;
using Perspex.Media;
using Perspex.Media.Imaging;
using System;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    internal sealed class BackgroundTracker<T> : VisualElementTracker<Page, T> where T : Control
	{
		readonly PerspexProperty _backgroundProperty;
		bool _backgroundNeedsUpdate = true;

		public BackgroundTracker(PerspexProperty backgroundProperty)
		{
			if (backgroundProperty == null)
				throw new ArgumentNullException("backgroundProperty");

			_backgroundProperty = backgroundProperty;
		}

		protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName || e.PropertyName == Page.BackgroundImageProperty.PropertyName)
			{
				UpdateBackground();
			}

			base.OnPropertyChanged(sender, e);
		}

		protected override void UpdateNativeControl()
		{
			base.UpdateNativeControl();

			if (_backgroundNeedsUpdate)
				UpdateBackground();
		}

		void UpdateBackground()
		{
			if (Element == null)
				return;

            Control element = Control ?? Container;
			if (element == null)
				return;

			string backgroundImage = Element.BackgroundImage;
			if (backgroundImage != null)
			{
                // backgroundImage
                element.SetValue(_backgroundProperty, new ImageBrush(new Bitmap(backgroundImage)));
            }
            else
			{
				Color backgroundColor = Element.BackgroundColor;
				if (!backgroundColor.IsDefault)
				{
					element.SetValue(_backgroundProperty, backgroundColor.ToBrush());
				}
				else
				{
					object localBackground = element.GetValue(_backgroundProperty);
					if (localBackground != null && localBackground != PerspexProperty.UnsetValue)
						element.ClearValue(_backgroundProperty);
				}
			}

			_backgroundNeedsUpdate = false;
		}
	}
}