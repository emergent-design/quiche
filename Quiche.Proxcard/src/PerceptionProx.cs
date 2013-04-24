using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;


namespace Quiche.Proxcard
{

	/// <summary>
	/// Perception prox - the serial prox card reader
	/// supplied by Perception-SI Ltd.
	/// 
	/// See www.perception-si.com
	/// 
	/// If you want to use a different prox card reader,
	/// implement an alternative IProx device.
	/// </summary>
	public class PerceptionProx : IProx, IDisposable
	{
		protected enum Page
		{
			PIN = 0x0,
			Type = 0x10,
			ProxyPIN = 0x11,
			Terminals = 0x14,
			Zones = 0x18
		}

		/// <summary>
		/// Calback for signalling progress of 
		/// card read/write operations
		/// </summary>
		public event CardProgress OnCardProgress = null;


		/// <summary>
		/// Serial connection to the reader hardware
		/// </summary>
		private SerialPort serial = null;


		/// <summary>
		/// Cancel reading flag
		/// </summary>
		protected bool cancel = false;


		/// <summary>
		/// Terminals listed on the last card read
		/// </summary>
		protected List<ushort> terminals = null;


		/// <summary>
		/// Zones listed on the last card read
		/// </summary>
		protected List<ushort> zones = null;


		/// <summary>
		/// The type of token on the last card read.
		/// </summary>
		private TokenType tokenType = TokenType.None;


		/// <summary>
		/// The PIN of the last card read.
		/// </summary>
		private uint pin = 0;


		/// <summary>
		/// Terminals listed on the last card read
		/// </summary>
		public List<ushort> Terminals { get { return this.terminals; } }


		/// <summary>
		/// Zones listed on the last card read
		/// </summary>
		public List<ushort> Zones { get { return this.zones; } }


		/// <summary>
		/// The type of token on the last card read.
		/// </summary>
		public TokenType TokenType {get	{return this.tokenType;}}


		/// <summary>
		/// The PIN of the last card read.
		/// </summary>
		public uint Pin {get {return this.pin;}}


		/// <summary>
		/// Send a reset command to the prox reader
		/// </summary>
		public void Reset()
		{
			this.SerialWrite(new SerialMessage(){ Data =  new byte[] { 0x46, 0x55, 0xAA }, Expected = 0});
		}


		/// <summary>
		/// Cancels an in-progress operation 
		/// </summary>
		public void Cancel()
		{
			this.cancel = true;
		}


		/// <summary>
		/// Writes a page of data to the card.
		/// </summary>
		/// <returns>
		/// Status response (inspect this to check the write was OK)
		/// </returns>
		/// <param name='page'>
		/// Address of the page to be written (must be less than 64;
		/// note that some pages are read-only; refer to the 
		/// manufacturers documentation for your tag of choice).
		/// </param>
		/// <param name='data'>
		/// Data to write - must be four bytes long.
		/// </param>
		protected Acknowledge WriteTagPage(byte page, byte[] data)
		{
			return (page<64 && page >= 0 && data!= null && data.Length==4) ? 
				this.SerialRead( new SerialMessage(){ Data = new byte[] { 0x57, page, data[0], data[1], data[2], data[3] }, Expected = 1 } ) : null;
		}


		/// <summary>
		/// Writes a block of pages to the card.
		/// </summary>
		/// <returns>
		/// Status response (inspect this to check the write was OK)
		/// </returns>
		/// <param name='page'>
		/// Address of the page to be written (must be less than 64;
		/// note that some pages are read-only; refer to the 
		/// manufacturers documentation for your tag of choice).
		/// </param>
		/// <param name='data'>
		/// Data to write
		/// </param>
		protected Acknowledge WriteTagBlock(byte page, byte[] data)
		{
			if (page<64 && page >= 0 && data != null)
			{
				byte[]packet = new byte[data.Length + 2];
				Array.Copy(data, 0, packet, 2, data.Length);
				packet[0] = 0x77;
				packet[1] = page;
				return this.SerialRead(new SerialMessage(){ Data = packet, Expected = 1 });
			}
			return null;
		}


