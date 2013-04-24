using System;
using System.Collections.Generic;

using Quiche.Data;

namespace Quiche
{
	/// <summary>
	/// Interface that any cache used by 
	/// Quiche.Client must implement (a cache
	/// is used to store a local copy of information
	/// that is retrieved from a Qui server to allow 
	/// a system to operate when no network connection 
	/// is available. It's also used to store connection
	/// settings for Quiche.Client
	/// </summary>
	public interface ICache
	{
		/// <summary>
		/// Gets all of a given type of IRemote
		/// </summary>
		/// <returns>
		/// All <T> present in the cache
		/// </returns>
		/// <typeparam name='T'>
		/// The type to get
		/// </typeparam>
		List<T> GetAll<T>() where T : IRemote, new();

		/// <summary>
		/// Get the <T> with the specified id from 
		/// the cache (or the default <T> if no such
		/// object exists.
		/// </summary>
		/// <param name='id'>
		/// Id of the desired <T>
		/// </param>
		/// <typeparam name='T'>
		/// The type of IRemote requested
		/// </typeparam>
		T Get<T> (string id) where T : IRemote, new();

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
		List<T> Synchronise<T>(List<T> items) where T : IRemote, new();

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
		T Synchronise<T>(T item) where T : IRemote, new();

		/// <summary>
		/// Store the specified User, overwriting the properties that can 
		/// be locally modified (the name and last issued timestamp effectively)
		/// </summary>
		/// <param name='user'>
		/// User to store
		/// </param>
		void Store (User user);

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
		Dictionary<string, string> Settings { get; set; }

		/// <summary>
		/// Saves a single setting.
		/// </summary>
		/// <param name='name'>
		/// Setting name
		/// </param>
		/// <param name='value'>
		/// Setting value
		/// </param>
		void SaveSetting(string name, string value);

		/// <summary>
		/// Remove a single setting from the setting cache
		/// </summary>
		/// <param name='name'>
		/// Name of the setting to remove
		/// </param>
		void DeleteSetting(string name);
	}
}

