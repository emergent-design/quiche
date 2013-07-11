using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using ServiceStack.OrmLite;

using Quiche;
using Quiche.Data;

namespace Quiche.LocalStorage
{
	/// <summary>
	/// An example of an ICache. Cacher uses ormlite
	/// to provide storage (create yourself an appropriate
	/// ormlite connection factory and hand it to cacher at
	/// construction), so you probably only need to replace
	/// this class if ormlite doesn't support whatever you
	/// want to use as a cache, or if you want to do something
	/// odd at point of storage. The point of cacher is simply
	/// to allow storage of connection settings for Quiche.Client,
	/// to keep track of locally created but as yet unenrolled
	/// Users and to provide some offline storage for data from
	/// Qui (so you can use this data, for example, for card
	/// issuing without having a network connection).
	/// </summary>
	public class Cacher : ICache
	{
		/// <summary>
		/// The tables Cacher is going to be looking after
		/// </summary>
		public static Type [] TABLES = { typeof(User), typeof(Log), typeof(Setting), typeof(Terminal), typeof(Zone) };

		/// <summary>
		/// Storage, provided by ormlite
		/// </summary>
		protected OrmLiteConnectionFactory storage = null;


		/// <summary>
		/// How long to keep log entries in the database
		/// before removing them (use the Logger class
		/// if you want to persist them permanently to 
		/// text file)
		/// </summary>
		/// <value>
		/// The time to elapse before a log entry
		/// should be removed from the cache
		/// </value>
		public TimeSpan LogExpiry { get; set; }


		/// <summary>
		/// If a status ping isn't received from
		/// a given terminal for more than
		/// this number of seconds, signal that
		/// terminal as disconnected
		/// </summary>
		public int terminalTimeout = 3;


		/// <summary>
		/// Internal dictionary of terminal IDs and 
		/// their last connection times
		/// </summary>
		protected Dictionary<string, DateTime> connectedTerminals = new Dictionary<string, DateTime>();


		/// <summary>
		/// Gets a list of currently connected terminals.
		/// </summary>
		/// <value>
		/// The currently connected terminals.
		/// </value>
		public List<Terminal> ConnectedTerminals
		{
			get
			{
				if (this.storage!=null)
				lock (this.storage)
				{
					if(this.storage!=null)
					{
						using (var db = this.storage.OpenDbConnection())
						using (var tx = db.BeginTransaction())
						{
							DateTime expired = DateTime.Now.AddSeconds(-this.terminalTimeout);
							return db.Select<Terminal>().Where(t	=> this.connectedTerminals.ContainsKey(t.Id) 
							                                   		&& this.connectedTerminals[t.Id] >= expired).ToList();
						}
					}
				}
				return this.connectedTerminals.Keys.Select(t => new Terminal(){ Id = t}).ToList();
			}
		}


