using System;
using System.Threading;
using System.Collections.Generic;

namespace Quiche.Proxcard
{

	/// <summary>
	/// Handles a card read. tokenType specifies the type of token on the card. pin contains the card PIN number.
	/// units contains a list of all terminals the card will grant access to on enrolment or permissions update.
	/// zones contains a list of all the zones the card will grant access to on enrolment or permissions update.
	/// </summary>
	public delegate void InputHandler(TokenType tokenType, uint pin, List<ushort> units, List<ushort> zones);

	/// <summary>
	/// Handles completion of a card write operation. success is false if the card write did not complete successfully.
	/// </summary>
	public delegate void WriteHandler(bool success);

	/// <summary>
	/// Handles a card operation being aborted. When this is called, the operation has already been aborted. If the
	/// aborted operation was a write, writeAborted is true, otherwise it is false.
	/// </summary>
	public delegate void AbortHandler(bool writeAborted);


	/// <summary>
	/// Prox input - Responsible for wrapping an IProx device.
	/// ProxInput also provides a thread for the IProx to run in.
	/// </summary>
	public class ProxInput  : IDisposable
	{
		/// <summary>
		/// The thread responsible for polling the 
		/// prox reader
		/// </summary>
		private Thread thread				= null;


		/// <summary>
		/// Shutdown flag
		/// </summary>
		private ManualResetEvent shutdown	= new ManualResetEvent(false);


		/// <summary>
		/// Pause flag (for suspending polling)
		/// </summary>
		private ManualResetEvent pause		= new ManualResetEvent(true);


		/// <summary>
		/// The prox hardware device
		/// </summary>
		private IProx prox;


		/// <summary>
		/// Signals that the device should be written
		/// to. False if the device is being read from.
		/// </summary>
		private bool writeMode = false;


		/// <summary>
		/// Gets a value indicating whether this <see cref="Quiche.Proxcard.ProxInput"/> is writing.
		/// </summary>
		/// <value>
		/// <c>true</c> if writing; otherwise, <c>false</c>.
		/// </value>
		public bool Writing 	{get {return this.writeMode;}}
	

		/// <summary>
		/// Signals that a card is present
		/// (works similarly to debouncing keys, 
		/// hence "keyDown")
		/// </summary>
		private bool keyDown  = false;


		/// <summary>
		/// Occurs when a card is read.
		/// If you haven't supplied a handler,
		/// you won't be able to read cards.
		/// </summary>
		public event InputHandler CardRead = null;

		/// <summary>
		/// Occurs when a card is written.
		/// </summary>
		public event WriteHandler CardWritten = null;

		/// <summary>
		/// Occurs when progress is made on a card operation
		/// </summary>
		public event CardProgress CardProgress = null;

		/// <summary>
		/// Occurs when a card operation has been cancelled.
		/// </summary>
		public event AbortHandler CancelCardOperation = null;

		/// <summary>
		/// Buffer containing the type of token
		/// which is to be written
		/// </summary>
		private TokenType toWrite;


		/// <summary>
		/// Buffer containing the PIN of the
		/// token to be written
		/// </summary>
		private uint pinToWrite;


		/// <summary>
		/// Buffer containing a list of Zone 
		/// reference IDs to be written to the card
		/// (only read during enrolment or
		/// access permissions modification)
		/// </summary>
		private List<ushort> zonesToWrite = null;


		/// <summary>
		/// Buffer containing a list of Terminal 
		/// reference IDs to be written to the card
		/// (only read during enrolment or
		/// access permissions modification)
		/// </summary>
		private List<ushort> terminalsToWrite = null;


		/// <summary>
		/// True if this is currently cancelling an operation
		/// </summary>
		private bool cancelling = false;


		/// <summary>
		/// Connect to the specified device.
		/// </summary>
		/// <param name='device'>
		/// If set to <c>true</c> device.
		/// </param>
		public bool Connect (IProx device)
		{
			if (device!= null)
			{
				if(this.prox!=null)
				{
					this.prox.Dispose();
					this.prox = null;
				}
				this.prox = device;	
				this.prox.OnCardProgress += (e, p, r) => { 
					if (this.CardProgress != null) this.CardProgress(e, p, r); 
				};
				this.Pause();
				this.Start();
				return true;
			}
			return false;
		}


