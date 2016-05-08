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

namespace XamarinFormsPerspexDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            InitializeLogging();

            var application = new Application();
            //var window = new Window();
            var window = new MainWindow();
            window.AttachDevTools();
            //var button = new Button
            //{
            //    Content = "TestButton",

            //};
            //button.Click += Button_Click;
            //var panel = new Panel();
            //panel.Children.Add(button);
            //window.Content = panel;

            window.Show();
            application.Run(window);

            //application
            //    .UseWin32()
            //    .UseDirect2D()
            //    .RunWithMainWindow<MainWindow>();           
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

        private static void Button_Click(object sender, Perspex.Interactivity.RoutedEventArgs e)
        {
            var alert = new Window();
            alert.Content = new TextBlock { Text = "alert" };
            alert.ShowDialog();
        }
    }
}
