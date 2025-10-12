using System;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Reply to a customer request</summary>
	public class Reply
	{
		/// <summary>Empty answer</summary>
		/// <remarks>Sent if the client's request has been processed but there is no response.</remarks>
		public static readonly Reply Empty = new Reply();

		/// <summary>Message title</summary>
		public virtual String Title { get; set; }

		/// <summary>Formatting used in response</summary>
		public ParseModeType ParseMode{ get; set; }

		/// <summary>The identifier of the message to which the response is being made</summary>
		public Int32 ReplyToMessageId { get; set; }

		/// <summary>It is displayed when it is necessary to edit the message.</summary>
		public Int32? EditMessageId { get; set; }

		/// <summary>Additional markup</summary>
		public IReplyMarkup Markup { get; set; }
	}
}