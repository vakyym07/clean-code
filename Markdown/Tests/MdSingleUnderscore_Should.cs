using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdSingleUnderscore_Should
    {
        [SetUp]
        public void SetUp()
        {
            mdParser = new Md();
        }

        private Md mdParser;

        [TestCase("_word_", ExpectedResult = "<em>word<\\em>", TestName = "when one word wrapped underscores")]
        [TestCase("_text with whitespaces_", ExpectedResult = "<em>text with whitespaces<\\em>",
            TestName = "when text wrapped underscores")]
        public string RenderToHtml_Should_InsertTags(string markdown)
        {
            return mdParser.RenderToHtml(markdown);
        }

        [TestCase("_word", ExpectedResult = "_word", TestName = "when word don't wrapped underscores")]
        [TestCase("_text with whitespaces", ExpectedResult = "_text with whitespaces",
            TestName = "when text don't wrapped underscores")]
        [TestCase("_", ExpectedResult = "_", TestName = "when text is underscore")]
        [TestCase("_ chemical_", ExpectedResult = "_ chemical_",
            TestName = "when after the underscore there is a space")]
        [TestCase("_chemical _romance", ExpectedResult = "_chemical _romance",
            TestName = "when before the undescore there is space")]
        [TestCase("word_1", ExpectedResult = "word_1", TestName = "when undescore is inside text with digits")]
        [TestCase("1_2", ExpectedResult = "1_2", TestName = "when undescore is inside digits string")]
        public string RenderToHtml_Should_DoNothing(string markdown)
        {
            return mdParser.RenderToHtml(markdown);
        }

        [Test]
        public void RenderToHtml_When_TextContainsDoubleUnderscore()
        {
            var markdown = "_Markdown __treats__ asterisks_";
            var expectedResult = "<em>Markdown treats asterisks<\\em>";
            mdParser.RenderToHtml(markdown).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void RenderToHtml_When_TextWrappedWithUnderscoreAndExistNonPairedUnderscore()
        {
            var markdown = "_Markdown treats asterisks (*) _and _underscores_ as_ indicators _of emphasis._";
            var expectedResult = "_Markdown treats asterisks (*) <em>and <em>underscores<\\em> " +
                                 "as<\\em> indicators <em>of emphasis.<\\em>";
            mdParser.RenderToHtml(markdown).Should().BeEquivalentTo(expectedResult);
        }
    }
}