using System;
using System.Collections.Generic;
using FaithEngage.Core.UserClasses;

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

		public User User { get; set; }
    }
}

