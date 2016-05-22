using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konves.Scripture
{
    /// <summary>
    /// Reference format exception.
    /// </summary>
    [Serializable]
    public class ReferenceFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Konves.Scripture.ReferenceFormatException"/> class.
        /// </summary>
        public ReferenceFormatException ()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Konves.Scripture.ReferenceFormatException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public ReferenceFormatException (string message) : base (message)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReferenceFormatException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public ReferenceFormatException (string message, Exception inner) : base (message, inner)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReferenceFormatException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected ReferenceFormatException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
    }

}
