using System;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Передача координат на карте</summary>
	public class GeoMarkup : IReplyMarkup
	{
		/// <summary>Долгота</summary>
		public Single Latitude { get; set; }

		/// <summary>Широта</summary>
		public Single Longitude { get; set; }
	}
}