using Perspex.Media.Imaging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public sealed class StreamImagesourceHandler : IImageSourceHandler
	{
		public async Task<IBitmap> LoadImageAsync(ImageSource imagesource, CancellationToken cancellationToken = new CancellationToken())
		{
			Bitmap bitmapimage = null;

			var streamsource = imagesource as StreamImageSource;
			if (streamsource != null && streamsource.Stream != null)
			{
				using (Stream stream = await ((IStreamImageSource)streamsource).GetStreamAsync(cancellationToken))
				{
					if (stream == null)
						return null;
					bitmapimage = new Bitmap(stream);
                    return bitmapimage;
				}
			}

			return bitmapimage;
		}
	}
}