using System;
using FaithEngage.Core;
using FaithEngage.Core.Containers;

namespace FaithEngage.Facade
{
	public class Bootloader
	{
		private readonly IContainer _container;
		public Bootloader(IContainer container)
		{
			_container = container;
		}

		public IBootstrapper GetBootstrapper()
		{
			return new FaithEngageBootLoader();
		}
	}
}

