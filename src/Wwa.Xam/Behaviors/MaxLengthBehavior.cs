using System;
using Xamarin.Forms;

namespace Wwa.Xam.Controls
{
	public class MaxLengthBehavior: Behavior<Entry>
	{
		public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create("MaxLength", typeof(int), typeof(MaxLengthBehavior), 0);

        public MaxLengthBehavior()
        {
        }

        public MaxLengthBehavior(int maxlength)
        {
            MaxLength = maxlength;
        }

		public int MaxLength
		{
			get { return (int)GetValue(MaxLengthProperty); }
			set { SetValue(MaxLengthProperty, value); }
		}

		protected override void OnAttachedTo(Entry bindable)
		{
			bindable.TextChanged += Bindable_TextChanged;
		}

		void Bindable_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e?.NewTextValue?.Length > 0 && e?.NewTextValue?.Length > MaxLength)
				((Entry)sender).Text = e.NewTextValue.Substring(0, MaxLength);
		}

		protected override void OnDetachingFrom(Entry bindable)
		{
			bindable.TextChanged -= Bindable_TextChanged;
		}
	}
}
