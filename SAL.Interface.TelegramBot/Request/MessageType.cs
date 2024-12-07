namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>The type of a Telegram.Bot.Types.Message</summary>
	public enum MessageType
	{
		/// <summary>The Telegram.Bot.Types.Message is unknown</summary>
		UnknownMessage = 0,
		/// <summary>The Telegram.Bot.Types.Message contains text</summary>
		TextMessage = 1,
		/// <summary>Клиент передал голосовое сообщение</summary>
		Voice = 5,
		/// <summary>Клиент передал документ или аудиофайл</summary>
		Document = 6,
		/// <summary>Клиент расшарил контакт</summary>
		Contact = 9,
		/// <summary>Попытка обернуть CallbackQuery в сообщение</summary>
		CallbackQuery = 12,
		/// <summary>Успешное присоединение клиента сайта к Телеграму</summary>
		WebsiteConnected = 15,
	}
}