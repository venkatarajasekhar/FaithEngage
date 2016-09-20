using System;
namespace FaithEngage.Core.TemplatingService
{
    public interface ITemplatingService
    {
        string CompileHtmlFromTemplate (string template, string templateName, object model);
		string CompileHtmlFromTemplateKey(string templateKey, object model);
        void RegisterTemplate (string template, string templateName);
    }
}

