using System;
namespace FaithEngage.Core.PluginManagers
{
	public class PluginDTO
	{
		public Guid? Id
		{
			get;
			set;
		}
		public string AssemblyLocation
		{
			get;
			set;
		}
		public string FullName { get; set; }
		public string PluginName { get; set; }
		public int[] PluginVersion { get; set; }
		public PluginTypeEnum PluginType{get;set;}
	}
}

