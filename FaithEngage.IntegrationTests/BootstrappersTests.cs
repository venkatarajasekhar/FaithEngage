using NUnit.Framework;
using System;
using FaithEngage.Facade;
using FaithEngage.Core;
using System.Collections.Generic;

namespace FaithEngage.IntegrationTests
{
	[TestFixture]
	public class BootstrappersTests
	{
		[Test]
		public void TestBootLoader()
		{
			var initializer = new Initializer();
			var booter = initializer.GetBootstrapper();
			var container = initializer.GetContainer();

			var booters = new List<IBootstrapper>();
			booter.LoadBootstrappers(booters);

			foreach (var boot in booters)
			{
				boot.RegisterDependencies(container);
			}

			foreach (var boot in booters)
			{
				boot.Execute(container);
			}
		}
	}
}

