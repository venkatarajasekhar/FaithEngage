using System;
using FaithEngage.Core.Containers;
using FaithEngage.Core;
using FaithEngage.Core.Bootstrappers;

namespace FaithEngage.Facade.Interfaces
{
    public interface IInitializer
    {
        IContainer Container { get; }
        IBootList GetEmptyBootList (IContainer container);
        IBootList LoadedBootList { get; }
        IBootstrapper CoreBootstrapper { get; }
    }
}