		/// <summary>
		/// Returns a status response from the card reader.
		/// </summary>
		/// <returns>
		/// Status response
		/// </returns>
		protected Acknowledge GetStatus()
		{
			return this.SerialRead(new SerialMessage(){ Data = new byte[] { 0x53 }, Expected = 1 });
		}


		/// <summary>
		/// Reads a single page (4 bytes + status) of data from the card.
		/// </summary>
		/// <returns>
		/// Response containing the requested data and the status of the reader
		/// </returns>
		/// <param name='page'>
		/// Page of data to read (between 0 and 64)
		/// </param>
		protected Acknowledge ReadTagPage(byte page)
		{
			return (page<64 && page >= 0 ) ? this.SerialRead(new SerialMessage(){ Data = new byte[] { 0x52, page}, Expected = 5 }) : null;				
		}


		/// <summary>
		/// Reads a block of data from the card. The amount of data read depends
		/// upon the location of the page within the block. A block is four pages,
		/// and the remainder of the block starting from the page will be read 
		/// (so if you start a read on the last page of the block, you'll get a 
		/// single page response).
		/// </summary>
		/// <returns>
		/// Response containing the requested data and the status of the reader
		/// </returns>
		/// <param name='page'>
		/// Start page of data to read (between 0 and 64)
		/// </param>
		protected Acknowledge ReadTagBlock(byte page)
		{
			int expected = 1 + 4 * (4 - (page % 4));
			return (page<64 && page >= 0) ? this.SerialRead(new SerialMessage(){ Data = new byte[] { 0x72, page }, Expected = expected }) : null;
		}

		/// <summary>
		/// Writes a permission list (valid Zones or valid Terminals) to a card.
		/// </summary>
		/// <param name='permissions'>
		/// Permissions to write
		/// </param>
		/// <param name='address'>
		/// Address to write to 
		/// </param>
		protected bool WritePermission(List<ushort> permissions, Page address)
		{
			var data = this.PermissionsToCard(permissions);
			for (int attempts = 0; attempts < 6; attempts++)
			{
				if (this.cancel) break;
				var acknowledge = this.WriteTagBlock((byte)address, data);
				if (acknowledge != null && acknowledge.TagOK && acknowledge.RxOK) return true;
			}
			return false;
		}


		/// <summary>
		/// Reads a permission list (Zones or Terminals) from a card.
		/// </summary>
		/// <param name='address'>
		/// Address to read
		/// </param>
		/// <returns>
		/// Response containing the requested data and the status of the reader
		/// </returns>
		protected Acknowledge ReadPermission(Page address)
		{
			Acknowledge result = null;
			if (address == Page.Zones || address == Page.Terminals)
			{
				for (int attempts = 0; attempts < 6; attempts++)
				{
					result = this.ReadTagBlock((byte)address);
					if (result != null && result.TagOK && result.RxOK) break;
				}
			}
			return result;
		}


		/// <summary>
		/// Helper to remove all those nasty "is the callback null" checks
		/// when reporting progress
		/// </summary>
		protected void ReportProgress(bool error, int progress, bool reading)
		{
			if (this.OnCardProgress!=null) this.OnCardProgress(error, progress, reading); 
		}

