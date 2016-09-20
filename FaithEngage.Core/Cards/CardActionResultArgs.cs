using System;
using System.Collections.Generic;

namespace FaithEngage.Core.Cards
{
    /// <summary>
    /// Key/Value pairs sent as a result of executing a card action.
    /// </summary>
	public class CardActionResultArgs
    {
        public Dictionary<string,string> Responses {
            get;
            set;
        }

        public Guid? DestinationDisplayUnit {
            get;
            set;
        }
			
    }
}

