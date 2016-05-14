using Perspex.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class DesktopResources
    {
        private static ResourceDictionary _defaultResources;

        internal static ResourceDictionary GetDefaultResources()
        {
            if (_defaultResources == null)
            {
                _defaultResources = new ResourceDictionary();
                _defaultResources.Add("ContentControlThemeFontFamily", "Segoe UI");
                _defaultResources.Add("ControlContentThemeFontSize", 22.667);
                _defaultResources.Add("FormsCancelForegroundBrush", Color.White.ToBrush());
                _defaultResources.Add("FormsCancelBackgroundBrush", Color.White.ToBrush());
                _defaultResources.Add("TextBoxButtonBackgroundThemeBrush", Color.White.ToBrush());
                _defaultResources.Add("SystemControlBackgroundChromeBlackMediumBrush", Color.White.ToBrush());
                // TODO:
                _defaultResources.Add("ListViewHeaderTextCell", null);
                _defaultResources.Add("ListViewTextCell", null);
                _defaultResources.Add("TextCell", null);
                _defaultResources.Add("EntryCell", null);
                _defaultResources.Add("ViewCell", null);
                _defaultResources.Add("SwitchCell", null);
                _defaultResources.Add("ListImageCell", null);
                _defaultResources.Add("ImageCell", null);

            }
            return _defaultResources;
        }

        public static object GetDefault(string name)
        {
            if(Application.Current.Resources.ContainsKey(name))
            {
                return Application.Current.Resources[name];
            }
            else
            {
                return GetDefaultResources()[name];
            }
        }
    }
}
