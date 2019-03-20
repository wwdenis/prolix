using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Prolix.Xam.Behaviors
{
	public sealed class ContentPageBehavior : BindableBehavior<ContentPage>
	{
		public static readonly BindableProperty AppearingCommandProperty =
	BindableProperty.Create("AppearingCommand", typeof(ICommand), typeof(ContentPageBehavior), null);

		public ICommand AppearingCommand
		{
			get { return (ICommand)GetValue(AppearingCommandProperty); }
			set { SetValue(AppearingCommandProperty, value); }
		}

		protected override void OnAttachedTo(BindableObject bindable)
		{
			base.OnAttachedTo(bindable);

			AssociatedObject.Appearing += AssociatedObject_Appearing;
		}

		void AssociatedObject_Appearing(object sender, EventArgs e)
		{
			if (AppearingCommand != null && AppearingCommand.CanExecute(null))
				AppearingCommand?.Execute(null);
		}

		protected override void OnDetachingFrom(ContentPage view)
		{
			AssociatedObject.Appearing -= AssociatedObject_Appearing;

			base.OnDetachingFrom(view);
		}
	}
}
