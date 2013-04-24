using System;
using System.IO;
using System.Collections.Generic;

using Quiche;
using Quiche.Data;
using Quiche.Proxcard;
using Quiche.LocalStorage;


namespace Quiche.Providers
{

	/// <summary>
	/// Operation type. Used to identify
	/// what type of proxcard operation
	/// caused a CardOperation event to occur
	/// </summary>
	public enum OperationType
	{
		/// <summary>
		/// A Card Read has started.
		/// </summary>
		ReadStarted,

		/// <summary>
		/// A Card Read has finished successfully.
		/// </summary>
		ReadFinished,

		/// <summary>
		/// A Card Read has finished unsuccessfully.
		/// </summary>
		ReadAborted,

		/// <summary>
		/// A Card Write has started.
		/// </summary>
		WriteStarted,

		/// <summary>
		/// A Card Write has finished successfully.
		/// </summary>
		WriteFinished,

		/// <summary>
		/// A Card Write has finished unsuccessfully.
		/// </summary>
		WriteAborted
	};


	/// <summary>
	/// Proxcard connection chaneged event handler. The port which has caused
	/// the event to fire is indicated by the port string. Bool connected indicates
	/// whether the event is a connection (true) or disconnection (false).
	/// </summary>
	public delegate void ProxConnectionEvent(string port, bool connected);

	/// <summary>
	/// Card operation event handler. OperationType operation indicates
	/// what card operation has fired the event.
	/// </summary>
	public delegate void CardOperation (OperationType operation);


	/// <summary>
	/// IQuiche provider. Anything that implements IQuicheProvider should tie together
	/// the local storage, client and proxcard portions of Quiche into one easy-to-use
	/// bundle. This is the interface expected by the QuicheProvider singleton, which
	/// can be optionally used to provide singleton-based access to your IQuicheProvider.
	/// </summary>
	public interface IQuicheProvider : IDisposable
	{
	
		/// <summary>
		/// Gets the Quiche client
		/// </summary>
		Client Client			{ get; }


		/// <summary>
		/// Gets the local Cacher.
		/// </summary>
		Cacher Cache 			{ get; }

		/// <summary>
		/// Gets or sets the path the logfile will be saved to
		/// </summary>
		string LogPath			{ get; set; }


		/// <summary>
		/// Occurs when progress is made on a card read/write 
		/// operation
		/// </summary>
		event CardProgress	CardProgress;

		/// <summary>
		/// Occurs when a card has been read
		/// </summary>
		event InputHandler	CardRead;

		/// <summary>
		/// Occurs when card read/write operation starts or ends.
		/// </summary>
		event CardOperation  CardOperation;

		/// <summary>
		/// Occurs when the connection state of the prox reader
		/// changes.
		/// </summary>
		event ProxConnectionEvent ProxConnectionChanged;

		/// <summary>
		/// Gets a value indicating whether a prox reader is connected.
		/// </summary>
		/// <value>
		/// <c>true</c> if the prox reader is connected; otherwise, <c>false</c>.
		/// </value>
		bool ProxConnected { get; }


		/// <summary>
		/// Initialise this IQuicheProvider with the specified connection string,
		/// and optionally wipe the local cache.
		/// </summary>
		/// <param name='cacheConnection'>
		/// Cache connection string
		/// </param>
		/// <param name='wipe'>
		/// if true, wipe the local cache at initialisation.
		/// </param>
		void Initialise(string cacheConnection, bool wipe);

		/// <summary>
		/// Connects the prox reader
		/// </summary>
		/// <param name='connection'>
		/// Connection string (typically the port the reader is on)
		/// </param>
		void ConnectProx(string connection);

		/// <summary>
		/// Initiate a card read. Use the CardRead event to receive
		/// the response.
		/// </summary>
		void ReadCard();

		/// <summary>
		/// Initiate a card write. Use the CardOperation event to determine
		/// when the write operation has completed.
		/// </summary>
		/// <param name='tokenType'>
		/// Token type.
		/// </param>
		/// <param name='pin'>
		/// PIN.
		/// </param>
		/// <param name='zones'>
		/// Zone whitelist
		/// </param>
		/// <param name='terminals'>
		/// Terminal whitelist
		/// </param>
		void WriteCard(TokenType tokenType, uint pin = 0, List<ushort> zones = null, List<ushort> terminals = null);

		/// <summary>
		/// Cancel any current proxcard operations
		/// </summary>
		void CancelProx();
	}
}