using Perspex.Controls;
using Perspex.Input;
using Perspex.Media;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class EntryRenderer : ViewRenderer<Entry, Perspex.Controls.TextBox>
	{
        IBrush _backgroundColorFocusedDefaultBrush;

        bool _fontApplied;
        IBrush _placeholderDefaultBrush;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var textBox = new Perspex.Controls.TextBox();
                    SetNativeControl(textBox);

                    textBox.TextInput += OnNativeTextChanged;
                    textBox.KeyUp += TextBoxOnKeyUp;
                }

                UpdateIsPassword();
                UpdateText();
                UpdatePlaceholder();
                UpdateTextColor();
                UpdateFont();
                UpdateInputScope();
                UpdateAlignment();
                UpdatePlaceholderColor();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Control != null)
            {
                Control.TextInput -= OnNativeTextChanged;
                Control.KeyUp -= TextBoxOnKeyUp;
            }

            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Entry.TextProperty.PropertyName)
                UpdateText();
            else if (e.PropertyName == Entry.IsPasswordProperty.PropertyName)
                UpdateIsPassword();
            else if (e.PropertyName == Entry.PlaceholderProperty.PropertyName)
                UpdatePlaceholder();
            else if (e.PropertyName == Entry.TextColorProperty.PropertyName)
                UpdateTextColor();
            else if (e.PropertyName == InputView.KeyboardProperty.PropertyName)
                UpdateInputScope();
            else if (e.PropertyName == Entry.FontAttributesProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.FontFamilyProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.FontSizeProperty.PropertyName)
                UpdateFont();
            else if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
                UpdateAlignment();
            else if (e.PropertyName == Entry.PlaceholderColorProperty.PropertyName)
                UpdatePlaceholderColor();
        }

        protected override void UpdateBackgroundColor()
        {
            base.UpdateBackgroundColor();

            if (Control == null)
            {
                return;
            }

            // By default some platforms have alternate default background colors when focused
            Color backgroundColor = Element.BackgroundColor;
            if (backgroundColor.IsDefault)
            {
                if (_backgroundColorFocusedDefaultBrush == null)
                {
                    return;
                }

                Control.Background = _backgroundColorFocusedDefaultBrush;
                return;
            }

            if (_backgroundColorFocusedDefaultBrush == null)
            {
                _backgroundColorFocusedDefaultBrush = Control.Background;
            }

            Control.Background = backgroundColor.ToBrush();
        }

        void OnNativeTextChanged(object sender, Perspex.Input.TextInputEventArgs args)
        {
            Element.SetValueCore(Entry.TextProperty, Control.Text);
        }

        void TextBoxOnKeyUp(object sender, Perspex.Input.KeyEventArgs args)
        {
            if (args?.Key != Key.Enter)
                return;

            ((IEntryController)Element).SendCompleted();
        }

        void UpdateAlignment()
        {
            Control.TextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment();
        }

        void UpdateFont()
        {
            if (Control == null)
                return;

            Entry entry = Element;

            if (entry == null)
                return;

            bool entryIsDefault = entry.FontFamily == null && entry.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Entry), true) && entry.FontAttributes == FontAttributes.None;

            if (entryIsDefault && !_fontApplied)
                return;

            if (entryIsDefault)
            {
                // ReSharper disable AccessToStaticMemberViaDerivedType
                // Resharper wants to simplify 'FormsTextBox' to 'Control', but then it'll conflict with the property 'Control'
                Control.ClearValue(TextBox.FontStyleProperty);
                Control.ClearValue(TextBox.FontSizeProperty);
                Control.ClearValue(TextBox.FontFamilyProperty);
                Control.ClearValue(TextBox.FontWeightProperty);
                //Control.ClearValue(FormsTextBox.FontStretchProperty);
                // ReSharper restore AccessToStaticMemberViaDerivedType
            }
            else
            {
                // TODO: ApplyFont
                //Control.ApplyFont(editor);
            }

            _fontApplied = true;
        }

        void UpdateInputScope()
        {
            // TODO: UpdateInputScope
            /*
            var custom = Element.Keyboard as CustomKeyboard;
            if (custom != null)
            {
                Control.IsTextPredictionEnabled = (custom.Flags & KeyboardFlags.Suggestions) != 0;
                Control.IsSpellCheckEnabled = (custom.Flags & KeyboardFlags.Spellcheck) != 0;
            }

            Control.InputScope = Element.Keyboard.ToInputScope();
            */
        }

        void UpdateIsPassword()
        {
            //Control.IsPassword = Element.IsPassword;
        }

        void UpdatePlaceholder()
        {
            //Control.PlaceholderText = Element.Placeholder ?? "";
        }

        void UpdatePlaceholderColor()
        {
            Color placeholderColor = Element.PlaceholderColor;

            // TODO: UpdatePlaceholderColor
            /*
            if (placeholderColor.IsDefault)
            {
                if (_placeholderDefaultBrush == null)
                {
                    return;
                }

                // Use the cached default brush
                Control.PlaceholderForegroundBrush = _placeholderDefaultBrush;
                return;
            }

            if (_placeholderDefaultBrush == null)
            {
                // Cache the default brush in case we need to set the color back to default
                _placeholderDefaultBrush = Control.PlaceholderForegroundBrush;
            }

            Control.PlaceholderForegroundBrush = placeholderColor.ToBrush();
            */
        }

        void UpdateText()
        {
            Control.Text = Element.Text ?? "";
        }

        void UpdateTextColor()
        {
            Control.Foreground = Element.TextColor.ToBrush();
        }
    }
}
 