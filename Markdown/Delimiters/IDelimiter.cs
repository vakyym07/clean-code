using System.Collections.Generic;

namespace Markdown.Delimiters
{
    public interface IDelimiter
    {
        RenderResult RentderToHtml(string markdown, int position, Dictionary<string, IDelimiter> delimiters);
    }
}