using System;

namespace SAL.Interface.TelegramBot
{
	/// <summary>Description of the method invoked by a command</summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class ChatShortcutAttribute : Attribute
	{
		/// <summary>Key by which the method can be called</summary>
		public String Key { get; set; }

		/// <summary>Reply message by which the method can be called</summary>
		public String ReplyToKey { get; set; }

		/// <summary>Description of the result of invoking the method by the key</summary>
		public String Description { get; set; }
	}
}