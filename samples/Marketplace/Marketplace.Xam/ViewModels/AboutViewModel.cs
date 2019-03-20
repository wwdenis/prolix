using System;
using System.Windows.Input;
using Prolix.Core.Mobile.Navigation;
using Prolix.Xam.Navigation;
using Xamarin.Forms;

namespace Marketplace.Xam.ViewModels
{
    public class AboutViewModel : ViewModel
    {
        public AboutViewModel(INavigationService navigation, IDialogService dialog) : base(navigation, dialog)
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        /// <summary>
        /// Command to open browser to xamarin.com
        /// </summary>
        public ICommand OpenWebCommand { get; }
    }
}
