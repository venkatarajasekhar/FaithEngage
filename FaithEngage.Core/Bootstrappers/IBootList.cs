using System;
using System.Collections.Generic;

namespace FaithEngage.Core
{
    public interface IBootList : IList<IBootstrapper>
    {
        void Load<T> () where T : IBootstrapper, new();
        string RegisterAllDependencies (bool checkDependencies);
        string ExecuteAllBooters ();

    }
}

