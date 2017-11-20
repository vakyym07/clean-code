using System;
using Markdown.Ast;
using Markdown.Delimiters;

namespace Markdown.Parser
{
    public interface IDelimiterParser
    {
        AstNode GetAstNode(IDelimiter delimiter, int position);
        Func<int, bool> GetStopFunction(IDelimiter delimiter, string markdown, int position);
    }
}