using System.Collections.Generic;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot
{
	/// <summary>Интерфейс для кастомизированной логики обработки получения запроса на использование плагина</summary>
	public interface IChatUsage : IChatMarker
	{
		/// <summary>Ответить списком комманд, которые понимает данный плагин</summary>
		/// <returns>Ответ списка комманд использования плагина</returns>
		IEnumerable<UsageReply> GetUsage(Message message);
	}
}