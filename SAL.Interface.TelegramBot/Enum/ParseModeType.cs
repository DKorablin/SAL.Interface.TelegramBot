namespace SAL.Interface.TelegramBot
{
	/// <summary>Форматирование используемое при парсинге сообщения пользователю в клиенте</summary>
	public enum ParseModeType
	{
		/// <summary>Парсинг по умолчанию</summary>
		Default,
		/// <summary>Форматирование по белому списку</summary>
		Markdown,
		/// <summary>Форматирование HTML вёрстки</summary>
		Html,
	}
}