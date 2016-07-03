using System;
namespace FaithEngage.Core.TemplatingService
{
    public interface ITemplatingService
    {
        string CompileHtmlFromTemplate (string template, dynamic model);
        string RegisterTemplate (string template, string templateName);
        string GetTemplate (string templateName);
    }
}

