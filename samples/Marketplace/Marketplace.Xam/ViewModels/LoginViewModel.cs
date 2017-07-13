using System.Windows.Input;
using Xamarin.Forms;

using Wwa.Core.Mobile.Navigation;
using Wwa.Xam.Navigation;

namespace Marketplace.Xam.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        private string _userName;
        private string _password;

        public LoginViewModel(INavigationService navigation, IDialogService dialog) : base(navigation, dialog)
        {
            Title = "Login";
            LoginCommand = new Command(Login);
        }

        public ICommand LoginCommand { get; set; }
        public ICommand TestCommand { get; set; }

        public string UserName
        {
            get { return _userName; }
            set { Set(ref _userName, value); }
        }

        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        async public void Login()
        {
            await Navigation.Push<MainViewModel>();
        }
    }
}
