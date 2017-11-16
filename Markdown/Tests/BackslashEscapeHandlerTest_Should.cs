using System.Collections.Generic;
using FluentAssertions;
using Markdown.Delimiters;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class BackslashEscapeHandlerTest_Should
    {
        [SetUp]
        public void SetUp()
        {
            delimiterHandler = new BackslashEscapeHandler();
            delimiters = new Dictionary<string, IDelimiter>
            {
                {"_", new SingleUnderscoreHandler()},
                {"__", new DoubleUnderscoreHandler()}
            };
        }

        private BackslashEscapeHandler delimiterHandler;
        private Dictionary<string, IDelimiter> delimiters;

        [TestCase("\\_word_", 0)]
        [TestCase("_word\\_", 5)]
        public void RenderToHtml_Should_NotEscapeDelimiter(string markdown, int position)
        {
            var result = delimiterHandler.RentderToHtml(markdown, position, delimiters).RenderedValue;
            result.Contains("<em>").Should().BeFalse();
            result.Contains("\\").Should().BeFalse();
        }

        [TestCase("_\\word_", 1)]
        [TestCase("_word_\\", 6)]
        public void RenderToHtml_Should_EscapeDelimiter(string markdown, int position)
        {
            var result = delimiterHandler.RentderToHtml(markdown, position, delimiters).RenderedValue;
            result.Contains("\\").Should().BeTrue();
        }
    }
}