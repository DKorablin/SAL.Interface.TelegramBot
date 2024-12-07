using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>This object represents a message</summary>
	public class Message
	{
		/// <summary>Conversation the message belongs to</summary>
		public Chat Chat { get; set; }

		/// <summary>Sender</summary>
		public User From { get; set; }

		/// <summary>Message is a shared contact, information about the contact</summary>
		public Contact Contact { get; set; }

		/// <summary>Date the message was sent</summary>
		public DateTime Date { get; set; }

		/// <summary>Message is a shared location, information about the location</summary>
		public Location Location { get; set; }

		/// <summary>Description is a general file, information about the file</summary>
		public Document Document { get; set; }

		/// <summary>Description is an audio file, information about the file</summary>
		public Audio Audio { get; set; }

		/// <summary>Description is a voice message, information about the file</summary>
		public Voice Voice { get; set; }

		/// <summary>For replies, the original message</summary>
		/// <remarks>Note that the Description object in this field will not contain further reply_to_message fields even if it itself is a reply.</remarks>
		public Message ReplyToMessage { get; set; }

		/// <summary>Unique message identifier</summary>
		public Int32 MessageId { get; set; }

		/// <summary>For text messages, the actual UTF-8 text of the message</summary>
		public String Text { get; set; }

		/// <summary>Data associated with the callback button</summary>
		public String Data { get; set; }

		/// <summary>Gets the Telegram.Bot.Types.Enums.MessageType of the Telegram.Bot.Types.Message</summary>
		public MessageType Type { get; set; }
	}
}