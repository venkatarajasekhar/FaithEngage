using System;
using System.Collections;
using System.Collections.Generic;
using FaithEngage.Core.Containers;
using System.Text;
using FaithEngage.Core.Factories;
using System.Linq;

namespace FaithEngage.Core.Bootstrappers
{
    public class BootList : List<IBootstrapper>, IBootList
    {
        private readonly IContainer _container;

        public IList<Type> MissingDependencies {
            get {
                return _container.CheckAllDependencies ();
            }
        }

        public BootList (IContainer container)
        {
            _container = container;
        }

        public void Add (IBootstrapper item)
        {
            base.Add (item);
            item.LoadBootstrappers (this);
        }

        public void Load<T>() where T : IBootstrapper, new()
        {
            this.Add (new T());
        }

        public string RegisterAllDependencies(bool checkDependencies){
            var sb = new StringBuilder ();
            sb.AppendLine ("Registering dependencies:");
            var orderedList = this.OrderBy (p => p.BootPriority);
            foreach(var booter in orderedList){
                sb.AppendLine ($"--Registering: {booter.GetType ().Name}");
                booter.RegisterDependencies (_container.GetRegistrationService());
            }
            if(checkDependencies){
                sb.AppendLine ("Checking Dependencies:");
                var missingDeps = MissingDependencies;
                foreach(var dep in missingDeps)
                {
                    sb.AppendLine($"--Missing Dependency: {dep.Name}");
                }
            }
            return sb.ToString ();
        }

        public string ExecuteAllBooters(){
            var sb = new StringBuilder ();
            sb.AppendLine ("Executing Bootstrappers:...");
            var orderedList = this.OrderBy (p => p.BootPriority);
            foreach(var booter in orderedList){
                sb.AppendLine($"--Executing on {booter.GetType ().Name}.");
                booter.Execute (_container.Resolve<IAppFactory>());
            }
            return sb.ToString ();
        }
    }
}

