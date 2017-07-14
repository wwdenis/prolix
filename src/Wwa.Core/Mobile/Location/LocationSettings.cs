using Wwa.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wwa.Core.Mobile.Location
{
	/// <summary>
	/// Geolocation settings
	/// </summary>
	public sealed class LocationSettings : Observable
	{
		#region Constants

		private const int DEFAULT_TIMEOUT = 15000;
		private const int DEFAULT_ACURRACY = 100;

		#endregion

		#region Constructors

		public LocationSettings()
		{
			Timeout = TimeSpan.FromMilliseconds(DEFAULT_TIMEOUT);
			Acurracy = DEFAULT_ACURRACY;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the timeout.
		/// </summary>
		public TimeSpan Timeout { get; set; }

		/// <summary>
		/// Gets or sets the desired acurracy.
		/// </summary>
		public double Acurracy { get; set; }

		#endregion
	}
}
