using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

using System;
using System.Threading.Tasks;

using Prolix.Client.Services;

namespace Prolix.Xam.Location
{
    /// <summary>
    /// Geolocation Service
    /// </summary>
    public class LocationService : ILocationService
	{
		#region Constructors

		public LocationService()
		{
			Settings = new LocationSettings();
		}

		#endregion

		#region Properties

		/// <summary>
		/// The current media plugin
		/// </summary>
		IGeolocator Locator
		{
			get { return CrossGeolocator.Current; }
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the device hardware status
		/// </summary>
		public HardwareStatus Status
		{
			get
			{
				if (Locator.IsListening)
					return HardwareStatus.Ready;

				if (Locator.IsGeolocationEnabled)
					return HardwareStatus.Enabled;

				if (Locator.IsGeolocationAvailable)
					return HardwareStatus.Available;

				return HardwareStatus.Unavailable;
			}
		}

		/// <summary>
		/// Geolocation Settings (e.g. Acurracy)
		/// </summary>
		public LocationSettings Settings
		{
			get;
			set;
		}

		#endregion

		#region Methods

		/// <summary>
		/// The the current position
		/// </summary>
		/// <returns>The current position</returns>
		async public Task<LocationPosition> GetPosition()
		{
			if (!Locator.IsGeolocationEnabled)
				throw new InvalidOperationException();

			Locator.DesiredAccuracy = Settings.Acurracy;

			Position pos = await Locator.GetPositionAsync();

			return new LocationPosition(pos.Latitude, pos.Longitude);
		}

		/// <summary>
		/// Starts geolocation listening
		/// </summary>
		/// <returns>TRUE if the command whs sucessfully executed.</return
		async public Task<bool> Start()
		{
			if (!Locator.IsGeolocationEnabled)
				throw new InvalidOperationException();

			return await Locator.StartListeningAsync(Settings.Timeout, Settings.Acurracy);
		}

		/// <summary>
		/// Stops geolocation listening
		/// </summary>
		/// <returns>TRUE if the command whs sucessfully executed.</returns>
		async public Task<bool> Stop()
		{
			if (!Locator.IsGeolocationEnabled)
				throw new PlatformNotSupportedException();

			return await Locator.StopListeningAsync();
		}

		/// <summary>
		/// Disposes the service
		/// </summary>
		async public void Dispose()
		{
			await Stop();
		}

		#endregion
	}
}