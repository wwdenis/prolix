namespace Prolix.Core.Mobile.Navigation
{
    public delegate void ViewNavigationEventHandler(object sender, ViewNavigationEventArgs e);

	public sealed class ViewNavigationEventArgs
	{
		public ViewNavigationEventArgs(IViewModel current, IViewModel previous = null, bool isBack = false)
		{
			Current = current;
			Previous = previous;
			IsBack = isBack;
		}

		public bool Cancel { get; set; }
		public bool IsBack { get; private set; }
		public IViewModel Previous { get; private set; }
		public IViewModel Current { get; private set; }
	}
}
