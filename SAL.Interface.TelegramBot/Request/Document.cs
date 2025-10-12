using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>The client transfers a file</summary>
	public class Document : FileBase
	{
		/// <summary>File name</summary>
		public String FileName { get; set; }
	}
}