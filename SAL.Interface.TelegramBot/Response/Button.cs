using System;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Фиксированные кнопки с фиксированными действиями на клавиатуре</summary>
	public class Button
	{
		/// <summary>Текст на кнопке</summary>
		public String Text { get; private set; }

		/// <summary>Запрос контактов у клиента</summary>
		public Boolean RequestContact { get; set; }

		/// <summary>Запрос местоположения у клиента</summary>
		public Boolean RequestLocation { get; set; }

		/// <summary>Создание экземпляра класса с фиксированной кнопкой и фиксированным действием</summary>
		/// <param name="text">Текст на кнопке</param>
		/// <exception cref="ArgumentNullException">Текс на кнопке - обязателен</exception>
		public Button(String text)
		{
			if(String.IsNullOrWhiteSpace(text))
				throw new ArgumentNullException("text");

			this.Text = text;
		}
	}
}