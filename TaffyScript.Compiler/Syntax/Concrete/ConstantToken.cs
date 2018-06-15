namespace TaffyScript.Compiler.Syntax
{
    public class ConstantToken<T> : SyntaxToken, IConstantToken<T>
    {
        public override SyntaxType Type => SyntaxType.Constant;

        public override string Name { get; }
        public T Value { get; }
        public ConstantType ConstantType { get; }
        public object WeakValue => Value;


        public ConstantToken(string name, T realValue, ConstantType type, TokenPosition position)
            : base(position)
        {
            Name = name;
            ConstantType = type;
            Value = realValue;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
