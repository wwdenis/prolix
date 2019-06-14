using Prolix.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Client.Services
{
	/// <summary>
	/// Media Service
	/// </summary>
	public interface IMediaService : IService
	{
		/// <summary>
		/// Gets the device hardware status
		/// </summary>
		HardwareStatus Status { get; }

		/// <summary>
		/// Take a picture using the available camera
		/// </summary>
		/// <returns>The picture binary representation</returns>
		Task<byte[]> TakePhoto();

		/// <summary>
		/// Pick a picture from storage
		/// </summary>
		/// <returns>The picture binary representation</returns>
		Task<byte[]> PickPhoto();
	}
}
