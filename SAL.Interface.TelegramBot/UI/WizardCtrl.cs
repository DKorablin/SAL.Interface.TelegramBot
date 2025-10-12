using System;
using System.Collections.Generic;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot.UI
{
	/// <summary>Control element that allows generating wizards</summary>
	public class WizardCtrl
	{
		/// <summary>Row describing a step in the wizard</summary>
		public class StepRow
		{
			/// <summary>Text describing the required data from the client</summary>
			private String Text { get; set; }

			/// <summary>Title displayed to the user</summary>
			internal String Title { get; private set; }

			/// <summary>Type of the required value</summary>
			public TypeCode ValueType { get; set; }

			/// <summary>Value received from the user</summary>
			public virtual Object Value { get; set; }

			/// <summary>Default value if the client provided an empty value</summary>
			public virtual Object DefaultValue{ get; set; }

			/// <summary>Create a step row for the wizard</summary>
			/// <param name="text">Text to display to the client</param>
			/// <param name="type">Type of the required value</param>
			public StepRow(String text, Type type)
				: this(text, StepRow.GetTypeCode(type))
			{
			}

			/// <summary>Create a step row for the wizard</summary>
			/// <param name="text">Text to display to the client</param>
			/// <param name="valueType">Type of the required value</param>
			public StepRow(String text,TypeCode valueType)
			{
				if(String.IsNullOrWhiteSpace(text))
					throw new ArgumentNullException(nameof(text));

				this.Text = text;
				this.ValueType = valueType;
			}

			private static TypeCode GetTypeCode(Type type)
			{
				if(type.IsGenericType)
				{
					return type.GetGenericTypeDefinition() == typeof(Nullable<>)
						? Type.GetTypeCode(type.GenericTypeArguments[0])
						: Type.GetTypeCode(type);
				} else
					return Type.GetTypeCode(type);
			}

			/// <summary>Internal method to set a fixed title</summary>
			/// <param name="index">Index of the step in the steps array</param>
			/// <param name="totalRows">Total number of rows</param>
			internal void SetTitle(Int32 index, Int32 totalRows)
				=> this.Title = totalRows == 1
					? this.Text
					: $"[{index}-{totalRows}] {this.Text}";

			internal void TrySetValue(String text)
			{
				this.Value = text == String.Empty && this.DefaultValue != null
					? this.DefaultValue
					: Utils.TryChangeValue(text, this.ValueType);
			}
		}

		private StepRow[] _steps;

	/// <summary>Message from which the wizard starts working</summary>
		public String TriggerMessage { get; private set; }

	/// <summary>Create a wizard with a list of steps</summary>
	/// <param name="triggerMessage">Message that starts the wizard</param>
	/// <param name="steps">List of steps with which the wizard starts</param>
		public WizardCtrl(String triggerMessage, params StepRow[] steps)
			: this(triggerMessage)
			=> this.InitSteps(steps);

	/// <summary>Create a wizard specifying the trigger message and the source creating the element</summary>
	/// <param name="triggerMessage">Message that starts the wizard</param>
		protected WizardCtrl(String triggerMessage)
		{
			if(triggerMessage != null && triggerMessage.IndexOfAny(new Char[] { ',', '_', ' ' }) > -1)
				throw new ArgumentException("Cannot use in message [',' '_' ' ']",nameof(triggerMessage));

			this.TriggerMessage = triggerMessage;
		}

	/// <summary>Initialize data for the steps</summary>
		protected void InitSteps(StepRow[] steps)
		{
			this._steps = steps;
			for(Int32 loop = 0; loop < this._steps.Length; loop++)
			{
				StepRow step = this._steps[loop];
				step.SetTitle(loop, this._steps.Length);
			}
		}

	/// <summary>Start the wizard</summary>
	/// <param name="restart">Clear all steps before start</param>
	/// <returns>The first step of the wizard</returns>
		public Reply Start(Boolean restart = false)
		{
			if(restart)
				this.Clear();

			StepRow nextStep = this.GetNextStep();
			return nextStep == null
				? null
				: new Reply() { Title = nextStep.Title, Markup = new ForceReplyMarkup(), };
		}

	/// <summary>Process a wizard step</summary>
	/// <param name="message">Message</param>
	/// <param name="isFinished">Wizard finished</param>
	/// <returns>Next step of the wizard or null if the message is unrecognized</returns>
		public Reply NextStep(Message message, out Boolean isFinished)
		{
			isFinished = false;
			if(message.ReplyToMessage != null)
			{
				Boolean isFound = false;
				if(this.IsTriggerMessage(message.ReplyToMessage.Text))
				{
					isFound = true;
					this._steps[0].TrySetValue(message.Text);
				} else
					foreach(StepRow row in this._steps)
						if(row.Title == message.ReplyToMessage.Text)
						{
							isFound = true;
							row.TrySetValue(message.Text);
							break;
						}
				if(!isFound)
					return null;
			} else if(this.IsTriggerMessage(message.Text))
			{
				String[] args = message.Text.Substring(this.TriggerMessage.Length).Split(new Char[] { ',', ' ','_' }, StringSplitOptions.RemoveEmptyEntries);
				this.Clear();
				if(args.Length > 0)
				{
					for(Int32 loop = 0; loop < args.Length && loop < this._steps.Length; loop++)
						this._steps[loop].TrySetValue(args[loop]);
				}
			} else
				return null;

			StepRow nextStep = this.GetNextStep();

			if(nextStep == null)
			{
				isFinished = true;
				return null;
			}

			return new Reply() { Title = nextStep.Title, ReplyToMessageId = message.MessageId, Markup = new ForceReplyMarkup(), };
		}

	/// <summary>Get the next step in the wizard</summary>
	/// <returns>Next step or null</returns>
		protected StepRow GetNextStep()
		{
			for(Int32 index = 0; index < this._steps.Length; index++)
				if(this._steps[index].Value == null)
					return this._steps[index];

			return null;
		}

		private Boolean IsTriggerMessage(String message)
		{
			if(message == null || this.TriggerMessage == null)
				return false;

			if(message.StartsWith(this.TriggerMessage, StringComparison.OrdinalIgnoreCase))
			{
				String[] args = message.Split(new Char[] { ',', ' ','_' }, StringSplitOptions.RemoveEmptyEntries);
				return args[0].Equals(this.TriggerMessage, StringComparison.OrdinalIgnoreCase);
			} else
				return false;
		}

	/// <summary>Clear all values of filled steps and return them as an object array</summary>
	/// <returns>Array of filled objects</returns>
		public Object[] Clear()
		{
			List<Object> result = new List<Object>(this._steps.Length);
			foreach(StepRow row in this._steps)
			{
				result.Add(row.Value);
				row.Value = null;
			}
			return result.ToArray();
		}
	}
}