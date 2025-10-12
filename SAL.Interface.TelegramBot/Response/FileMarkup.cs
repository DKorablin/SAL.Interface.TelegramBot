using System;
using System.IO;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Reply with file</summary>
	public class FileMarkup : IReplyMarkup
	{
		/// <summary>File name</summary>
		public String Name { get; set; }
		/// <summary>Link to data stream</summary>
		public Stream Stream { get; set; }
	}
}