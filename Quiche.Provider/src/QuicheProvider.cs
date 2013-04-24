using System;

namespace Quiche.Providers
{
	/// <summary>
	/// Singleton which can provide access to an
	/// IQuicheProvider instance.
	/// </summary>
	public sealed class QuicheProvider
	{
	    private static QuicheProvider instance = null;
		private IQuicheProvider qp = null;

	    QuicheProvider()
	    {
	    }

		public static IQuicheProvider Instance
	    {
	        get
	        {
				return (instance == null) ? null : instance.qp;
	        }
	    }

		public static void Initialise<T>() where T : IQuicheProvider
		{
            if (instance == null)
            {
                instance = new QuicheProvider();
				instance.qp = (IQuicheProvider) Activator.CreateInstance(typeof(T));
            }
		}
	}
}