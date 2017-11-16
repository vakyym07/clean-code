using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Delimiters
{
    public class DoubleUnderscoreHandler : AbstractDelimiterHandler, IDelimiter
    {
        private readonly string closeTag = "<\\strong>";
        private readonly string openTag = "<strong>";
        private readonly string delimiterValue = "__";
        private readonly HashSet<string> ignoredDelimiters;
        private readonly List<Func<string, int, bool>> rulesForClosedUnderscore;
        private readonly List<Func<string, int, bool>> rulesForOpenedUnderscore;

        public DoubleUnderscoreHandler()
        {
            ignoredDelimiters = new HashSet<string>();
            rulesForOpenedUnderscore = new List<Func<string, int, bool>>
            {
                IsNotInsideStringWithDigits,
                WhiteSpacesIsCorrectRelativeOpendUnderscore
            };
            rulesForClosedUnderscore = new List<Func<string, int, bool>>
            {
                (markdown, pos) => markdown.Substring(pos, delimiterValue.Length) == delimiterValue,
                IsNotInsideStringWithDigits,
                WhiteSpacesIsCorrectRelativeClosedUnderscore
            };
        }

        public RenderResult RentderToHtml(string markdown, int position, Dictionary<string, IDelimiter> delimiters)
        {
            if (!IsCorrectUnderscoreTag(markdown, position))
                return new RenderResult(markdown, position + delimiterValue.Length);
            var specialTerminatedSymbols = delimiters.Keys.OrderByDescending(s => s.Length).ToList();
            return MdParse(markdown, position, delimiters, delimiterValue,
                ignoredDelimiters, specialTerminatedSymbols, openTag, closeTag);
        }

        public override string RemoveIgnoredTag(string markdown, int position, int countSign)
        {
            throw new NotImplementedException();
        }

        public override bool IsTerminatedMdTag(string markdown, int position)
        {
            return rulesForClosedUnderscore.Aggregate(true, (isDone, rule) => isDone && rule(markdown, position));
        }

        private bool IsCorrectUnderscoreTag(string markdown, int position)
        {
            return rulesForOpenedUnderscore.Aggregate(true, (isDone, rule) => isDone && rule(markdown, position));
        }

        private bool WhiteSpacesIsCorrectRelativeOpendUnderscore(string markdown, int position)
        {
            return position + delimiterValue.Length >= markdown.Length || markdown[position + delimiterValue.Length] != ' ';
        }

        private bool WhiteSpacesIsCorrectRelativeClosedUnderscore(string markdown, int position)
        {
            return position - 1 < 0 || markdown[position - 1] != ' ';
        }

        private bool IsNotInsideStringWithDigits(string markdown, int position)
        {
            if (position - 1 <= 0 || position + delimiterValue.Length >= markdown.Length) return true;
            if (char.IsLetter(markdown[position - 1]) && char.IsDigit(markdown[position + delimiterValue.Length]))
                return false;
            return !(char.IsDigit(markdown[position - 1]) && char.IsDigit(markdown[position + delimiterValue.Length]));
        }
    }
}