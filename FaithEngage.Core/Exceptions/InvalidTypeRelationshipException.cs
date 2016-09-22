using System;
using FaithEngage.Core.Exceptions;

namespace FaithEngage.Core.Exceptions
{
	/// <summary>
	/// Thrown when a type relationship is being established that is invalid, such as when the ConcreteType
	/// is not actually related to the AbstractType. 
	/// </summary>
	public class InvalidTypeRelationshipException : DependencyException
    {
        /// <summary>
        /// Gets or sets the abstract type being related.
        /// </summary>
        /// <value>The type of the abstract.</value>
		public Type AbstractType {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the concrete type being related.
		/// </summary>
		/// <value>The type of the concrete.</value>
        public Type ConcreteType {
            get;
            set;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.InvalidTypeRelationshipException"/> class.
		/// </summary>
		/// <param name="abstactType">Abstact type.</param>
		/// <param name="concreteType">Concrete type.</param>
        public InvalidTypeRelationshipException (Type abstactType, Type concreteType) : base()
        {
            setTypes (abstactType, concreteType);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.InvalidTypeRelationshipException"/> class.
		/// </summary>
		/// <param name="abstactType">Abstact type.</param>
		/// <param name="concreteType">Concrete type.</param>
		/// <param name="message">Message.</param>
		public InvalidTypeRelationshipException (Type abstactType, Type concreteType, string message) : base (null, message)
        {
            setTypes (abstactType, concreteType);
        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.InvalidTypeRelationshipException"/> class.
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="context">Context.</param>
        public InvalidTypeRelationshipException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.Exceptions.InvalidTypeRelationshipException"/> class.
		/// </summary>
		/// <param name="abstactType">Abstact type.</param>
		/// <param name="concreteType">Concrete type.</param>
		/// <param name="message">Message.</param>
		/// <param name="innerException">Inner exception.</param>
        public InvalidTypeRelationshipException (Type abstactType, Type concreteType,string message, Exception innerException) : base (null, message, innerException)
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

