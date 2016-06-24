using System;
using NUnit.Framework;
using FakeItEasy;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories
{
	[TestFixture]
	public class DisplayUnitPluginDtoFactoryTests
	{
		private Guid VALID_GUID = Guid.NewGuid();
		private const string VALID_STRING = "VALID STRING";

		[Test]
		public void ConvertFromPlugin_ValidPluginWithId_ValidPlugin()
		{
			var plugin = A.Fake<DisplayUnitPlugin> ();
			plugin.PluginId = VALID_GUID;
			A.CallTo (() => plugin.AssemblyLocation).Returns (VALID_STRING);
			A.CallTo (() => plugin.PluginName).Returns (VALID_STRING);
			A.CallTo (() => plugin.PluginVersion).Returns (new int[]{ 1, 0, 0 });
			var fac = new DisplayUnitPluginDtoFactory ();
			var dto = fac.Convert (plugin);

			Assert.That (dto.AssemblyLocation, Is.Not.Null);
			Assert.That (dto.Id, Is.EqualTo (VALID_GUID));
			Assert.That (dto.PluginName, Is.EqualTo (VALID_STRING));
			Assert.That (dto.PluginVersion, Is.EqualTo (new int[]{ 1, 0, 0 }));
		}

		[Test]
		public void ConvertFromPlugin_ValidPluginWithoutId_ValidPlugin()
		{
			var plugin = A.Fake<DisplayUnitPlugin> ();

			A.CallTo (() => plugin.AssemblyLocation).Returns (VALID_STRING);
			A.CallTo (() => plugin.PluginName).Returns (VALID_STRING);
			A.CallTo (() => plugin.PluginVersion).Returns (new int[]{ 1, 0, 0 });
			var fac = new DisplayUnitPluginDtoFactory ();
			var dto = fac.Convert (plugin);

			Assert.That (dto.AssemblyLocation, Is.Not.Null);
			Assert.That (dto.Id, Is.Null);
			Assert.That (dto.PluginName, Is.EqualTo (VALID_STRING));
			Assert.That (dto.PluginVersion, Is.EqualTo (new int[]{ 1, 0, 0 }));
		}
	}
}

