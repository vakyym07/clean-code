using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Delimiters
{
    public class SingleUnderscoreHandler :
        AbstractDelimiterHandler, IDelimiter
    {
        private readonly string closeTag = "<\\em>";
        private readonly string delimiterValue = "_";
        private readonly HashSet<string> ignoredDelimiters;
        private readonly string openTag = "<em>";
        private readonly List<Func<string, int, bool>> rulesForClosedUnderscore;
        private readonly List<Func<string, int, bool>> rulesForOpenedUnderscore;

        public SingleUnderscoreHandler()
        {
            rulesForOpenedUnderscore = new List<Func<string, int, bool>>
            {
                IsNotInsideStringWithDigits,
                WhiteSpacesIsCorrectRelativeOpendUnderscore,
            };
            rulesForClosedUnderscore = new List<Func<string, int, bool>>
            {
                (markdown, pos) => markdown[pos].ToString() == delimiterValue,
                IsNotInsideStringWithDigits,
                WhiteSpacesIsCorrectRelativeClosedUnderscore
            };
            ignoredDelimiters = new HashSet<string> {"__"};
        }

        public RenderResult RentderToHtml(string markdown, int position, Dictionary<string, IDelimiter> delimiters)
        {
            var specialTerminatedSymbols = delimiters.Keys.OrderByDescending(s => s.Length).ToList();
            if (!IsCorrectUnderscoreTag(markdown, position) ||
                !FindDelimiterWithCondition(markdown, delimiterValue, position + delimiterValue.Length, 
                specialTerminatedSymbols, IsTerminatedMdTag))
                return new RenderResult(markdown, position + delimiterValue.Length);
            return MdParse(markdown, position, delimiters, delimiterValue,
                    ignoredDelimiters, specialTerminatedSymbols, openTag, closeTag);
        }

        public override string RemoveIgnoredTag(string markdown, int position, int countSign)
        {
            return markdown.Remove(position, countSign);
        }

        public override bool IsTerminatedMdTag(string markdown, int position)
        {
            return rulesForClosedUnderscore.Aggregate(true, (isDone, rule) => isDone && rule(markdown, position));
        }

        private bool IsCorrectUnderscoreTag(string markdown, int position)
        {
            return rulesForOpenedUnderscore.Aggregate(true, (isDone, rule) => isDone && rule(markdown, position));
        }

        private static bool WhiteSpacesIsCorrectRelativeOpendUnderscore(string markdown, int position)
        {
            return position + 1 >= markdown.Length || markdown[position + 1] != ' ';
        }

        private static bool WhiteSpacesIsCorrectRelativeClosedUnderscore(string markdown, int position)
        {
            return position - 1 < 0 || markdown[position - 1] != ' ';
        }

        private static bool IsNotInsideStringWithDigits(string markdown, int position)
        {
            if (position - 1 <= 0 || position + 1 >= markdown.Length) return true;
            if (char.IsLetter(markdown[position - 1]) && char.IsDigit(markdown[position + 1]))
                return false;
            return !(char.IsDigit(markdown[position - 1]) && char.IsDigit(markdown[position + 1]));
        }
    }
}