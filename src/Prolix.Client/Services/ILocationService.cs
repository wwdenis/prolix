using System;
using System.Threading.Tasks;
using Prolix.Ioc;

namespace Prolix.Client.Services
{
    /// <summary>
    /// Geolocation Service
    /// </summary>
    public interface ILocationService : IService, IDisposable
	{
		/// <summary>
		/// Gets the device hardware status
		/// </summary>
		HardwareStatus Status { get; }

		/// <summary>
		/// Geolocation Settings (e.g. Acurracy)
		/// </summary>
		LocationSettings Settings { get; }

		/// <summary>
		/// Starts geolocation listening
		/// </summary>
		/// <returns>TRUE if the command whs sucessfully executed.</returns>
		Task<bool> Start();

		/// <summary>
		/// Stops geolocation listening
		/// </summary>
		/// <returns>TRUE if the command whs sucessfully executed.</returns>
		Task<bool> Stop();

		/// <summary>
		/// The the current position
		/// </summary>
		/// <returns>The current position</returns>
		Task<LocationPosition> GetPosition();
	}
}
