using Perspex.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public abstract class Platform : IPlatform, INavigation
    {
        private Window _window;
        Page _currentPage;

        internal Platform(Window window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            _window = window;

            _container = new Canvas { };

            _window.Content = _container;
        }

        public IReadOnlyList<Page> ModalStack
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IReadOnlyList<Page> NavigationStack
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void InsertPageBefore(Page page, Page before)
        {
            throw new NotImplementedException();
        }

        public Task<Page> PopAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Page> PopAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        public Task<Page> PopModalAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Page> PopModalAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        public Task PopToRootAsync()
        {
            throw new NotImplementedException();
        }

        public Task PopToRootAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        public Task PushAsync(Page page)
        {
            throw new NotImplementedException();
        }

        public Task PushAsync(Page page, bool animated)
        {
            throw new NotImplementedException();
        }

        public Task PushModalAsync(Page page)
        {
            throw new NotImplementedException();
        }

        public Task PushModalAsync(Page page, bool animated)
        {
            throw new NotImplementedException();
        }

        public void RemovePage(Page page)
        {
            throw new NotImplementedException();
        }

        internal void SetPage(Page newRoot)
        {
            if (newRoot == null)
                throw new ArgumentNullException("newRoot");

            SetCurrent(newRoot, false, true);
        }

        #region Renderer
        internal static readonly BindableProperty RendererProperty = BindableProperty.CreateAttached("Renderer", typeof(IVisualElementRenderer), typeof(Platform), default(IVisualElementRenderer));

        public static IVisualElementRenderer GetRenderer(VisualElement element)
        {
            return (IVisualElementRenderer)element.GetValue(RendererProperty);
        }

        public static void SetRenderer(VisualElement element, IVisualElementRenderer value)
        {
            element.SetValue(RendererProperty, value);
            element.IsPlatformEnabled = value != null;
        }

        public static IVisualElementRenderer CreateRenderer(VisualElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            IVisualElementRenderer renderer = Registrar.Registered.GetHandler<IVisualElementRenderer>(element.GetType()) ?? new DefaultRenderer();
            renderer.SetElement(element);
            return renderer;
        }
        #endregion

        readonly Canvas _container;

        void SetCurrent(Page newPage, bool animated, bool popping = false, Action completedCallback = null)
        {
            if (newPage == _currentPage)
                return;

            newPage.Platform = this;

            if (_currentPage != null)
            {
                Page previousPage = _currentPage;
                IVisualElementRenderer previousRenderer = GetRenderer(previousPage);
                _container.Children.Remove(previousRenderer.ContainerElement);
            }

            newPage.Layout(new Rectangle(0, 0, _window.ClientSize.Width, _window.ClientSize.Height));

            IVisualElementRenderer pageRenderer = newPage.GetOrCreateRenderer();
            _container.Children.Add(pageRenderer.ContainerElement);

            pageRenderer.ContainerElement.Width = _container.Width;
            pageRenderer.ContainerElement.Height = _container.Height;

            if (completedCallback != null)
                completedCallback();

            _currentPage = newPage;
        }

        SizeRequest IPlatform.GetNativeSize(VisualElement element, double widthConstraint, double heightConstraint)
        {
            // Hack around the fact that Canvas ignores the child constraints.
            // It is entirely possible using Canvas as our base class is not wise.
            // FIXME: This should not be an if statement. Probably need to define an interface here.
            if (widthConstraint > 0 && heightConstraint > 0)
            {
                IVisualElementRenderer elementRenderer = GetRenderer(element);
                if (elementRenderer != null)
                    return elementRenderer.GetDesiredSize(widthConstraint, heightConstraint);
            }

            return new SizeRequest();
        }
    }
}
