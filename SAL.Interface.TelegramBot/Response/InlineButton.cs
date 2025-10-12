using System;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>A button that is drawn on the keyboard</summary>
	public class InlineButton
	{
		/// <summary>Label text on the button</summary>
		public String Text { get; private set; }

		/// <summary>Optional. Data to be sent in a callback query to the bot when button is pressed</summary>
		public String CallbackData { get; private set; }

		/// <summary>Optional. HTTP url to be opened when button is pressed</summary>
		public String Url { get; set; }

		/// <summary>Create an instance of a class that draws a button for the client</summary>
		/// <param name="title">Button title</param>
		/// <param name="callbackData">Callback data</param>
		/// <exception cref="ArgumentNullException">Button title is required</exception>
		/// <exception cref="ArgumentNullException">Callback data is required</exception>
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