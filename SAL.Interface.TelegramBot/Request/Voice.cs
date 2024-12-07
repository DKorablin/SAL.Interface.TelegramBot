using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>Голосовое сообщение от пользователя</summary>
	public class Voice : FileBase
	{
		/// <summary>Duration of the audio in seconds as defined by sender</summary>
		public Int32 Duration { get; set; }
	}
}