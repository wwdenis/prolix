using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Prolix.Xam.Behaviors
{
	public sealed class ListViewItemBehavior : BindableBehavior<ListView>
	{
		public static readonly BindableProperty TappedCommandProperty = 
			BindableProperty.Create("TappedCommand", typeof(ICommand), typeof(ListViewItemBehavior), null);

		public ICommand TappedCommand
		{
			get { return (ICommand)GetValue(TappedCommandProperty); }
			set { SetValue(TappedCommandProperty, value); }
		}

		protected override void OnAttachedTo(BindableObject bindable)
		{
			base.OnAttachedTo(bindable);

			AssociatedObject.ItemTapped += AssociatedObject_ItemTapped;
		}

		void AssociatedObject_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			TappedCommand?.Execute(e.Item);
		}

		protected override void OnDetachingFrom(ListView view)
		{
			AssociatedObject.ItemTapped -= AssociatedObject_ItemTapped;

			base.OnDetachingFrom(view);
		}
	}
}
