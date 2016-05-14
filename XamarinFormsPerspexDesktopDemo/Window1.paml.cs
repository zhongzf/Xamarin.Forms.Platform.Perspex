using Perspex.Controls;
using Perspex.Controls.Templates;
using Perspex.Markup.Xaml;
using System.Collections.Generic;

namespace XamarinFormsPerspexDemo
{
    public class Window1 : Window
    {
        public Window1()
        {
            this.InitializeComponent();
            //App.AttachDevTools(this);
            var mainPanel = this.FindControl<Panel>("MainPanel");
            var listBox = new ListBox
            {
                DataTemplates = new DataTemplates {
                    new FuncDataTemplate<Xamarin.Forms.Color>(x =>
                                new StackPanel
                                {
                                    Gap = 4,
                                    Orientation = Orientation.Horizontal,
                                    Children = new Controls
                                    {
                                        new TextBlock { Text = x.ToString() + "test", FontSize = 18 }
                                    }
                                })
                      }
            };
            listBox.Items = new List<Xamarin.Forms.Color>
            {
                Xamarin.Forms.Color.Red,
                Xamarin.Forms.Color.Blue,
                Xamarin.Forms.Color.Yellow,
                Xamarin.Forms.Color.Green
            };
            mainPanel.Children.Add(listBox);
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
