/*
 * The IBootstrapper is a core class within the FaithEngage application. Instead 
 * of having a large config file with the application root that maps every single
 * interface with its implementation (as is often the case with dependency injection
 * systems), the web of IBootstrappers in FaithEngage.Core (and other packaged 
 * projects like FaithEngage.CorePlugins) provides all the interface definitions
 * and initialization code necessary. The reason for this is simple:
 * 
 * While loose coupling is the goal within this application's architecture, there
 * is no need for overly complex configuration measures to accomplish that within
 * a given application layer. FaithEngage.Core is completely decoupled from other
 * projects, but it doesn't need to be overly decoupled within itself. Thus,
 * decoupling within the application layers can be handled with a network of IBootstrappers.
 * 
 * Making use of the BootList and its supplied IContainer, additional dependencies
 * can be supplied or changed at the application root, replacing or supplementing
 * the dependencies registered and executed within any given Bootstrapper. In fact,
 * bootstrappers can be manually removed from a BootList before execution dependency
 * registration takes place.
 */

using System;
using FaithEngage.Core.Containers;
using System.Collections.Generic;
using FaithEngage.Core.Factories;

namespace FaithEngage.Core.Bootstrappers
{
	/// <summary>
    /// An IBootstrapper initializes its consituent classes by (in this order):
    /// 1. Loading sub-bootstrappers for internal namespaces into the supplied
    /// bootlist.
    /// 2. Registering any associated dependencies of types for interfaces.
    /// 3. Executing any initialization code needed for classes within that namespace.
    /// </summary>
    public interface IBootstrapper
	{
        /// <summary>
        /// Specifies where this bootstrapper falls in the order of execution.
        /// </summary>
        /// <value>The boot priority.</value>
        BootPriority BootPriority { get; }
        /// <summary>
        /// Registers any dependencies supplied within this namespace as interface
        /// implementations.
        /// </summary>
        /// <param name="regService">The IRegistrationService supplied by the
        /// applications's IContainer.</param>
        void RegisterDependencies(IRegistrationService regService);
        /// <summary>
        /// Executes any initialization code the bootstrapper might provide for
        /// its namespace.
        /// </summary>
        /// <param name="factory">The application's IAppFactory for dependency
        /// access within the method.</param>
        void Execute(IAppFactory factory);
        /// <summary>
        /// Loads any sub-bootstrappers into the supplied BootList for any internal
        /// namespaces.
        /// </summary>
        /// <param name="bootstrappers">The BootList into which the other Bootstrappers
        /// are to be added.</param>
        void LoadBootstrappers (IBootList bootstrappers);
	}
}

