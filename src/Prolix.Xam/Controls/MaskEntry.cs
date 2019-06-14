using System;
using System.Windows.Input;
using Prolix.Extensions.Parsing;
using Xamarin.Forms;

namespace Prolix.Xam.Controls
{
	public class MaskEntry : Entry
	{
        public enum MaskType
        {
            Numeric,
            Text
        }

		public string Mask
		{
            get;
            set;
		}

		public MaskType Type
		{
			get;
			set;
		}
        
		public MaskEntry()
		{	
			TextChanged += Handle_TextChanged;
		}

		void Handle_TextChanged(object sender, TextChangedEventArgs e)
		{
            if (string.IsNullOrWhiteSpace(Mask))
                return;

            Behaviors.Add(new MaxLengthBehavior(Mask.Length));
            
			switch (Type)
			{
				case MaskType.Numeric:
					//000-000-0000
					Keyboard = Keyboard.Numeric;

                    string digits = e?.NewTextValue?.FormatDigits();

                    if (string.IsNullOrWhiteSpace(digits) || !int.TryParse(digits, out int number))
						return;

                    string template = "{0:" + Mask + "}";

                    Text = string.Format(template, number);

					break;
				
				default:
					Keyboard = Keyboard.Text;
					break;
			}

            if (Text.Length >= Mask.Length)
                Unfocus();
        }
	}
}
