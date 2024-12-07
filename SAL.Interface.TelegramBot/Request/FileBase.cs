using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>Базовая информация о передаваемом клиентов файле</summary>
	public class FileBase
	{
		/// <summary>Идентификатор файла для загрузки</summary>
		public String FileId { get; set; }

		/// <summary>Размер файла</summary>
		public Int32 FileSize { get; set; }

		/// <summary>Mime-Type</summary>
		public String MimeType { get; set; }
	}
}