using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core
{
    /// <summary>
    /// FEF actory.
	/// This class is responsible for providing easy access access through a static method to the current IAppFactory.
    /// </summary>
	public class FEFactory
    {
        private static IAppFactory _fac;

        internal static void Activate(IAppFactory appFactory)
        {
            _fac = appFactory;
        }
        public static IAppFactory Get{ get { return _fac;}}
    }
}

