using System;

namespace FaithEngage.Core.Exceptions
{
    public class TypeNotRegisteredException : DependencyException
    {
		public TypeNotRegisteredException(Type typeAtIssue) : base(typeAtIssue, $"Type not registered: {typeAtIssue.FullName}")
        {
        }
        

        public TypeNotRegisteredException (Type typeAtIssue, string message) : base (typeAtIssue, message)
        {
        }
        

        public TypeNotRegisteredException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public TypeNotRegisteredException (Type typeAtIssue, string message, Exception innerException) : base (typeAtIssue, message, innerException)
        {
        }
        
    }
}

