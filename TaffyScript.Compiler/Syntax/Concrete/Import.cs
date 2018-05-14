using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ImportNode : SyntaxNode
    {
        public IConstantToken<string> ExternalName => Children[0] as IConstantToken<string>;
        public IConstantToken<string> InternalName => Children[Children.Count - 1] as IConstantToken<string>;
        public override SyntaxType Type => SyntaxType.Import;

        public ImportNode(string value, TokenPosition position) : base(value, position)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public List<IConstantToken<string>> GetArguments()
        {
            var result = new List<IConstantToken<string>>();
            for (var i = 1; i < Children.Count - 1; ++i)
                result.Add(Children[i] as IConstantToken<string>);

            return result;
        }
    }
}
