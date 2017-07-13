using Wwa.Core.Extensions.Reflection;
using Wwa.Core.Ioc;
using Wwa.Core.Mobile.Navigation;
using Wwa.Xam.Navigation;

using Xamarin.Forms;

namespace Wwa.Xam.App
{
    /// <summary>
    /// Application Bootstrapper
    /// </summary>
    public class FormsManager
	{
		#region Constructors

        public FormsManager(Application formsApp, IDependencyManager resolverManager)
		{
			FormsApp = formsApp;
            ResolverManager = resolverManager;
		}

        #endregion

        #region Properties

        IDependencyManager ResolverManager { get; }
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
            var resolver = ResolverManager.Resolver;
            
            // IOC container and ViewFactory
            var viewFactory = new ViewFactory(resolver);

            // Views and view models
            var coreAssembly = FormsApp.GetAssembly();
            viewFactory.Register(coreAssembly);

            // Register instance dependencies
            resolver.Register<IXamViewFactory>(viewFactory, DepedencyLifetime.PerLifetime);

            // Finish the ioc continer
            ResolverManager.Build();

			// Setting the BindingContext for later use
			FormsApp.BindingContext = resolver;

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
