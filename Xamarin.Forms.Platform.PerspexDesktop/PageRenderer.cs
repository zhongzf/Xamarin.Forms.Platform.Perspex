using Perspex.Controls;
using Perspex.Controls.Primitives;
using Perspex.Interactivity;
using System.Collections.ObjectModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class PageRenderer : VisualElementRenderer<Page, Perspex.Controls.StackPanel>
    {
        bool _disposed;

        bool _loaded;

        protected override void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
                return;

            _disposed = true;

            if (Element != null)
            {
                ReadOnlyCollection<Element> children = Element.LogicalChildren;
                for (var i = 0; i < children.Count; i++)
                {
                    var visualChild = children[i] as VisualElement;
                    visualChild?.Cleanup();
                }
                Element?.SendDisappearing();
            }

            base.Dispose();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.SendDisappearing();
            }

            if (e.NewElement != null)
            {
                if (e.OldElement == null)
                {
                    //Loaded += OnLoaded;
                    //Unloaded += OnUnloaded;
                    AttachedToVisualTree += PageRenderer_AttachedToVisualTree;
                    DetachedFromVisualTree += PageRenderer_DetachedFromVisualTree;

                    Tracker = new BackgroundTracker<Perspex.Controls.StackPanel>(TemplatedControl.BackgroundProperty);
                }

                if (_loaded)
                    e.NewElement.SendAppearing();
            }
        }

        private void PageRenderer_AttachedToVisualTree(object sender, Perspex.VisualTreeAttachmentEventArgs e)
        {
            OnLoaded(sender, new RoutedEventArgs());
        }

        private void PageRenderer_DetachedFromVisualTree(object sender, Perspex.VisualTreeAttachmentEventArgs e)
        {
            OnUnloaded(sender, new RoutedEventArgs());
        }

        void OnLoaded(object sender, RoutedEventArgs args)
        {
            var carouselPage = Element?.Parent as CarouselPage;
            if (carouselPage != null && carouselPage.Children[0] != Element)
            {
                return;
            }
            _loaded = true;
            Element?.SendAppearing();
        }

        void OnUnloaded(object sender, RoutedEventArgs args)
        {
            _loaded = false;
            Element?.SendDisappearing();
        }
    }
}