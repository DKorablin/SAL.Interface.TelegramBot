using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot.UI
{
	/// <summary>Строготипизированная версия элемента управления визардом</summary>
	/// <typeparam name="T">Тип объекта для получения из него шагов</typeparam>
	/// <remarks>Из объекта берутся все публичные свойства, а описание каждого шага берётся из <see cref="DescriptionAttribute"/></remarks>
	public class WizardTypedCtrl<T> : WizardCtrl where T: class, new()
	{
		/// <summary>Типизированный шаг визарда</summary>
		private class TypedStepRow : WizardCtrl.StepRow
		{
			private readonly PropertyInfo _property;
			private Object[] _stepInstance;

			/// <summary>Значение в инстансе</summary>
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

			private static Object GetPropertyDefaultValue(PropertyInfo property){
				DefaultValueAttribute attr = property.GetCustomAttribute<DefaultValueAttribute>();
				return attr == null
				? null
				: attr.Value;
			}
		}

		private T[] _stepInstanceReference;
		private Func<Message, T, IEnumerable<Reply>> _callback;

		/// <summary>Создание экземпляра контролла визарда, с передачей всех необходимых аргументов</summary>
		/// <param name="triggerMessage">
		/// Стартовое сообщение с которого начинается визард.
		/// Или null, если визард начинается вручную
		/// </param>
		/// <param name="instance">Источник визарда</param>
		/// <param name="callback">Метод обратного вызова</param>
		public WizardTypedCtrl(String triggerMessage, Object instance, Func<Message, T, IEnumerable<Reply>> callback)
			: base(triggerMessage, instance)
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

		/// <summary>Запустить визард, в качестве аргумента указывается (частично) заполненные шаги визарда</summary>
		/// <param name="steps">Частично заполненный визард (К примеру, заполнено одно из свойств для будущей идентификации)</param>
		/// <returns>Сообщение первого ответа</returns>
		public Reply Start(T steps)
		{
			this._stepInstanceReference[0] = steps;
			return base.Start(false);
		}

		/// <summary>Обработат полученное сообщение</summary>
		/// <param name="message">Сообщение от клиента</param>
		/// <returns>Массив ответов следующего шага или null</returns>
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