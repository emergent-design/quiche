using System;
using System.Collections.Generic;

namespace Quiche.Data
{
	/// <summary>
	/// Class for collecting a complete set of Quiche
	/// data for easy serialisation
	/// </summary>
	public class QuicheBackup
	{
		public List<Setting> 	Settings 	{ get; set; }
		public List<User> 		Users		{ get; set; }
		public List<Terminal> 	Terminals	{ get; set; }
		public List<Log>		Logs		{ get; set; }
		public List<Zone>		Zones		{ get; set; }
		
		public QuicheBackup()
		{
			this.Settings	= new List<Setting>();
			this.Terminals	= new List<Terminal>();
			this.Users 		= new List<User>();
			this.Logs 		= new List<Log>();
			this.Zones 		= new List<Zone>();
		}
	}
}
