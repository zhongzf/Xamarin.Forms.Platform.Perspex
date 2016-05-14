using Perspex.Media;
using System;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class SearchBarRenderer : ViewRenderer<SearchBar, Perspex.Controls.TextBox>
	{
		IBrush _defaultPlaceholderColorBrush;
		IBrush _defaultPlaceholderColorFocusBrush;
		IBrush _defaultTextColorBrush;
		IBrush _defaultTextColorFocusBrush;

		bool _fontApplied;

        Perspex.Controls.TextBox _queryTextBox;

		protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null)
				{
					SetNativeControl(new Perspex.Controls.TextBox { /*QueryIcon = new SymbolIcon(Symbol.Find)*/ });
					//Control.QuerySubmitted += OnQuerySubmitted;
					//Control.TextChanged += OnTextChanged;
					//Control.Loaded += OnControlLoaded;
				}

				UpdateText();
				UpdatePlaceholder();
				UpdateCancelButtonColor();
				UpdateAlignment();
				UpdateFont();
				UpdateTextColor();
				UpdatePlaceholderColor();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == SearchBar.TextProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == SearchBar.PlaceholderProperty.PropertyName)
				UpdatePlaceholder();
			else if (e.PropertyName == SearchBar.CancelButtonColorProperty.PropertyName)
				UpdateCancelButtonColor();
			else if (e.PropertyName == SearchBar.HorizontalTextAlignmentProperty.PropertyName)
				UpdateAlignment();
			else if (e.PropertyName == SearchBar.FontAttributesProperty.PropertyName)
				UpdateFont();
			else if (e.PropertyName == SearchBar.FontFamilyProperty.PropertyName)
				UpdateFont();
			else if (e.PropertyName == SearchBar.FontSizeProperty.PropertyName)
				UpdateFont();
			else if (e.PropertyName == SearchBar.TextColorProperty.PropertyName)
				UpdateTextColor();
			else if (e.PropertyName == SearchBar.PlaceholderColorProperty.PropertyName)
				UpdatePlaceholderColor();
		}

		//void OnControlLoaded(object sender, EventArgs routedEventArgs)
		//{
		//	_queryTextBox = Control.GetFirstDescendant<Perspex.Controls.TextBox>();

		//	UpdateAlignment();
		//	UpdateTextColor();
		//	UpdatePlaceholderColor();
		//}

		//void OnQuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
		//{
		//	((ISearchBarController)Element).OnSearchButtonPressed();
		//}

		//void OnTextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
		//{
		//	if (e.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange)
		//		return;

		//	((IElementController)Element).SetValueFromRenderer(SearchBar.TextProperty, sender.Text);
		//}

		void UpdateAlignment()
		{
			if (_queryTextBox == null)
				return;

			_queryTextBox.TextAlignment = Element.HorizontalTextAlignment.ToNativeTextAlignment();
		}

		void UpdateCancelButtonColor()
		{
            var foregroundBrush = DesktopResources.GetDefault("FormsCancelForegroundBrush") as SolidColorBrush;
            var backgroundBrush = DesktopResources.GetDefault("FormsCancelBackgroundBrush") as SolidColorBrush;

            Color cancelColor = Element.CancelButtonColor;

            //if (cancelColor.IsDefault)
            //{
            //    backgroundBrush.Color = (DesktopResources.GetDefault("TextBoxButtonBackgroundThemeBrush") as SolidColorBrush).Color;
            //    foregroundBrush.Color = (DesktopResources.GetDefault("SystemControlBackgroundChromeBlackMediumBrush") as SolidColorBrush).Color;
            //}
            //else
            //{
            //    Perspex.Media.Color newColor = cancelColor.ToWindowsColor();
            //    backgroundBrush.Color = newColor;
            //    foregroundBrush.Color = newColor.GetIdealForegroundForBackgroundColor();
            //}
        }

		void UpdateFont()
		{
			if (Control == null)
				return;

			SearchBar searchBar = Element;

			if (searchBar == null)
				return;

			bool searchBarIsDefault = searchBar.FontFamily == null && searchBar.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(SearchBar), true) && searchBar.FontAttributes == FontAttributes.None;

			if (searchBarIsDefault && !_fontApplied)
				return;

			//if (searchBarIsDefault)
			//{
			//	Control.ClearValue(Windows.UI.Xaml.Controls.Control.FontStyleProperty);
			//	Control.ClearValue(Windows.UI.Xaml.Controls.Control.FontSizeProperty);
			//	Control.ClearValue(Windows.UI.Xaml.Controls.Control.FontFamilyProperty);
			//	Control.ClearValue(Windows.UI.Xaml.Controls.Control.FontWeightProperty);
			//	Control.ClearValue(Windows.UI.Xaml.Controls.Control.FontStretchProperty);
			//}
			//else
			//	Control.ApplyFont(searchBar);

			_fontApplied = true;
		}

		void UpdatePlaceholder()
		{
			//Control.PlaceholderText = Element.Placeholder ?? string.Empty;
		}

		void UpdatePlaceholderColor()
		{
			if (_queryTextBox == null)
				return;

			Color placeholderColor = Element.PlaceholderColor;

            //if (placeholderColor.IsDefault)
            //{
            //    if (_defaultPlaceholderColorBrush == null)
            //        return;

            //    _queryTextBox.PlaceholderForegroundBrush = _defaultPlaceholderColorBrush;
            //    _queryTextBox.PlaceholderForegroundBrush = _defaultPlaceholderColorFocusBrush;
            //}

            //if (_defaultPlaceholderColorBrush == null)
            //{
            //    _defaultPlaceholderColorBrush = _queryTextBox.PlaceholderForegroundBrush;
            //    _defaultPlaceholderColorFocusBrush = _queryTextBox.PlaceholderForegroundFocusBrush;
            //}

            //_queryTextBox.PlaceholderForegroundBrush = placeholderColor.ToBrush();
            //_queryTextBox.PlaceholderForegroundFocusBrush = placeholderColor.ToBrush();
        }

		void UpdateText()
		{
			Control.Text = Element.Text ?? string.Empty;
		}

		void UpdateTextColor()
		{
			if (_queryTextBox == null)
				return;

			Color textColor = Element.TextColor;

			if (textColor.IsDefault)
			{
				if (_defaultTextColorBrush == null)
					return;

				_queryTextBox.Foreground = _defaultTextColorBrush;
				//_queryTextBox.ForegroundFocusBrush = _defaultTextColorFocusBrush;
			}

			if (_defaultTextColorBrush == null)
			{
				_defaultTextColorBrush = _queryTextBox.Foreground;
				//_defaultTextColorFocusBrush = _queryTextBox.ForegroundFocusBrush;
			}

			_queryTextBox.Foreground = textColor.ToBrush();
			//_queryTextBox.ForegroundFocusBrush = textColor.ToBrush();
		}
	}
}