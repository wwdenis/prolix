using System.Threading.Tasks;

namespace Wwa.Core.Mobile.Navigation
{
    /// <summary>
    /// Generic View model
    /// </summary>
    public interface IViewModel
	{
		/// <summary>
		/// Initialization method
		/// </summary>
		Task Init();
	}
}
