using System;
using System.Windows.Input;
using Xamarin.Forms;

using Prolix.Client.Navigation;
using Prolix.Xam.Navigation;

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
