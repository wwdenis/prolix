using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Client.Services
{
	/// <summary>
	/// Geolocation positioning information
	/// </summary>
	public class LocationPosition
	{
		#region Constructors

		public LocationPosition()
		{
		}

		public LocationPosition(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Latitude
		/// </summary>
		public double Latitude { get; private set; }

		/// <summary>
		/// Longitude
		/// </summary>
		public double Longitude { get; private set; }

		#endregion
	}
}
