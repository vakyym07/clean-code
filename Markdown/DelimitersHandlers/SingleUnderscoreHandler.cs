using System.Collections.Generic;

namespace Markdown.DelimitersHandlers
{
    public class SingleUnderscoreHandler : IDelimiterHandler
    {
        private readonly string closeTag = "<\\em>";
        private readonly string openTag = "<em>";
        private readonly HashSet<string> ignoredDelimiters = new HashSet<string> {"__"};


        public string RenderToHtml(string markdown)
        {
            return string.Join(string.Empty, openTag, markdown, closeTag);
        }

        public bool IsIgnoredDelimiter(string delimiter)
        {
            return ignoredDelimiters.Contains(delimiter);
        }
    }
}