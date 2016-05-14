using Perspex;
using Perspex.Controls;
using Perspex.Diagnostics;
using Perspex.Markup.Xaml;
using Perspex.Themes.Default;
using System;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class DesktopApplication : Perspex.Application
    {
        public DesktopApplication()
        {
            RegisterServices();
            InitializeSubsystems((int)Environment.OSVersion.Platform);

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var loader = new PerspexXamlLoader();

            Styles.Add(new DefaultTheme());
            var baseLight = (Perspex.Styling.IStyle)loader.Load(
                new Uri("resm:Perspex.Themes.Default.Accents.BaseLight.xaml?assembly=Perspex.Themes.Default"));
            Styles.Add(baseLight);

            loader.Load(typeof(DesktopApplication), this);
        }

        public void LoadApplication(Type applicationType)
        {
            var window = new DesktopWindow();
            Forms.Init(window);
            var application = (Xamarin.Forms.Application)Activator.CreateInstance(applicationType);
            window.LoadApplication(application);
            window.Show();
            this.Run(window);
        }

        public static void Run(Type applicationType)
        {
            var application = new DesktopApplication();
            application.LoadApplication(applicationType);
        }
    }
}