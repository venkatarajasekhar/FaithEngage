using System;
using System.Linq;
using System.Reflection;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.Containers
{
    /// <summary>
    /// This is used exclusively by the IocContainer. A Registered Object contains a reference to two types,
	/// an abstract type (though it doesn't necessarily HAVE to be abstract/interface) and a concrete implementation.
	/// The abstract and concrete types, COULD be the same. The key is that abstract functions like a key and the concrete
	/// functions like a value. They allow the IocContainer to instantiate types associated with specified interfaces or abstract
	/// classes.
    /// </summary>
	public class RegisteredObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:FaithEngage.Core.Containers.RegisteredObject"/> class.
        /// </summary>
        /// <param name="abstractType">Abstract type.</param>
        /// <param name="concreteType">Concrete type.</param>
        /// <param name="lifecycle">Lifecycle.</param>
		public RegisteredObject(Type abstractType, Type concreteType, LifeCycle lifecycle)
        {
            AbtractType = abstractType;
            ConcreteType = concreteType;
            LifeCycle = lifecycle;
        }

		/// <summary>
		/// The abstract/interface type registered. This COULD also be concrete, in the event of a singleton concrete
		/// class.
		/// </summary>
		/// <value>The type of the abtract.</value>
        public Type AbtractType {
            get;
            private set;
        }

		/// <summary>
		/// The concrete type mapped to the AbstractType, that will be instantiated when requested.
		/// </summary>
		/// <value>The type of the concrete.</value>
        public Type ConcreteType {
            get;
            private set;
        }

		/// <summary>
		/// The type lifecycle specified for this registered object.
		/// </summary>
		/// <value>The life cycle.</value>
        public LifeCycle LifeCycle {
            get;
            private set;
        }

		/// <summary>
		/// When the concrete type is instantiated, the instance of that type is populated here.
		/// </summary>
		/// <value>The instance.</value>
        public object Instance {
            get;
            private set;
        }

		/// <summary>
		/// Creates an instance of the ConcreteType with the parameters supplied.
		/// </summary>
		/// <param name="parameters">Parameters.</param>
        public void CreateInstance(Object[] parameters)
        {
            string message;
            try {
                //Invoke the first constructor of the concrete type.
				Instance = ConcreteType.GetConstructors ().FirstOrDefault ().Invoke (parameters);
            } catch(NullReferenceException ex){
                message = "Type " + ConcreteType.Name + " does not have any constructors.";
                throwException (message, ex);
            }catch (MemberAccessException ex) {
                message = "This class is abstract and cannot be instantiated.";                               
                throwException (message, ex);
            }catch(ArgumentException ex){
                message = "The parameters don't fit the constructor";
                throwException (message, ex);
            }catch(TargetInvocationException ex){
                message = "The invoked constructor threw an exception";
                throwException (message, ex);
            }catch(TargetParameterCountException ex){
                message = "The number of parameters did not fit the constructor";
                throwException (message, ex);
            }catch(Exception ex){
                message = ex.Message;
                throwException (message, ex);
            }
        }

        private void throwException(string message, Exception ex)
        {
            throw new RegisteredObjectInstantiationException (message, ex);
        }

    }
}

