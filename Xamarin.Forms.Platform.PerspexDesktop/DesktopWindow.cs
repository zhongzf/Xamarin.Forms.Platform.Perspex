using Perspex.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class DesktopWindow : Window
    {
        private Application _application;
        protected Platform Platform { get; private set; }

        protected virtual Platform CreatePlatform()
        {
            return new DesktopPlatform(this);
        }

        protected void LoadApplication(Application application)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            _application = application;
            Application.Current = application;
            Platform = CreatePlatform();
            Platform.SetPage(Application.Current.MainPage);
            application.PropertyChanged += OnApplicationPropertyChanged;
        }

        private void OnApplicationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {            
        }
    }
}
