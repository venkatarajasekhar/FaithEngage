using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core;
namespace FaithEngage.Facade.Interfaces
{
    public interface IInitializer
    {
        IContainer GetContainer ();
        IBootstrapper GetBootstrapper ();
    }
}

