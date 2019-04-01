using System;

using Marketplace.Client.Models;
using Marketplace.Xam.ViewModels;

using Xamarin.Forms;
using Prolix.Xam.Navigation;

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
