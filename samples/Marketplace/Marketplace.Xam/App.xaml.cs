using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Wwa.Ioc.Autofac;
using Wwa.Xam.App;
using Wwa.Xam.Navigation;

using Marketplace.Xam.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Marketplace.Xam
{
    public partial class App : Application
    {
        public App(object platform)
        {
            InitializeComponent();

            var dependency = new AutofacDependencyManager();
            dependency.MapAssembly<App>();
            dependency.MapAssembly<NavigationService>();

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
