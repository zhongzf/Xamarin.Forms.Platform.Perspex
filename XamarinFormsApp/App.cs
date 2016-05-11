using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace XamarinFormsApp
{
    public class App : Application
    {
        Button button = null;
        Label label = null;
        StackLayout container = null;
        ProgressBar progressBar = null;
        public App()
        {
            button = new Button
            {
                Text = "test2"
            };
            button.Clicked += Button_Clicked;

            label = new Label
            {
                Text = "test",
                BackgroundColor = Color.Silver,
                HorizontalTextAlignment = TextAlignment.Center
            };
            container = new StackLayout();
            progressBar= new ProgressBar { Progress = 30 };
            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Button {
                            Command = new Command(() => { label.Text = "Better"; }),
                            Text = "Welcome to Xamarin Forms Perspex Desktop!",
                            BackgroundColor = Color.Blue,
                            BorderColor = Color.Yellow,
                            TextColor = Color.Red,
                            BorderWidth = 2
                        },
                        new StackLayout
                        {
                            Padding = new Thickness(10, 20, 30, 40),
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.Center,
                            Children =
                            {
                                label,
                                button
                            }
                        },
                        new Entry { Text = "default test", TextColor = Color.Black },
                        new ScrollView
                        {
                            Content = new StackLayout
                            {
                                Children =
                                {
                                    progressBar,  
                                    container
                                }
                            }
                        }
                    }
                }
            };
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            button.Text = "Clicked";
            label.HorizontalTextAlignment = TextAlignment.End;
            label.Text = "Good.";

            for (int i = 0; i < 100; i++)
            {
                var b = new Button { Text = "button" + i.ToString() };
                b.Clicked += B_Clicked;
                container.Children.Add(b);
            }
        }

        private void B_Clicked(object sender, EventArgs e)
        {
            progressBar.Progress+=10;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
