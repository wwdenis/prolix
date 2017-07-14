namespace Wwa.Core.Mobile
{
    /// <summary>
    /// List
    /// </summary>
    public enum HardwareStatus
	{
		/// <summary>
		/// The device does not support the hardware
		/// </summary>
		Unavailable,

		/// <summary>
		/// The hardware is supported, but it is disabled
		/// </summary>
		Available,

		/// <summary>
		/// The hardware is enabled, but is not started
		/// </summary>
		Enabled,

		/// <summary>
		/// The hardware is ready for use
		/// </summary>
		Ready
	}
}
