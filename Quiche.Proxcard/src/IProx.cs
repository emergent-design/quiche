using System;
using System.Collections.Generic;

namespace Quiche.Proxcard
{
	/// <summary>
	/// Card read progress delegate. Use this if you want to update
	/// the user on read progress. 
	/// </summary>
	/// <param name='error'>
	/// If set to <c>true</c>, the current card operation has encountered
	/// an error.
	/// </param>
	/// <param name='progress'>
	/// Progress of the current card operation, from 0 to 100
	/// </param>
	/// <param name='read'>
	/// If set to <c>true</c>, the current card operation is a read;
	/// otherwise the current card operation is a write.
	/// </param>
	public delegate void CardProgress(bool error, int progress, bool read);

	/// <summary>
	/// Enum describing the types of Token available
	/// (a Token is a valid user or control card)
	/// </summary>
	public enum TokenType
	{
		/// <summary>
		/// A normal User card
		/// </summary>
		User,


		/// <summary>
		/// A control card for switching into Verify mode
		/// </summary>
		Verify,


		/// <summary>
		/// A control card for switching into Enrol mode
		/// </summary>
		Enrol,


		/// <summary>
		/// A control card for switching into Revoke mode
		/// </summary>
		Revoke,


		/// <summary>
		/// A control card for switching into Admin Permissions mode
		/// </summary>
		AdminToggle,


		/// <summary>
		/// A proxy (replacement user) card - treated interchangeably with
		/// a User card by the system; the only difference is that a User
		/// card bases the Token PIN on the unique (and fixed) card ID
		/// whereas a Proxy card writes a PIN to a different location on the
		/// card.
		/// </summary>
		Proxy,


		/// <summary>
		/// A control card for switching into Access Permissions mode
		/// </summary>
		Access,


		/// <summary>
		/// An invalid or absent card
		/// </summary>
		None
	};


	/// <summary>
	/// Prox reader interface.
	/// </summary>
	public interface IProx : IDisposable
	{

		/// <summary>
		/// Initialise the prox reader with the specified connection string.
		/// </summary>
		/// <param name='connection'>
		/// The device-specific connection string.
		/// </param>
		/// <param name='onCardProgress'>
		/// Progress handler for card operations
		/// </param>
		/// <returns>
		/// <c>true</c> on successful initialisation; otherwise, <c>false</c>.
		/// </returns>
		bool Initialise(string connection);


		/// <summary>
		/// Determines whether the reader has card.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the reader has card; otherwise, <c>false</c>.
		/// </returns>
		bool HasCard();


		/// <summary>
		/// Gets the type of the token read from the prox device
		/// </summary>
		/// <value>
		/// The type of the token.
		/// </value>
		TokenType TokenType { get; }


		/// <summary>
		/// Gets the PIN of the card presented to the prox device.
		/// </summary>
		/// <value>
		/// The PIN
		/// </value>
		uint Pin { get; }


		/// <summary>
		/// Gets the Terminals listed on the card presented.
		/// </summary>
		/// <value>
		/// A list of the terminals (by reference ID)
		/// </value>
		List<ushort> Terminals { get; }


		/// <summary>
		/// Gets the Zones listed on the card presented.
		/// </summary>
		/// <value>
		/// A list of the zones (by reference ID)
		/// </value>
		List<ushort> Zones { get; }


		/// <summary>
		/// Read from the prox card.
		/// </summary>
		bool Read();


		/// <summary>
		/// Tests the prox reader connection.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the prox reader is connected.
		/// </returns>
		bool TestConnection();


		/// <summary>
		/// Writes a token to a prox card
		/// </summary>
		/// <param name='type'>
		/// Type of the token
		/// </param>
		/// <param name='pin'>
		/// Token PIN
		/// </param>
		/// <param name='zones'>
		/// Zones the token is valid for
		/// </param>
		/// <param name='terminals'>
		/// Terminals the token is valid for
		/// </param>
		bool WriteToken(TokenType type, uint pin=0, List<ushort> zones = null, List<ushort> terminals = null);


		/// <summary>
		/// Cancerls an in-progress operation
		/// </summary>
		void Cancel();


		/// <summary>
		/// Occurs when a card operation makes progress.
		/// </summary>
		event CardProgress OnCardProgress;
	}
}

