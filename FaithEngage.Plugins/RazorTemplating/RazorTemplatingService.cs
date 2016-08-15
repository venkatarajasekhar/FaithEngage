using System;
using FaithEngage.Core.TemplatingService;
using RazorEngine;
using RazorEngine.Templating;

namespace FaithEngage.Plugins.RazorTemplating
{
	public class RazorTemplatingService : ITemplatingService
	{
		public string CompileHtmlFromTemplate(string template, object model)
		{
			var html = Engine.Razor.RunCompile(template, null, model);
			return html;
		}

		public string CompileHtmlFromTemplateKey(string templateKey, object model)
		{
			var key = Engine.Razor.GetKey(templateKey);
			var html = Engine.Razor.Run(key, null, model);
			return html;
		}

		public void RegisterTemplate(string template, string templateName)
		{
			var service = Engine.Razor;
			service.AddTemplate(templateName, template);
			service.Compile(templateName);
		}
	}
}

