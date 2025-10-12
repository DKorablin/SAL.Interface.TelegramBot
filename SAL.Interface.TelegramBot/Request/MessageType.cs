namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>The type of a Telegram.Bot.Types.Message</summary>
	public enum MessageType
	{
		/// <summary>The Telegram.Bot.Types.Message is unknown</summary>
		UnknownMessage = 0,
		/// <summary>The Telegram.Bot.Types.Message contains text</summary>
		TextMessage = 1,
		/// <summary>The client sent a voice message</summary>
		Voice = 5,
		/// <summary>The client has submitted a document or audio file</summary>
		Document = 6,
		/// <summary>The client shared the contact</summary>
		Contact = 9,
		/// <summary>Trying to wrap a CallbackQuery in a message</summary>
		CallbackQuery = 12,
		/// <summary>Successfully connecting the website client to Telegram</summary>
		WebsiteConnected = 15,
	}
}