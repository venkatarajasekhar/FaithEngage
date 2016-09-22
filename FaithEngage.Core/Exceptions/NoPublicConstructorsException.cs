using System;
namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// Indicates that a type attempting to be constructed has no public constructors. Possible reasons might be if it
	/// is an abstract class or an interface, or if the class is constructed via its own internal logic through a static
	/// method.
    /// </summary>
	public class NoPublicConstructorsException : DependencyException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.NoPublicConstructorsException"/> class.
		/// </summary>
        public NoPublicConstructorsException ()
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.NoPublicConstructorsException"/> class.
		/// </summary>
		/// <param name="typeAtIssue">The the type with no public constructors</param>
		/// <param name="message">Message.</param>
		public NoPublicConstructorsException (Type typeAtIssue, string message) : base (typeAtIssue,message)
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.NoPublicConstructorsException"/> class.
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="context">Context.</param>
        public NoPublicConstructorsException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.NoPublicConstructorsException"/> class.
		/// </summary>
		/// <param name="typeAtIssue">The the type with no public constructors</param>
		/// <param name="message">Message.</param>
		/// <param name="innerException">Inner exception.</param>
        public NoPublicConstructorsException (Type typeAtIssue, string message, Exception innerException) : base (typeAtIssue, message, innerException)
        {
        }
    }
}

