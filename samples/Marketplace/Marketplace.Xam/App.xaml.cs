using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Marketplace.Xam.Services;
using Marketplace.Xam.ViewModels;

using Wwa.Http.Client;
using Wwa.Ioc.Autofac;
using Wwa.Xam.App;
using Wwa.Xam.Navigation;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Marketplace.Xam
{
    public partial class App : Application
    {
        public App(object platform)
        {
            InitializeComponent();

            var dependency = new AutofacDependencyManager();
            dependency.MapAssembly<CategoryService>();
            dependency.MapAssembly<NavigationService>();
            dependency.MapAssembly<RestService>();

            var forms = new FormsManager(this, dependency);

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
