﻿namespace TaffyScriptCompiler.Syntax
{
    public class ArrayAccessNode : SyntaxNode
    {
        public ISyntaxElement Left => Children[0];
        public ISyntaxElement Right => Children[1];
        public override SyntaxType Type => SyntaxType.ArrayAccess;

        public ArrayAccessNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
