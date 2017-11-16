using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Delimiters
{
    public class BaseHandlerDelimiter : AbstractDelimiterHandler
    {
        public string RentderToHtml(string markdown, int position, Dictionary<string, IDelimiter> delimiters)
        {
            var sordedByLengthDelimiters = delimiters.Keys.OrderByDescending(e => e.Length).ToList();
            var nextDelimiter =
                FindNextDelimiter(markdown, position, sordedByLengthDelimiters);
            while (nextDelimiter.Position != -1)
            {
                var innerRenderResult = delimiters[nextDelimiter.Name]
                    .RentderToHtml(markdown, nextDelimiter.Position, delimiters);
                markdown = innerRenderResult.RenderedValue;
                nextDelimiter = FindNextDelimiter(markdown, innerRenderResult.NewPosition, sordedByLengthDelimiters);
            }
            return markdown;
        }

        public override string RemoveIgnoredTag(string markdown, int position, int countSign)
        {
            throw new NotImplementedException();
        }

        public override bool IsTerminatedMdTag(string markdown, int position)
        {
            throw new NotImplementedException();
        }
    }
}