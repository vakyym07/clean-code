using System;
using System.Collections.Generic;
using Markdown.Ast;
using Markdown.Delimiters;

namespace Markdown.Parser
{
    internal class SingleDelimiterParser : IDelimiterParser
    {
        private readonly Dictionary<string, AstNodeType> astNodeTypes;

        public SingleDelimiterParser(Dictionary<string, AstNodeType> astNodeTypes)
        {
            this.astNodeTypes = astNodeTypes;
        }

        public AstNode GetAstNode(IDelimiter delimiter, int position)
        {
            if (delimiter.GetDelimiterType() == DelimiterType.Single)
                return new AstNode(astNodeTypes[delimiter.GetName()], delimiter.GetName());
            return null;
        }

        public Func<int, bool> GetStopFunction(IDelimiter delimiter, string markdown, int position)
        {
            return (pos) =>
            {
                if (pos < markdown.Length)
                    return (delimiter as ISingleDelimiter).IsEndOfVisibleArea(markdown[pos]);
                return (delimiter as ISingleDelimiter).IsEndOfVisibleArea((char) 0);
            };
        }
    }
}