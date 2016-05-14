using Perspex.Media;
using Perspex.Media.Imaging;
using System;
using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class ImageRenderer : ViewRenderer<Image, Perspex.Controls.Image>
	{
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var image = new Perspex.Controls.Image();
                    image.Initialized += Image_Initialized;
                    SetNativeControl(image);
                }

                UpdateSource();
                UpdateAspect();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Image.SourceProperty.PropertyName)
                UpdateSource();
            else if (e.PropertyName == Image.AspectProperty.PropertyName)
                UpdateAspect();
        }

        private void Image_Initialized(object sender, EventArgs e)
        {
            OnImageOpened(this, e);
        }

        static Stretch GetStretch(Aspect aspect)
        {
            switch (aspect)
            {
                case Aspect.Fill:
                    return Stretch.Fill;
                case Aspect.AspectFill:
                    return Stretch.UniformToFill;
                default:
                case Aspect.AspectFit:
                    return Stretch.Uniform;
            }
        }

        void OnImageOpened(object sender, EventArgs routedEventArgs)
        {
            if (_measured)
            {
                RefreshImage();
            }
        }

        void RefreshImage()
        {
            ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.RendererReady);
        }

        void UpdateAspect()
        {
            Control.Stretch = GetStretch(Element.Aspect);
        }

        async void UpdateSource()
        {
            ((IImageController)Element).SetIsLoading(true);

            ImageSource source = Element.Source;
            IImageSourceHandler handler;
            if (source != null && (handler = Registrar.Registered.GetHandler<IImageSourceHandler>(source.GetType())) != null)
            {
                IBitmap imagesource;
                try
                {
                    imagesource = await handler.LoadImageAsync(source);
                }
                catch (OperationCanceledException)
                {
                    imagesource = null;
                }

                // In the time it takes to await the imagesource, some zippy little app
                // might have disposed of this Image already.
                if (Control != null)
                    Control.Source = imagesource;

                RefreshImage();
            }
            else
                Control.Source = null;

            ((IImageController)Element)?.SetIsLoading(false);
        }

        bool _measured;

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (Control.Source == null)
                return new SizeRequest();

            _measured = true;

            var result = new Size { Width = ((Bitmap)Control.Source).PixelWidth, Height = ((Bitmap)Control.Source).PixelHeight };

            return new SizeRequest(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.Initialized -= Image_Initialized;
            }

            base.Dispose(disposing);
        }
    }
}