using Prism.Unity;
using PrismUnityDemoApp.Views;

namespace PrismUnityDemoApp
{
    public partial class App : PrismApplication
    {
        public App()
        {
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("MainPage?title=Hello%20from%20Xamarin.Forms%20Perspex Desktop");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<MainPage>();
        }
    }
}
