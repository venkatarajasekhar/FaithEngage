using System;
namespace FaithEngage.Core.Bootstrappers
{
    /// <summary>
    /// Specifies the order in which bootstrappers are executed.
    /// </summary>
    public enum BootPriority
    {
        Normal = 2,
        Last = 3,
        First = 1
    }
}

