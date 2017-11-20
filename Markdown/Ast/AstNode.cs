using System.Collections.Generic;

namespace Markdown.Ast
{
    public class AstNode
    {

        public AstNode(AstNodeType type, string text)
        {
            Type = type;
            Text = text;
            Childs = new List<AstNode>();
        }

        public List<AstNode> Childs { get; }

        public AstNodeType Type { get; }
        public string Text { get; }

        public void AddChild(AstNode child)
        {
            Childs.Add(child);
        }

        public int CountChild()
        {
            return Childs.Count;
        }
    }
}