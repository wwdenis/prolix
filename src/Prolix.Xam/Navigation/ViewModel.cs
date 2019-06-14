using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using Prolix.Domain;
using Prolix.Extensions.Reflection;
using Prolix.Client.Navigation;

namespace Prolix.Xam.Navigation
{
    /// <summary>
    /// Base View Model
    /// </summary>
    public abstract class ViewModel : Observable, IViewModel
	{
		#region Fields

		string _version;
		string _title;
		bool _isBusy;

		#endregion

		#region Constructors

		public ViewModel(INavigationService navigation, IDialogService dialog)
		{
			BackCommand = new Command(Back);
			InitCommand = new Command(async () => await Init());

			IsBusy = false;
			Navigation = navigation;
			Dialog = dialog;

#if DEBUG
			Version = this.GetAssemblyVersion(3);
#endif
		}

		#endregion

		#region Services

		/// <summary>
		/// Navigation service
		/// </summary>
		protected INavigationService Navigation { get; set; }

		/// <summary>
		/// Dialog service
		/// </summary>
		protected IDialogService Dialog { get; set; }

		#endregion

		#region Commands

		public ICommand BackCommand { get; set; }
		public ICommand InitCommand { get; set; }

		#endregion

		#region Properties

		/// <summary>
		/// Core App Version
		/// </summary>
		public string Version
		{
			get { return _version; }
			set { Set(ref _version, value); }
		}

		/// <summary>
		/// Page Title
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { Set(ref _title, value); }
		}

		/// <summary>
		/// Gets or sets if the Viewmodel is busy
		/// </summary>
		public bool IsBusy
		{
			get { return _isBusy; }
			set { Set(ref _isBusy, value); }
		}

		#endregion

		#region Methods

		async public void Back()
		{
			await Navigation.Pop();
		}

		async public virtual Task Init()
		{
			await Task.Yield();
		}

		#endregion
	}
}
