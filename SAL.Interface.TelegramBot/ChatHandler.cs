using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot
{
	/// <summary>Abstract chat handler class for inheriting instances</summary>
	public abstract class ChatHandler : IChatProcessor, IChatUsage
	{
		/// <summary>Message being processed</summary>
		public Message Message { get; set; }

		/// <summary>Create a handler instance for global caching</summary>
		protected ChatHandler()
		{

		}

		/// <summary>Create a handler instance for each user request</summary>
		/// <param name="message">Message from the user</param>
		protected ChatHandler(Message message)
			=> this.Message = message;

		/// <summary>Create a handler instance for caching at the chat level</summary>
		/// <param name="chat">Chat data for which to cache the handler</param>
		protected ChatHandler(Chat chat)
		{
		}

		/// <summary>Create a handler instance for caching at the user level</summary>
		/// <param name="user">User data for which to cache the handler</param>
		protected ChatHandler(User user)
		{
		}

		/// <summary>Processing the message by the plugin without binding to a method</summary>
		/// <returns>By default the result is empty</returns>
		public virtual IEnumerable<Reply> ProcessMessage()
		{
			yield break;
		}

		/// <summary>Obtaining information about plugin usage by querying all methods via reflection</summary>
		/// <returns>Response with information about plugin usage via Telegram</returns>
		public virtual IEnumerable<UsageReply> GetUsage()
		{
			MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
			foreach(MethodInfo method in methods.Where(m => m.ReturnType == typeof(IEnumerable<Reply>) || m.ReturnType == typeof(Reply[]) || m.ReturnType == typeof(Reply)))
			{
				ChatShortcutAttribute chatShortcut = (ChatShortcutAttribute)method.GetCustomAttribute(typeof(ChatShortcutAttribute));
				if(chatShortcut != null && chatShortcut.Key != null)
					yield return new UsageReply(chatShortcut);
			}
		}

		IEnumerable<Reply> IChatProcessor.ProcessMessage(Message message)
			=> this.ProcessMessage();

		IEnumerable<UsageReply> IChatUsage.GetUsage(Message message)
			=> this.GetUsage();
	}
}