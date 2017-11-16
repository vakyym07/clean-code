using System.Collections.Generic;
using FluentAssertions;
using Markdown.Delimiters;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class DoubleUnderscoreHandlerTest_Should
    {
        [SetUp]
        public void SetUp()
        {
            delimiterHandler = new DoubleUnderscoreHandler();
            delimiters = new Dictionary<string, IDelimiter>
            {
                {"_", new SingleUnderscoreHandler()},
                {"__", new DoubleUnderscoreHandler()}
            };
        }

        private DoubleUnderscoreHandler delimiterHandler;
        private Dictionary<string, IDelimiter> delimiters;

        [TestCase("__word__", 0, TestName = "when one word wrapped double underscores")]
        [TestCase("__text with whitespaces__", 0, TestName = "when text wrapped double underscores")]
        public void RenderToHtml_Should_InsertTags(string markdown, int position)
        {
            var delimiterResult = delimiterHandler.RentderToHtml(markdown, position, delimiters);
            delimiterResult.RenderedValue.Contains("<strong>").Should().BeTrue();
        }

        [TestCase("__word", 0, TestName = "when word don't wrapped double underscores")]
        [TestCase("__text with whitespaces", 0, TestName = "when text don't wrapped double underscores")]
        [TestCase("__", 0, TestName = "when text is underscore")]
        [TestCase("__ chemical__", 0, TestName = "when after the double underscore there is a space")]
        [TestCase("__chemical __romance", 0, TestName = "when before the double undescore there is space")]
        [TestCase("word__1", 4, TestName = "when double undescore is inside text with digits")]
        [TestCase("1__2", 1, TestName = "when double undescore is inside digits string")]
        public void RenderToHtml_Should_DoNothing(string markdown, int position)
        {
            var delimiterResult = delimiterHandler.RentderToHtml(markdown, position, delimiters);
            delimiterResult.RenderedValue.Should().BeEquivalentTo(markdown);
        }

        [Test]
        public void RenderToHtml_When_TextWrappedWithDoubleUnderscoreAndExistNonPairedUnderscore()
        {
            var markdown = "_Markdown treats asterisks_1 (*) __and _underscores_ as__ indicators _of emphasis._";
            var expectedResult = "_Markdown treats asterisks_1 (*) <strong>and <em>underscores<\\em> " +
                                 "as<\\strong> indicators <em>of emphasis.<\\em>";
            delimiterHandler.RentderToHtml(markdown, 0, delimiters).RenderedValue.Should()
                .BeEquivalentTo(expectedResult);
        }
    }
}