using System;
using FaithEngage.Core.TemplatingService;
using RazorEngine;
using RazorEngine.Templating;
using FaithEngage.Plugins.Exceptions;

namespace FaithEngage.Plugins.RazorTemplating
{
	public class RazorTemplatingService : ITemplatingService
	{
		public string CompileHtmlFromTemplate(string template, string templateName, object model)
		{
            string html = null;
            try {
                html = Engine.Razor.RunCompile (template, templateName, null, model);
            } catch (Exception ex) {
                throw new TemplatingException ("There was a problem compiling the template.", ex);
            }
			return html;
		}

		public string CompileHtmlFromTemplateKey(string templateKey, object model)
		{
            string html = null;
            try {
                var key = Engine.Razor.GetKey (templateKey);
                html = Engine.Razor.Run (key, null, model);
            } catch (Exception ex) {
                throw new TemplatingException (
                    "There was a problem compiling the razor template.", ex
                );
            }
			return html;
		}

		public void RegisterTemplate(string template, string templateName)
		{
            try {
                Engine.Razor.Compile (template, templateName);
            } catch (Exception ex) {
                throw new TemplatingException (
                    "There was a problem registering the template", ex);
            }
		}
	}
}

