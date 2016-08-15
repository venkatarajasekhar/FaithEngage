using System;
using System.Collections.Generic;

namespace FaithEngage.Core.UserClasses
{
	public class User
	{
		public Dictionary<Guid,Organization> AssignedOrganizations{ get; set; }

		public string UserName {
			get;
			set;
		}

		public string FirstName {
			get;
			set;
		}

		public string LastName {
			get;
			set;
		}

		public string EmailAddress {
			get;
			set;
		}

		public Dictionary<Guid, UserPrivileges> OrgPrivileges {
			get;
			set;
		}
	}
}

