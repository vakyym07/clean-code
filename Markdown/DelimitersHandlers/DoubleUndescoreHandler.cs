using System.Collections.Generic;

namespace Markdown.DelimitersHandlers
{
    public class DoubleUndescoreHandler : IDelimiterHandler
    {
        private readonly string closeTag = "<\\strong>";
        private readonly string openTag = "<strong>";
        private readonly HashSet<string> ignoredDelimiters = new HashSet<string>();

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