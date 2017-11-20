using System.Collections.Generic;

namespace Markdown.DelimitersHandlers
{
    public class SharpHandler : IDelimiterHandler
    {
        private readonly string closeTag = "<\\H1>";
        private readonly string openTag = "<H1>";
        private readonly HashSet<string> ignoredDelimiters = new HashSet<string> { "#" };

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