using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>Данные о пользователе, который пользуется чатом</summary>
	public class User
	{
		/// <summary>User's user identifier in Telegram</summary>
		public Int64 UserId { get; set; }
		/// <summary>User's first name</summary>
		public String FirstName { get; set; }
		/// <summary>User's last name</summary>
		public String LastName { get; set; }
		/// <summary>User's nick name</summary>
		public String UserName { get; set; }
	}
}