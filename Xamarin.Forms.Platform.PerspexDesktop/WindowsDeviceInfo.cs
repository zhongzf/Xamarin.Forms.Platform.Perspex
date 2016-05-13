using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    // TODO:
    internal class WindowsDeviceInfo : DeviceInfo
    {
        DesktopWindow _window;

        public WindowsDeviceInfo(DesktopWindow window)
        {
            _window = window;
        }

        public override Size PixelScreenSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Size ScaledScreenSize
        {
            get
            {
                var windowSize = _window.Bounds;
                return new Size(windowSize.Width, windowSize.Height);
            }
        }

        public override double ScalingFactor
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
