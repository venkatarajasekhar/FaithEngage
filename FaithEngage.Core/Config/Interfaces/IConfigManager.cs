using System;
namespace FaithEngage.Core.Config
{
    /// <summary>
    /// The ConfigManager provides get/set access to configurations throughout the application.
    /// </summary>
	public interface IConfigManager
    {
        string TempFolderPath { get; set;}
        string PluginsFolderPath { get; set;}


		string this[string key] { get; set; }
        string GetValue (string key);
        string SetValue (string Key, string value);


    }
}

