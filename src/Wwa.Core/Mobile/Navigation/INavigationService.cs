using System;
using System.Threading.Tasks;
using Wwa.Core.Ioc;

namespace Wwa.Core.Mobile.Navigation
{
	/// <summary>
	/// Navigation service
	/// </summary>
	public interface INavigationService : IService
	{
		/// <summary>
		/// Triggerred after a navigation sucessfully executed.
		/// </summary>
		event ViewNavigationEventHandler Navigated;

		/// <summary>
		/// Triggerred before a navigation os going to happen.
		/// The navigation can be cancelled by setting the Cancel property of the NavigationEventArgs parameter
		/// </summary>
		event ViewNavigationEventHandler Navigating;

		/// <summary>
		/// Navigate to the previous page.
		/// </summary>
		/// <returns>The View Model instance of the previous page.</returns>
		Task<IViewModel> Pop();

		/// <summary>
		/// Go back to the first Page
		/// </summary>
		/// <returns>The View Model instance of the first page.</returns>
		Task<IViewModel> Reset();

		/// <summary>
		/// Navigate to a page mapped to a specific ViewModel.
		/// </summary>
		/// <typeparam name="ViewModelType">The ViewModel</typeparam>
		/// <param name="modal">Sets true if the Page will be show in a modal window.</param>
		/// <param name="initAction">An expression to initialise the ViewModel.</param>
		/// <returns>The desired ViewModel.</returns>
		Task<ViewModelType> Push<ViewModelType>(bool modal = false, Action<ViewModelType> initAction = null) 
			where ViewModelType : class, IViewModel;

		/// <summary>
		/// Navigate to a page mapped to a specific ViewModel.
		/// </summary>
		/// <typeparam name="ViewModelType">The ViewModel</typeparam>
		/// <param name="initAction">An expression to initialise the ViewModel.</param>
		/// <returns>The desired ViewModel.</returns>
		Task<ViewModelType> Push<ViewModelType>(Action<ViewModelType> initAction)
			where ViewModelType : class, IViewModel;
	}
}