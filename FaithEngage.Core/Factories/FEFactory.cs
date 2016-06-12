using System;
using FaithEngage.Core.Containers;

namespace FaithEngage.Core.Factories
{
    public class FEFactory
    {
        private static IAppFactory _fac;

        internal static void Activate(IContainer container)
        {
            _fac = container.Resolve<IAppFactory> ();
        }

        public static IAppFactory Get()
        {
            return _fac;
        }
    }
}

