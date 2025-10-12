using System;

namespace SAL.Interface.TelegramBot.Response
{
	/// <summary>Transferring coordinates on the map</summary>
	public class GeoMarkup : IReplyMarkup
	{
		/// <summary>Latitude</summary>
		public Single Latitude { get; set; }

		/// <summary>Longitude</summary>
		public Single Longitude { get; set; }
	}
}