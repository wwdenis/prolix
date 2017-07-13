using Wwa.Core.Ioc;
using System.Threading.Tasks;

namespace Wwa.Core.Mobile.Navigation
{
	/// <summary>
	/// Simple dialog service
	/// </summary>
	public interface IDialogService : IService
	{
		/// <summary>
		/// Shows an alert message
		/// </summary>
		/// <param name="message">The message text</param>
		/// <param name="title">The title</param>
		/// <param name="button">The mutton text</param>
		Task Alert(string message, string title = null, string button = "OK");

		/// <summary>
		/// Shows an confirmation message
		/// </summary>
		/// <param name="message">The message text</param>
		/// <param name="title">The title text</param>
		/// <param name="acceptButton">The accept button text</param>
		/// <param name="cancelButton">The cancel button text</param>
		/// <returns>TRUE if the accept button was clicked</returns>
		Task<bool> Confirm(string message, string title = null, string acceptButton = "OK", string cancelButton = "Cancel");

		/// <summary>
		/// Shows an generic error message
		/// </summary>
		Task Error();
	}
}