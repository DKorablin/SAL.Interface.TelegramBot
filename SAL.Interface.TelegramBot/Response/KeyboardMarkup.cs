using System;
using System.Collections.Generic;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Ответить клиенту клавиатурой</summary>
	public class KeyboardMarkup : IReplyMarkup
	{
		/// <summary>Array of button rows, each represented by an Array of KeyboardButton objects</summary>
		public List<Button[]> Keyboard { get; set; }

		/// <summary>Requests clients to hide the keyboard as soon as it's been used</summary>
		public Boolean OneTimeKeybord { get; set; }

		/// <summary>Создание класса пусотй клавиатуры</summary>
		public KeyboardMarkup()
		: this(new List<Button[]>())
		{
		}

		/// <summary>Создание экземпляра класса клавиатуры</summary>
		/// <param name="keyboard">Ряды и колонки кнопок клавиатуры</param>
		public KeyboardMarkup(List<Button[]> keyboard)
			=> this.Keyboard = keyboard?? throw new ArgumentNullException(nameof(keyboard));
	}
}