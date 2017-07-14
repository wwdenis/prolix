namespace Marketplace.Xam.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            var coreApp = new Marketplace.Xam.App(this);

            LoadApplication(coreApp);
        }
    }
}