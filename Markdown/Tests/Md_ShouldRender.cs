﻿using System.Collections.Generic;
using FluentAssertions;
using Markdown.Delimiters;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_ShouldRender
    {
        [SetUp]
        public void SetUp()
        {
            mdParser = new Md(new Dictionary<string, IDelimiter>
            {
                {"_", new SingleUnderscoreHandler()},
                {"__", new DoubleUnderscoreHandler()},
                {"#", new SharpHandler()},
                {"\\", new BackslashEscapeHandler()}
            });
        }

        private Md mdParser;

        [Test]
        public void RenderToHtml_When_TextContainsUnderscoreAndDoubleUnderscore()
        {
            var markdown = "_Markdown treats asterisks_1 (*) __and underscores as__ indicators _of emphasis.";
            var expectedResult = "_Markdown treats asterisks_1 (*) <strong>and underscores " +
                                 "as<\\strong> indicators _of emphasis.";
            mdParser.RenderToHtml(markdown).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void RenderToHtml_When_TextContainsUnderscoreDoubleUnderscoreSharp()
        {
            var markdown = "# _Markdown treats asterisks_1 (*) __and underscores as__ indicators _of emphasis.";
            var expectedResult = "<H1>_Markdown treats asterisks_1 (*) <strong>and underscores " +
                                 "as<\\strong> indicators _of emphasis.<\\H1>";
            mdParser.RenderToHtml(markdown).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void RenderToHtml_When_TextContainsUnderscoreDoubleUnderscoreSharpBackslash()
        {
            var markdown = "\\# _Markdown treats asterisks_1 (*) __and underscores as__ indicators _of emphasis.";
            var expectedResult = "# _Markdown treats asterisks_1 (*) <strong>and underscores " +
                                 "as<\\strong> indicators _of emphasis.";
            mdParser.RenderToHtml(markdown).Should().BeEquivalentTo(expectedResult);
        }
    }
}