using System;
using FaithEngage.Core.UserClasses;

namespace FaithEngage.Core
{
	public interface IOrganizationRepoManager
	{
		Organization GetById(Guid id);
		void Update(Organization org);
		Guid Save(Organization org);
		void Delete(Guid id);
	}
}

