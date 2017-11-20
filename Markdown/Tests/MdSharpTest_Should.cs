using System.Collections.Generic;
using FluentAssertions;
using Markdown.Delimiters;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class SharpHandlerTest_Should
    {
        [SetUp]
        public void SetUp()
        {
            mdParser = new Md();
        }

        private Md mdParser;
        
        [TestCase("# word", ExpectedResult = "<H1> word<\\H1>", TestName = "when sharp is start of string")]
        [TestCase("text with\n# whitespaces", ExpectedResult = "text with\n<H1> whitespaces<\\H1>", 
            TestName = "when sharp stand after new line")]
        public string RenderToHtml_Should_InsertTags(string markdown)
        {
            return mdParser.RenderToHtml(markdown);
        }

        [TestCase("#word", ExpectedResult = "#word", TestName = "when word contains sharp")]
        [TestCase(" #text with whitespaces", ExpectedResult = " #text with whitespaces", 
            TestName = "when there is space before sharp")]
        public string RenderToHtml_Should_DoNothing(string markdown)
        {
            return mdParser.RenderToHtml(markdown);
        }
    }
}