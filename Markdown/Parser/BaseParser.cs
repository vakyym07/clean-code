using System;
using System.Collections.Generic;

namespace Markdown.Parser
{
    public class BaseParser
    {
        private readonly string markdown;

        public BaseParser(string markdown)
        {
            this.markdown = markdown;
            Position = 0;
        }

        public string Source => markdown;

        public int Position { get; private set; }

        protected char this[int index] => index < markdown.Length ? markdown[index] : (char) 0;

        public char Current => this[Position];

        public bool End => Current == 0;

        public void Next()
        {
            if (!End)
                Position++;
        }

        public void Skip(int count)
        {
            Position += count;
        }

        protected string MatchNoExceptAndShiftPointer(IEnumerable<string> terms)
        {
            var position = Position;
            foreach (var str in terms)
            {
                var match = true;
                foreach (var chr in str)
                    if (Current == chr)
                        Next();
                    else
                    {
                        Position = position;
                        match = false;
                        break;
                    }
                if (match)
                    return str;
            }
            return null;
        }

        public bool IsMatch(IEnumerable<string> terms)
        {
            var position = Position;
            var result = MatchNoExceptAndShiftPointer(terms);
            Position = position;
            return result != null;
        }

        public bool IsMatchWithCondition(Func<char, bool> condition)
        {
            return condition(Current);
        }
    }
}