using System;
using System.Collections.Generic;
using Markdown.Ast;
using Markdown.Delimiters;

namespace Markdown.Parser
{
    internal class DoubleDelimiterParser : IDelimiterParser
    {
        private readonly Dictionary<string, AstNodeType> astNodeTypes;
        private readonly Dictionary<int, int> correctDoubleDelimitersPositions;

        public DoubleDelimiterParser(Dictionary<int, int> correctDoubleDelimitersPositions,
            Dictionary<string, AstNodeType> astNodeTypes)
        {
            this.correctDoubleDelimitersPositions = correctDoubleDelimitersPositions;
            this.astNodeTypes = astNodeTypes;
        }

        public AstNode GetAstNode(IDelimiter delimiter, int position)
        {
            return AstDoubleDelimiter(delimiter, position);
        }

        public Func<int, bool> GetStopFunction(IDelimiter delimiter, string mardown, int position)
        {
            return (pos) => pos == correctDoubleDelimitersPositions[position];
        }

        private AstNode AstDoubleDelimiter(IDelimiter delimiter, int position)
        {
            if (delimiter.GetDelimiterType() == DelimiterType.Double)
            {
                if (correctDoubleDelimitersPositions.ContainsKey(position))
                    return new AstNode(astNodeTypes[delimiter.GetName()], delimiter.GetName());
                return new AstNode(AstNodeType.Text, delimiter.GetName());
            }
            return null;
        }
    }
}