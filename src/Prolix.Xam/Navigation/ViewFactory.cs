using System;
using System.Collections.Generic;
using System.Reflection;

using Xamarin.Forms;

using Prolix.Core.Extensions.Reflection;
using Prolix.Core.Ioc;
using Prolix.Client.Navigation;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Prolix.Xam.Navigation
{
    /// <summary>
    /// View Factory
    /// </summary>
    public class ViewFactory : IXamViewFactory
    {
		#region Fields

		readonly IDictionary<Type, ViewMapAttribute> _mappings = new Dictionary<Type, ViewMapAttribute>();
		readonly Resolver _resolver;

		#endregion

		#region Constructors

		public ViewFactory(Resolver resolver)
		{
			_resolver = resolver;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Register a ViewModel agains a View
		/// </summary>
		/// <typeparam name="View">The View type</typeparam>
		public void Register<View>()
			where View : Xamarin.Forms.Page
        {
			Register(typeof(View), null);
		}

		/// <summary>
		/// Register a ViewModel agains a View
		/// </summary>
		/// <typeparam name="View">The View type</typeparam>
		/// <typeparam name="ViewModel">The ViewModel type</typeparam>
		public void Register<View, ViewModel>()
			where View : Xamarin.Forms.Page
            where ViewModel : class, IViewModel
		{
			Register(typeof(View), typeof(ViewModel));
		}

		/// <summary>
		/// Register a ViewModel agains a View
		/// </summary>
		/// <param name="viewModel">The ViewModel type</param>
		/// <param name="view">The View type</param>
		public void Register(Type view, Type viewModel = null)
		{
			ViewMapAttribute map = view.GetAttribute<ViewMapAttribute>();

			if (map == null)
			{
				if (viewModel == null)
					return;

				map = new ViewMapAttribute(viewModel);
			}

			map.View = view;

			_resolver.Register(view);
			_resolver.Register(map.ViewModel);

			_mappings[map.ViewModel] = map;
		}

        /// <summary>
		/// Register a ViewModel against a View
		/// </summary>
		/// <param name="views">The view types</param>
		public void Register(Type[] views)
        {
            foreach (Type viewType in views)
            {
                Register(viewType);
            }
        }

        /// <summary>
		/// Register all Views and ViewModels for the core application (PCL)
		/// </summary>
		/// <param name="coreAssembly">The Xamarin Forms application assembly</param>
		public void Register(Assembly coreAssembly)
        {
            Type[] views = coreAssembly.FindTypes<Xamarin.Forms.Page>();
            Register(views);
        }

        /// <summary>
        /// Resolves a View and its related ViewModel
        /// </summary>
        /// <typeparam name="ViewModel">The desired ViewModel type</typeparam>
        /// <param name="initAction">A expression for ViewModel initialisation</param>
        /// <returns>The ViewModel instance</returns>
        public Xamarin.Forms.Page Resolve<ViewModel>(Action<ViewModel> initAction = null) 
			where ViewModel : class, IViewModel
		{
			ViewModel viewModel = null;
			return Resolve<ViewModel>(out viewModel, initAction);
		}

		/// <summary>
		/// Resolves a View and its related ViewModel
		/// </summary>
		/// <typeparam name="ViewModel">The desired ViewModel type</typeparam>
		/// <param name="viewModel">The output ViewModel instance</param>
		/// <param name="initAction">A expression for ViewModel initialisation</param>
		/// <returns>The ViewModel instance</returns>
		public Xamarin.Forms.Page Resolve<ViewModel>(out ViewModel viewModel, Action<ViewModel> initAction = null)
			where ViewModel : class, IViewModel
		{
			IViewModel resultViewModel = null;
            Xamarin.Forms.Page view = Resolve(typeof(ViewModel), out resultViewModel);

			viewModel = resultViewModel as ViewModel;

			// Initializes the View Model
			initAction?.Invoke(viewModel);

			return view;
		}

		/// <summary>
		/// Resolves a View and its related ViewModel
		/// </summary>
		/// <param name="viewModelType">The ViewModel type</param>
		/// <param name="vm">The output ViewModel instance</param>
		/// <returns>The ViewModel instance</returns>
		public Xamarin.Forms.Page Resolve(Type viewModelType, out IViewModel viewModel)
		{
			var vm = _resolver.Resolve(viewModelType) as ViewModel;

            Xamarin.Forms.Application.Current.On<iOS>().SetPanGestureRecognizerShouldRecognizeSimultaneously(true);

            Xamarin.Forms.Page view = null;
			ViewMapAttribute map = _mappings[viewModelType];

			if (vm == null || map == null)
				throw new InvalidOperationException("View Model not mapped!");

            Xamarin.Forms.Page pageView = _resolver.Resolve(map.View) as Xamarin.Forms.Page;

			if (pageView == null)
			{
				string message = string.Format("Page not mapped! View Model: {0}", viewModelType);
				throw new InvalidOperationException(message);
			}

			if (map.MenuViewModel == null)
			{
				view = pageView;
                view.On<iOS>().SetUseSafeArea(true);
            }
			else
			{
				IViewModel menuViewModel = null;
                Xamarin.Forms.Page menuView = Resolve(map.MenuViewModel, out menuViewModel);

				if (menuView != null)
				{
					pageView.BindingContext = vm;
					pageView.Parent = null;
					menuView.Parent = null;

					if (string.IsNullOrWhiteSpace(menuView.Title))
					{
						menuView.Title = menuView.GetType().Name;
					}

					if (string.IsNullOrWhiteSpace(pageView.Title))
					{
						pageView.Title = pageView.GetType().Name;
					}

					view = new MasterDetailPage
					{
						Master = menuView,
						Detail = pageView
					};

                    pageView.On<iOS>().SetUseSafeArea(true);
                }
            }

			view.Title = vm.Title;
			view.BindingContext = vm;

			if (map.HideNavigation)
			{
				Xamarin.Forms.NavigationPage.SetHasNavigationBar(view, false);
			}

			viewModel = vm;

			return view;
		}

		/// <summary>
		/// Builds the Launch Page
		/// </summary>
		/// <typeparam name="LaunchViewModel">The Launch View Model type</typeparam>
		/// <param name="navigation">The INavigation instance for navigation</param>
		/// <returns>The Launch Page</returns>
		public Xamarin.Forms.Page Launch<LaunchViewModel>()
			where LaunchViewModel : class, IViewModel
		{
			var startPage = Resolve<LaunchViewModel>();
			var navigationPage = new Xamarin.Forms.NavigationPage(startPage);
			return navigationPage;
		}

		#endregion
	}
}
