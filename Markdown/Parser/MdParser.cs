using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Ast;
using Markdown.Delimiters;
using Markdown.MarkdownAnalizator;

namespace Markdown.Parser
{
    public class MdParser : BaseParser
    {
        private readonly Dictionary<string, AstNodeType> astNodeTypes;
        private readonly Dictionary<string, IDelimiter> delimiters;
        private readonly List<IDelimiterParser> parsers;

        public MdParser(Dictionary<string, IDelimiter> delimiters, string markdown) :
            base(markdown)
        {
            this.delimiters = delimiters;
            astNodeTypes = new Dictionary<string, AstNodeType>
            {
                {"_", AstNodeType.SingleUnderscore},
                {"__", AstNodeType.DoubleUnderscore},
                {"\\", AstNodeType.Escape},
                {"#", AstNodeType.SingleShrap}
            };
            parsers = new List<IDelimiterParser>
            {
                new DoubleDelimiterParser(new MdAnalyzator(markdown, delimiters).FindCorrectDoubleDelimiters(), astNodeTypes),
                new SingleDelimiterParser(astNodeTypes)
            };
        }

        public AstNode Parse()
        {
            var delimitersName = delimiters.Select(p => p.Value.GetName())
                .OrderByDescending(s => s.Length).ToList();
            var root = new AstNode(AstNodeType.Root, string.Empty);
            return MdParse(delimitersName, root, (pos) => pos >= Source.Length);
        }

        private AstNode MdParse(List<string> delimitersNames, AstNode currentNode, Func<int, bool> interruptFunction)
        {
            var skip = false;
            while (!interruptFunction(Position))
            {
                if (!IsMatch(delimitersNames))
                    currentNode.AddChild(AstTextAndShiftPointer(delimitersNames));
                if (interruptFunction(Position))
                {
                    Skip(currentNode.Text.Length);
                    break;
                }
                var currentDelimiter = delimiters[MatchNoExceptAndShiftPointer(delimitersNames)];
                var delimiterPosition = Position - currentDelimiter.GetName().Length;
                if (!skip && currentDelimiter.IsCorrectDelimiter(Source, delimiterPosition))
                {
                    if (currentDelimiter.GetDelimiterType() == DelimiterType.Escape)
                    {
                        skip = true;
                        continue;
                    }

                    foreach (var delimiterParser in parsers)
                    {
                        var astNode = delimiterParser.GetAstNode(currentDelimiter, delimiterPosition);
                        if (astNode == null) continue;
                        if (astNode.Type == AstNodeType.Text)
                            currentNode.AddChild(astNode);
                        else
                        {
                            var stopFunction =
                                delimiterParser.GetStopFunction(currentDelimiter, Source, delimiterPosition);
                            currentNode.AddChild(MdParse(delimitersNames, astNode, stopFunction));
                        }
                        break;
                    }
                }
                else
                {
                    skip = false;
                    currentNode.AddChild(new AstNode(AstNodeType.Text, currentDelimiter.GetName()));
                }
            }
            return currentNode;
        }

        private AstNode AstTextAndShiftPointer(List<string>delimitersNames)
        {
            var buffer = new StringBuilder();
            while (!IsMatch(delimitersNames) && !End)
            {
                buffer.Append(Current);
                Next();
            }
            return new AstNode(AstNodeType.Text, buffer.ToString());
        }
    }
}