using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Delimiters
{
    public class DoubleUnderscoreDelimiter : IDoubleDelimiter
    {
        private readonly string name = "__";
        private readonly List<Func<string, int, bool>> rulesForClosedUnderscore;
        private readonly List<Func<string, int, bool>> rulesForOpenedUnderscore;
        private readonly DelimiterType type = DelimiterType.Double;

        public DoubleUnderscoreDelimiter()
        {
            rulesForOpenedUnderscore = new List<Func<string, int, bool>>
            {
                IsNotInsideStringWithDigits,
                WhiteSpacesIsCorrectRelativeOpendUnderscore
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

        private bool WhiteSpacesIsCorrectRelativeOpendUnderscore(string markdown, int position)
        {
            return position + name.Length < markdown.Length && markdown[position + name.Length] != ' ';
        }

        private bool WhiteSpacesIsCorrectRelativeClosedUnderscore(string markdown, int position)
        {
            return position - 1 >= 0 && markdown[position - 1] != ' ';
        }

        private bool IsNotInsideStringWithDigits(string markdown, int position)
        {
            if (position - 1 <= 0 || position + name.Length >= markdown.Length) return true;
            if (char.IsLetter(markdown[position - 1]) && char.IsDigit(markdown[position + name.Length]))
                return false;
            return !(char.IsDigit(markdown[position - 1]) && char.IsDigit(markdown[position + name.Length]));
        }
    }
}