﻿namespace GmParser.Syntax
{
    public class ContinueToken : SyntaxToken
    {
        public ContinueToken(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}