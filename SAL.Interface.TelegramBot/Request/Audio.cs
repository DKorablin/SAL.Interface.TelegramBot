using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>Клиент передаёт аудио файл</summary>
	public class Audio : FileBase
	{
		/// <summary>Название аудиофайла как указал отправитель или как написано в тегах</summary>
		public String Title { get; set; }

		/// <summary>Длительность аудиофайла</summary>
		public Int32 Duration { get; set; }
	}
}