using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Delimiters
{
    public class SingleSharpDelimiter : ISingleDelimiter
    {
        private readonly string name = "#";
        private readonly DelimiterType type = DelimiterType.Single;
        private readonly List<Func<string, int, bool>> rulesForSharp;

        public SingleSharpDelimiter()
        {
            rulesForSharp = new List<Func<string, int, bool>>
            {
                WhiteSpacesAndNewLineAreCorrectRelativeSharp
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
            return rulesForSharp.Aggregate(true, (isDone, rule) => isDone && rule(markdown, position));
        }

        public bool IsEndOfVisibleArea(char chr)
        {
            return chr == '\n' || chr == 0;
        }

        private bool WhiteSpacesAndNewLineAreCorrectRelativeSharp(string markdown, int position)
        {
            if (position == 0 && markdown[position + 1] == ' ')
                return true;
            if (position - 1 >= 0 && markdown[position - 1] == '\n' && markdown[position + 1] == ' ')
                return true;
            return false;
        }
    }
}