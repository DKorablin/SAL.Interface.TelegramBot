using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>This object represents a chat</summary>
	public class Chat
	{
		/// <summary>Unique identifier for this chat, not exceeding 1e13 by absolute value</summary>
		public Int64 Id { get; set; }

		/// <summary>First name of the other party in a private chat</summary>
		public String FirstName { get; set; }

		/// <summary>Last name of the other party in a private chat</summary>
		public String LastName { get; set; }

		/// <summary>Title, for channels and group chats</summary>
		public String Title { get; set; }

		/// <summary>Username, for private chats and channels if available</summary>
		public String UserName { get; set; }
	}
}