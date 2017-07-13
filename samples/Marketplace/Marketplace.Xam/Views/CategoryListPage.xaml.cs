using System;

using Marketplace.Xam.Models;
using Marketplace.Xam.ViewModels;

using Xamarin.Forms;
using Wwa.Xam.Navigation;

namespace Marketplace.Xam.Views
{
    [ViewMap(typeof(CategoryListViewModel), typeof(MainViewModel))]
    public partial class CategoryListPage : ContentPage
    {
        public CategoryListPage()
        {
            InitializeComponent();
        }
    }
}
