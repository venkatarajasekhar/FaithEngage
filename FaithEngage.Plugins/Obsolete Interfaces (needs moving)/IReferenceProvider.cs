using System;

namespace FaithEngage.CorePlugins.Interfaces
{
    public interface IReferenceProvider
    {
        IReference Parse (string reference);

        string GetReference (IReference reference);
    }
}

