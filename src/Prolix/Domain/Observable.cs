using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Prolix.Domain
{
	/// <summary>
	/// Base bindable object
	/// </summary>
	public abstract class Observable : INotifyPropertyChanged
	{
		/// <summary>
		/// Fired on property changed
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Fires the PropertyChanged event.
		/// </summary>
		/// <param name="propertyName">Property Name</param>
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Sets a value to a field and fires the PropertyChanged event if the value has changed.
		/// </summary>
		/// <typeparam name="T">The field type.</typeparam>
		/// <param name="field">The field reference.</param>
		/// <param name="value">The new value.</param>
		/// <param name="propertyName">The property name</param>
		protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (!object.ReferenceEquals(field, value) || !object.Equals(field, value))
			{
				field = value;
				OnPropertyChanged(propertyName);
			}
		}
	}
}
