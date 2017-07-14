using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Wwa.Xam.Behaviors
{
	public sealed class SearchBarBehavior : BindableBehavior<SearchBar>
	{
		public static readonly BindableProperty FocusedCommandProperty =
			BindableProperty.Create("FocusedCommand", typeof(ICommand), typeof(SearchBarBehavior), null);

		public ICommand FocusedCommand
		{
			get { return (ICommand)GetValue(FocusedCommandProperty); }
			set { SetValue(FocusedCommandProperty, value); }
		}

		protected override void OnAttachedTo(BindableObject bindable)
		{
			base.OnAttachedTo(bindable);

			AssociatedObject.Focused += AssociatedObject_Focused;
		}

		void AssociatedObject_Focused(object sender, FocusEventArgs e)
		{
			FocusedCommand?.Execute(null);
		}

		protected override void OnDetachingFrom(SearchBar view)
		{
			AssociatedObject.Focused -= AssociatedObject_Focused;

			base.OnDetachingFrom(view);
		}
	}
}
