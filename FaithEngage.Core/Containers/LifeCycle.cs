using System;

namespace FaithEngage.Core.Containers
{
    /// <summary>
    /// The specification of dependency lifecycle.
    /// </summary>
	public enum LifeCycle
    {
        Transient, //Object is created anew every time requested
        Singleton //A single instance is stored in memory and shared across the application
    }
}

