using System.Windows.Input;
using Xamarin.Forms;

using Prolix.Client.Navigation;
using Prolix.Xam.Navigation;

namespace Marketplace.Xam.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel(INavigationService navigation, IDialogService dialog) : base(navigation, dialog)
        {
            Title = "Marketplace";
            CategoryCommand = new Command(Category);
            ProductCommand = new Command(Product);
            DealerCommand = new Command(Dealer);
            CustomerCommand = new Command(Customer);
            OrderCommand = new Command(Order);
            UserCommand = new Command(User);
        }

        public ICommand CategoryCommand { get; }
        public ICommand ProductCommand { get; }
        public ICommand DealerCommand { get; }
        public ICommand CustomerCommand { get; }
        public ICommand OrderCommand { get; }
        public ICommand UserCommand { get; }

        async public void Category()
        {
            await Navigation.Push<CategoryListViewModel>();
        }

        async public void Product()
        {
            // await Navigation.Push<ProductViewModel>();
            await Dialog.Error();
        }

        async public void Dealer()
        {
            // await Navigation.Push<DealerViewModel>();
            await Dialog.Error();
        }

        async public void Customer()
        {
            // await Navigation.Push<CustomerViewModel>();
            await Dialog.Error();
        }

        async public void Order()
        {
            // await Navigation.Push<OrderViewModel>();
            await Dialog.Error();
        }

        async public void User()
        {
            // await Navigation.Push<UserViewModel>();
            await Dialog.Error();
        }
    }
}
