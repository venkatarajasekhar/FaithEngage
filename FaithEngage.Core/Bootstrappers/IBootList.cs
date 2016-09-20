using System;
using System.Collections.Generic;
using FaithEngage.Core.Containers;

namespace FaithEngage.Core.Bootstrappers
{
    /// <summary>
    /// The BootList is a container for Bootstrappers. As such, it inherits from
    /// List&lt;IBootstrapper> and implements IBootList. It supplements and adds
    /// functionality.
    /// 
    /// The purpose is to provide a convenient way to accumulate bootstrappers and
    /// interact with them all as a single unit, or individually, however desired.
    /// </summary>
    public interface IBootList : IList<IBootstrapper>
    {
        /// <summary>
        /// Loads a bootstrapper into the list.
        /// </summary>
        /// <typeparam name="T">A type implementing IBootstrapper</typeparam>
        void Load<T> () where T : IBootstrapper, new();
        /// <summary>
        /// Registers all dependencies in the BootList.
        /// </summary>
        /// <returns>A log of registations.</returns>
        /// <param name="checkDependencies">If set to <c>true</c> checks dependencies
        /// and reports any missing dependencies in the log.</param>
        string RegisterAllDependencies (bool checkDependencies);
        /// <summary>
        /// Executes all bootstrappers in the list.
        /// </summary>
        /// <returns>A log for the execution procedure.</returns>
        string ExecuteAllBootstrappers ();
        /// <summary>
        /// Gets the missing dependencies for the currently loaded list of bootstrappers.
        /// It will only return a meaningful list after RegisterAllDependencies() has
        /// been called.
        /// </summary>
        /// <value>A list of unimplemented dependencies.</value>
        IList<Type> MissingDependencies { get; }
    }
}

