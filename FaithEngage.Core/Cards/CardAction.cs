using System;
using System.Collections.Generic;

namespace FaithEngage.Core.Cards
{
    public class CardAction
    {
        public string ActionName {
            get;
            set;
        }

        public Dictionary<string,string> Parameters {
            get;
            set;
        }

        public Guid OriginatingDisplayUnit{
            get;
            set;
        }
    }
}

