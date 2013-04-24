using System;

namespace Quiche.Data
{

	/// <summary>
	/// Any Quiche.Data objects which can be sourced
	/// from a Qui Watchman server should implement IRemote
	/// </summary>
	public interface IRemote
	{
		/// <summary>
		/// Gets or sets the name of this IRemote object
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		string Name 		{ get; set; }

		/// <summary>
		/// Gets or sets the unique ID of this IRemote object
		/// </summary>
		/// <value>
		/// The ID
		/// </value>
		string Id 			{ get; set; }
	}
}
