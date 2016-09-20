using System;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.Exceptions
{
    public class InvalidTypeRelationshipException : Exception
    {
        public Type AbstractType {
            get;
            set;
        }

        public Type ConcreteType {
            get;
            set;
        }

        public InvalidTypeRelationshipException (Type abstactType, Type concreteType) : base()
        {
            setTypes (abstactType, concreteType);
        }

        public InvalidTypeRelationshipException (Type abstactType, Type concreteType,string message) : base (message)
        {
            setTypes (abstactType, concreteType);
        }
        

        public InvalidTypeRelationshipException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public InvalidTypeRelationshipException (Type abstactType, Type concreteType,string message, Exception innerException) : base (message, innerException)
        {
            setTypes (abstactType, concreteType);
        }

        private void setTypes(Type abstractType, Type concreteType)
        {
            AbstractType = abstractType;
            ConcreteType = concreteType;
        }
        
    }
}

