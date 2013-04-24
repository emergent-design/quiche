using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Quiche;
using Quiche.Data;
using Quiche.Proxcard;
using Quiche.LocalStorage;

using ServiceStack.OrmLite;

namespace Quiche.Providers
{
	/// <summary>
	/// An example of an IQuicheProvider which bundles together a Quiche.Client,
	/// a Logger which logs to file, a Cacher which uses a Sqlite datastore and a
	/// PerceptionProx proxcard reader/writer device. This hooks together these
	/// components such that events are fired and consumed correctly between them.
	/// Generally speaking, the Cache should only be used to access
	/// </summary>
	public sealed class SqliteBackedPerceptionProx : IQuicheProvider
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="Quiche.Providers.SqliteBackedPerceptionProx"/> class.
		/// </summary>
		public SqliteBackedPerceptionProx()
		{
			this.ProxConnected = false;
		}

		/// <summary>
		///  Gets the Quiche client 
		/// </summary>
		/// <value>
		/// The client.
		/// </value>
	   	public Client Client {get { return this.client; }}

		/// <summary>
		/// The internal quiche client.
		/// </summary>
		private Quiche.Client client = new Quiche.Client();

		/// <summary>
		/// The internal logger.
		/// </summary>
		private Quiche.LocalStorage.Logger logger = null;

		/// <summary>
		/// The internal card reader.
		/// </summary>
		private Quiche.Proxcard.ProxInput cardReader = null;


		/// <summary>
		///  Gets the local Cacher. 
		/// </summary>
		/// <value>
		/// The cache.
		/// </value>
		public Cacher Cache {	get; private set; }


		/// <summary>
		///  Gets a value indicating whether a prox reader is connected. 
		/// </summary>
		/// <value>
		/// <c>true</c> if prox connected; otherwise, <c>false</c>.
		/// </value>
		public bool ProxConnected
		{
			get; private set;
		}


		/// <summary>
		/// Gets or sets the log expiry.
		/// </summary>
		/// <value>
		/// The log expiry.
		/// </value>
		public TimeSpan LogExpiry
		{
			get
			{
				return this.logExpiry;
			}
			set
			{
				this.logExpiry = value;
				if (this.Cache!=null) this.Cache.LogExpiry = this.LogExpiry;
			}
		}

		/// <summary>
		/// The log expiry.
		/// </summary>
		private TimeSpan logExpiry = new TimeSpan(7, 0, 0, 0);


		/// <summary>
		/// Indicates whether this instance has been initialised
		/// </summary>
		private bool initialised = false;

		/// <summary>
		///  Occurs when progress is made on a card read/write operation 
		/// </summary>
		public event CardProgress	CardProgress = null;

		/// <summary>
		///  Occurs when a card has been read 
		/// </summary>
		public event InputHandler	CardRead = null;

		/// <summary>
		///  Occurs when card read/write operation starts or ends. 
		/// </summary>
		public event CardOperation  CardOperation = null;

		/// <summary>
		///  Occurs when the connection state of the prox reader changes. 
		/// </summary>
		public event ProxConnectionEvent ProxConnectionChanged = null;


