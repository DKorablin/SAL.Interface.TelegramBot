using System;
using System.Collections.Generic;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Ответ клиенту с встроенной клавиатурой</summary>
	public class InlineKeyboardMarkup : IReplyMarkup
	{
		/// <summary>Выставляется при необходимости отредактировать сообщение</summary>
		public Int32? EditMessageId { get; set; }

		/// <summary>Клавиатура с кнопками</summary>
		public List<InlineButton[]> Keyboard { get; set; }

		/// <summary>Создание класса пустой клавиатуры без кнопок</summary>
		public InlineKeyboardMarkup()
			: this(new List<InlineButton[]>())
		{
		}

		/// <summary>Создание класса клавиатуры с рядами кнопок</summary>
		/// <param name="keyboard">Ряды кнопок</param>
		public InlineKeyboardMarkup(List<InlineButton[]> keyboard)
			=> this.Keyboard = keyboard ?? throw new ArgumentNullException(nameof(keyboard));
	}
}