using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Delimiters
{
    public class SharpHandler : AbstractDelimiterHandler, IDelimiter
    {
        private readonly string closeTag = "<\\H1>";
        private readonly string delimiterValue = "#";
        private readonly HashSet<string> ignoredDelimiters;
        private readonly string openTag = "<H1>";
        private readonly List<Func<string, int, bool>> rulesForSharp;

        public SharpHandler()
        {
            ignoredDelimiters = new HashSet<string>();
            rulesForSharp = new List<Func<string, int, bool>>
            {
                WhiteSpacesAndNewLineAreCorrectRelativeSharp
            };
        }

        public RenderResult RentderToHtml(string markdown, int position, Dictionary<string, IDelimiter> delimiters)
        {
            if (!IsCorrectUnderscoreTag(markdown, position))
                return new RenderResult(markdown, position + delimiterValue.Length);
            var endPosition = markdown.IndexOf("\n", position, StringComparison.Ordinal);
            if (endPosition == -1)
                endPosition = markdown.Length;
            var htmlString = RenderStringToHtml(markdown, position + 1, endPosition, delimiterValue, openTag, closeTag);
            if (endPosition != markdown.Length)
                return new RenderResult(markdown.Substring(0, position) + htmlString 
                    + markdown.Substring(endPosition), position + 2);
            return new RenderResult(markdown.Substring(0, position) + htmlString, position + 2);
        }

        private bool IsCorrectUnderscoreTag(string markdown, int position)
        {
            return rulesForSharp.Aggregate(true, (isDone, rule) => isDone && rule(markdown, position));
        }

        private bool WhiteSpacesAndNewLineAreCorrectRelativeSharp(string markdown, int position)
        {
            if (position == 0 && markdown[position + 1] == ' ')
                return true;
            if (position - 1 >= 0 && markdown[position - 1] == '\n' && markdown[position + 1] == ' ')
                return true;
            return false;
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