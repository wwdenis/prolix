using System;

namespace Wwa.Xam.Navigation
{
    /// <summary>
    /// Configures a View
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
	public sealed class ViewMapAttribute : Attribute
	{
		#region Constructors

		/// <summary>
		/// Configures a View
		/// </summary>
		/// <param name="viewModel">The View Model type</param>
		/// 
		public ViewMapAttribute(Type viewModel)
		{
			ViewModel = viewModel;
		}

		/// <summary>
		/// Configures a View
		/// </summary>
		/// <param name="viewModel">The View Model type</param>
		/// <param name="menuViewModel">The Menu View Model type</param>
		public ViewMapAttribute(Type viewModel, Type menuViewModel) : this(viewModel)
		{
			MenuViewModel = menuViewModel;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The View Model type
		/// </summary>
		public Type ViewModel { get; set; }

		/// <summary>
		/// The Menu View Model
		/// </summary>
		public Type MenuViewModel { get; set; }

		/// <summary>
		/// The View type
		/// </summary>
		public Type View { get; set; }

		/// <summary>
		/// View created for big screens
		/// </summary>
		public bool IsTablet { get; set; }

		/// <summary>
		/// Hide the native navigation bar
		/// </summary>
		public bool HideNavigation { get; set; }

		#endregion
	}
}
