using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using Marketplace.Models.Configuration;
using Marketplace.Xam.Services;

using Wwa.Core.Collections;
using Wwa.Core.Logic;
using Wwa.Core.Mobile.Navigation;
using Wwa.Xam.Navigation;

namespace Marketplace.Xam.ViewModels
{
    public class CategoryListViewModel : ViewModel
    {
        public CategoryListViewModel(INavigationService navigation, IDialogService dialog, ICategoryService categoryService) : base(navigation, dialog)
        {
            CategoryService = categoryService;

            Title = "Categories";
            LoadCommand = new Command(async () => await Load());
        }

        ICategoryService CategoryService { get; }
        public ICommand LoadCommand { get; set; }
        public NotifiableCollection<CategoryModel> Items { get; } = new NotifiableCollection<CategoryModel>();

        async Task Load()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var paged = await CategoryService.List();

                Items.ReplaceRange(paged.Items);
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