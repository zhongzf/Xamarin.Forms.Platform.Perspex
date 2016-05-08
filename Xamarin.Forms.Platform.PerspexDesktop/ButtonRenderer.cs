using Perspex.Interactivity;
using Perspex.Media;
using System;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class ButtonRenderer : ViewRenderer<Button, Perspex.Controls.Button>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var button = new Perspex.Controls.Button();
                    button.Click += OnButtonClick;
                    SetNativeControl(button);
                }

                UpdateContent();

                if (Element.BackgroundColor != Color.Default)
                    UpdateBackground();

                if (Element.TextColor != Color.Default)
                    UpdateTextColor();

                if (Element.BorderColor != Color.Default)
                    UpdateBorderColor();

                if (Element.BorderWidth != 0)
                    UpdateBorderWidth();

                if (Element.BorderRadius != (int)Button.BorderRadiusProperty.DefaultValue)
                    UpdateBorderRadius();

                UpdateFont();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Button.TextProperty.PropertyName || e.PropertyName == Button.ImageProperty.PropertyName)
            {
                UpdateContent();
            }
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
            {
                UpdateBackground();
            }
            else if (e.PropertyName == Button.TextColorProperty.PropertyName)
            {
                UpdateTextColor();
            }
            else if (e.PropertyName == Button.FontProperty.PropertyName)
            {
                UpdateFont();
            }
            else if (e.PropertyName == Button.BorderColorProperty.PropertyName)
            {
                UpdateBorderColor();
            }
            else if (e.PropertyName == Button.BorderWidthProperty.PropertyName)
            {
                UpdateBorderWidth();
            }
            else if (e.PropertyName == Button.BorderRadiusProperty.PropertyName)
            {
                UpdateBorderRadius();
            }
        }

        void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button buttonView = Element;
            if (buttonView != null)
                ((IButtonController)buttonView).SendClicked();
        }

        void UpdateBackground()
        {
            var backgroudColor = Element.BackgroundColor;
            Control.Background = backgroudColor.ToBrush();
        }

        void UpdateBorderColor()
        {
            var borderColor = Element.BorderColor;
            Control.BorderBrush = borderColor.ToBrush();
        }

        void UpdateBorderRadius()
        {
            var borderRadius = Element.BorderRadius;
            // TODO:
        }

        void UpdateBorderWidth()
        {
            var borderWidth = Element.BorderWidth;
            Control.BorderThickness = borderWidth;
        }

        void UpdateContent()
        {
            var text = Element.Text;
            var elementImage = Element.Image;

            // No image, just the text
            if (elementImage == null)
            {
                Control.Content = text;
                return;
            }
        }

        void UpdateFont()
        {
            // TODO:
        }

        void UpdateTextColor()
        {
            var textColor = Element.TextColor;
            Control.Foreground = textColor.ToBrush();
        }

    }
}