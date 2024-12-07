using System;

namespace SAL.Interface.TelegramBot.Request
{
	/// <summary>This object represents a point on the map</summary>
	public class Location
	{
		/// <summary>Latitude as defined by sender</summary>
		public Single Latitude { get; set; }

		/// <summary>Longitude as defined by sender</summary>
		public Single Longitude { get; set; }
	}
}