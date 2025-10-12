using System;
using System.Collections.Generic;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Reply to the client using the keyboard</summary>
	public class KeyboardMarkup : IReplyMarkup
	{
		/// <summary>Array of button rows, each represented by an Array of KeyboardButton objects</summary>
		public List<Button[]> Keyboard { get; set; }

		/// <summary>Requests clients to hide the keyboard as soon as it's been used</summary>
		public Boolean OneTimeKeyboard { get; set; }

		/// <summary>Creating an empty keyboard class</summary>
		public KeyboardMarkup()
		: this(new List<Button[]>())
		{
		}

		/// <summary>Creating an instance of the keyboard class</summary>
		/// <param name="keyboard">Rows and columns of keyboard buttons</param>
		public KeyboardMarkup(List<Button[]> keyboard)
			=> this.Keyboard = keyboard?? throw new ArgumentNullException(nameof(keyboard));
	}
}