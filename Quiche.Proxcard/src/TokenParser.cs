using System;

namespace Quiche.Proxcard
{
	/// <summary>
	/// Token parser. Converts data to and from a format suitable
	/// for storage on proxcards.
	/// </summary>
	public class TokenParser
	{
		/// <summary>
		/// Interprets the type byte array from a proxcard.
		/// </summary>
		/// <returns>
		/// The type of prox card
		/// </returns>
		/// <param name='typecode'>
		/// Prox card type as a byte array
		/// </param>
		public static TokenType TypeFromProx (byte[] typecode)
		{
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(typecode);
			}
			switch(BitConverter.ToInt32(typecode, 0))
			{
				case (0x0F0F0F0F): return TokenType.AdminToggle;
				case (0x0A0A0A0A): return TokenType.Revoke;
				case (0x09090909): return TokenType.Enrol;
				case (0x03030303): return TokenType.Verify;
				case (0x10101010): return TokenType.Proxy;
				case (0x1F1F1F1F): return TokenType.Access;
				default: return TokenType.User;
			}
		}
				
		/// <summary>
		/// Interprets the PIN byte array from a proxcard.
		/// </summary>
		/// <returns>
		/// The PIN
		/// </returns>
		/// <param name='id'>
		/// The PIN as a byte array from a prox card
		/// </param>
		public static uint PinFromProx(byte[] id)
		{
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(id);
			}
			return BitConverter.ToUInt32(id, 0);
		}


		/// <summary>
		/// Converts a PIN to a byte array suitable for 
		/// storage to a proxcard
		/// </summary>
		/// <returns>
		/// The PIN as a byte array
		/// </returns>
		/// <param name='id'>
		/// The PIN to be converted
		/// </param>
		public static byte[] PinToProx(uint id)
		{
			byte[] result = BitConverter.GetBytes(id);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(result);
			}
			return result;
		}


		/// <summary>
		/// Converts a TokenType to a byte array suitable for
		/// storage to a proxcard
		/// </summary>
		/// <returns>
		/// The token type as a byte array.
		/// </returns>
		/// <param name='type'>
		/// The token type to convert
		/// </param>
		public static byte[] TypeToBytes(TokenType type)
		{
			byte[] result = new byte[]{0, 0, 0, 0};
			switch (type)
			{
				case TokenType.AdminToggle: 	result = BitConverter.GetBytes(0x0F0F0F0F); break;
				case TokenType.Revoke: 			result = BitConverter.GetBytes(0x0A0A0A0A); break;
				case TokenType.Enrol: 			result = BitConverter.GetBytes(0x09090909); break;
				case TokenType.Verify: 			result = BitConverter.GetBytes(0x03030303); break;
				case TokenType.Proxy: 			result = BitConverter.GetBytes(0x10101010); break;
				case TokenType.User: 			result = BitConverter.GetBytes(0x00000000); break;
				case TokenType.Access:			result = BitConverter.GetBytes(0x1F1F1F1F); break;
				default: break;
			}
			if (BitConverter.IsLittleEndian) Array.Reverse(result);
			return result;
		}
	}
}

