using Perspex.Media.Imaging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public sealed class ImageLoaderSourceHandler : IImageSourceHandler
    {
        public async Task<IBitmap> LoadImageAsync(ImageSource imagesoure, CancellationToken cancellationToken = new CancellationToken())
        {
            var imageLoader = imagesoure as UriImageSource;
            if (imageLoader?.Uri == null)
                return null;

            Stream streamImage = await imageLoader.GetStreamAsync(cancellationToken);
            if (streamImage == null || !streamImage.CanRead)
            {
                return null;
            }

            try
            {
                var image = new Bitmap(streamImage);
                return image;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                // Because this literally throws System.Exception
                // According to https://msdn.microsoft.com/library/windows/apps/jj191522
                // this can happen if the image data is bad or the app is close to its 
                // memory limit
                return null;
            }
        }
    }
}