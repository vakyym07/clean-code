using System.Collections.Generic;
using Markdown.Ast;
using Markdown.Delimiters;
using Markdown.Parser;

namespace Markdown
{
    public class Md
    {
        private readonly Dictionary<string, IDelimiter> delimiters;

        public Md()
        {
            delimiters = new Dictionary<string, IDelimiter>
            {
                {"_", new SingleUnderscoreDelimiter()},
                {"__", new DoubleUnderscoreDelimiter()},
                {"#", new SingleSharpDelimiter()},
                {"\\", new BackslashEscapeDelimiter(new List<string> {"_", "__", "#", "\\"})}
            };
        }

        public string RenderToHtml(string markdown)
        {
            var parser = new MdParser(delimiters, markdown);
            var astRoot = parser.Parse();
            return new AstRenderToHtml(astRoot).RenderToHtml();
        }
    }
}