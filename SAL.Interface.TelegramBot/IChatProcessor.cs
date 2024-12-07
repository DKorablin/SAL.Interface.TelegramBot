using System.Collections.Generic;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot
{
	/// <summary>Хендлер обработку нетипизированных запросов от клиента через Telegram</summary>
	public interface IChatProcessor : IChatMarker
	{
		/// <summary>Ответить на сообщение клиента</summary>
		/// <param name="message">Сообщение от клеинта, на которое подготовить ответ</param>
		/// <returns>Ответ подготовленный плагином или null, если ответить по запросу - не вышло</returns>
		IEnumerable<Reply> ProcessMessage(Message message);
	}
}