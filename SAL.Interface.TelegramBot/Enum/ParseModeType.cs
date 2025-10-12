namespace SAL.Interface.TelegramBot
{
	/// <summary>Formatting used when parsing a message to a user in the client</summary>
	public enum ParseModeType
	{
		/// <summary>Parsing by default</summary>
		Default,
		/// <summary>Whitelist formatting</summary>
		Markdown,
		/// <summary>HTML layout formatting</summary>
		Html,
	}
}