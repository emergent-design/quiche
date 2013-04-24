using System;

namespace Quiche
{
	/// <summary>
	/// Quiche exception.
	/// </summary>
	public class QuicheException : Exception
	{
		public QuicheException() : base () {}
		public QuicheException(string message) : base (message) {}
		public QuicheException(string message, Exception innerException) : base (message, innerException) {}
	}
}

