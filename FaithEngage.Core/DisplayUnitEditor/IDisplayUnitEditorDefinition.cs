using FaithEngage.Core.TemplatingService;

namespace FaithEngage.Core.DisplayUnitEditor
{
    /// <summary>
    /// Every Display Unit plugin needs to provide an IDisplayUnitEditorDefinition. This is an editor
	/// factory, of sorts, which provides an html form that provides all the input necessary to create
	/// a new DisplayUnit of that type. The inputs must have names that correspond with the dictionary
	/// of keys/values that the particular DisplayUnit can parse and apply.
    /// </summary>
	public interface IDisplayUnitEditorDefinition
    {
        /// <summary>
        /// Gets the html editor form for the DisplayUnit. This is to take the form of an html string.
		/// The ITemplatingService is provided to enable the form to be compiled from whatever input the 
		/// DisplayUnit might need.
        /// </summary>
        /// <returns>The html editor form.</returns>
        /// <param name="tService">The templating service</param>
        /// <param name="context">The DisplayUnitEditorContext</param>
		string GetHtmlEditorForm (ITemplatingService tService, DisplayUnitEditorContext context);
    }
}

