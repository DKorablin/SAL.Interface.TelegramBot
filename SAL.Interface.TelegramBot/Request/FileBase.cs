using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>Basic information about the client file being transferred</summary>
	public class FileBase
	{
		/// <summary>File ID for upload</summary>
		public String FileId { get; set; }

		/// <summary>File size</summary>
		public Int32 FileSize { get; set; }

		/// <summary>Mime-Type</summary>
		public String MimeType { get; set; }
	}
}