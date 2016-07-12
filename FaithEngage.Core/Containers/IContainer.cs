
namespace FaithEngage.Core.Containers
{
    public interface IContainer
    {
        T Resolve<T>();
        void Register<Tabstract,Tconcrete>();
        void Register<Tabstract,Tconcrete> (LifeCycle lifecycle);
		int DeRegister<T>();
		void Replace<Tabstract, TnewConcrete>(LifeCycle lifecycle);
    }
}

