using System;
using System.Collections.Generic;
using FaithEngage.Core.UserClasses;

namespace FaithEngage.Core.Cards
{
    /// <summary>
	/// The CardAction is generated through an API (TBD) and fed to a DisplayUnit's
	/// ExecuteCardAction() method to produce a cardActionResult.
    /// </summary>
	public class CardAction
    {
        public CardAction ()
		{
			this.Parameters = new Dictionary<string, string> ();
		}

		public string ActionName {
            get;
            set;
        }

		/// <summary>
		/// Key/Value pairs used by the Display Unit to process the action.
		/// </summary>
		/// <value>The parameters.</value>
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

