using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;

namespace FaithEngage.Core.Bootstrappers
{
    public interface IBootList : IList<IBootstrapper>
    {
        void Load<T> () where T : IBootstrapper, new();
        string RegisterAllDependencies (bool checkDependencies);
        string ExecuteAllBootstrappers ();
        IList<Type> MissingDependencies { get; }
    }
}

