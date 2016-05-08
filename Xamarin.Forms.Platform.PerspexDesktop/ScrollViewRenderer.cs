using System;
using System.ComponentModel;
using Perspex;
using Perspex.Controls;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class ScrollViewRenderer : ViewRenderer<ScrollView, Perspex.Controls.ScrollViewer>
	{
        VisualElement _currentView;

        public ScrollViewRenderer()
        {
            AutoPackage = false;
        }

        protected IScrollViewController Controller
        {
            get { return Element; }
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            SizeRequest result = base.GetDesiredSize(widthConstraint, heightConstraint);
            result.Minimum = new Size(40, 40);
            return result;
        }

        protected override Perspex.Size ArrangeOverride(Perspex.Size finalSize)
        {
            if (Element == null)
                return finalSize;

            Element.IsInNativeLayout = true;

            Control?.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));

            Element.IsInNativeLayout = false;

            return finalSize;
        }

        protected override Perspex.Size MeasureOverride(Perspex.Size availableSize)
        {
            if (Element == null)
                return new Perspex.Size(0, 0);

            double width = Math.Max(0, Element.Width);
            double height = Math.Max(0, Element.Height);
            var result = new Perspex.Size(width, height);

            Control?.Measure(result);

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                //Control.ViewChanged -= OnViewChanged;
            }

            base.Dispose(disposing);
        }


        protected override void OnElementChanged(ElementChangedEventArgs<ScrollView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                ((IScrollViewController)e.OldElement).ScrollToRequested -= OnScrollToRequested;
            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var scrollViewer = new Perspex.Controls.ScrollViewer
                    {
                        HorizontalScrollBarVisibility = Perspex.Controls.Primitives.ScrollBarVisibility.Auto,
                        VerticalScrollBarVisibility = Perspex.Controls.Primitives.ScrollBarVisibility.Auto
                    };
                    SetNativeControl(scrollViewer);

                    //Control.ViewChanged += OnViewChanged;
                }

                Controller.ScrollToRequested += OnScrollToRequested;

                UpdateOrientation();

                LoadContent();
            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Content")
                LoadContent();
            else if (e.PropertyName == Layout.PaddingProperty.PropertyName)
                UpdateMargins();
            else if (e.PropertyName == ScrollView.OrientationProperty.PropertyName)
                UpdateOrientation();
        }

        void LoadContent()
        {
            if (_currentView != null)
            {
                _currentView.Cleanup();
            }

            _currentView = Element.Content;

            IVisualElementRenderer renderer = null;
            if (_currentView != null)
            {
                renderer = _currentView.GetOrCreateRenderer();
            }

            Control.Content = renderer != null ? renderer.ContainerElement : null;

            UpdateMargins();
        }

        void OnScrollToRequested(object sender, ScrollToRequestedEventArgs e)
        {
            double x = e.ScrollX, y = e.ScrollY;

            ScrollToMode mode = e.Mode;
            if (mode == ScrollToMode.Element)
            {
                Point pos = Controller.GetScrollPositionForElement((VisualElement)e.Element, e.Position);
                x = pos.X;
                y = pos.Y;
                mode = ScrollToMode.Position;
            }

            if (mode == ScrollToMode.Position)
            {
                //Control.ChangeView(x, y, null, !e.ShouldAnimate);
                Control.Offset = new Vector(x, y);
            }
        }

        //void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        //{
        //    Controller.SetScrolledPosition(Control.HorizontalOffset, Control.VerticalOffset);

        //    if (!e.IsIntermediate)
        //        Controller.SendScrollFinished();
        //}

        void UpdateMargins()
        {
            var element = Control.Content as Control;
            if (element == null)
                return;

            switch (Element.Orientation)
            {
                case ScrollOrientation.Horizontal:
                    // need to add left/right margins
                    element.Margin = new Perspex.Thickness(Element.Padding.Left, 0, Element.Padding.Right, 0);
                    break;
                case ScrollOrientation.Vertical:
                    // need to add top/bottom margins
                    element.Margin = new Perspex.Thickness(0, Element.Padding.Top, 0, Element.Padding.Bottom);
                    break;
                case ScrollOrientation.Both:
                    // need to add all margins
                    element.Margin = new Perspex.Thickness(Element.Padding.Left, Element.Padding.Top, Element.Padding.Right, Element.Padding.Bottom);
                    break;
            }
        }

        void UpdateOrientation()
        {
            if (Element.Orientation == ScrollOrientation.Horizontal || Element.Orientation == ScrollOrientation.Both)
            {
                Control.HorizontalScrollBarVisibility = Perspex.Controls.Primitives.ScrollBarVisibility.Auto;
            }
            else
            {
                Control.HorizontalScrollBarVisibility = Perspex.Controls.Primitives.ScrollBarVisibility.Auto;
            }
        }
    }
}