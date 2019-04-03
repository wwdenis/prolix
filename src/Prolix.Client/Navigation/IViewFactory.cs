using System;
using System.Reflection;

namespace Prolix.Client.Navigation
{
    /// <summary>
    /// View Factory
    /// </summary>
    public interface IViewFactory<ViewType>
	{
		/// <summary>
		/// Register a ViewModel agains a View
		/// </summary>
		/// <typeparam name="View">The View type</typeparam>
		void Register<View>()
			where View : ViewType;

		/// <summary>
		/// Register a ViewModel agains a View
		/// </summary>
		/// <typeparam name="View">The View type</typeparam>
		/// <typeparam name="ViewModel">The ViewModel type</typeparam>
		void Register<View, ViewModel>()
			where View : ViewType
            where ViewModel : class, IViewModel;

		/// <summary>
		/// Register a View against a ViewModel
		/// </summary>
		/// <param name="view">The View type</param>
		/// <param name="viewModel">The ViewModel type</param>
		void Register(Type view, Type viewModel = null);

        /// <summary>
		/// Register a ViewModel against a View
		/// </summary>
		/// <param name="views">The view types</param>
		void Register(Type[] views);

        /// <summary>
        /// Register all Views and ViewModels for the core application (PCL)
        /// </summary>
        /// <param name="coreAssembly">The Xamarin Forms application assembly</param>
        void Register(Assembly coreAssembly);

        /// <summary>
        /// Resolves a View and its related ViewModel
        /// </summary>
        /// <typeparam name="ViewModel">The desired ViewModel type</typeparam>
        /// <param name="initAction">A expression for ViewModel initialisation</param>
        /// <returns>The ViewModel instance</returns>
        ViewType Resolve<ViewModel>(Action<ViewModel> initAction = null)
			where ViewModel : class, IViewModel;

        /// <summary>
        /// Resolves a View and its related ViewModel
        /// </summary>
        /// <typeparam name="ViewModel">The desired ViewModel type</typeparam>
        /// <param name="viewModel">The output ViewModel instance</param>
        /// <param name="initAction">A expression for ViewModel initialisation</param>
        /// <returns>The ViewModel instance</returns>
        ViewType Resolve<ViewModel>(out ViewModel viewModel, Action<ViewModel> initAction = null)
			where ViewModel : class, IViewModel;

        /// <summary>
        /// Resolves a View and its related ViewModel
        /// </summary>
        /// <param name="viewModelType">The ViewModel type</param>
        /// <param name="viewModel">The output ViewModel instance</param>
        /// <returns>The ViewModel instance</returns>
        ViewType Resolve(Type viewModelType, out IViewModel viewModel);

        /// <summary>
        /// Builds the Launch Page
        /// </summary>
        /// <typeparam name="LaunchViewModel">The Launch View Model type</typeparam>
        /// <param name="navigation">The INavigation instance for navigation</param>
        /// <returns>The Launch Page</returns>
        ViewType Launch<LaunchViewModel>()
			where LaunchViewModel : class, IViewModel;
	}
}
