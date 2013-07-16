using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

using WebSocketSharp;
using WebSocketSharp.Frame;
using ServiceStack.ServiceClient.Web;

using Quiche.Data;

namespace Quiche
{
	/// <summary>
	/// Log event handler. Log item is the log entry
	/// received.
	/// </summary>
	public delegate void LogEventHandler (Log item);


	/// <summary>
	/// Connection event enum - signals the type of event
	/// which caused the ConnectionEventHandler to be
	/// invoked. 
	/// </summary>
	public enum ConnectionEvent
	{
		Connected,
		Disconnected,
		Error
	};

	/// <summary>
	/// Disconnection event handler. The event type specified has
	/// occurred on the connection at address:port
	/// </summary>
	public delegate void ConnectionEventHandler (ConnectionEvent type, string address, int port);

	/// <summary>
	/// Status event handler. Guid terminal contains the serial number
	/// of the terminal which caused the status ping, string node contains
	/// the serial number of the Qui node the terminal is attached to.
	/// </summary>
	public delegate void StatusEventHandler (string node, Terminal terminal);

	/// <summary>
	/// Quiche.Client - provides a client for consuming
	/// data produced by Qui, the software running on 
	/// Psiloc devices. For more information about Psiloc,
	/// see www.perception-si.com
	/// </summary>
	public class Client : IDisposable
	{
		/// <summary>
		/// WebSocket which listens for log publications from Qui.
		/// Log publications contain information about the operation.
		/// See the Quiche.Data.Log class for details.
		/// </summary>
		protected WebSocket data 								= null;


		/// <summary>
		/// WebSocket which listens for status publications from Qui.
		/// Status publications simply announce that a given terminal
		/// is operational.
		/// </summary>
		protected WebSocket status								= null;


		/// <summary>
		/// A client which allows Quiche to request lists of all
		/// Terminals, Users and Zones that Qui is currently aware
		/// of.
		/// </summary>
		protected JsonServiceClient watchman 					= null;


		/// <summary>
		/// Address of the Qui Watchman server that Quiche connects to.
		/// Watchman typically runs on the central server for a Psiloc
		/// network.
		/// </summary>
		protected string address 								= "";


		/// <summary>
		/// The port over which all websocket data is being broadcast
		/// from Qui Watchman. You shoudn't typically need to change
		/// this.
		/// </summary>
		protected int port										= 8181;


		/// <summary>
		/// Occurs when a log publication is received from Qui
		/// </summary>
		public event LogEventHandler LogReceived				= null;

		/// <summary>
		/// Occurs when the client connection to Qui changes
		/// </summary>
		public event ConnectionEventHandler ConnectionChanged 	= null;


		/// <summary>
		/// Occurs when a status ping is received from a terminal
		/// connected to the Qui server
		/// </summary>
		public event StatusEventHandler StatusPinged		= null;


		/// <summary>
		/// An event which will be set when the Client is exiting
		/// </summary>
		protected ManualResetEvent exit = new ManualResetEvent(false);

		/// <summary>
		/// The client thread
		/// </summary>
		protected Thread thread = null;


		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Quiche.Client"/> is connected.
		/// </summary>
		/// <value>
		/// <c>true</c> if connected; otherwise, <c>false</c>.
		/// </value>
		public bool Connected	{ get; protected set; }


		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Quiche.Client"/> should
		/// attempt to make and maintain a connection to the Qui server.
		/// </summary>
		/// <value>
		/// <c>true</c> if a connection should be made; otherwise, <c>false</c>.
		/// </value>
		public bool Connect 
		{
			get
			{
				if (this.Cache!=null)
				{
					var setting = this.Cache.Settings.Where(s => s.Key == "quiche.connect").SingleOrDefault();
					this.connect = !setting.Equals(default(KeyValuePair<string, string>)) ? setting.Value == "true" : false;
					if (this.connect) this.connecting = true;
				}
				return this.connect;
			}
			set
			{
				this.connect = value;
				if (value) this.connecting = true;
				if (this.Cache!=null) this.Cache.SaveSetting("quiche.connect", value ? "true" : "false");
			}
		}


