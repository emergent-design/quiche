using System;

namespace Quiche.Proxcard
{
	public class Acknowledge : SerialMessage
	{

		/// <summary>
		/// Checks the internal data buffer appears to be valid.
		/// </summary>
		/// <returns>
		/// <c>true</c> if valid; otherwise, <c>false</c>.
		/// </returns>
		protected bool CheckData()
		{
			return this.Data != null && this.Data.Length > 0;
		}

		/// <summary>
		/// Gets a value indicating whether an EEPROM error occurred.
		/// </summary>
		/// <value>
		/// <c>true</c> if an error occurred; otherwise, <c>false</c>.
		/// </value>
		public bool EEPROM
		{
			get	{ return !this.CheckData() || (this.Data[0] & 0x01) == 0x01; }
		}

		/// <summary>
		/// Gets a value indicating whether the received tag appears valid.
		/// </summary>
		/// <value>
		/// <c>true</c> if the tag seems valid; otherwise, <c>false</c>.
		/// </value>
		public bool TagOK
		{
			get { return this.CheckData() && (this.Data[0] & 0x02) == 0x02; }
		}	

		/// <summary>
		/// Gets a value indicating whether the last tag read completed correctly.
		/// </summary>
		/// <value>
		/// <c>true</c> if the tag read completed; otherwise, <c>false</c>.
		/// </value>
		public bool RxOK
		{
			get	{ return this.CheckData() && (this.Data[0] & 0x04) == 0x04;	}
		}


		/// <summary>
		/// Gets a value indicating whether a serial error occurred.
		/// </summary>
		/// <value>
		/// <c>true</c> on serial error; otherwise, <c>false</c>.
		/// </value>
		public bool SerialError
		{
			get	{ return !this.CheckData() || (this.Data[0] & 0x08) == 0x08; }
		}


		/// <summary>
		/// Gets a value indicating whether the relay (if present) is enabled.
		/// Unless you have a good reason to use this and know it's available
		/// on your hardware, assume no such device is present.
		/// </summary>
		/// <value>
		/// <c>true</c> if the relay is enabled; otherwise, <c>false</c>.
		/// </value>
		public bool RelayEnabled
		{
			get {	return this.CheckData() && (this.Data[0] & 0x10) == 0x10; }
		}


		/// <summary>
		/// Gets a value indicating whether an antenna fault occurred.
		/// </summary>
		/// <value>
		/// <c>true</c> on antenna fault; otherwise, <c>false</c>.
		/// </value>
		public bool AntennaFault
		{
			get {	return !this.CheckData() || (this.Data[0] & 0x20) == 0x20; }
		}


		/// <summary>
		/// Gets the response from the prox reader.
		/// </summary>
		/// <value>
		/// The response (less the status byte)
		/// </value>
		public byte[] Response
		{
			get
			{
				byte[] result = null;
				if (this.Data!=null && this.Data.Length>1) 
				{
					result = new byte[this.Data.Length - 1];
	    			Array.Copy(this.Data, 1, result, 0, this.Data.Length - 1);
				}
    			return result;
			}
		}
		
	}
}

