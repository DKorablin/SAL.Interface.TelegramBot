using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>This object represents a phone contact</summary>
	public class Contact : User
	{
		/// <summary>Contact's phone number</summary>
		public String PhoneNumber { get; set; }
	}
}