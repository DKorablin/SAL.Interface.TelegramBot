using System;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Plugins' response when requesting command descriptions</summary>
	public class UsageReply
	{
		/// <summary>Plugin usage request key</summary>
		public String Key { get; set; }

		/// <summary>Description of the plugin usage request</summary>
		public String Description { get; set; }

		/// <summary>Constructor of instructions for using a method in a chat</summary>
		public UsageReply()
		{

		}

		/// <summary>Constructor of the method for using the method in a couple, with the transfer of the method description attribute</summary>
		/// <param name="shortcut">Method description attribute</param>
		public UsageReply(ChatShortcutAttribute shortcut)
		{
			this.Key = shortcut.Key;
			this.Description = shortcut.Description;
		}
	}
}