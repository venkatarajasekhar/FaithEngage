using System;

namespace FaithEngage.Core
{
    /// <summary>
    /// A plugin is being registered for which its id is already registered.
    /// </summary>
	public class PluginAlreadyRegisteredException : Exception
    {
		/// <summary>
		/// Gets or sets id of the already existing plugin.
		/// </summary>
		/// <value>The existing plugin identifier.</value>
		public Guid ExistingPluginId { get; set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.PluginAlreadyRegisteredException"/> class.
		/// </summary>
		public PluginAlreadyRegisteredException ()
        {
        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.PluginAlreadyRegisteredException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
        public PluginAlreadyRegisteredException (string message) : base (message)
        {
        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.PluginAlreadyRegisteredException"/> class.
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="context">Context.</param>
        public PluginAlreadyRegisteredException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
        {
        }
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FaithEngage.Core.PluginAlreadyRegisteredException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="innerException">Inner exception.</param>
        public PluginAlreadyRegisteredException (string message, Exception innerException) : base (message, innerException)
        {
        }
        
    }
}

