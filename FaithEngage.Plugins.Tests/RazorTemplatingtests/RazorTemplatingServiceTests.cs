using System;
using FaithEngage.Plugins.Tests;
using NUnit.Framework;
using FaithEngage.CorePlugins.Exceptions;
namespace FaithEngage.CorePlugins.RazorTemplating
{
    public class RazorTemplatingServiceTests
    {
        [Test]
        public void CompileHtmlFromTemplate_ValidTemplate_ValidObject_ReturnsString()
        {
            var template = @"<p>@Model.Text</p>";

            var model = new { Text = "Hello" };

            var tempService = new RazorTemplatingService ();

            var html = tempService.CompileHtmlFromTemplate (template, "TestTemplate1", model);

            Assert.That (html == "<p>Hello</p>");
        }

        [Test]
        public void CompileHtmlFromTemplate_InvalidTemplate_ValidObject_ThrowsTemplatingException()
        {
            string template = @"@Model.Text.bluga()";
            var model = new { Text = "Hello" };
            var tempService = new RazorTemplatingService ();
            var e = TestHelpers.TryGetException(()=> tempService.CompileHtmlFromTemplate (template, "TestTemplate2", model));

            Assert.That (e, Is.InstanceOf<TemplatingException> ());
        }

        [Test]
        public void RegisterTemplateAndCompileFromTemplateKey_ValidTemplate_ValidTemplateName_Registers()
        {
            var template = @"<p>@Model.Text</p>";
            var tempService = new RazorTemplatingService ();
            tempService.RegisterTemplate (template, "testTemplate3");
            var model = new { Text = "Hello" };
            var html = tempService.CompileHtmlFromTemplateKey ("testTemplate3", model);

            Assert.That (html == "<p>Hello</p>");
        }


    }
}

