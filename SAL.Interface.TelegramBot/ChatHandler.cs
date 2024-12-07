using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAL.Interface.TelegramBot.Request;
using SAL.Interface.TelegramBot.Response;

namespace SAL.Interface.TelegramBot
{
	/// <summary>Абстрактный класс хендлера чата для наследования инстансов</summary>
	public abstract class ChatHandler : IChatProcessor, IChatUsage
	{
		/// <summary>Обрабатываемое сообщение</summary>
		public Message Message { get; set; }

		/// <summary>Создание экземпляра хендлера для глобального кеширования</summary>
		public ChatHandler()
		{

		}

		/// <summary>Создание экземпляра хендлера для каждого запроса пользователя</summary>
		/// <param name="message">Сообщение от пользователя</param>
		public ChatHandler(Message message)
			=> this.Message = message;

		/// <summary>Создание экземпляра хендлера для кеширования на уровень чата</summary>
		/// <param name="chat">Данные чата для которого закешировать хендлер</param>
		public ChatHandler(Chat chat)
		{
		}

		/// <summary>Создание экземпляра хендлера для кеширования на уровень пользователя</summary>
		/// <param name="user">Данные пользователя для которого закешировать хендлер</param>
		public ChatHandler(User user)
		{
		}

		/// <summary>Обработка сообщением плагином, без привязки к методу</summary>
		/// <returns>По умолчанию результат пустой</returns>
		public virtual IEnumerable<Reply> ProcessMessage()
		{
			yield break;
		}

		/// <summary>Получение информации об использовании плагинов, путём запроса через рефлексию всех методов</summary>
		/// <returns>Ответ с информацией об использовании плагина через тегеграм</returns>
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
		{
			return this.ProcessMessage();
		}

		IEnumerable<UsageReply> IChatUsage.GetUsage(Message message)
		{
			return this.GetUsage();
		}
	}
}