using System;
using System.IO;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Ответ файлом</summary>
	public class FileMarkup : IReplyMarkup
	{
		/// <summary>Наименование файла</summary>
		public String Name { get; set; }
		/// <summary>Ссылка на поток данных</summary>
		public Stream Stream { get; set; }
	}
}