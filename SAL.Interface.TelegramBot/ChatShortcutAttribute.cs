using System;

namespace SAL.Interface.TelegramBot
{
	/// <summary>Описание метода, вызываемого по комманде</summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class ChatShortcutAttribute : Attribute
	{
		/// <summary>Ключ по которому можно вызвать метод</summary>
		public String Key { get; set; }

		/// <summary>Ответное сообщение по которому можно вызвать метод</summary>
		public String ReplyToKey { get; set; }

		/// <summary>Описание результата вызова метода по ключу</summary>
		public String Description { get; set; }
	}
}