		/// <summary>
		///  Initialise this instance with the specified connection string, and optionally wipe the local cache. 
		/// </summary>
		/// <param name='cacheConnection'>
		///  Cache connection string 
		/// </param>
		/// <param name='wipe'>
		///  if true, wipe the local cache at initialisation. 
		/// </param>
		public void Initialise(string cacheConnection, bool wipe)
		{
			if (!this.initialised)
			{
				this.Cache = new Quiche.LocalStorage.Cacher(new OrmLiteConnectionFactory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), cacheConnection, "quiche.sqlite"), false, SqliteDialect.Provider), this.logExpiry, wipe);
				this.client.LogReceived+=this.Cache.Log;
				this.client.Initialise(this.Cache);
				var settings = this.Cache.Settings;
				this.logger = settings.ContainsKey("logger.path") ? new Logger(settings["logger.path"]) : null;
				this.initialised = true;
				this.cardReader = new ProxInput();
				this.cardReader.CardProgress += (error, progress, read) => {
					if (this.CardProgress!=null) this.CardProgress(error, progress, read);
				};
				this.cardReader.CardRead += (tokenType, pin, units, zones) => {
					if (this.CardRead != null) this.CardRead(tokenType, pin, units, zones);
					if (this.CardOperation != null) this.CardOperation(OperationType.ReadFinished);
					this.cardReader.Pause();
				};
				this.cardReader.CardWritten += (success) => {
					if (this.CardOperation != null) this.CardOperation(success ? OperationType.WriteFinished : OperationType.WriteAborted);
					this.cardReader.Pause();
				};
				this.cardReader.CancelCardOperation += (writeAborted) => {
					if (this.CardOperation != null) this.CardOperation(writeAborted ? OperationType.WriteAborted : OperationType.ReadAborted);
					this.cardReader.Pause();
				};

				this.client.StatusPinged+= (node, terminal) => this.Cache.TerminalStatusPing(node, terminal);

				if (settings.ContainsKey("quiche.proxcard")) this.ConnectProx(this.Cache.Settings["quiche.proxcard"]);
			}
		}


		/// <summary>
		///  Connects the prox reader 
		/// </summary>
		/// <param name='connection'>
		///  Connection string (typically the port the reader is on) 
		/// </param>
		/// <param name='commPort'>
		/// Comm port.
		/// </param>
		public 	void ConnectProx(string commPort)
		{
			bool result = false;
			this.ProxConnected = false;
			this.cardReader.Disconnect();
			if (commPort!="Disconnect")
			{
				var device = new PerceptionProx();
				if (device.Initialise(commPort) && device.TestConnection()) 
				{
					this.cardReader.Connect(device);
					if (this.Cache!=null) this.Cache.SaveSetting("quiche.proxcard", commPort);
					this.ProxConnected = true;
					result = true;
				}
				else
				{
					device.Dispose();
				}
			}
			if (this.ProxConnectionChanged!=null) this.ProxConnectionChanged(commPort, result);
		}


		/// <summary>
		///  Gets or sets the path the logfile will be saved to 
		/// </summary>
		/// <value>
		/// The log path.
		/// </value>
		public string LogPath
		{
			get { return logger != null ? logger.Path : ""; }
			set
			{
				if (this.logger != null) this.client.LogReceived -= this.logger.Log;
				this.logger = new Quiche.LocalStorage.Logger(value, this.Cache);
				if (this.Cache != null) this.Cache.SaveSetting("logger.path", value);
				this.client.LogReceived += this.logger.Log;
			}
		}


		/// <summary>
		///  Initiate a card read. Use the CardRead event to receive the response. 
		/// </summary>
		public void ReadCard()
		{
			if (this.cardReader!= null)
			{
				if (this.CardOperation != null) this.CardOperation(OperationType.ReadStarted);
				this.cardReader.Read();
			}
		}



		/// <summary>
		///  Cancel any current proxcard operations 
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance cancel prox; otherwise, <c>false</c>.
		/// </returns>
		public void CancelProx()
		{
			if (this.cardReader!=null)
			{
				this.cardReader.Cancel();
			}
		}



		/// <summary>
		///  Initiate a card write. Use the CardOperation event to determine when the write operation has completed. 
		/// </summary>
		/// <param name='tokenType'>
		///  Token type. 
		/// </param>
		/// <param name='pin'>
		///  PIN. 
		/// </param>
		/// <param name='zones'>
		///  Zone whitelist 
		/// </param>
		/// <param name='terminals'>
		///  Terminal whitelist 
		/// </param>
		public void WriteCard(TokenType tokenType, uint pin = 0, List<ushort> zones = null, List<ushort> terminals = null)
		{
			if (this.cardReader!= null)
			{
				if (this.CardOperation != null) this.CardOperation(OperationType.WriteStarted);
				this.cardReader.WriteCard(tokenType, pin, zones, terminals);
			}
		}



		/// <summary>
		/// Releases all resource used by the <see cref="Quiche.Providers.SqliteBackedPerceptionProx"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="Quiche.Providers.SqliteBackedPerceptionProx"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="Quiche.Providers.SqliteBackedPerceptionProx"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="Quiche.Providers.SqliteBackedPerceptionProx"/> so the garbage collector can reclaim the memory that the
		/// <see cref="Quiche.Providers.SqliteBackedPerceptionProx"/> was occupying.
		/// </remarks>
		public void Dispose ()
		{
			if (this.cardReader!= null) this.cardReader.Dispose();
			this.Client.Dispose();
		}

	}
}