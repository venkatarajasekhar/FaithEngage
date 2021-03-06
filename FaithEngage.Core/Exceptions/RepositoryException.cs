﻿using System;

namespace FaithEngage.Core.Exceptions
{
    /// <summary>
    /// The base Exception type from which all repository-associated exceptions derive.
    /// </summary>
	public class RepositoryException : Exception
    {
        public RepositoryException ()
        {
        }
        

        public RepositoryException (string message) : base (message)
        {
        }
        

        public RepositoryException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        

        public RepositoryException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

