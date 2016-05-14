using Perspex.Media.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public interface IImageSourceHandler : IRegisterable
	{
		Task<IBitmap> LoadImageAsync(ImageSource imagesoure, CancellationToken cancellationToken = default(CancellationToken));
	}
}