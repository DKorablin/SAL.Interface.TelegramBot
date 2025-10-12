using System.Collections.Generic;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot
{
	/// <summary>Handler for processing untyped client requests via Telegram</summary>
	public interface IChatProcessor : IChatMarker
	{
		/// <summary>Reply to the client's message</summary>
		/// <param name="message">Message from the client to prepare a response</param>
		/// <returns>Response prepared by the plugin or null if responding failed</returns>
		IEnumerable<Reply> ProcessMessage(Message message);
	}
}