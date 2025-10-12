using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot.UI
{
	/// <summary>Strongly typed version of the wizard control</summary>
	/// <typeparam name="T">Type of the object from which steps are obtained</typeparam>
	/// <remarks>All public properties are taken from the object, and the description of each step is taken from <see cref="DescriptionAttribute"/></remarks>
	public class WizardTypedCtrl<T> : WizardCtrl where T: class, new()
	{
		/// <summary>Typed wizard step</summary>
		private sealed class TypedStepRow : WizardCtrl.StepRow
		{
			private readonly PropertyInfo _property;
			private readonly Object[] _stepInstance;

			/// <summary>Value in the instance</summary>
			public override Object Value
			{
				get => this._property.GetValue(this._stepInstance[0]);
				set => this._property.SetValue(this._stepInstance[0], value);
			}

			public TypedStepRow(Object[] stepInstance, PropertyInfo property)
				: base(GetPropertyText(property), property.PropertyType)
			{
				this._property = property;
				this._stepInstance = stepInstance;
				base.DefaultValue = GetPropertyDefaultValue(property);
			}

			private static String GetPropertyText(PropertyInfo property)
			{
				DescriptionAttribute attr = property.GetCustomAttribute<DescriptionAttribute>();
				return attr == null
					? property.Name
					: attr.Description;
			}

			private static Object GetPropertyDefaultValue(PropertyInfo property)
			{
				DefaultValueAttribute attr = property.GetCustomAttribute<DefaultValueAttribute>();
				return attr?.Value;
			}
		}

		private readonly T[] _stepInstanceReference;
		private readonly Func<Message, T, IEnumerable<Reply>> _callback;

		/// <summary>Create an instance of the wizard control, passing all necessary arguments</summary>
		/// <param name="triggerMessage">
		/// Start message from which the wizard begins.
		/// Or null if the wizard starts manually
		/// </param>
		/// <param name="callback">Callback method</param>
		public WizardTypedCtrl(String triggerMessage, Func<Message, T, IEnumerable<Reply>> callback)
			: base(triggerMessage)
		{
			this._stepInstanceReference = new T[] { new T(), };
			this._callback = callback;

			PropertyInfo[] properties = this._stepInstanceReference[0].GetType().GetProperties();
			List<TypedStepRow> stepInfo = new List<TypedStepRow>(properties.Length);
			for(Int32 loop = 0; loop < properties.Length; loop++)
			{
				PropertyInfo prop = properties[loop];
				if(prop.CanRead && prop.CanWrite)
					stepInfo.Add(new TypedStepRow(this._stepInstanceReference, prop));
			}

			base.InitSteps(stepInfo.ToArray());
		}

		/// <summary>Start the wizard, passing (partially) filled wizard steps as an argument</summary>
		/// <param name="steps">Partially filled wizard (e.g., one of the properties is filled for future identification)</param>
		/// <returns>Message of the first response</returns>
		public Reply Start(T steps)
		{
			this._stepInstanceReference[0] = steps;
			return base.Start(false);
		}

		/// <summary>Process the received message</summary>
		/// <param name="message">Message from the client</param>
		/// <returns>Array of replies of the next step or null</returns>
		public IEnumerable<Reply> NextStep(Message message)
		{
			Reply result = base.NextStep(message, out Boolean isFinished);
			if(result != null)
				yield return result;
			else if(result == null && isFinished)
			{
				foreach(Reply reply in this._callback(message, this._stepInstanceReference[0]))
					yield return reply;
				base.Clear();
			} else
				yield return null;
		}
	}
}