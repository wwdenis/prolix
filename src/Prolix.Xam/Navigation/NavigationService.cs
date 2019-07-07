using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

using Prolix.Client.Navigation;

namespace Prolix.Xam.Navigation
{
    /// <summary>
    /// Navigation service
    /// </summary>
    public sealed class NavigationService : INavigationService
	{
		#region Events

		/// <summary>
		/// Triggerred after a navigation sucessfully executed.
		/// </summary>
		public event ViewNavigationEventHandler Navigated;

		/// <summary>
		/// Triggerred before a navigation os going to happen.
		/// The navigation can be cancelled by setting the Cancel property of the NavigationEventArgs parameter
		/// </summary>
		public event ViewNavigationEventHandler Navigating;

		#endregion

		#region Fields

		readonly IXamViewFactory _viewFactory;

		#endregion

		#region Constructor

		public NavigationService(IXamViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The current Navigation instance
		/// </summary>
		INavigation Navigation
		{
			get { return MainPage?.Navigation; }
		}

		/// <summary>
		/// The current Navigation page
		/// </summary>
		NavigationPage MainPage
		{
			get { return Application.Current.MainPage as NavigationPage; }
		}

		/// <summary>
		/// The current child page
		/// </summary>
		Page CurrentPage
		{
			get { return Navigation?.NavigationStack?.LastOrDefault(); }
		}

		/// <summary>
		/// The current view model
		/// </summary>
		IViewModel CurrentViewModel
		{
			get { return CurrentPage?.BindingContext as IViewModel; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Navigate to the previous page.
		/// </summary>
		/// <returns>The View Model instance of the previous page.</returns>
		public async Task<IViewModel> Pop()
		{
			bool cancel = OnNavigating(true);

			if (cancel)
				return null;

			IViewModel previousViewModel = CurrentViewModel;
			Page view = await Navigation.PopAsync();
			IViewModel currentViewModel = view?.BindingContext as IViewModel;

			OnNavigated(previousViewModel);

			return currentViewModel;
		}

		/// <summary>
		/// Go back to the first Page
		/// </summary>
		public async Task<IViewModel> Reset()
		{
			bool cancel = OnNavigating(true);

			if (cancel)
				return null;

			IViewModel previousViewModel = CurrentViewModel;
			await Navigation.PopToRootAsync();
			Page view = Navigation.NavigationStack?.FirstOrDefault();
			IViewModel currentViewModel = view?.BindingContext as IViewModel;

			OnNavigated(previousViewModel);

			return currentViewModel;
		}

		/// <summary>
		/// Navigate to a page mapped to a specific ViewModel.
		/// </summary>
		/// <typeparam name="T">The ViewModel</typeparam>
		/// <param name="modal">Sets true if the Page will be show in a modal window.</param>
		/// <param name="initAction">An expression to initialise the ViewModel.</param>
		/// <returns>The desired ViewModel.</returns>
		public async Task<T> Push<T>(bool modal = false, Action<T> initAction = null)
			where T : class, IViewModel
		{
			bool cancel = OnNavigating();

			if (cancel)
				return null;

			if (CurrentViewModel is T)
				return null;

			T currentViewModel;
			Page view = _viewFactory.Resolve<T>(out currentViewModel, initAction);

			if (view == null)
				throw new InvalidOperationException("View Model not mapped!");

			IViewModel previousViewModel = CurrentViewModel;

			if (modal)
				await Navigation.PushModalAsync(view);
			else
				await Navigation.PushAsync(view);

			OnNavigated(previousViewModel);

			return currentViewModel;
		}

		/// <summary>
		/// Navigate to a page mapped to a specific ViewModel.
		/// </summary>
		/// <typeparam name="T">The ViewModel</typeparam>
		/// <param name="initAction">An expression to initialise the ViewModel.</param>
		/// <returns>The desired ViewModel.</returns>
		public async Task<T> Push<T>(Action<T> initAction)
			where T : class, IViewModel
		{
			return await Push<T>(false, initAction);
		}

		public void ToggleMenu()
		{
			MasterDetailPage page = MainPage.CurrentPage as MasterDetailPage;

			if (page != null)
			{
				page.IsPresented = !page.IsPresented;
			}
		}

		#endregion

		#region Methods

		bool OnNavigated(IViewModel previous, bool isBack = false)
		{
			var args = new ViewNavigationEventArgs(CurrentViewModel, previous, isBack);
			Navigated?.Invoke(this, args);
			return args.Cancel;
		}

		bool OnNavigating(bool isBack = false)
		{
			var args = new ViewNavigationEventArgs(CurrentViewModel, null, isBack);
			Navigating?.Invoke(this, args);
			return args.Cancel;
		}

		#endregion
	}
}
