using Perspex.Markup.Xaml;
using Perspex.Styling;
using Perspex.Themes.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsPerspexDemo
{
    public class Application : Perspex.Application
    {
        public Application()
        {
            RegisterServices();
            InitializeSubsystems((int)Environment.OSVersion.Platform);
            Styles.Add(new DefaultTheme());

            var loader = new PerspexXamlLoader();
            var baseLight = (IStyle)loader.Load(
                new Uri("resm:Perspex.Themes.Default.Accents.BaseLight.xaml?assembly=Perspex.Themes.Default"));
            Styles.Add(baseLight);
        }
    }
}
