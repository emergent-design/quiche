using System;

namespace Quiche.Proxcard
{
	/// <summary>
	/// A messaged received over the serial bus
	/// </summary>
	public class SerialMessage
	{
	
		/// <summary>
		/// Gets the data read / sets the data to write.
		/// </summary>
		public byte [] Data	{ get; set; }


		/// <summary>
		/// Gets or sets the expected length of the data.
		/// </summary>
		public int Expected	{ get; set; }


		/// <summary>
		/// Gets a human-readable description of the message.
		/// </summary>
		public string Description
		{
			get { return string.Format("Serial message containing {0}", BitConverter.ToString(this.Data)); }
		}
	}
}