		/// <summary>
		/// Gets a list of currently disconnected terminals.
		/// </summary>
		/// <value>
		/// The currently disconnected terminals.
		/// </value>
		public List<Terminal> DisconnectedTerminals
		{
			get
			{
				if (this.storage!=null)
				lock(this.storage)
				{
					if(this.storage!=null)
					{
						using (var db = this.storage.OpenDbConnection()) 
						using (var tx = db.BeginTransaction())
						{
							DateTime expired = DateTime.Now.AddSeconds(-this.terminalTimeout);
							return db.Select<Terminal>().Where(t	=> !this.connectedTerminals.ContainsKey(t.Id) 
							                                   		|| this.connectedTerminals[t.Id] < expired).ToList();
						}
					}

				}
				return new List<Terminal>();
			}
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Quiche.LocalStorage.Cacher"/> class.
		/// </summary>
		/// <param name='storage'>
		/// The OrmLiteConnectionFactory that's going to provide
		/// persistence for this Cacher.
		/// </param>
		/// <param name='logExpiry'>
		/// How long to store log entries for
		/// </param>
		/// <param name='wipe'>
		/// If true, wipe the database.
		/// Only really useful if you know you have a corrupt db
		/// or you're doing testing.
		/// </param>
		public Cacher(OrmLiteConnectionFactory storage, TimeSpan logExpiry, bool wipe = false)
		{
			this.storage = storage;
			this.LogExpiry = logExpiry;
			if (!wipe) this.storage.Run(d => d.CreateTables(false, TABLES));
			else this.storage.Run(d => d.DropAndCreateTables(TABLES));
		}


		/// <summary>
		/// Exports the entire cache to file.
		/// </summary>
		/// <returns>
		/// True if the cache was exported successfully
		/// </returns>
		/// <param name='cacheFile'>
		/// File to write the cache to.
		/// </param>
		public bool Export(string cacheFile)
		{
			if (this.storage == null) return false;
			lock(this.storage)
			{
				TextWriter file = null;
				using (var db = this.storage.OpenDbConnection())
				using (var t = db.BeginTransaction())
				{
					try
					{
						file = new System.IO.StreamWriter(cacheFile);
						QuicheBackup backup = new QuicheBackup(){
							Settings = db.Select<Setting>(),
							Users = db.Select<User>(),
							Terminals = db.Select<Terminal>(),
							Zones = db.Select<Zone>(),
							Logs = db.Select<Log>()
						};
						XmlSerializer serializer = new XmlSerializer(typeof(QuicheBackup));
						serializer.Serialize(file, backup);
						t.Commit();
					}
					catch (Exception e)
					{
						throw new QuicheException("Unable to export Qui cache", e);
					}
					finally
					{
						if (file!=null) file.Close();
					}
				}
				return true;
			}
		}


		/// <summary>
		/// Imports the cache from an XML file
		/// </summary>
		/// <returns>
		/// True on successful import
		/// </returns>
		/// <param name='cacheFile'>
		/// Filename of the cache file to import.
		/// </param>
		public bool Import(string cacheFile)
		{
			if (this.storage == null) return false;
			lock (this.storage)
			{
				TextReader file = null;
				try
				{
					file = new System.IO.StreamReader(cacheFile);
					XmlSerializer serializer = new XmlSerializer(typeof(QuicheBackup));
					QuicheBackup backup = (QuicheBackup)serializer.Deserialize(file);
					using (var db = this.storage.OpenDbConnection())
					using (var t = db.BeginTransaction())
					{
						db.DropAndCreateTables(Cacher.TABLES);
						foreach (Setting s in backup.Settings)		db.Insert<Setting>(s);
						foreach (User u in backup.Users) 			db.Insert<User>(u);
						foreach (Terminal u in backup.Terminals)	db.Insert<Terminal>(u);
						foreach (Zone z in backup.Zones)			db.Insert<Zone>(z);
						foreach (Log l in backup.Logs)				db.Insert<Log>(l); 
						t.Commit();
					}

				}
				catch (Exception e)
				{
					throw new QuicheException("Unable to import Qui cache", e);
				}
				finally
				{
					if (file!=null) file.Close();
				}
				Console.WriteLine("Imported");
				return true;
			}
		}

	
		/// <summary>
		/// Adds a Terminal, connected to a given Node, to the cache.
		/// You can hook this directly into the OnStatus event raised
		/// by Quiche.Client to store new terminals into the cache as
		/// they appear. This also maintains the volatile
		/// ConnectedTerminals list
		/// </summary>
		/// <param name='node'>
		/// Identifies the node that a terminal is connected to
		/// </param>
		/// <param name='terminal'>
		/// The serial identifier of the terminal
		/// </param>
		public void TerminalStatusPing(string node, Terminal terminal)
		{
			if (this.storage!=null && terminal!=null)
			{
				lock(this.storage)
				{
					using (var db = this.storage.OpenDbConnection())
					using (var t = db.BeginTransaction())
					{
						if (terminal.QuiNode!= node)
						{
							terminal.QuiNode = node;
							db.Save(terminal);
						}
						t.Commit();
					}
				}
			}
			this.connectedTerminals[terminal.Id] = DateTime.Now;
		}


		/// <summary>
		/// Log the specified log entry in the cache
		/// </summary>
		/// <param name='item'>
		/// Item to log
		/// </param>
		public void Log(Log item)
		{
			if (this.storage!=null)
			lock (this.storage)
			{
				using (var db = this.storage.OpenDbConnection())
				using (var t = db.BeginTransaction())
				{
					db.Insert(item);
					DateTime reference = DateTime.Now - this.LogExpiry;
					db.Delete<Log>(l => l.Timestamp < reference);
					t.Commit();
				}
			}
		}	


		/// <summary>
		/// Gets all logs in the cache.
		/// </summary>
		/// <value>
		/// The logs.
		/// </value>
		public List<Log> Logs
		{
			get
			{
				if (this.storage==null) return new List<Log>();
				lock (this.storage)
				{
					using (var db = this.storage.OpenDbConnection())
					using (var t = db.BeginTransaction())
						return db.Select<Log>();
				}
			}
		}


		/// <summary>
		///  Gets all of a given type of IRemote 
		/// </summary>
		/// <returns>
		/// All <T> in the cache or an empty list
		/// if none are present.
		/// </returns>
		/// <typeparam name='T'>
		/// The type of IRemote to retrieve.
		/// </typeparam>
		public List<T> GetAll<T> () where T : IRemote, new()
		{
			if  (this.storage!=null) 
			lock (this.storage)
			{
				using (var db = this.storage.OpenDbConnection())
				using (var t = db.BeginTransaction())
					return db.Select<T>();
			}
			return new List<T>();
		}


		/// <summary>
		/// Get the <T> containing the specified id.
		/// </summary>
		/// <param name='id'>
		/// Id of the <T> to retrieve
		/// </param>
		/// <typeparam name='T'>
		/// Type of IRemote to retrieve
		/// </typeparam>
		public T Get<T> (string id) where T : IRemote, new()
		{
			if (this.storage!=null)
			lock (this.storage)
			{
				using (var db = this.storage.OpenDbConnection()) 
				using (var tx = db.BeginTransaction())
					return db.Select<T>().Where(t => t.Id == id).SingleOrDefault();
			}
			return default(T);
		}


		/// <summary>
		/// Synchronise the specified items in the cache.
		/// Items present only in the cache should not be affected.
		/// Locally modified items should not have their modifications
		/// overwritten.
		/// Remotely sourced data passed in through the items list should
		/// replace stale data in the cache.
		/// Returns the new cache of <T>.
		/// </summary>
		/// <param name='items'>
		/// The items to synchronise with
		/// </param>
		/// <typeparam name='T'>
		/// The type of IRemote to be synchronised
		/// </typeparam>
		public List<T> Synchronise<T> (List<T> items) where T : IRemote, new()
		{
			if (this.storage!=null)
			lock (this.storage)
			{
				using (var db = this.storage.OpenDbConnection()) 
				using (var t = db.BeginTransaction())
				{
					if (typeof(T) == typeof(User))	
					{
						//need to make sure we don't mess up the
						//local-only or unsent name data
						foreach(IRemote i in items)
						{
							User u = i as User;
							var cached = db.GetByIdOrDefault<User>(u.Id);
							if (cached != null) 
							{
								u.LastIssued = cached.LastIssued;
							}
							db.Save(u);
						}
					}
					else db.SaveAll<T>(items);
					items = db.Select<T>();
					t.Commit ();
				}
			}
			return items;
		}


		/// <summary>
		/// Synchronise the specified item in the cache. Local
		/// modifications should not be affected, but all remotely
		/// sourced data associated with the item in the cache should
		/// be replaced by the data from the remote item.
		/// </summary>
		/// <param name='item'>
		/// The remote version of the item
		/// </param>
		/// <typeparam name='T'>
		/// The type of item
		/// </typeparam>
		public T Synchronise<T> (T item) where T : IRemote, new()
		{
			if (this.storage != null)
			lock(this.storage)
			{
				using (var db = this.storage.OpenDbConnection())
				using (var t = db.BeginTransaction())
				{
					if (item is User)
					{
						User user = item as User;
						var cached = db.GetByIdOrDefault<User>(item.Id);
						if (cached != null) 
						{
							user.LastIssued = cached.LastIssued;
						}
					}
					db.Save(item);
					t.Commit();
				}
			}
			return item;
		}

		/// <summary>
		/// Store the specified user.
		/// Use this when you want to set local changes
		/// that should be pushed to Qui at some point.
		/// </summary>
		/// <param name='user'>
		/// User.
		/// </param>
		public void Store (User user)
		{
			if (this.storage != null)
			lock(this.storage)
			{
				using (var db = this.storage.OpenDbConnection())
				using (var t = db.BeginTransaction())
				{
					var cached = db.GetByIdOrDefault<User>(user.Id);
					if (cached != null) 
					{
						cached.LastIssued = user.LastIssued;
						cached.Name = user.Name;
						db.Save(cached);
					}
					else db.Save(user);
					t.Commit();
				}
			}
		}

		/// <summary>
		/// Gets or sets the settings dictionary. Use set with care;
		/// it will remove all settings and replace them with whatever
		/// is in the dictionary you pass in. To modify individual
		/// settings, use SaveSetting or DeleteSetting. The settings
		/// dictionary contains the connection settings for Quiche.Client
		/// along with any application-specific settings you care to 
		/// cache.
		/// </summary>
		/// <value>
		/// The settings.
		/// </value>
		public Dictionary<string, string> Settings
		{
			get 
			{
				if (this.storage!=null)
				lock(this.storage)
				{
					using (var db = this.storage.OpenDbConnection())
					using (var t = db.BeginTransaction())
					{
						return db.Select<Setting>().ToDictionary(i => i.Id, i => i.Value);
					}
				}
				return new Dictionary<string, string>();
			}

			set
			{
				if (this.storage!=null)
				lock(this.storage)
				{
					using (var db = this.storage.OpenDbConnection())
					using (var t = db.BeginTransaction())
					{
						db.DeleteAll<Setting>();
						db.SaveAll(value.Select(s => new Setting { Id = s.Key, Value = s.Value}));
						t.Commit();
					}
				}
			}
		}


		/// <summary>
		/// Saves a single setting.
		/// </summary>
		/// <param name='name'>
		/// Setting name
		/// </param>
		/// <param name='value'>
		/// Setting value
		/// </param>
		public void SaveSetting(string name, string value)
		{
			if (this.storage!=null)
			lock(this.storage)
			{
				using (var db = this.storage.OpenDbConnection())
				using (var t = db.BeginTransaction())
				{
					db.Save(new Setting { Id = name, Value = value });
					t.Commit();
				}
			}
		}


		/// <summary>
		/// Remove a single setting from the setting cache
		/// </summary>
		/// <param name='name'>
		/// Name of the setting to remove
		/// </param>
		public void DeleteSetting(string name)
		{
			if (this.storage!=null)
			lock(this.storage)
			{
				using (var db = this.storage.OpenDbConnection()) 
				using (var t = db.BeginTransaction())
				{
					if (db.Select<Setting>(s => s.Id == name).Count > 0) db.Delete<Setting>(s => s.Id == name);
					t.Commit();
				}
			}
		}
	}
}

