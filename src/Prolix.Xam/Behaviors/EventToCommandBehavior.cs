using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace Prolix.Xam.Behaviors
{
	public class EventToCommandBehavior : BindableBehavior<View>
	{
		public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), string.Empty);
		public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null);
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior), null);
		public static readonly BindableProperty EventArgsConverterProperty = BindableProperty.Create("EventArgsConverter", typeof(IValueConverter), typeof(EventToCommandBehavior), null);
		public static readonly BindableProperty EventArgsParameterProperty = BindableProperty.Create("EventArgsParameter", typeof(object), typeof(EventToCommandBehavior), null);

		Delegate _handler;
		EventInfo _eventInfo;

		public string EventName
		{
			get { return (string)GetValue(EventNameProperty); }
			set { SetValue(EventNameProperty, value); }
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public IValueConverter EventArgsConverter
		{
			get { return (IValueConverter)GetValue(EventArgsConverterProperty); }
			set { SetValue(EventArgsConverterProperty, value); }
		}

		public object EventArgsParameter
		{
			get { return GetValue(EventArgsParameterProperty); }
			set { SetValue(EventArgsParameterProperty, value); }
		}

		protected override void OnAttachedTo(View visualElement)
		{
			base.OnAttachedTo(visualElement);

			var events = AssociatedObject.GetType().GetRuntimeEvents().ToArray();
			if (events.Any())
			{
				_eventInfo = events.FirstOrDefault(e => e.Name == EventName);
				if (_eventInfo == null)
					throw new ArgumentException(String.Format("EventToCommand: Can't find any event named '{0}' on attached type", EventName));

				AddEventHandler(_eventInfo, AssociatedObject, OnFired);
			}
		}

		protected override void OnDetachingFrom(View view)
		{
			if (_handler != null)
				_eventInfo.RemoveEventHandler(AssociatedObject, _handler);

			base.OnDetachingFrom(view);
		}

		void AddEventHandler(EventInfo eventInfo, object item, Action<object, EventArgs> action)
		{
			var eventParameters = eventInfo.EventHandlerType
				.GetRuntimeMethods().First(m => m.Name == "Invoke")
				.GetParameters()
				.Select(p => Expression.Parameter(p.ParameterType))
				.ToArray();

			var actionInvoke = action.GetType()
				.GetRuntimeMethods().First(m => m.Name == "Invoke");

			_handler = Expression.Lambda(
				eventInfo.EventHandlerType,
				Expression.Call(Expression.Constant(action), actionInvoke, eventParameters[0], eventParameters[1]),
				eventParameters
			)
			.Compile();

			eventInfo.AddEventHandler(item, _handler);
		}

		void OnFired(object sender, EventArgs eventArgs)
		{
			if (Command == null)
				return;

			var parameter = CommandParameter;

			if (eventArgs != null && eventArgs != EventArgs.Empty)
			{
				parameter = eventArgs;

				if (EventArgsConverter != null)
				{
					parameter = EventArgsConverter.Convert(eventArgs, typeof(object), EventArgsParameter, CultureInfo.CurrentUICulture);
				}
			}

			if (Command.CanExecute(parameter))
			{
				Command.Execute(parameter);
			}
		}
	}
}
