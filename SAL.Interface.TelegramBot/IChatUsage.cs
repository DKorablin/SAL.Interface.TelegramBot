using System.Collections.Generic;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot
{
	/// <summary>Interface for customized logic handling a request for plugin usage</summary>
	public interface IChatUsage : IChatMarker
	{
		/// <summary>Reply with a list of commands understood by this plugin</summary>
		/// <returns>Response containing the list of plugin usage commands</returns>
		IEnumerable<UsageReply> GetUsage(Message message);
	}
}