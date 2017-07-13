
using Foundation;
using UIKit;

namespace Marketplace.Xam.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

            var coreApp = new App(this);

            LoadApplication(coreApp);

			return base.FinishedLaunching(app, options);
		}
	}
}
