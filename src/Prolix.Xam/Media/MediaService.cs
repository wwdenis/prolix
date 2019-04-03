using Plugin.Media;
using Plugin.Media.Abstractions;

using System;
using System.Threading.Tasks;

using Prolix.Core.Extensions.IO;
using Prolix.Client.Services;

namespace Prolix.Xam.Media
{
    /// <summary>
    /// Geolocation Service
    /// </summary>
    public class MediaService : IMediaService
	{
		#region Constructors

		public MediaService()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// The current media plugin
		/// </summary>
		IMedia Media
		{
			get { return CrossMedia.Current; }
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
				if (!Media.IsCameraAvailable)
					return HardwareStatus.Unavailable;

				return HardwareStatus.Ready;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Take a picture using the available camera
		/// </summary>
		/// <returns>The picture binary representation</returns>
		async public Task<byte[]> TakePhoto()
		{
			await Media.Initialize();

			if (!Media.IsCameraAvailable || !Media.IsTakePhotoSupported)
				throw new InvalidOperationException();

			var options = new StoreCameraMediaOptions
			{
				SaveToAlbum = true
			};
			
			MediaFile file = await Media.TakePhotoAsync(options);

			byte[] result = file.GetStream().ToByteArray();

			return result;
		}

		/// <summary>
		/// Pick a picture from storage
		/// </summary>
		/// <returns>The picture binary representation</returns>
		async public Task<byte[]> PickPhoto()
		{
			await Media.Initialize();

			if (!Media.IsPickPhotoSupported)
				throw new InvalidOperationException();

			MediaFile file = await Media.PickPhotoAsync();

			byte[] result = file.GetStream().ToByteArray();

			return result;
		}

        #endregion
    }
}