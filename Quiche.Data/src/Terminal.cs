using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Quiche.Data
{
	/// <summary>
	/// Representation of a Psiloc terminal,
	/// sourced from a remote Qui Watchman server
	/// </summary>
	public class Terminal : IRemote
	{
		/// <summary>
		///  Gets or sets the name of this terminal 
		/// </summary>
		/// <value>
		///  The name. 
		/// </value>
		[XmlAttribute]
		public string Name 				{ get; set; }

		/// <summary>
		///  Gets or sets the unique ID of this terminal.
		/// This can be parsed into a GUID if required.
		/// </summary>
		/// <value>
		///  The ID 
		/// </value>
		[XmlAttribute]
		public string Id				
		{ 
			get	{ return this.id.ToString(); }
			set { this.id = Guid.Parse(value); }
		}
		private Guid id;

		/// <summary>
		/// Gets or sets the serial of the node that this
		/// terminal is connected to
		/// </summary>
		/// <value>
		/// The node serial
		/// </value>
		[XmlAttribute]
		public string QuiNode			{ get; set; }


		/// <summary>
		/// Gets or sets the reference ID of this terminal.
		/// The reference ID is the identifier used for 
		/// access control, rather than the longer GUID based
		/// ID due to the limited storage space available on
		/// typical prox devices.
		/// </summary>
		/// <value>
		/// The reference id.
		/// </value>
		[XmlAttribute]
		public ushort ReferenceId		{ get; set; }


		/// <summary>
		/// Gets or sets the zones this terminal is a member of.
		/// </summary>
		/// <value>
		/// The zones.
		/// </value>
		public List<ushort> Zones { get; set; }

		/// <summary>
		/// Disables serialization of Zones if the list is null or empty
		/// </summary>
		public bool ShouldSerializeZones() { return Zones!=null && Zones.Count > 0; }
	

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Quiche.Data.Terminal"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Quiche.Data.Terminal"/>.
		/// </returns>
		public override string ToString()
		{
			return (Name != null && Name != string.Empty) ? Name : Id.ToString();
		}
	}
}
