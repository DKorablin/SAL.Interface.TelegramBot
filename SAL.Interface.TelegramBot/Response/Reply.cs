using System;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Ответ на запрос клиента</summary>
	public class Reply
	{
		/// <summary>Пустой ответ</summary>
		/// <remarks>Отправляется, в случае если запрос клиента обработан, но на него нет ответа</remarks>
		public static readonly Reply Empty = new Reply();

		/// <summary>Заголовок сообщения</summary>
		public virtual String Title { get; set; }

		/// <summary>Форматирование используемое при ответе</summary>
		public ParseModeType ParseMode{ get; set; }

		/// <summary>Идентификатор сообщения на которое производится ответ</summary>
		public Int32 ReplyToMessageId { get; set; }

		/// <summary>Выставляется при необходимости отредактировать сообщение</summary>
		public Int32? EditMessageId { get; set; }

		/// <summary>Дополнительная разметка</summary>
		public IReplyMarkup Markup { get; set; }
	}
}