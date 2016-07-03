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
            var container = initializer.GetContainer ();

			var booters = new List<IBootstrapper>();
            Console.WriteLine ("Loading Bootstrappers...");
            booter.LoadBootstrappers(booters);
            Console.WriteLine ("Registering Dependencies...");
			foreach (var boot in booters)
			{
                Console.WriteLine (boot);
                boot.RegisterDependencies(container);
			}

            Console.WriteLine ("Executing Booters...");
			foreach (var boot in booters)
			{
                boot.Execute (container);
			}
		}
	}
}

