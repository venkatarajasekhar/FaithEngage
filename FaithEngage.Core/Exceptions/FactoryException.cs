﻿using System;
namespace FaithEngage.Core.Exceptions
{
	public class FactoryException : Exception
	{
		public FactoryException()
		{
		}

		public FactoryException(string message) : base(message)
		{
		}

		public FactoryException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public FactoryException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}

