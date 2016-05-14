using System;
using Xamarin.Forms;

namespace PrismUnityDemoApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Device.StartTimer(TimeSpan.FromSeconds(1),
                () =>
                {
                    Resources["currentDateTime"] = DateTime.Now.ToString();
                    return true;
                });
        }

        void OnButtonTest2Clicked(object sender, EventArgs e)
        {
            this.slider.Value = 0.5; 
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            //Func<double, double> customEase = t => 9 * t * t * t - 13.5 * t * t + 5.5 * t;
            //double scale = Math.Min(Width / button.Width, Height / button.Height);
            //await button.ScaleTo(scale, 1000, customEase);
            //await Task.Delay(1000);
            //await button.ScaleTo(1, 1000, customEase);

            /*
            // Swing down from lower - left corner.
            button.AnchorX = 0;
            button.AnchorY = 1;
            await button.RotateTo(90, 3000, new Easing(t => 1 - Math.Cos(10 * Math.PI * t) * Math.Exp(-5 * t)));

            // Drop to the bottom of the screen. 
            await button.TranslateTo(0, (Height - button.Height) / 2 - button.Width, 1000, Easing.BounceOut);

            // Prepare AnchorX and AnchorY for next rotation. 
            button.AnchorX = 1; button.AnchorY = 0;

            // Compensate for the change in AnchorX and AnchorY. 
            button.TranslationX -= button.Width - button.Height;
            button.TranslationY += button.Width + button.Height;

            // Fall over. 
            await button.RotateTo(180, 1000, Easing.BounceOut);

            // Fade out while ascending to the top of the screen. 
            await Task.WhenAll ( 
                button.FadeTo(0, 4000), 
                button.TranslateTo(0, -Height, 5000, Easing.CubicIn) 
                );

            // After three seconds, return the Button to normal. 
            await Task.Delay(3000);
            button.TranslationX = 0;
            button.TranslationY = 0;
            button.Rotation = 0;
            button.Opacity = 1;
            */
            Button button = (Button)sender;
            new Animation {
                { 0, 0.5, new Animation(v => button.Scale = v, 1, 5) },
                { 0.25, 0.75, new Animation(v => button.Rotation = v, 0, 360) },
                { 0.5, 1, new Animation(v => button.Scale = v, 5, 1) }
            }.Commit(this, "Animation4", 16, 5000);
        }
    }
}
