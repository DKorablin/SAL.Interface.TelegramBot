using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>Client sends an audio file</summary>
	public class Audio : FileBase
	{
		/// <summary>Title of the audio file as specified by the sender or contained in tags</summary>
		public String Title { get; set; }

		/// <summary>Duration of the audio file</summary>
		public Int32 Duration { get; set; }
	}
}