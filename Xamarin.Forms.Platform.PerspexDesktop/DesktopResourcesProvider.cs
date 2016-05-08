using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    internal sealed class DesktopResourcesProvider : ISystemResourcesProvider
    {
        public IResourceDictionary GetSystemResources()
        {
            var resources = new ResourceDictionary();
            // TODO: Resources
            return resources;
        }
    }
}
