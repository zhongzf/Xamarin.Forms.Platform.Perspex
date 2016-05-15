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

            //NavigationService.NavigateAsync("MainPage?title=Hello%20from%20Xamarin.Forms%20Perspex Desktop");
            var viewModel = new ViewModels.SchoolViewModel();
            MainPage = new StudentListMainPage();
            MainPage.BindingContext = viewModel;
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<MainPage>();
        }
    }
}
