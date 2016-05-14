using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perspex;
using Perspex.Controls;
using Perspex.Diagnostics;
using Serilog;
using Perspex.Logging.Serilog;
using Xamarin.Forms.Platform.PerspexDesktop;

namespace XamarinFormsPerspexDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            InitializeLogging();

            //DesktopApplication.Run(typeof(XamarinFormsApp.App));
            DesktopApplication.Run(typeof(PrismUnityDemoApp.App), true);  
            //var application = new DesktopApplication();
            //var window = new Window1();
            //window.Show();
            //application.Run(window);
        }

        private static void InitializeLogging()
        {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
        }
    }
}
