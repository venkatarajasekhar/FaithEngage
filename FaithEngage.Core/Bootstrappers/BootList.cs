using System;
using System.Collections;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using System.Text;
using FaithEngage.Core.Factories;
using System.Linq;

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
    public class BootList : List<IBootstrapper>, IBootList
    {
        private readonly IContainer _container;

        /// <summary>
        /// Obtains a list of missing dependencies.
        /// </summary>
        /// <value>The missing dependencies.</value>
        public IList<Type> MissingDependencies {
            get {
                return _container.CheckAllDependencies ();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FaithEngage.Core.Bootstrappers.BootList"/> class.
        /// </summary>
        /// <param name="container">The IContainer that is to be used by the BootList.</param>
        public BootList (IContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Adds a Bootstrapper to the list. This method operates recursively, so that
        /// once added to the list, that bootstrapper's "LoadBootstrappers" method is
        /// called, down until all bootstrappers are loaded in that tree. Duplicate
        /// registrations will be ignored.
        /// </summary>
        /// <param name="item">The bootstrapper to add.</param>
        public void Add (IBootstrapper item)
        {
            if (this.Any (p => p.GetType () == item.GetType ())) 
                return;
            base.Add (item);
            item.LoadBootstrappers (this);
        }

        /// <summary>
        /// A shortcut to calling Add(). This will instantiate and add the bootstrapper
        /// to the list.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Load<T>() where T : IBootstrapper, new()
        {
            this.Add (new T());
        }

        /// <summary>
        /// Registers all dependencies in this list and returns a log of all dependencies.
        /// Optionally, it will also check dependencies after and report in the log
        /// any missing dependencies amongst the loaded dependencies.
        /// </summary>
        /// <returns>A log of the action.</returns>
        /// <param name="checkDependencies">If set to <c>true</c> checks dependencies.</param>
        public string RegisterAllDependencies(bool checkDependencies = false){
            var sb = new StringBuilder ();
            sb.AppendLine ("Registering dependencies:");
            var regService = _container.GetRegistrationService ();
            foreach(var booter in this){
                sb.AppendLine ($"--Registering: {booter.GetType ().Name}");
                booter.RegisterDependencies (regService);
            }
            if(checkDependencies){
                sb.AppendLine ("Checking Dependencies:");
                var missingDeps = this.MissingDependencies;
                foreach(var dep in missingDeps)
                {
                    sb.AppendLine($"--Missing Dependency: {dep.Name}");
                }
                if (missingDeps.Count == 0) sb.AppendLine ("--No Missing Dependencies found.");
            }
            return sb.ToString ();
        }

        /// <summary>
        /// Calls the execute function on each bootstrapper in the list, in the order
        /// specified by the bootpriority.
        /// </summary>
        /// <returns>Log of execution</returns>
        public string ExecuteAllBootstrappers(){
            var sb = new StringBuilder ();
            var fac = _container.Resolve<IAppFactory>();
            sb.AppendLine ("Executing Bootstrappers:...");
            var orderedList = this.OrderBy (p => p.BootPriority);
            foreach(var booter in orderedList){
                sb.AppendLine($"--Executing on {booter.GetType ().Name}.");
                booter.Execute (fac);
            }
            return sb.ToString ();
        }
    }
}

