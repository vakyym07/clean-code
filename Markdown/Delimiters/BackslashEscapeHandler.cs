using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Delimiters
{
    public class BackslashEscapeHandler : IDelimiter
    {
        private readonly string delimiterValue = "\\";

        public RenderResult RentderToHtml(string markdown, int position, Dictionary<string, IDelimiter> delimiters)
        {
            if (IsEscapeSymbol(markdown, position, delimiters.Keys.ToList()))
                return new RenderResult(markdown.Remove(position, delimiterValue.Length), position + 1);
            return new RenderResult(markdown, position + delimiterValue.Length);
        }

        private bool IsEscapeSymbol(string markdown, int position, List<string>delimiters)
        {
            if (position + 1 < markdown.Length && 
                delimiters.Contains(markdown[position + 1].ToString()))
                return true;
            return false;
        }
    }
}