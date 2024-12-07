using System;
using System.Collections.Generic;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot.UI
{
	/// <summary>Элемент управления, который позволяет генерить визарды</summary>
	public class WizardCtrl
	{
		/// <summary>Ряд, описывающий шаг в визарде</summary>
		public class StepRow
		{
			/// <summary>Текст, описывающий требуемые данные от клиента</summary>
			private String Text { get; set; }

			/// <summary>Заголовок, отображаемый пользователю</summary>
			internal String Title { get; private set; }

			/// <summary>Тип требуемого значения</summary>
			public TypeCode ValueType { get; set; }

			/// <summary>Полученное значение от пользователя</summary>
			public virtual Object Value { get; set; }

			/// <summary>Значение по умолчанию, если клиент передал пустое значение</summary>
			public virtual Object DefaultValue{ get; set; }

			/// <summary>Создание ряда шага для визарда</summary>
			/// <param name="text">Текст для отображения клиенту</param>
			/// <param name="type">Тип требуемого значения</param>
			public StepRow(String text, Type type)
				: this(text, StepRow.GetTypeCode(type))
			{
			}

			/// <summary>Создание ряда шага для визарда</summary>
			/// <param name="text">Текст для отображения клиенту</param>
			/// <param name="valueType">Тип требуемого значения</param>
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

			/// <summary>Внутренний класс установки фиксированного заголовка</summary>
			/// <param name="index">Индекс шага в массиве шагов</param>
			/// <param name="totalRows">Общее количество рядов</param>
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

		private readonly Object _instance;

		private StepRow[] _steps;

		/// <summary>Сообщение, с которого начинается работа визарда</summary>
		public String TriggerMessage { get; private set; }

		/// <summary>Создание визарда со списком шагов</summary>
		/// <param name="triggerMessage">Сообщение, с которого начинается визард</param>
		/// <param name="instance">Источник создания элемента управления визардом</param>
		/// <param name="steps">Список шагов, в которых начинается визард</param>
		public WizardCtrl(String triggerMessage, Object instance, params StepRow[] steps)
			: this(triggerMessage, instance)
			=> this.InitSteps(steps);

		/// <summary>Создание визарда с указанием сообщение срабатывания и источник создания элемента</summary>
		/// <param name="triggerMessage">Сообщение, с которого начинается визард</param>
		/// <param name="instance">Источник создания элемента управления визардом</param>
		protected WizardCtrl(String triggerMessage, Object instance)
		{
			if(triggerMessage != null && triggerMessage.IndexOfAny(new Char[] { ',', '_', ' ' }) > -1)
				throw new ArgumentException("Нельзя использовать в сообщении [',' '_' ' ']",nameof(triggerMessage));

			this.TriggerMessage = triggerMessage;
			this._instance = instance;
		}

		/// <summary>Инициализировать данные по шагам</summary>
		protected void InitSteps(StepRow[] steps)
		{
			this._steps = steps;
			for(Int32 loop = 0; loop < this._steps.Length; loop++)
			{
				StepRow step = this._steps[loop];
				step.SetTitle(loop, this._steps.Length);
			}
		}

		/// <summary>Запуск визарда</summary>
		/// <param name="restart">Почистить все шаги перед стартом</param>
		/// <returns>Первый шаг визарда</returns>
		public Reply Start(Boolean restart = false)
		{
			if(restart)
				this.Clear();

			StepRow nextStep = this.GetNextStep();
			return nextStep == null
				? null
				: new Reply() { Title = nextStep.Title, Markup = new ForceReplyMarkup(), };
		}

		/// <summary>Обработка шага визарда</summary>
		/// <param name="message">Сообщение</param>
		/// <param name="isFinished">Визард завершился</param>
		/// <returns>Следующий шаг визарда или null, если сообщение нераспознанно</returns>
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

		/// <summary>Получить следующий шаг в визарде</summary>
		/// <returns>Следующий шаг, либо null</returns>
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

		/// <summary>Почистить все значения заполненных хагов и вернуть их в качестве массива объектов</summary>
		/// <returns>Массив заполненных объектов</returns>
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