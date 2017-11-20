using System.Collections.Generic;
using System.Text;
using Markdown.DelimitersHandlers;

namespace Markdown.Ast
{
    public class AstRenderToHtml
    {
        private readonly Dictionary<AstNodeType, IDelimiterHandler> handlers;
        private readonly AstNode root;

        public AstRenderToHtml(AstNode root)
        {
            this.root = root;
            handlers = new Dictionary<AstNodeType, IDelimiterHandler>
            {
                {AstNodeType.SingleUnderscore, new SingleUnderscoreHandler()},
                {AstNodeType.DoubleUnderscore, new DoubleUndescoreHandler()},
                {AstNodeType.SingleShrap, new SharpHandler()}
            };
        }

        public string RenderToHtml()
        {
            return RecursiveDescent(root, false);
        }

        private string RecursiveDescent(AstNode currentNode, bool cancelAction)
        {
            var buffer = new StringBuilder();
            if (currentNode.CountChild() != 0)
                foreach (var currentNodeChild in currentNode.Childs)
                {
                    if (currentNodeChild.Type == AstNodeType.Text)
                        buffer.Append(currentNodeChild.Text);
                    else
                    {
                        if (handlers.ContainsKey(currentNode.Type))
                            buffer.Append(RecursiveDescent(currentNodeChild,
                                handlers[currentNode.Type].IsIgnoredDelimiter(currentNodeChild.Text)));
                        else buffer.Append(RecursiveDescent(currentNodeChild, false));
                    }
                }
            if (!cancelAction && handlers.ContainsKey(currentNode.Type))
                return handlers[currentNode.Type].RenderToHtml(buffer.ToString());
            return buffer.ToString();
        }
    }
}