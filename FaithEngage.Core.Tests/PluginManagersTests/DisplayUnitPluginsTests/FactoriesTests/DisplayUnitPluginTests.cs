using System;
using NUnit.Framework;
using FaithEngage.Core.DisplayUnits;
using System.Linq;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories
{
	[TestFixture]
	public class DisplayUnitPluginTests
	{
		[Test]
		public void LoadPluginFromDto_ValidDto_ValidAssembly_ValidPlugin()
		{
			var dto = new DisplayUnitPluginDTO ();
			dto.AssemblyLocation = "Dummy_PluginAssembly.dll";
			dto.FullName = "Dummy_PluginAssembly.DummyPlugin";
			dto.Id = Guid.NewGuid ();

			var factory = new DisplayUnitPluginFactory ();
			var plugin = factory.LoadPluginFromDto (dto);

			Assert.That (plugin, Is.Not.Null);
			Assert.That (plugin.PluginId, Is.EqualTo (dto.Id));
			Assert.That (plugin.DisplayUnitType, Is.Not.Null);
			Assert.That(plugin.DisplayUnitType.IsAssignableFrom(typeof(DisplayUnit)));
		}

		[Test]
		public void LoadPluginFromDto_InvalidDto_Null()
		{
			var dto = new DisplayUnitPluginDTO ();

			var factory = new DisplayUnitPluginFactory ();
			var plugin = factory.LoadPluginFromDto (dto);

			Assert.That (plugin, Is.Null);
		}

		[Test]
		public void LoadPluginsFromDtos_ValidDtos_ValidPlugins()
		{
			var dto = new DisplayUnitPluginDTO ();
			dto.AssemblyLocation = "Dummy_PluginAssembly.dll";
			dto.FullName = "Dummy_PluginAssembly.DummyPlugin";
			dto.Id = Guid.NewGuid ();

			var dtos = Enumerable.Repeat (dto, 5);

			var factory = new DisplayUnitPluginFactory ();
			var plugins = factory.LoadPluginsFromDtos (dtos);

			Assert.That (plugins.Count (), Is.EqualTo (5));
			Assert.That (plugins.All (p => p.PluginId == dto.Id));
		}

		[Test]
		public void LoadPluginsFromDtos_SomeInvalid_ValidPluginsLessInvalidOnes()
		{
			var dto = new DisplayUnitPluginDTO ();
			dto.AssemblyLocation = "Dummy_PluginAssembly.dll";
			dto.FullName = "Dummy_PluginAssembly.DummyPlugin";
			dto.Id = Guid.NewGuid ();

			var dtos = Enumerable.Repeat (dto, 3).ToList ();
			dtos.Add(new DisplayUnitPluginDTO());
			dtos.Add(new DisplayUnitPluginDTO());

			var factory = new DisplayUnitPluginFactory ();
			var plugins = factory.LoadPluginsFromDtos (dtos);

			Assert.That (plugins.Count (), Is.EqualTo (3));
			Assert.That (plugins.All (p => p.PluginId == dto.Id));
		}

		[Test]
		public void LoadPluginsFromDtos_AllInvalid_ReturnsEmptyIEnumerable()
		{
			var dto = new DisplayUnitPluginDTO ();
			var dtos = Enumerable.Repeat (dto, 5);

			var factory = new DisplayUnitPluginFactory ();
			var plugins = factory.LoadPluginsFromDtos (dtos);

			Assert.That (plugins.Count (), Is.EqualTo (0));
		}
	}
}

