using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class DoubleUnderscoreHandlerTest_Should
    {
        [SetUp]
        public void SetUp()
        {
            mdParser = new Md();
        }

        private Md mdParser;


        [TestCase("__word__", ExpectedResult = "<strong>word<\\strong>",
            TestName = "when one word wrapped double underscores")]
        [TestCase("__text with whitespaces__", ExpectedResult = "<strong>text with whitespaces<\\strong>",
            TestName = "when text wrapped double underscores")]
        public string RenderToHtml_Should_InsertTags(string markdown)
        {
            return mdParser.RenderToHtml(markdown);
        }

        [TestCase("__word", ExpectedResult = "__word", TestName = "when word don't wrapped double underscores")]
        [TestCase("__text with whitespaces", ExpectedResult = "__text with whitespaces",
            TestName = "when text don't wrapped double underscores")]
        [TestCase("__", ExpectedResult = "__", TestName = "when text is underscore")]
        [TestCase("__ chemical__", ExpectedResult = "__ chemical__",
            TestName = "when after the double underscore there is a space")]
        [TestCase("__chemical __romance", ExpectedResult = "__chemical __romance",
            TestName = "when before the double undescore there is space")]
        [TestCase("word__1", ExpectedResult = "word__1", TestName = "when double undescore is inside text with digits")]
        [TestCase("1__2", ExpectedResult = "1__2", TestName = "when double undescore is inside digits string")]
        public string RenderToHtml_Should_DoNothing(string markdown)
        {
            return mdParser.RenderToHtml(markdown);
        }

        [Test]
        public void RenderToHtml_When_TextWrappedWithDoubleUnderscoreAndExistNonPairedUnderscore()
        {
            var markdown = "_Markdown treats asterisks_1 (*) __and _underscores_ as__ indicators _of emphasis._";
            var expectedResult = "_Markdown treats asterisks_1 (*) <strong>and <em>underscores<\\em> " +
                                 "as<\\strong> indicators <em>of emphasis.<\\em>";
            mdParser.RenderToHtml(markdown).Should().BeEquivalentTo(expectedResult);
        }
    }
}