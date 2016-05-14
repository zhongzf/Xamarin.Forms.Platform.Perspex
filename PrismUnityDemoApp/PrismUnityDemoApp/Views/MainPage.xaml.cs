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
            this.webView.Source = @"<p><strong>Pellentesque habitant morbi tristique</strong> senectus et netus et malesuada fames ac turpis egestas. Vestibulum tortor quam, feugiat vitae, ultricies eget, tempor sit amet, ante. Donec eu libero sit amet quam egestas semper. <em>Aenean ultricies mi vitae est.</em> Mauris placerat eleifend leo. Quisque sit amet est et sapien ullamcorper pharetra. Vestibulum erat wisi, condimentum sed, <code>commodo vitae</code>, ornare sit amet, wisi. Aenean fermentum, elit eget tincidunt condimentum, eros ipsum rutrum orci, sagittis tempus lacus enim ac dui. <a href=""#"">Donec non enim</a> in turpis pulvinar facilisis. Ut felis.</p>
										<h2>Header Level 2</h2>
											       
										<ol>
										   <li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li>
										   <li>Aliquam tincidunt mauris eu risus.</li>
										</ol>

										<blockquote><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus magna. Cras in mi at felis aliquet congue. Ut a est eget ligula molestie gravida. Curabitur massa. Donec eleifend, libero at sagittis mollis, tellus est malesuada tellus, at luctus turpis elit sit amet quam. Vivamus pretium ornare est.</p></blockquote>

										<h3>Header Level 3</h3>

										<ul>
										   <li>Lorem ipsum dolor sit amet, consectetuer adipiscing elit.</li>
										   <li>Aliquam tincidunt mauris eu risus.</li>
										</ul>";

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