		/// <summary>
		///  Writes a token to a prox card 
		/// </summary>
		/// <param name='type'>
		///  Type of the token 
		/// </param>
		/// <param name='pin'>
		///  Token PIN 
		/// </param>
		/// <param name='zones'>
		///  Zones the token is valid for 
		/// </param>
		/// <param name='terminals'>
		///  Terminals the token is valid for 
		/// </param>
		public bool WriteToken(TokenType type, uint pin=0, List<ushort> zones = null, List<ushort> terminals = null)
		{
			bool result = false;
			this.cancel = false;
			while (!this.HasCard())
			{
				if (this.cancel) 
				{
					if(this.OnCardProgress!=null) this.OnCardProgress(false, 0, false); 
					return false; 
				}
				System.Threading.Thread.Sleep(100);	
			}
			this.ReportProgress(false, 30, false); 

			if (!this.cancel) 
			{
				if (this.WritePermission(this.terminals, Page.Terminals))this.ReportProgress(false, 50, false); 
				else return false;
			}

			if (!this.cancel)
			{
				if (this.WritePermission(this.zones, Page.Zones)) this.ReportProgress(false, 70, false); 
				else return false;
			}
					
			for (int attempts = 0; attempts < 6; attempts++)
			{
				if (this.cancel) break;
				var acknowledge = this.WriteTagPage((byte)Page.ProxyPIN, TokenParser.PinToProx(pin));
				if (acknowledge != null && acknowledge.TagOK && acknowledge.RxOK) 
				{
					this.ReportProgress(false, 90, false); 
					result = true;
					break;
				}
			}

			for (int attempts = 0; attempts < 6; attempts++)
			{
				if (this.cancel) break;
				var acknowledge = this.WriteTagPage((byte)Page.Type, TokenParser.TypeToBytes(type));
				if (acknowledge != null && acknowledge.TagOK && acknowledge.RxOK) 
				{
					this.ReportProgress(false, 100, false); 
					result = true;
					break;
				}
			}

			return result;
		}

		/// <summary>
		///  Initialise the prox reader with the specified connection string. 
		/// </summary>
		/// <param name='connection'>
		///  The device-specific connection string. 
		/// </param>
		/// <param name='onCardProgress'>
		///  Progress handler for card operations 
		/// </param>
		public bool Initialise (string connection)
		{
			try
			{
				this.serial = new SerialPort(connection, 9600, Parity.None, 8, StopBits.One);
				this.serial.WriteTimeout	= 1000;
				this.serial.ReadTimeout		= 1000;
				this.serial.Handshake		= Handshake.RequestToSend;
				this.serial.Open();
				return true;
			}
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}
		}


		/// <summary>
		///  Tests the prox reader connection. 
		/// </summary>
		/// <returns>
		/// <c>true</c> if ANY response is received from the prox reader
		/// (even if this response is signalling errors)
		/// </returns>
		public bool TestConnection()
		{
			return (this.GetStatus()!=null);
		}


		/// <summary>
		///  Determines whether the reader has a card available to read. 
		/// </summary>
		/// <returns>
		/// <c>true</c> if a card appears to be present; otherwise, <c>false</c>.
		/// </returns>
		public bool HasCard ()
		{
			var ack = this.GetStatus();	
			return (ack!=null) ? ack.RxOK : false;
		}


		/// <summary>
		/// Checks a response appears to be valid.
		/// </summary>
		/// <returns>
		/// <c>true</c> if response is non-null and reporting no errors
		/// </returns>
		/// <param name='response'>
		/// The serial response to check
		/// </param>
		protected bool CheckResponse(Acknowledge response)
		{
			return response!=null && response.RxOK && response.TagOK;
		}


		/// <summary>
		/// Read from the prox card into the buffer of this instance.
		/// Retrieve the data read from this.Pin, this.TokenType, 
		/// this.Zones and this.Terminals
		/// </summary>
		/// <returns>
		/// <c>true</c> if the read was successful; otherwise <c>false</c>
		/// </returns>
		public bool Read ()
		{
			this.zones = null;
			this.terminals = null;
			this.cancel = false;
			this.pin = 0;
			this.tokenType = TokenType.None;

			if (cancel) return false;
			var type = this.ReadTagPage((byte)Page.Type);
			if (cancel) return false;

			if (!this.CheckResponse(type) || type.Data.Length!=5)
			{
				this.ReportProgress(true, 10, true);
				return false;
			}

			this.tokenType = TokenParser.TypeFromProx(type.Response);
			this.ReportProgress(false, 10, true);

			if (tokenType != TokenType.User && tokenType != TokenType.Proxy) 
			{
				this.ReportProgress(false, 100, true);
				return true;
			}

			if (cancel) return false;
			var id = this.ReadTagPage( (byte)(this.tokenType == TokenType.Proxy ? Page.ProxyPIN : Page.PIN));
			if (cancel) return false;

			if (!this.CheckResponse(id) || id.Data.Length!=5)
			{
				this.ReportProgress(true, 10, true);
				return false;
			}

			this.pin = TokenParser.PinFromProx(id.Response);//BitConverter.ToUInt32(id.Response, 0);
			this.ReportProgress(false, 20, true);

			if (cancel) return false;
			var terminals = this.ReadPermission(Page.Terminals); 
			if (cancel) return false;

			if (!this.CheckResponse(terminals))
			{
				this.ReportProgress(true, 20, true);
				return false;
			}

			this.ReportProgress(false, 50, true);

			if (cancel) return false;
			var zones = this.ReadPermission(Page.Zones);
			if (cancel) return false;

			if (!this.CheckResponse(zones))
			{
				this.ReportProgress(true, 50, true);
				return false;
			}

			this.ReportProgress(false, 80, true);

			this.ParseSupplemental(terminals.Response, zones.Response);
			this.ReportProgress(false, 100, true);

			return true;
		}

