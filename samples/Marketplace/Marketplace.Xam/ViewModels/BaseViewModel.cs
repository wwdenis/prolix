using Marketplace.Client.Models;
using Prolix.Client.Navigation;
using Prolix.Xam.Navigation;

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