		/// <summary>
		/// Flag indicating whether a connection with the Qui
		/// server should be made and maintained or not
		/// </summary>
		protected bool connect = false;


		/// <summary>
		/// Flag indicating whether a connection attempt to 
		/// Qui has begun. Cleared only when a Disconnect 
		/// has occurred (unlike connect, which is cleared
		/// by the calling code)
		/// </summary>
		protected bool connecting = false;


		/// <summary>
		/// Gets or sets the local cache provider.
		/// </summary>
		/// <value>
		/// The cache.
		/// </value>
		public ICache Cache { get; protected set; }


		/// <summary>
		/// Gets or sets the address of the Qui Watchman server.
		/// </summary>
		/// <value>
		/// The address of the server
		/// </value>
		public string Address					
		{ 
			get { return this.address; }
			set
			{
				if (value!=null)
				{
					value.Replace("http://", "");
					value.Replace("ws://", "");
				}
				if (this.Cache!=null) this.Cache.SaveSetting("quiche.address", value.ToString());
				this.address = value;
			}
		}


		/// <summary>
		/// Gets or sets the port for status and log websockets.
		/// </summary>
		/// <value>
		/// The port number
		/// </value>
		public int Port
		{
			get	{ return this.port;	}
			set
			{
				this.port = value;
				if (this.Cache!=null) this.Cache.SaveSetting("quiche.port", value.ToString());
			}
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Quiche.Client"/> class.
		/// </summary>
		public Client()
		{
			this.Cache = null;
			this.Connected = false;
		}


		/// <summary>
		/// Initialise this client with an (optional) local cache
		/// </summary>
		/// <param name='cache'>
		/// The local cache to use (optional)
		/// </param>
		/// <returns>
		/// True if the client has now been initialised.
		/// False if the client was already initialised.
		/// </returns>
		public bool Initialise(ICache cache = null)
		{
			if (thread == null)
			{
				this.Cache = cache;
				if (this.Cache!= null)
				{
					var settings = this.Cache.Settings;
					this.port = settings.ContainsKey("quiche.port") ? int.Parse(settings["quiche.port"]) : this.port;
					this.address = settings.ContainsKey("quiche.address") ? settings["quiche.address"] : this.address;                                                                        
				}
				this.thread = new Thread(new ThreadStart(this.Entry));
				this.thread.Start();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Disconnect from the server.
		/// </summary>
		/// <param name='expected'>
		/// <c>true</c> if this disconnection is expected, <c>false</c> if this is
		/// being called due to a fault situation.
		/// </param>
		protected void Disconnect(bool expected)
		{
			if (this.data!=null)
			{
				this.data.Close();
				this.data.Dispose();
			}
			this.data = null;
	
			if (this.status!=null)
			{
				this.status.Close();
				this.status.Dispose();
			}
	
			if (this.watchman!=null)
			{
				this.watchman.Dispose();
				this.watchman = null;
			}

			this.Connected = false;
			if (expected && this.connecting) this.connecting = false;

			if (this.ConnectionChanged!=null)
			{
				if (expected)	this.ConnectionChanged(ConnectionEvent.Disconnected, this.address, this.port);
				else if (this.connecting) this.ConnectionChanged(ConnectionEvent.Error, this.address, this.port);
			}
		}

		/// <summary>
		/// Connect this instance using the current connection parameters.
		/// </summary>
		protected void EstablishConnection()
		{
			if (this.Connected) this.Disconnect(true);
			string wsAddress 	= string.Format("ws://{0}:{1}", this.address, this.port);
			string jsonAddress 	= string.Format("http://{0}/watchman", this.address);
			try
			{
				data = new WebSocket(wsAddress);
				data.OnError+= (sender, e) => this.Disconnect(false);
				data.OnMessage+= (sender, e) => {
					if(e.Type == Opcode.TEXT)
					{
						string[] entries = e.Data.Split(new char[]{';'});
						if (entries.Length == 4 && this.LogReceived != null) this.LogReceived(new Log(){
							Timestamp = DateTime.Now, Event = entries[0], TerminalId = entries[1], UserId = entries[2], Result = entries[3] 
						});
					}
				
				};

				data.Connect();

				if(data.ReadyState == WsState.OPEN)
				{

					this.status = new WebSocket(wsAddress+"/status/");

					this.status.OnMessage+= (sender, e) => {
						if(e.Type == Opcode.TEXT)
						{
							string[] entries = e.Data.Split(new char[]{':'});
							if (entries.Length==2 && this.StatusPinged!=null) 
							{
								this.StatusPinged( entries[0].Contains("emergent") ? entries[0].Remove(0, 8) : entries[0], this.Get<Terminal>(Guid.Parse(entries[1]).ToString()));
							}
						}		
					};

					this.status.Connect();
					if (status.ReadyState == WsState.OPEN)
					{
						this.watchman = new JsonServiceClient(jsonAddress);
						this.watchman.Timeout = System.TimeSpan.FromSeconds(5);
						this.Connected = true;
						if (this.ConnectionChanged != null) this.ConnectionChanged(ConnectionEvent.Connected, this.address, this.port);
					}
					else this.Disconnect(false);
				}
				else this.Disconnect(false);

			}
			catch(Exception)
			{
				this.Disconnect(false);
			}
		}


		/// <summary>
		/// Gets all Terminals, Zones or Users from the Qui Watchman server
		/// and local Cache
		/// </summary>
		/// <returns>
		/// All available objects of the requested type
		/// </returns>
		/// <typeparam name='T'>
		/// The type of object to request
		/// </typeparam>
		public List<T> GetAll<T>() where T : IRemote, new()
		{
			if(this.watchman!=null)
			{
				var result = this.watchman.Get<List<T>>(string.Format("/{0}s/", typeof(T).Name.ToLower()));
				if (this.Cache!=null) result = this.Cache.Synchronise(result);
				return result;
			}
			else return (this.Cache!= null) ? this.Cache.GetAll<T>() : new List<T>();
		}



		/// <summary>
		/// Get the <IRemote> with the specified id from the Qui Watchman
		/// server or cache, if it cannot be retrieved from the server.
		/// </summary>
		/// <param name='id'>
		/// Identifier of the IRemote to retrieve
		/// </param>
		/// <typeparam name='T'>
		/// Type of the object to retrieve.
		/// </typeparam>
		/// <returns>
		/// The requested IRemote or null if no such object is found
		/// </returns>
		public T Get<T>(string id) where T : IRemote, new()
		{
			return this.GetAll<T>().Where(i => i.Id == id).SingleOrDefault();
		}

		/// <summary>
		/// Entry point for the client. The client thread will iterate
		/// until the exit event is set, and will attempt to maintain 
		/// the desired connection state for the client.
		/// </summary>
		protected void Entry()
		{
			while(!this.exit.WaitOne(100))
			{
				if (!this.Connected && this.Connect) this.EstablishConnection();
				if (this.connecting && !this.Connect) this.Disconnect(true);
				if (this.Connected)
				{
					if (this.watchman == null || this.data==null || this.status==null || this.data.ReadyState != WsState.OPEN || this.status.ReadyState != WsState.OPEN) this.Disconnect(false);
					else 
					{
						try { this.watchman.Get<object>(string.Format("/ping/")); }
						catch { this.Disconnect(false);	}
					}
				}
			}
		}


		/// <summary>
		/// Stops the client thread and releases all resource used by the <see cref="Quiche.Client"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="Quiche.Client"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="Quiche.Client"/> in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="Quiche.Client"/> so the garbage collector can reclaim the memory that the
		/// <see cref="Quiche.Client"/> was occupying.
		/// </remarks>
		public void Dispose ()
		{
			this.exit.Set();
			if (this.thread!=null) this.thread.Join();
			this.thread = null;
			if (this.Connected) this.Disconnect(true);
		}

	}
}

