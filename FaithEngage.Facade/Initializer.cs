using System;
using FaithEngage.Core;
using FaithEngage.Core.Containers;
using FaithEngage.Facade.Interfaces;
namespace FaithEngage.Facade
{
    public class Initializer : IInitializer
    {
        public IBootstrapper GetBootstrapper ()
        {
            return new Bootloader ();
        }

        public IContainer GetContainer ()
        {
            return new IocContainer ();
        }
    }
}

