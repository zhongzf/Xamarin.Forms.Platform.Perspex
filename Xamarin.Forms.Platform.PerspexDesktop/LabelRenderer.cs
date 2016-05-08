using Perspex.Controls;
using System;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
	public class LabelRenderer : ViewRenderer<Label, Perspex.Controls.TextBlock>
	{
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var textBlock = new Perspex.Controls.TextBlock();
                    SetNativeControl(textBlock);
                }

                UpdateText(Control);
                UpdateColor(Control);
                UpdateAlign(Control);
                UpdateFont(Control);
                UpdateLineBreakMode(Control);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Label.TextProperty.PropertyName)
                UpdateText(Control);
            else if (e.PropertyName == Label.TextColorProperty.PropertyName)
                UpdateColor(Control);
            else if (e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName || e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName)
                UpdateAlign(Control);
            else if (e.PropertyName == Label.FontProperty.PropertyName)
                UpdateFont(Control);
            else if (e.PropertyName == Label.LineBreakModeProperty.PropertyName)
                UpdateLineBreakMode(Control);

            base.OnElementPropertyChanged(sender, e);
        }

        void UpdateAlign(TextBlock textBlock)
        {
            if (textBlock == null)
                return;

            Label label = Element;
            if (label == null)
                return;

            textBlock.TextAlignment = label.HorizontalTextAlignment.ToNativeTextAlignment();
            textBlock.VerticalAlignment = label.VerticalTextAlignment.ToNativeVerticalAlignment();
        }

        void UpdateColor(TextBlock textBlock)
        {
            if (textBlock == null)
                return;

            Label label = Element;
            if (label != null && label.TextColor != Color.Default)
            {
                textBlock.Foreground = label.TextColor.ToBrush();
            }
            else
            {
                textBlock.ClearValue(TextBlock.ForegroundProperty);
            }
        }

        void UpdateFont(TextBlock textBlock)
        {
            if (textBlock == null)
                return;
        }

        void UpdateLineBreakMode(TextBlock textBlock)
        {
            if (textBlock == null)
                return;

            switch (Element.LineBreakMode)
            {
                case LineBreakMode.NoWrap:
                    textBlock.TextWrapping = TextWrapping.NoWrap;
                    break;
                case LineBreakMode.WordWrap:
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    break;
                case LineBreakMode.CharacterWrap:
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    break;
                case LineBreakMode.HeadTruncation:
                    textBlock.TextWrapping = TextWrapping.NoWrap;
                    break;
                case LineBreakMode.TailTruncation:
                    textBlock.TextWrapping = TextWrapping.NoWrap;
                    break;
                case LineBreakMode.MiddleTruncation:
                    textBlock.TextWrapping = TextWrapping.NoWrap;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void UpdateText(TextBlock textBlock)
        {
            if (textBlock == null)
                return;

            Label label = Element;
            if (label != null)
            {
                FormattedString formatted = label.FormattedText;

                if (formatted == null)
                {
                    textBlock.Text = label.Text ?? string.Empty;
                }
                else
                {
                    //textBlock.Inlines.Clear();

                    //for (var i = 0; i < formatted.Spans.Count; i++)
                    //{
                    //    textBlock.Inlines.Add(formatted.Spans[i].ToRun());
                    //}
                }
            }
        }

    }
}