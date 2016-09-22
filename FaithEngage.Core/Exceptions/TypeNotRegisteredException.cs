using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Indicates a type is being requested for which there is no concrete implementation registered. This is thrown
	/// by an IContainer.
    /// </summary>
	public class TypeNotRegisteredException : DependencyException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.TypeNotRegisteredException"/> class.
		/// </summary>
		/// <param name="typeAtIssue">The type that is not registered.</param>
		public TypeNotRegisteredException(Type typeAtIssue) : base(typeAtIssue, $"Type not registered: {typeAtIssue.FullName}")
        {
        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.TypeNotRegisteredException"/> class.
		/// </summary>
		/// <param name="typeAtIssue">The type that is not registered.</param>
		/// <param name="message">Message.</param>
        public TypeNotRegisteredException (Type typeAtIssue, string message) : base (typeAtIssue, message)
        {
        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.TypeNotRegisteredException"/> class.
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="context">Context.</param>
        public TypeNotRegisteredException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.TypeNotRegisteredException"/> class.
		/// </summary>
		/// <param name="typeAtIssue">The type that is not registered.</param>
		/// <param name="message">Message.</param>
		/// <param name="innerException">Inner exception.</param>
        public TypeNotRegisteredException (Type typeAtIssue, string message, Exception innerException) : base (typeAtIssue, message, innerException)
        {
        }
        
    }
}

