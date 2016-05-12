using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class ListViewRenderer : ViewRenderer<ListView, Perspex.Controls.Control>
	{
        protected Perspex.Controls.ListBox List { get; private set; }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				e.OldElement.ItemSelected -= OnElementItemSelected;
				e.OldElement.ScrollToRequested -= OnElementScrollToRequested;
			}

			if (e.NewElement != null)
			{
				e.NewElement.ItemSelected += OnElementItemSelected;
                e.NewElement.ScrollToRequested += OnElementScrollToRequested;

				if (List == null)
				{
					List = new Perspex.Controls.ListBox
					{
					};

                    // In order to support tapping on elements within a list item, we handle
                    // ListView.Tapped (which can be handled by child elements in the list items
                    // and prevented from bubbling up) rather than ListView.ItemClick
                    List.Tapped += ListOnTapped;

                    if (ShouldCustomHighlight)
                    {
                        List.SelectionChanged += OnControlSelectionChanged;
                    }
                }

                // WinRT throws an exception if you set ItemsSource directly to a CVS, so bind it.
                //List.DataContext = new CollectionViewSource { Source = Element.ItemsSource, IsSourceGrouped = Element.IsGroupingEnabled };
                // TODO: DataContext

                UpdateGrouping();
				UpdateHeader();
				UpdateFooter();
				ClearSizeEstimate();
			}
		}


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ListView.IsGroupingEnabledProperty.PropertyName)
			{
				UpdateGrouping();
			}
			else if (e.PropertyName == ListView.HeaderProperty.PropertyName)
			{
				UpdateHeader();
			}
			else if (e.PropertyName == ListView.FooterProperty.PropertyName)
			{
				UpdateFooter();
			}
			else if (e.PropertyName == ListView.RowHeightProperty.PropertyName)
			{
				ClearSizeEstimate();
			}
			else if (e.PropertyName == ListView.HasUnevenRowsProperty.PropertyName)
			{
				ClearSizeEstimate();
			}
			else if (e.PropertyName == ListView.ItemTemplateProperty.PropertyName)
			{
				ClearSizeEstimate();
			}
			else if (e.PropertyName == ListView.ItemsSourceProperty.PropertyName)
			{
				ClearSizeEstimate();
                //((CollectionViewSource)List.DataContext).Source = Element.ItemsSource;
                // TODO: DataContext
            }
        }

        private void List_Tapped(object sender, Perspex.Interactivity.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void List_SelectionChanged(object sender, Perspex.Controls.SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void OnElementScrollToRequested(object sender, ScrollToRequestedEventArgs e)
        {
            ScrollTo(e.Group, e.Item, e.Position, e.ShouldAnimate);
        }

        protected override void Dispose(bool disposing)
		{
			if (List != null)
			{
				List.Tapped -= List_Tapped;

				if (ShouldCustomHighlight)
				{
					List.SelectionChanged -= List_SelectionChanged;
				}

				List.DataContext = null;
				List = null;
			}

            base.Dispose(disposing);
		}


		bool ShouldCustomHighlight
		{
			get
			{
				return Device.Idiom == TargetIdiom.Phone;
			}
		}

		void ClearSizeEstimate()
		{
			//Element.ClearValue(CellControl.MeasuredEstimateProperty);
		}

		void UpdateFooter()
		{
			//List.Footer = ((IListViewController)Element).FooterElement;
		}

		void UpdateHeader()
		{
			//List.Header = ((IListViewController)Element).HeaderElement;
		}

		void UpdateGrouping()
		{
		}


		void OnElementItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (Element == null)
				return;

			if (_deferSelection)
			{
				// If we get more than one of these, that's okay; we only want the latest one
				_deferredSelectedItemChangedEvent = new Tuple<object, SelectedItemChangedEventArgs>(sender, e);
				return;
			}

			if (e.SelectedItem == null)
			{
				List.SelectedIndex = -1;
				return;
			}

			var index = 0;
			if (Element.IsGroupingEnabled)
			{
				int selectedItemIndex = Element.TemplatedItems.GetGlobalIndexOfItem(e.SelectedItem);
				var leftOver = 0;
				int groupIndex = Element.TemplatedItems.GetGroupIndexFromGlobal(selectedItemIndex, out leftOver);

				index = selectedItemIndex - (groupIndex + 1);
			}
			else
			{
				index = Element.TemplatedItems.GetGlobalIndexOfItem(e.SelectedItem);
			}

			List.SelectedIndex = index;
		}

		void ListOnTapped(object sender, Perspex.Interactivity.RoutedEventArgs args)
		{
			//var orig = args.Source as DependencyObject;
			//int index = -1;

			//// Work our way up the tree until we find the actual list item 
			//// the user tapped on
			//while (orig != null && orig != List)
			//{
			//	var lv = orig as ListViewItem;

			//	if (lv != null)
			//	{
			//		index = Element.TemplatedItems.GetGlobalIndexOfItem(lv.Content);
			//		break;
			//	}

			//	orig = VisualTreeHelper.GetParent(orig);
			//}

			//if (index > -1)
			//{
			//	OnListItemClicked(index);
			//}
		}

		void OnListItemClicked(int index)
		{
		}

		void OnControlSelectionChanged(object sender, Perspex.Controls.SelectionChangedEventArgs e)
		{
            // TODO: OnControlSelectionChanged
        }

        async void ScrollTo(object group, object item, ScrollToPosition toPosition, bool shouldAnimate, bool includeGroup = false, bool previouslyFailed = false)
        {
            // TODO: ScrollTo
        }

        bool _deferSelection = false;
        Tuple<object, SelectedItemChangedEventArgs> _deferredSelectedItemChangedEvent;
    }
}