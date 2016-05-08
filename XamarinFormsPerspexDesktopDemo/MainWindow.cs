using Perspex;
using Perspex.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.PerspexDesktop;

namespace XamarinFormsPerspexDemo
{
    public class MainWindow : DesktopWindow
    {
        private bool loaded = false;
        public MainWindow()
        {
            this.Activated += MainWindow_Activated;
            //this.AttachDevTools();
            if (!loaded)
            {
                LoadApplication(new XamarinFormsApp.App());
                loaded = true;
            }
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
        }
    }
}
