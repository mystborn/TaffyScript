﻿namespace GmParser.Syntax
{
    public class ConstantToken<T> : SyntaxToken, IConstantToken<T>
    {
        private T _value;

        public T Value => _value;
        public ConstantType ConstantType { get; }
        public object WeakValue => _value;


        internal ConstantToken(string text, ConstantType type, T realValue, SyntaxNode parent = null) 
            : base(parent, SyntaxType.Constant, text)
        {
            ConstantType = type;
            _value = realValue;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}