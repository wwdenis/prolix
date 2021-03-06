﻿using System.Threading.Tasks;
using Xamarin.Forms;

using Prolix.Client.Navigation;

namespace Prolix.Xam.Navigation
{
    /// <summary>
    /// Simple dialog service
    /// </summary>
    public class DialogService : IDialogService
	{
		#region Constants

		const string DEFAULT_ALERT_TITLE = "Alert";
		const string DEFAULT_CONFIRM_TITLE = "Confirm";
		const string DEFAULT_ERROR_MESSAGE = "Command not available or not working!";

		#endregion

		#region Constructors

		public DialogService()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// The application's main page
		/// </summary>
		Page MainPage => Application.Current.MainPage;

		#endregion

		#region Methods

		/// <summary>
		/// Shows an alert message
		/// </summary>
		/// <param name="message">The message text</param>
		/// <param name="title">The title</param>
		/// <param name="button">The mutton text</param>
		public async Task Alert(string message, string title = null, string button = "OK")
		{
			if (string.IsNullOrWhiteSpace(title))
				title = DEFAULT_ALERT_TITLE;

			await MainPage.DisplayAlert(title, message, button);
		}

		/// <summary>
		/// Shows an confirmation message
		/// </summary>
		/// <param name="message">The message text</param>
		/// <param name="title">The title text</param>
		/// <param name="acceptButton">The accept button text</param>
		/// <param name="cancelButton">The cancel button text</param>
		/// <returns>TRUE if the accept button was clicked</returns>
		public async Task<bool> Confirm(string message, string title = null, string acceptButton = "OK", string cancelButton = "Cancel")
		{
			if (string.IsNullOrWhiteSpace(title))
				title = DEFAULT_CONFIRM_TITLE;

			return await MainPage.DisplayAlert(title, message, acceptButton, cancelButton);
		}

		/// <summary>
		/// Shows an generic error message
		/// </summary>
		public async Task Error()
		{
			await Alert(DEFAULT_ERROR_MESSAGE, DEFAULT_ALERT_TITLE);
		}

		#endregion
	}
}
