using System.Threading.Tasks;

namespace Prolix.Client.Navigation
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
