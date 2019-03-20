using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prolix.Xam.Behaviors
{
	public class BindableBehavior<T> : Behavior<T> where T : BindableObject
	{
		public T AssociatedObject { get; set; }

		protected override void OnAttachedTo(T visualElement)
		{
			base.OnAttachedTo(visualElement);

			AssociatedObject = visualElement;

			if (visualElement.BindingContext != null)
				BindingContext = visualElement.BindingContext;

			visualElement.BindingContextChanged += OnBindingContextChanged;
		}

		void OnBindingContextChanged(object sender, EventArgs e)
		{
			OnBindingContextChanged();
		}

		protected override void OnDetachingFrom(T view)
		{
			view.BindingContextChanged -= OnBindingContextChanged;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			BindingContext = AssociatedObject.BindingContext;
		}
	}
}
