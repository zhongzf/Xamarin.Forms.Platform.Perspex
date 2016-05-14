using Perspex.Controls;
using Perspex.Interactivity;
using Perspex.Media;
using Perspex.Media.Imaging;
using System;
using System.ComponentModel;
using Perspex.Layout;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class ButtonRenderer : ViewRenderer<Button, Perspex.Controls.Button>
    {
        bool _fontApplied;

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
            // TODO: BorderRadius
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

            var image = new Perspex.Controls.Image
            {
                Source = new Bitmap(elementImage.File),
                Width = 30.0,
                Height = 30.0,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // No text, just the image
            if (string.IsNullOrEmpty(text))
            {
                Control.Content = image;
                return;
            }

            // Both image and text, so we need to build a container for them
            var layout = Element.ContentLayout;
            var container = new StackPanel();
            var textBlock = new TextBlock
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var spacing = layout.Spacing;

            container.HorizontalAlignment = HorizontalAlignment.Center;
            container.VerticalAlignment = VerticalAlignment.Center;

            switch (layout.Position)
            {
                case Button.ButtonContentLayout.ImagePosition.Top:
                    container.Orientation = Orientation.Vertical;
                    image.Margin = new Perspex.Thickness(0, 0, 0, spacing);
                    container.Children.Add(image);
                    container.Children.Add(textBlock);
                    break;
                case Button.ButtonContentLayout.ImagePosition.Bottom:
                    container.Orientation = Orientation.Vertical;
                    image.Margin = new Perspex.Thickness(0, spacing, 0, 0);
                    container.Children.Add(textBlock);
                    container.Children.Add(image);
                    break;
                case Button.ButtonContentLayout.ImagePosition.Right:
                    container.Orientation = Orientation.Horizontal;
                    image.Margin = new Perspex.Thickness(spacing, 0, 0, 0);
                    container.Children.Add(textBlock);
                    container.Children.Add(image);
                    break;
                default:
                    // Defaults to image on the left
                    container.Orientation = Orientation.Horizontal;
                    image.Margin = new Perspex.Thickness(0, 0, spacing, 0);
                    container.Children.Add(image);
                    container.Children.Add(textBlock);
                    break;
            }

            Control.Content = container;

        }

        void UpdateFont()
        {
            if (Control == null || Element == null)
                return;

            if (Element.Font == Font.Default && !_fontApplied)
                return;

            Font fontToApply = Element.Font == Font.Default ? Font.SystemFontOfSize(NamedSize.Medium) : Element.Font;

            Control.ApplyFont(fontToApply);
            _fontApplied = true;
        }

        void UpdateTextColor()
        {
            var textColor = Element.TextColor;
            Control.Foreground = textColor.ToBrush();
        }
    }
}