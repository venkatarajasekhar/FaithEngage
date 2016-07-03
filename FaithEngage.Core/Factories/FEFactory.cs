using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core
{
    public class FEFactory
    {
        private static IAppFactory _fac;

        internal static void Activate(IContainer container)
        {
            _fac = container.Resolve<IAppFactory> ();
        }

        public static IAppFactory Get{ get { return _fac;}}
    }
}

