using Perspex.VisualTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public sealed class VisualTreeHelper
    {
        public static IVisual GetParent(IVisual reference)
        {
            return reference.VisualParent;
        }
    }
}
