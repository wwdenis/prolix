using Marketplace.Xam.ViewModels;
using Wwa.Xam.Navigation;
using Xamarin.Forms;

namespace Marketplace.Xam.Views
{
    [ViewMap(typeof(MainViewModel))]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }
}