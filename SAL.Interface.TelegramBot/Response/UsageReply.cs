using System;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Ответ плагинов при запросе описаний комманд</summary>
	public class UsageReply
	{
		/// <summary>Ключ запроса использования плагина</summary>
		public String Key { get; set; }

		/// <summary>Описание запроса использования плагина</summary>
		public String Description { get; set; }

		/// <summary>Конструктор инструкции использования метода в чате</summary>
		public UsageReply()
		{

		}

		/// <summary>Конструктор метода использования метода в чете, с передачей атрибута описания метода</summary>
		/// <param name="shortcut">Атрибут описания метода</param>
		public UsageReply(ChatShortcutAttribute shortcut)
		{
			this.Key = shortcut.Key;
			this.Description = shortcut.Description;
		}
	}
}