using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Marketplace.Client.Services;
using Marketplace.Xam.ViewModels;

using Prolix.Client.Api;
using Prolix.Ioc.Autofac;
using Prolix.Xam.App;
using Prolix.Xam.Navigation;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Marketplace.Xam
{
    public partial class App : Application
    {
        public App(object platform)
        {
            InitializeComponent();

            var resolver = new AutofacResolver();
            resolver.ScanAssembly<CategoryService>();   // Marketplacxe.Client
            resolver.ScanAssembly<NavigationService>(); // Prolix.Xam
            resolver.ScanAssembly<RestService>();       // Prolix.Client

            var forms = new FormsManager(this, resolver);

            // Init the application and the Main page
            forms.Run<LoginViewModel>();
        }

        public App()
        {
            InitializeComponent();

            // App not initialised
            FormsManager.Warn(this);
        }
    }
}
