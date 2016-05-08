using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    internal class DesktopDeviceInfo : DeviceInfo
    {
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
                throw new NotImplementedException();
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
