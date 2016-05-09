using Perspex.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public abstract class DesktopBaseWindow : Window
    {
        private Application _application;

        public DesktopBaseWindow()
        {
            this.Activated += DesktopBaseWindow_Activated;
            this.Deactivated += DesktopBaseWindow_Deactivated;
        }

        private void DesktopBaseWindow_Activated(object sender, EventArgs e)
        {
            OnApplicationResuming(sender, e);
        }

        private void DesktopBaseWindow_Deactivated(object sender, EventArgs e)
        {
            OnApplicationSuspending(sender, e);
        }

        protected Platform Platform { get; private set; }

        protected abstract Platform CreatePlatform();

        protected void LoadApplication(Application application)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            _application = application;
            Application.Current = application;
            Platform = CreatePlatform();
            Platform.SetPage(Application.Current.MainPage);
            application.PropertyChanged += OnApplicationPropertyChanged;

            Application.Current.SendStart();
        }


        void OnApplicationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MainPage")
                Platform.SetPage(Application.Current.MainPage);
        }

        void OnApplicationResuming(object sender, object e)
        {
            Application.Current.SendResume();
        }

        async void OnApplicationSuspending(object sender, object e)
        {
            //SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            await Application.Current.SendSleepAsync();
            //deferral.Complete();
        }
    }
}
