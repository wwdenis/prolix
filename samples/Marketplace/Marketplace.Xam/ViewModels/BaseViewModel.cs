using Marketplace.Xam.Models;
using Wwa.Core.Mobile.Navigation;
using Wwa.Xam.Navigation;

namespace Marketplace.Xam.ViewModels
{
    public abstract class BaseViewModel : ViewModel
    {
        public BaseViewModel(INavigationService navigation, IDialogService dialog, ApplicationContext context) : base(navigation, dialog)
        {
            Context = context;
        }

        protected ApplicationContext Context { get; }
    }
}
