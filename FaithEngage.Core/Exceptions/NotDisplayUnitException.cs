using System;

namespace FaithEngage.Core
{
    /// <summary>
    /// Indicates that the type being constructed is not derived from DisplayUnit
    /// </summary>
	public class NotDisplayUnitException : Exception
    {
        public Type InvalidType {
            get;
            set;
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.NotDisplayUnitException"/> class.
		/// </summary>
		/// <param name="invalidType">The type at issue that isn't a DisplayUnit</param>
        public NotDisplayUnitException (Type invalidType)
        {
            InvalidType = invalidType;
        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.NotDisplayUnitException"/> class.
		/// </summary>
		/// <param name="invalidType">The type at issue that isn't a DisplayUnit</param>
		/// <param name="message">Message.</param>
        public NotDisplayUnitException (Type invalidType, string message) : base (message)
        {
            InvalidType = invalidType;

        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.NotDisplayUnitException"/> class.
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="context">Context.</param>
        public NotDisplayUnitException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.NotDisplayUnitException"/> class.
		/// </summary>
		/// <param name="invalidType">The type at issue that isn't a DisplayUnit</param>
		/// <param name="message">Message.</param>
		/// <param name="innerException">Inner exception.</param>
        public NotDisplayUnitException (Type invalidType, string message, Exception innerException) : base (message, innerException)
        {
            InvalidType = invalidType;
        }
        
    }
}

