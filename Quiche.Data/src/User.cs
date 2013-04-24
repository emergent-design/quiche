using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Quiche.Data
{
	/// <summary>
	/// Represents a User enrolled onto a
	/// Psiloc/Qui system, usually sourced
	/// from a remote Qui Watchman server.
	/// </summary>
	public class User : IRemote
	{
		/// <summary>
		///  Gets or sets the name of the User 
		/// </summary>
		/// <value>
		///  The name of the user. 
		/// </value>
		[XmlAttribute]
		public string Name 				{ get; set; }

		/// <summary>
		/// Disables serialization of Name if it is null or empty
		/// </summary>
		public bool ShouldSerializeName() { return !String.IsNullOrEmpty(Name); }


		/// <summary>
		///  Gets or sets the unique ID of this User 
		/// </summary>
		/// <value>
		///  The ID 
		/// </value>
		[XmlAttribute]
		public string Id					{ get; set; }


		/// <summary>
		/// Gets or sets the time that this User was last issued
		/// with a token
		/// </summary>
		/// <value>
		/// The time this user was last issued a token.
		/// </value>
		[XmlAttribute]
		public DateTime LastIssued 		{ get; set; }


		/// <summary>
		/// Gets or sets the list of Zones this user is authorised
		/// to access.
		/// </summary>
		/// <value>
		/// The zone white list.
		/// </value>
		[XmlAttribute]
		public List<ushort> ZoneWhiteList { get; set; }

		/// <summary>
		/// Disables serialization of ZoneWhiteList if it is null or empty
		/// </summary>
		public bool ShouldSerializeZoneWhiteList() { return ZoneWhiteList!=null && ZoneWhiteList.Count > 0; }


		/// <summary>
		/// Gets or sets the list of Terminals this user is authorised
		/// to access.
		/// </summary>
		/// <value>
		/// The terminal white list.
		/// </value>
		[XmlAttribute]
		public List <ushort> TerminalWhiteList { get; set; }

		/// <summary>
		/// Disables serialization of TerminalWhiteList if it is null or empty
		/// </summary>
		public bool ShouldSerializeTerminalWhiteList() { return TerminalWhiteList!=null && TerminalWhiteList.Count > 0; }



		/// <summary>
		/// Initializes a new instance of the <see cref="Quiche.Data.User"/> class.
		/// </summary>
		public User()
		{
			this.ZoneWhiteList = new List<ushort>();
			this.TerminalWhiteList = new List<ushort>();
			this.Name = "";
			this.LastIssued = DateTime.Now;
		}
	}
}
