using Perspex.Controls;
using Perspex.Interactivity;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class EditorRenderer : ViewRenderer<Editor, Perspex.Controls.TextBox>
    {
        bool _fontApplied;

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var textBox = new Perspex.Controls.TextBox();
                    SetNativeControl(textBox);

                    textBox.TextInput += OnNativeTextChanged;
                    textBox.LostFocus += OnLostFocus;
                }
                UpdateText();
                UpdateInputScope();
                UpdateTextColor();
                UpdateFont();
            }

            base.OnElementChanged(e);
        }

        private void TextBox_TextInput(object sender, Perspex.Input.TextInputEventArgs e)
        {
            OnNativeTextChanged(sender, e);
        }

        private void TextBox_LostFocus(object sender, Perspex.Interactivity.RoutedEventArgs e)
        {
            OnLostFocus(sender, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Control != null)
            {
                Control.TextInput -= OnNativeTextChanged;
                Control.LostFocus -= OnLostFocus;
            }

            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Editor.TextColorProperty.PropertyName)
            {
                UpdateTextColor();
            }
            else if (e.PropertyName == Editor.FontAttributesProperty.PropertyName)
            {
                UpdateFont();
            }
            else if (e.PropertyName == Editor.FontFamilyProperty.PropertyName)
            {
                UpdateFont();
            }
            else if (e.PropertyName == Editor.FontSizeProperty.PropertyName)
            {
                UpdateFont();
            }
            else if (e.PropertyName == Editor.TextProperty.PropertyName)
            {
                UpdateText();
            }

            base.OnElementPropertyChanged(sender, e);
        }

        void OnLostFocus(object sender, RoutedEventArgs e)
        {
            Element.SendCompleted();
        }

        void OnNativeTextChanged(object sender, Perspex.Input.TextInputEventArgs args)
        {
            Element.SetValueCore(Editor.TextProperty, Control.Text);
        }

        void UpdateFont()
        {
            if (Control == null)
                return;

            Editor editor = Element;

            if (editor == null)
                return;

            bool editorIsDefault = editor.FontFamily == null && editor.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Editor), true) && editor.FontAttributes == FontAttributes.None;

            if (editorIsDefault && !_fontApplied)
                return;

            if (editorIsDefault)
            {
                // ReSharper disable AccessToStaticMemberViaDerivedType
                // Resharper wants to simplify 'TextBox' to 'Control', but then it'll conflict with the property 'Control'
                Control.ClearValue(TextBox.FontStyleProperty);
                Control.ClearValue(TextBox.FontSizeProperty);
                Control.ClearValue(TextBox.FontFamilyProperty);
                Control.ClearValue(TextBox.FontWeightProperty);
                //Control.ClearValue(TextBox.FontStretchProperty);
                // ReSharper restore AccessToStaticMemberViaDerivedType
            }
            else
            {
                // ApplyFont
                Control.ApplyFont(editor);
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
            else
            {
                Control.ClearValue(TextBox.IsTextPredictionEnabledProperty);
                Control.ClearValue(TextBox.IsSpellCheckEnabledProperty);
            }

            Control.InputScope = Element.Keyboard.ToInputScope();
            */
        }

        void UpdateText()
        {
            string newText = Element.Text ?? "";

            if (Control.Text == newText)
            {
                return;
            }

            Control.Text = newText;
            Control.SelectionStart = Control.Text.Length;
        }

        void UpdateTextColor()
        {
            Color textColor = Element.TextColor;

            if (textColor.IsDefault || !Element.IsEnabled)
            {
                // ReSharper disable once AccessToStaticMemberViaDerivedType
                Control.ClearValue(TextBox.ForegroundProperty);
            }
            else
            {
                Control.Foreground = textColor.ToBrush();
            }
        }
    }
}