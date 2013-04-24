using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Quiche.Data 
{
	/// <summary>
	/// Represents a Zone (a collection of 
	/// Psiloc terminals which Users can be 
	/// granted access to as a group). usually 
	/// sourced from a remote Qui Watchman server.
	/// </summary>
	public class Zone : IRemote
	{
		/// <summary>
		///  Gets or sets the name of this Zone 
		/// </summary>
		/// <value>
		///  The Zone name. 
		/// </value>
		[XmlAttribute]
		public string Name 				{ get; set; }


		/// <summary>
		///  Gets or sets the unique ID of this Zone 
		/// </summary>
		/// <value>
		///  The Zone ID 
		/// </value>
		[XmlAttribute]
		public string Id				{ get; set; }


		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Quiche.Data.Zone"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Quiche.Data.Zone"/>.
		/// </returns>
		public override string ToString()
		{
			return (Name != null && Name != string.Empty) ? Name : Id.ToString();
		}

	}
}
