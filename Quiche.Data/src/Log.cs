using System;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;

namespace Quiche.Data
{
	/// <summary>
	/// Abstract base for Quiche.Data
	/// objects which need to have an auto-incrementing
	/// ID
	/// </summary>
	public abstract class Entity
	{	
		[AutoIncrement]
		[XmlIgnore]
		public long Id { get; set; }
	}

	/// <summary>
	/// A log entry from Qui Watchman
	/// </summary>
	public class Log : Entity
	{
		/// <summary>
		/// Gets or sets the timestamp of the log event.
		/// </summary>
		/// <value>
		/// The timestamp.
		/// </value>
		[XmlAttribute]
		public DateTime Timestamp	{ get; set; }


		/// <summary>
		/// Gets or sets the name of the event being logged
		/// </summary>
		/// <value>
		/// The event name
		/// </value>
		[XmlAttribute]
		public string Event 		{ get; set; }


		/// <summary>
		/// Gets or sets the ID of the terminal that
		/// published this log entry
		/// </summary>
		/// <value>
		/// The terminal ID
		/// </value>
		[XmlAttribute]
		public string TerminalId	{ get; set; }


		/// <summary>
		/// Gets or sets the ID of the user which
		/// caused this log entry
		/// </summary>
		/// <value>
		/// The user ID
		/// </value>
		[XmlAttribute]
		public string UserId 		{ get; set; }


		/// <summary>
		/// Gets or sets the string describing the outcome
		/// of the event being logged
		/// </summary>
		/// <value>
		/// The result.
		/// </value>
		[XmlAttribute]
		public string Result	 	{ get; set; }
	}
}
