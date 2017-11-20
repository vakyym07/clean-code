using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Delimiters;
using Markdown.Parser;

namespace Markdown.MarkdownAnalizator
{
    public class MdAnalyzator : BaseParser

    {
        private readonly Dictionary<string, IDelimiter> delimiters;

        public MdAnalyzator(string markdown, Dictionary<string, IDelimiter> delimiters)
            : base(markdown)
        {
            this.delimiters = delimiters;
        }

        public Dictionary<int, int> FindCorrectDoubleDelimiters()
        {
            var stack = new Stack<Tuple<string, int>>();
            var doubleDelimitersPosition = new Dictionary<int, int>();
            var doubleDelimiters = delimiters.Where(p => p.Value.GetDelimiterType() == DelimiterType.Double)
                .Select(p => p.Value.GetName())
                .OrderByDescending(s => s.Length)
                .ToList();

            while (!End)
            {
                var currentDelimiter = MatchNoExceptAndShiftPointer(doubleDelimiters);
                if (currentDelimiter != null)
                {
                    var delimiter = delimiters[currentDelimiter] as IDoubleDelimiter;
                    var delimiterPosition = Position - currentDelimiter.Length;
                    if (delimiter == null || !delimiter.IsCorrectDelimiter(Source, delimiterPosition))
                        continue;
                    if (delimiter.IsCorrectOpenedDelimiter(Source, delimiterPosition))
                    {
                        stack.Push(Tuple.Create(delimiter.GetName(), delimiterPosition));
                        continue;
                    }
                    if (delimiter.IsCorrectClosedDelimiter(Source, delimiterPosition))
                        if (stack.Count != 0 && stack.Peek().Item1 == delimiter.GetName())
                        {
                            doubleDelimitersPosition[stack.Pop().Item2] = delimiterPosition;
                        }
                }
                else Next();
            }
            return doubleDelimitersPosition;
        }
    }
}