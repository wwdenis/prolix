using Prolix.Extensions.Reflection;
using Prolix.Ioc;
using Prolix.Client.Navigation;
using Prolix.Xam.Navigation;

using Xamarin.Forms;

namespace Prolix.Xam.App
{
    /// <summary>
    /// Application Bootstrapper
    /// </summary>
    public class FormsManager
	{
		#region Constructors

        public FormsManager(Application formsApp, Resolver resolver)
		{
			FormsApp = formsApp;
            Resolver = resolver;
		}

        #endregion

        #region Properties

        Resolver Resolver { get; }
        Application FormsApp { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the application and set the master page
        /// </summary>
        /// <typeparam name="StartViewModel">The start ViewModel (for view navigation)</typeparam>
        public void Run<StartViewModel>()
            where StartViewModel : class, IViewModel
        {
            // IOC container and ViewFactory
            var viewFactory = new ViewFactory(Resolver);

            // Views and view models
            var coreAssembly = FormsApp.GetAssembly();
            viewFactory.Register(coreAssembly);

            // Register instance dependencies
            Resolver.Register<IXamViewFactory>(viewFactory, DepedencyLifetime.PerLifetime);

            // Finish the ioc continer
            Resolver.Build();

			// Setting the BindingContext for later use
			FormsApp.BindingContext = Resolver;

			// Set the start page
			var launchPage = viewFactory.Launch<StartViewModel>();
			FormsApp.MainPage = launchPage;
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Show an error page if the application was not initialized
		/// </summary>
		/// <param name="app">The Xamarin Forms application instance</param>
		public static void Warn(Application app)
		{
			app.MainPage = new ContentPage
			{
				Content = new Label
				{
					BackgroundColor = Color.Red,
					TextColor = Color.White,
					FontSize = 20,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Text = "Application Not Initialized !"
				}
			};
		}

		#endregion
	}
}
