using Perspex.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class DesktopWindow : DesktopBaseWindow
    {
        protected override Platform CreatePlatform()
        {
            return new WindowsPlatform(this);
        }
    }
}
