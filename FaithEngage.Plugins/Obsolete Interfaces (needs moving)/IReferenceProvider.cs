using System;

namespace FaithEngage.Core.Interfaces
{
    public interface IReferenceProvider
    {
        IReference Parse (string reference);

        string GetReference (IReference reference);
    }
}

