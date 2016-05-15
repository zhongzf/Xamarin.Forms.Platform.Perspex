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
        }

#if DEBUG
        public void LoadApplication(Type applicationType, bool debugMode = true, bool loadWhenActivated = false)
#else
        public void LoadApplication(Type applicationType, bool debugMode = false, bool loadWhenActivated = false)
#endif
        {
            var window = new DesktopWindow();
            if (loadWhenActivated)
            {
                window.Activated += (sender, e) =>
                {
                    var application = (Xamarin.Forms.Application)Activator.CreateInstance(applicationType);
                    window.LoadApplication(application);
                };
            }
            Forms.Init(window);
            if(debugMode)
            {
                window.AttachDevTools();
            }
            if(!loadWhenActivated)
            {
                var application = (Xamarin.Forms.Application)Activator.CreateInstance(applicationType);
                window.LoadApplication(application);
            }
            window.Show();
            this.Run(window);
        }

#if DEBUG
        public static void Run(Type applicationType, bool debugMode = true, bool loadWhenActivated = false)
#else
        public static void Run(Type applicationType, bool debugMode = false, bool loadWhenActivated = false)
#endif
        {
            var application = new DesktopApplication();
            application.LoadApplication(applicationType, debugMode, loadWhenActivated);
        }
    }
}