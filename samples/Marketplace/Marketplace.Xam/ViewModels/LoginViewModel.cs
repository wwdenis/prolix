using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

using Marketplace.Client.Models.Security;
using Marketplace.Client.Models;
using Marketplace.Client.Services;

using Prolix.Logic;
using Prolix.Client.Navigation;

namespace Marketplace.Xam.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;

        public LoginViewModel(INavigationService navigation, IDialogService dialog, ApplicationContext context, IIdentityService identityService) : base(navigation, dialog, context)
        {
            Title = "Login";
            LoginCommand = new Command(Login);

            IdentityService = identityService;
        }

        IIdentityService IdentityService { get; }

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
            try
            {
                IsBusy = true;

                var model = new LoginModel
                {
                    UserName = UserName,
                    Password = Password
                };

                var result = await IdentityService.Login(model);

                Context.Credentials = result;

                await Navigation.Push<MainViewModel>();
            }
            catch (RuleException ex)
            {
                await Dialog.Alert(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Dialog.Error();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
