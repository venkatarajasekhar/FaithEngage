using System;
using System.Collections;
using System.Collections.Generic;

namespace FaithEngage.Core.Interfaces
{
    public interface IReference
    {
        IEnumerable<IReference> GetSubReferences();

        bool HasSubReferences {
            get;
        }

        object LastVerse {
            get;
        }
        object FirstVerse {
            get;
        }

        bool IsParsed {
            get;
            set;
        }
        int Length{
            get;
        }


    }
}

