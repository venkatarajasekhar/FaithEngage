using System;
using System.Linq;
using System.Reflection;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.Containers
{
    public class RegisteredObject
    {
        public RegisteredObject(Type abstractType, Type concreteType, LifeCycle lifecycle)
        {
            AbtractType = abstractType;
            ConcreteType = concreteType;
            LifeCycle = lifecycle;
        }

        public Type AbtractType {
            get;
            private set;
        }

        public Type ConcreteType {
            get;
            private set;
        }

        public LifeCycle LifeCycle {
            get;
            private set;
        }

        public object Instance {
            get;
            private set;
        }

        public void CreateInstance(Object[] parameters)
        {
            string message;
            try {
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

