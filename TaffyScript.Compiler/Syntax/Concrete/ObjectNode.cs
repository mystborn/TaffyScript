using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ObjectNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Object;
        public string Name { get; }
        public ISyntaxElement Inherits { get; }
        public List<ScriptNode> Scripts { get; }
        public List<ScriptNode> StaticScripts { get; }

        public ObjectNode(string name, ISyntaxElement inherits, List<ScriptNode> scripts, List<ScriptNode> staticScripts, TokenPosition position)
            : base(position)
        {
            Name = name;
            Inherits = inherits;
            Scripts = scripts;
            StaticScripts = staticScripts;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
