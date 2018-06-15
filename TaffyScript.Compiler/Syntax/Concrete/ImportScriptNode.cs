using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ImportScriptNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.ImportScript;
        public string DotNetType { get; }
        public string MethodName { get; }
        public string ImportName { get; }
        public List<string> Arguments { get; }

        public ImportScriptNode(string dotNetType, string methodName, string internalName, List<string> arguments, TokenPosition position)
            : base(position)
        {
            DotNetType = dotNetType;
            MethodName = methodName;
            ImportName = internalName;
            Arguments = arguments;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
