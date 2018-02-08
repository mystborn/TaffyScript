namespace GmParser.Syntax
{
    public class ConstantToken<T> : SyntaxToken, IConstantToken<T>
    {
        private T _value;

        public T Value => _value;
        public ConstantType ConstantType { get; }
        public object WeakValue => _value;
        public override SyntaxType Type => SyntaxType.Constant;


        internal ConstantToken(string text, ConstantType type, T realValue) 
            : base(text)
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
