using System.Collections.Generic;

namespace GmParser.Syntax
{
    public class ImportNode : SyntaxNode
    {
        public IConstantToken<string> ExternalName => Children[0] as IConstantToken<string>;
        public IEnumerable<IConstantToken<string>> Arguments
        {
            get
            {
                for (var i = 1; i < Children.Count - 1; ++i)
                    yield return Children[i] as IConstantToken<string>;
            }
        }
        public IConstantToken<string> InternalName => Children[Children.Count - 1] as IConstantToken<string>;

        public ImportNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
