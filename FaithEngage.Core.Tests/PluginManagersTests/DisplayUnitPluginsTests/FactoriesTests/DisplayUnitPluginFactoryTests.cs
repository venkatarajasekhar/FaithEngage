using System;
using NUnit.Framework;
using FaithEngage.Core.DisplayUnits;
using System.Linq;

namespace FaithEngage.Core.PluginManagers.DisplayUnitPlugins.Factories
{
	[TestFixture]
	public class DisplayUnitPluginFactoryTests
	{
		private DisplayUnitPlugin _plgin;

		[TestFixtureSetUp]
		public void init()
		{
			_plgin = new Dummy_PluginAssembly.DummyPlugin ();
		}

		[Test]
		public void LoadPluginFromDto_ValidDto_ValidAssembly_ValidPlugin()
		{
			var dto = new PluginDTO ();
			dto.AssemblyLocation = _plgin.AssemblyLocation;
			dto.FullName = _plgin.FullName;
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
			var dto = new PluginDTO ();

			var factory = new DisplayUnitPluginFactory ();
			var plugin = factory.LoadPluginFromDto (dto);

			Assert.That (plugin, Is.Null);
		}

		[Test]
		public void LoadPluginsFromDtos_ValidDtos_ValidPlugins()
		{
			var dto = new PluginDTO ();
			dto.AssemblyLocation = _plgin.AssemblyLocation;
			dto.FullName = _plgin.FullName;
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
			var dto = new PluginDTO ();
			dto.AssemblyLocation = _plgin.AssemblyLocation;
			dto.FullName = _plgin.FullName;
			dto.Id = Guid.NewGuid ();

			var dtos = Enumerable.Repeat (dto, 3).ToList ();
			dtos.Add(new PluginDTO());
			dtos.Add(new PluginDTO());

			var factory = new DisplayUnitPluginFactory ();
			var plugins = factory.LoadPluginsFromDtos (dtos);

			Assert.That (plugins.Count (), Is.EqualTo (3));
			Assert.That (plugins.All (p => p.PluginId == dto.Id));
		}

		[Test]
		public void LoadPluginsFromDtos_AllInvalid_ReturnsEmptyIEnumerable()
		{
			var dto = new PluginDTO ();
			var dtos = Enumerable.Repeat (dto, 5);

			var factory = new DisplayUnitPluginFactory ();
			var plugins = factory.LoadPluginsFromDtos (dtos);

			Assert.That (plugins.Count (), Is.EqualTo (0));
		}
	}
}

