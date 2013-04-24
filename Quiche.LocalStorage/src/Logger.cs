using System;
using System.Linq;
using System.IO;
using ServiceStack.OrmLite;

using Quiche;
using Quiche.Data;

namespace Quiche.LocalStorage
{
	/// <summary>
	/// Example logger - this just logs data to a text file.
	/// It can also optionally use an ICache to make the log
	/// more readable
	/// </summary>
	public class Logger
	{
		/// <summary>
		/// A local cache (used to make
		/// the log prettier)
		/// </summary>
		protected ICache cache = null;

		/// <summary>
		/// Path for logging to
		/// </summary>
		protected string path = "";

		/// <summary>
		/// Gets the logging path.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public string Path { get { return this.path; }}

		/// <summary>
		/// Initializes a new instance of the <see cref="Quiche.LocalStorage.Logger"/> class.
		/// </summary>
		/// <param name='path'>
		/// Where you want to log to
		/// </param>
		/// <param name='cache'>
		/// (optional) cache provider, for prettier logs
		/// </param>
		public Logger(string path, ICache cache = null)
		{
			this.path = path;
			this.cache = cache;
		}

		/// <summary>
		/// Log the specified item.
		/// </summary>
		/// <param name='item'>
		/// Item to log
		/// </param>
		public void Log(Log item)
		{
			if (!String.IsNullOrEmpty(this.path))
			{
				User user = this.cache!=null ? cache.Get<User>(item.UserId) : null;
																			//normalise the string format
				Terminal terminal = this.cache!=null ? cache.Get<Terminal>(Guid.Parse(item.TerminalId).ToString()) : null;
				TextWriter file = null;

				try
				{
					string filePath = System.IO.Path.Combine(this.path, DateTime.Now.ToString("dd-MMM-yyyy.lo\\g"));
					file = new System.IO.StreamWriter(filePath, true);
					if(!File.Exists(filePath)) file.WriteLine("Timestamp;Psiloc Serial;Psiloc Name;Event;User ID;Username;Result");
					file.WriteLine(string.Format("{0};{1};\"{2}\";{3};{4};\"{5}\";{6}", item.Timestamp, item.TerminalId, terminal!=null ? terminal.Name : "", item.Event, item.UserId, user==null ? "" : user.Name, item.Result));
				}
				catch (Exception e)
				{
					throw new QuicheException(string.Format("Unable to write log file {0}", this.path), e);
				}
				finally
				{
					if (file!=null) file.Close();
				}
			}
		}

	}
}

