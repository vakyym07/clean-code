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
            delimiterHandler = new SharpHandler();
            delimiters = new Dictionary<string, IDelimiter>
            {
                {"_", new SingleUnderscoreHandler()},
                {"__", new DoubleUnderscoreHandler()},
                {"#", new SharpHandler()}
            };
        }

        private SharpHandler delimiterHandler;
        private Dictionary<string, IDelimiter> delimiters;

        [TestCase("# word", 0, TestName = "when sharp is start of string")]
        [TestCase("text with\n# whitespaces", 10, TestName = "when sharp stand after new line")]
        public void RenderToHtml_Should_InsertTags(string markdown, int position)
        {
            var delimiterResult = delimiterHandler.RentderToHtml(markdown, position, delimiters);
            delimiterResult.RenderedValue.Contains("<H1>").Should().BeTrue();
        }

        [TestCase("#word", 0, TestName = "when word contains sharp")]
        [TestCase(" #text with whitespaces", 1, TestName = "when there is space before sharp")]
        public void RenderToHtml_Should_DoNothing(string markdown, int position)
        {
            var delimiterResult = delimiterHandler.RentderToHtml(markdown, position, delimiters);
            delimiterResult.RenderedValue.Should().BeEquivalentTo(markdown);
        }
    }
}