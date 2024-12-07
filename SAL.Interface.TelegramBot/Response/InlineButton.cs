using System;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Кнопка, которая рисуется на клавиатуре</summary>
	public class InlineButton
	{
		/// <summary>Label text on the button</summary>
		public String Text { get; private set; }

		/// <summary>Optional. Data to be sent in a callback query to the bot when button is pressed</summary>
		public String CallbackData { get; private set; }

		/// <summary>Optional. HTTP url to be opened when button is pressed</summary>
		public String Url { get; set; }

		/// <summary>Создание экземпляра класса, который рисует кнопку для клиента</summary>
		/// <param name="title">Заголовок кнопки</param>
		/// <param name="callbackData">Данные обратного вызова</param>
		/// <exception cref="ArgumentNullException">Заголовок кнопки обязательный</exception>
		/// <exception cref="ArgumentNullException">Данные обратного вызова обязательны</exception>
		public InlineButton(String title, String callbackData)
		{
			if(String.IsNullOrWhiteSpace(title))
				throw new ArgumentNullException(nameof(title));
			if(String.IsNullOrWhiteSpace(callbackData))
				throw new ArgumentNullException(nameof(callbackData));

			this.Text = title;
			this.CallbackData = callbackData;
		}
	}
}