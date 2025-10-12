using System;
using System.Collections.Generic;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Reply to a client with a built-in keyboard</summary>
	public class InlineKeyboardMarkup : IReplyMarkup
	{
		/// <summary>It is displayed when it is necessary to edit the message.</summary>
		public Int32? EditMessageId { get; set; }

		/// <summary>Keyboard with buttons</summary>
		public List<InlineButton[]> Keyboard { get; set; }

		/// <summary>Creating a blank keyboard class with no buttons</summary>
		public InlineKeyboardMarkup()
			: this(new List<InlineButton[]>())
		{
		}

		/// <summary>Creating a Keyboard Class with Rows of Buttons</summary>
		/// <param name="keyboard">Rows of buttons</param>
		public InlineKeyboardMarkup(List<InlineButton[]> keyboard)
			=> this.Keyboard = keyboard ?? throw new ArgumentNullException(nameof(keyboard));
	}
}