		/// <summary>
		/// Convert a permissions list to a byte array ready for
		/// writing to a card
		/// </summary>
		/// <returns>
		/// Permissions in a byte array for writing to card
		/// </returns>
		/// <param name='permlist'>
		/// Permissions as a nice ushort list
		/// </param>
		protected byte[] PermissionsToCard(List<ushort> permlist)
		{
			byte[] result = new byte[16];
			Array.Clear(result, 0, result.Length);
			if (permlist!=null)
			{
				for (int i = 0; i< permlist.Count; i++)
				{
					BitConverter.GetBytes(permlist[i]).CopyTo(result, i*2);
				}
			}
			return result;
		}


		/// <summary>
		/// Converts supplemental information (zones and terminals) from
		/// the byte array format stored on card to a nice ushort list
		/// and stores it in this
		/// </summary>
		/// <param name='terminals'>
		/// Terminal permissions read from card
		/// </param>
		/// <param name='zones'>
		/// Zone permissions read from card
		/// </param>
		protected void ParseSupplemental(byte[] terminals, byte[] zones)
		{
			this.terminals = new List<ushort>();
			this.zones = new List<ushort>();
	
			if (terminals!=null && zones!= null && terminals.Length == 16 && zones.Length == 16)
			{
				for (int i = 0; i<16; i+=2)
				{
					this.terminals.Add(BitConverter.ToUInt16(terminals, i));
					this.zones.Add(BitConverter.ToUInt16(zones, i));
				}
			}

		}


		/// <summary>
		/// Send a request for data to the serial device,
		/// read and parse the response into an Acknowledge
		/// object, waiting for appropriate. Unless your hardware
		/// produces the same response data as the PerceptionProx,
		/// this is probably not all that useful.
		/// CtsHolding transitions.
		/// </summary>
		/// <param name='request'>
		/// A request for data.
		/// </param>
		protected Acknowledge SerialRead(SerialMessage request)
		{
			int read = 0;
			Acknowledge result	= null;
			try
			{
				if ( this.SerialWrite(request) )
				{
					result = new Acknowledge { Data = new byte[request.Expected] };
					while (read < request.Expected) 
					{
						read += this.serial.Read(result.Data, read, request.Expected - read);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				result = null;
			}
			return (read == request.Expected) ? result : null;
		}


		/// <summary>
		/// Write the specified message over the port
		/// </summary>
		/// <param name='message'>
		/// Message to write over the port.
		/// </param>
		public bool SerialWrite(SerialMessage message)
		{
			try
			{
				this.serial.Write(message.Data, 0, message.Data.Length);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}
			
			return true;
		}


		/// <summary>
		/// Releases all resource used by the <see cref="QuiRing.IO.PerceptionProx"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="QuiRing.IO.PerceptionProx"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="QuiRing.IO.PerceptionProx"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="QuiRing.IO.PerceptionProx"/> so
		/// the garbage collector can reclaim the memory that the <see cref="QuiRing.IO.PerceptionProx"/> was occupying.
		/// </remarks>
		public void Dispose ()
		{
			if(this.serial!=null)
			{
				this.serial.Dispose();
			}
		}
	}
}

