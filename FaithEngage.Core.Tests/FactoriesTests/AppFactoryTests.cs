using System;
using NUnit.Framework;
using FakeItEasy;
using FaithEngage.Core.Containers;
using FaithEngage.Core.Tests;
namespace FaithEngage.Core.Factories
{
    [TestFixture]
    public class AppFactoryTests
    {
        [Test]
        public void GetOther_Container_ThrowsFactoryException()
        {
            var container = A.Fake<IContainer> ();
            var appFac = new AppFactory (container);
            TestHelpers.TryGetException (() => appFac.GetOther<IContainer> ());
        }
    }
}

