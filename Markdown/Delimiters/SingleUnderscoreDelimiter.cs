using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Delimiters
{
    public class SingleUnderscoreDelimiter : IDoubleDelimiter
    {
        private readonly List<Func<string, int, bool>> rulesForClosedUnderscore;
        private readonly List<Func<string, int, bool>> rulesForOpenedUnderscore;
        private readonly string name = "_";
        private readonly DelimiterType type = DelimiterType.Double;

        public SingleUnderscoreDelimiter()
        {
            rulesForOpenedUnderscore = new List<Func<string, int, bool>>
            {
                IsNotInsideStringWithDigits,
                WhiteSpacesIsCorrectRelativeOpendUnderscore,
            };
            rulesForClosedUnderscore = new List<Func<string, int, bool>>
            {
                IsNotInsideStringWithDigits,
                WhiteSpacesIsCorrectRelativeClosedUnderscore
            };
        }

        public string GetName()
        {
            return name;
        }

        public DelimiterType GetDelimiterType()
        {
            return type;
        }
        
        public bool IsCorrectDelimiter(string markdown, int position)
        {
            return IsCorrectOpenedDelimiter(markdown, position) || IsCorrectClosedDelimiter(markdown, position);
        }

        public bool IsCorrectOpenedDelimiter(string markdown, int position)
        {
            return rulesForOpenedUnderscore.Aggregate(true, (isDone, rule) => isDone && rule(markdown, position));
        }

        public bool IsCorrectClosedDelimiter(string markdown, int position)
        {
            return rulesForClosedUnderscore.Aggregate(true, (isDone, rule) => isDone && rule(markdown, position));
        }

        private static bool WhiteSpacesIsCorrectRelativeOpendUnderscore(string markdown, int position)
        {
            return position + 1 < markdown.Length && markdown[position + 1] != ' ';
        }

        private static bool WhiteSpacesIsCorrectRelativeClosedUnderscore(string markdown, int position)
        {
            return position - 1 >= 0 && markdown[position - 1] != ' ';
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