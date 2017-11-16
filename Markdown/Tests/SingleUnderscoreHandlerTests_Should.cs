using System.Collections.Generic;
using FluentAssertions;
using Markdown.Delimiters;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class SingleUnderscoreHandlerTests_Should
    {
        [SetUp]
        public void SetUp()
        {
            delimiterHandler = new SingleUnderscoreHandler();
            delimiters = new Dictionary<string, IDelimiter>()
            {
                {"_", new SingleUnderscoreHandler()},
                {"__", new DoubleUnderscoreHandler()},
            };
        }

        private SingleUnderscoreHandler delimiterHandler;
        private Dictionary<string, IDelimiter> delimiters;

        [TestCase("_word_", 0, TestName = "when one word wrapped underscores")]
        [TestCase("_text with whitespaces_", 0, TestName = "when text wrapped underscores")]
        public void RenderToHtml_Should_InsertTags(string markdown, int position)
        {
            var delimiterResult = delimiterHandler.RentderToHtml(markdown, position, delimiters);
            delimiterResult.RenderedValue.Contains("<em>").Should().BeTrue();
        }

        [TestCase("_word", 0, TestName = "when word don't wrapped underscores")]
        [TestCase("_text with whitespaces", 0, TestName = "when text don't wrapped underscores")]
        [TestCase("_", 0, TestName = "when text is underscore")]
        [TestCase("_ chemical_", 0, TestName = "when after the underscore there is a space")]
        [TestCase("_chemical _romance", 0, TestName = "when before the undescore there is space")]
        [TestCase("word_1", 4, TestName = "when undescore is inside text with digits")]
        [TestCase("1_2", 1, TestName = "when undescore is inside digits string")]
        public void RenderToHtml_Should_DoNothing(string markdown, int position)
        {
            var delimiterResult = delimiterHandler.RentderToHtml(markdown, position, delimiters);
            delimiterResult.RenderedValue.Contains("<em>").Should().BeFalse();
        }

        [Test]
        public void RenderToHtml_When_TextWrappedWithUnderscoreAndExistNonPairedUnderscore()
        {
            var markdown = "_Markdown treats asterisks (*) _and _underscores_ as_ indicators _of emphasis._";
            var expectedResult = "_Markdown treats asterisks (*) <em>and <em>underscores<\\em> " +
                                 "as<\\em> indicators <em>of emphasis.<\\em>";
            delimiterHandler.RentderToHtml(markdown, 0, delimiters).RenderedValue.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void RenderToHtml_When_TextContainsDoubleUnderscore()
        {
            var markdown = "_Markdown __treats__ asterisks_";
            var expectedResult = "<em>Markdown treats asterisks<\\em>";
            delimiterHandler.RentderToHtml(markdown, 0, delimiters).RenderedValue.Should().BeEquivalentTo(expectedResult);
        }
    }
}