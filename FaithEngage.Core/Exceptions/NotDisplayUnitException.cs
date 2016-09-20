using System;

namespace FaithEngage.Core
{
    public class NotDisplayUnitException : Exception
    {
        public Type InvalidType {
            get;
            set;
        }

        public NotDisplayUnitException (Type invalidType)
        {
            InvalidType = invalidType;
        }
        

        public NotDisplayUnitException (Type invalidType, string message) : base (message)
        {
            InvalidType = invalidType;

        }
        

        public NotDisplayUnitException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public NotDisplayUnitException (Type invalidType, string message, Exception innerException) : base (message, innerException)
        {
            InvalidType = invalidType;

        }
        
    }
}

