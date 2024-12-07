using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>Клиент передаёт файл</summary>
	public class Document : FileBase
	{
		/// <summary>Наименование файла</summary>
		public String FileName { get; set; }
	}
}