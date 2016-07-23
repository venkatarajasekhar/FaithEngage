using System;
using FaithEngage.Core;
using FaithEngage.Core.Containers;
using FaithEngage.Facade.Interfaces;
namespace FaithEngage.Facade
{
    public class Initializer : IInitializer
    {
        private IContainer _container;
        private IBootList _bootlist;

        public IBootList LoadedBootList {
            get {
                if(_bootlist == null){
                    _bootlist = new BootList (this.Container);
                    _bootlist.Load<Bootloader> ();
                }
                return _bootlist;
            }
        }

        public IContainer Container {
            get {
                if(_container == null){
                    _container = new IocContainer ();
                }
                return _container;
            }
        }

        public IBootstrapper CoreBootstrapper {
            get {
                return new Bootloader ();
            }
        }

        public IBootList GetEmptyBootList (IContainer container)
        {
            return new BootList (container);
        }
    }
}

