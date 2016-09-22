using System;

namespace FaithEngage.Core.Exceptions
{
	/// <summary>
	/// Indicates a failure in authentication.
	/// </summary>
	public class AuthenticationException : Exception
	{
		public AuthenticationException ()
		{
		}
		

		public AuthenticationException (string message) : base (message)
		{
		}
		

		public AuthenticationException (string message, Exception innerException) : base (message, innerException)
		{
		}
		

		public AuthenticationException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
		}
		
	}
}