		/// <summary>
		/// Disconnect from the prox hardware.
		/// </summary>
		public void Disconnect()
		{
			this.Stop();
			if (this.prox!=null)
			{
				this.prox.Dispose();
				this.prox = null;
			}
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		public void Start()
		{
			if(this.thread==null)
			{
				this.thread = new Thread(new ThreadStart(this.Entry));
				this.cancelling = false;
				this.shutdown.Reset();
				this.thread.Start();
			}	
		}


		/// <summary>
		/// Writes a token to card using the internal IProx device
		/// </summary>
		/// <param name='type'>
		/// Type of token to write
		/// </param>
		/// <param name='pin'>
		/// PIN of the token to write
		/// </param>
		/// <param name='zones'>
		/// List of zones this token will be valid for
		/// </param>
		/// <param name='terminals'>
		/// List of terminals this token will be valid for
		/// </param>
		public void WriteCard(TokenType type, uint pin=0, List<ushort> zones = null, List<ushort> terminals = null)
		{
			this.cancelling = false;
			this.toWrite = type;
			this.writeMode = true;
			this.pinToWrite = pin;
			this.zonesToWrite = zones;
			this.terminalsToWrite = terminals;
			this.Resume();
		}


		/// <summary>
		/// Aborts any pending or in-progress operations.
		/// </summary>
		public void Cancel()
		{
			this.cancelling = true;
			if (this.prox!=null) this.prox.Cancel();
		}


		/// <summary>
		/// Resets the internal status flags and signals that a
		/// card operation has been cancelled
		/// </summary>
		/// <param name='writeCancelled'>
		/// Write cancelled.
		/// </param>
		protected void CardOperationCancelled(bool writeCancelled)
		{
			this.writeMode = false;
			this.cancelling = false;
			if (this.CancelCardOperation!=null) this.CancelCardOperation(writeCancelled);
		}


		/// <summary>
		/// Stop this instance.
		/// </summary>
		public void Stop()
		{
			this.cancelling = true;
			this.shutdown.Set();
			this.pause.Set();
			if (this.thread !=null)	this.thread.Join();
			this.thread = null;
		}


		/// <summary>
		/// Pause this instance.
		/// </summary>
		public void Pause()
		{
			this.pause.Reset();
		}


		/// <summary>
		/// Resume this instance.
		/// </summary>
		public void Resume()
		{
			this.pause.Set();
		}

		/// <summary>
		/// Start a read operation
		/// </summary>
		public void Read()
		{
			this.cancelling = false;
			this.Resume();
		}

		/// <summary>
		/// Entry point of the polling thread.
		/// </summary>
		protected void Entry()
		{
			while(!this.shutdown.WaitOne(0))
			{
				this.pause.WaitOne(Timeout.Infinite);
				if (!this.cancelling)
				{
					if (this.prox.HasCard())
					{
						if (!this.cancelling)
						{
							if (!this.keyDown)
							{
								this.keyDown = true;
								if(this.writeMode)
								{
									if (!cancelling)
									{
										bool result = this.prox.WriteToken(this.toWrite, this.pinToWrite, this.zonesToWrite, this.terminalsToWrite);
										if (this.CardWritten != null) this.CardWritten(result);
									}
									else this.CardOperationCancelled(true);
									this.writeMode = false;
								}
								else
								{
									if (!this.cancelling)
									{
										if(this.CardRead != null && this.prox.Read())
											this.CardRead(this.prox.TokenType, this.prox.Pin, this.prox.Terminals, this.prox.Zones);
									}
									else this.CardOperationCancelled(false);
								}
							}
						}
						else this.CardOperationCancelled(this.writeMode);

					}
					else this.keyDown = false;
				}
				else CardOperationCancelled(this.writeMode);
				Thread.Sleep(10);
			}
		}
		

		/// <summary>
		/// Releases all resource used by the <see cref="QuiRing.IO.ProxInput"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="QuiRing.IO.ProxInput"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="QuiRing.IO.ProxInput"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="QuiRing.IO.ProxInput"/> so the garbage
		/// collector can reclaim the memory that the <see cref="QuiRing.IO.ProxInput"/> was occupying.
		/// </remarks>
		public void Dispose ()
		{
			this.Disconnect();
		}
	}
}

