using System;
using System.Collections.Generic;

namespace Markdown.Delimiters
{
    public abstract class AbstractDelimiterHandler
    {
        public abstract string RemoveIgnoredTag(string markdown, int position, int countSign);
        public abstract bool IsTerminatedMdTag(string markdown, int position);

        public RenderResult MdParse(string markdown, int startPosition,
            Dictionary<string, IDelimiter> delimiters, string delimiterValue,
            HashSet<string> ignoredDelimiters, List<string> specialTerminatedSymbols, string openTag, string closeTag)
        {
            var nextDelimiter =
                FindNextDelimiter(markdown, startPosition + delimiterValue.Length, specialTerminatedSymbols);
            while (nextDelimiter.Position != -1)
            {
                if (ignoredDelimiters.Contains(nextDelimiter.Name))
                {
                    markdown = RemoveIgnoredTag(markdown, nextDelimiter.Position, nextDelimiter.Name.Length);
                    nextDelimiter = FindNextDelimiter(markdown, nextDelimiter.Position, specialTerminatedSymbols);
                    continue;
                }

                if (IsTerminatedMdTag(markdown, nextDelimiter.Position))
                {
                    var htmlString = RenderStringToHtml(markdown, startPosition,
                        nextDelimiter.Position, delimiterValue, openTag,
                        closeTag);
                    markdown = markdown.Substring(0, startPosition) + htmlString
                                      + markdown.Substring(nextDelimiter.Position + delimiterValue.Length);
                    return new RenderResult(markdown, startPosition + htmlString.Length);
                }
                var innerRenderResult = delimiters[nextDelimiter.Name]
                    .RentderToHtml(markdown, nextDelimiter.Position, delimiters);
                markdown = innerRenderResult.RenderedValue;
                nextDelimiter = FindNextDelimiter(markdown, innerRenderResult.NewPosition, specialTerminatedSymbols);
            }
            return new RenderResult(markdown, markdown.Length);
        }

        public bool FindDelimiterWithCondition(string markdown, string delimiterValue, int position, 
            List<string> specialTerminatedSymbols, Func<string, int, bool> condition)
        {
            var nextDelimiter = FindNextDelimiter(markdown, position, specialTerminatedSymbols);
            while (nextDelimiter.Position != -1)
            {
                if (nextDelimiter.Name == delimiterValue && condition(markdown, nextDelimiter.Position))
                    return true;
                nextDelimiter = FindNextDelimiter(markdown, 
                    nextDelimiter.Position + nextDelimiter.Name.Length, specialTerminatedSymbols);
            }
            return false;
        }

        public string RenderStringToHtml(string markdown, int startPosition, int endPosition, 
            string delimiterValue, string openTag, string closeTag)
        {
            var htmlString =
                WrapInTags(
                    markdown.Substring(startPosition + delimiterValue.Length, 
                    endPosition - startPosition - delimiterValue.Length),
                    openTag, closeTag);
            return htmlString;
            
        }

        public Token FindNextDelimiter(string markdown, int position,
            List<string> specialTerminatedSymbols)
        {
            var delimiterName = "";
            var minDelimiterPosition = markdown.Length;

            foreach (var delimiter in specialTerminatedSymbols)
            {
                var delimiterPosition = markdown.IndexOf(delimiter, position, StringComparison.Ordinal);
                if (delimiterPosition != -1 && delimiterPosition < minDelimiterPosition)
                {
                    delimiterName = delimiter;
                    minDelimiterPosition = delimiterPosition;
                }
            }
            return minDelimiterPosition == markdown.Length
                ? new Token(-1, TokenStatus.NotDelimiter)
                : new Token(delimiterName, minDelimiterPosition, TokenStatus.Delimiter);
        }

        public string WrapInTags(string text, string openTag, string closeTag)
        {
            return string.Join(string.Empty, openTag, text, closeTag);
        }
    }
}