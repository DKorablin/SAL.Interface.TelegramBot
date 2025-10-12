using System;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Fixed buttons with fixed actions on the keyboard</summary>
	public class Button
	{
		/// <summary>Text on the button</summary>
		public String Text { get; private set; }

		/// <summary>Requesting contact information from the client</summary>
		public Boolean RequestContact { get; set; }

		/// <summary>Requesting location from the client</summary>
		public Boolean RequestLocation { get; set; }

		/// <summary>Creating an instance of a class with a fixed button and a fixed action</summary>
		/// <param name="text">Text on the button</param>
		/// <exception cref="ArgumentNullException">Button text is required.</exception>
		public Button(String text)
		{
			if(String.IsNullOrWhiteSpace(text))
				throw new ArgumentNullException(nameof(text));

			this.Text = text;
		}
	}
}