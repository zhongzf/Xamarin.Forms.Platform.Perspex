using Perspex.Media.Imaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public sealed class FileImageSourceHandler : IImageSourceHandler
	{
		public Task<IBitmap> LoadImageAsync(ImageSource imagesoure, CancellationToken cancellationToken = new CancellationToken())
		{
            IBitmap image = null;
			var filesource = imagesoure as FileImageSource;
			if (filesource != null)
			{
				string file = filesource.File;
				image = new Bitmap(file);
			}

			return Task.FromResult(image);
		}
	}
}