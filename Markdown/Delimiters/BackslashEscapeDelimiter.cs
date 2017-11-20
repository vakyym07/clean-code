using System.Collections.Generic;

namespace Markdown.Delimiters
{
    public class BackslashEscapeDelimiter : IDelimiter
    {
        private readonly List<string> delimiters;
        private readonly string name = "\\";
        private readonly DelimiterType type = DelimiterType.Escape;

        public BackslashEscapeDelimiter(List<string> delimiters)
        {
            this.delimiters = delimiters;
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
            return IsEscapeSymbol(markdown, position);
        }

        private bool IsEscapeSymbol(string markdown, int position)
        {
            if (position + 1 < markdown.Length &&
                delimiters.Contains(markdown[position + 1].ToString()))
                return true;
            return false;
        }
    }
}