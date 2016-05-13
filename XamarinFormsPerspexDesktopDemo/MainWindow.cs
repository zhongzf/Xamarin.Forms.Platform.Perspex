using Perspex;
using Perspex.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.PerspexDesktop;

namespace XamarinFormsPerspexDemo
{
    public class MainWindow : DesktopWindow
    {
        private bool loaded = false;
        public MainWindow()
        {
            Forms.Init(this);

            this.Activated += MainWindow_Activated;
            //this.AttachDevTools();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            if (!loaded)
            {
                //LoadApplication(new XamarinFormsApp.App());
                LoadApplication(new PrismUnityDemoApp.App());
                loaded = true;
            }
        }
    }
}
