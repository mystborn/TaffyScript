using System.Collections.Generic;

namespace TaffyScript.Compiler.Syntax
{
    public class ObjectNode : SyntaxNode
    {
        public override SyntaxType Type => SyntaxType.Object;
        public string Name { get; }
        public ISyntaxElement Inherits { get; }
        public List<ObjectField> Fields { get; }
        public List<ObjectProperty> Properties { get; }
        public List<ScriptNode> Scripts { get; }

        public ObjectNode(string name, ISyntaxElement inherits, List<ObjectField> fields, List<ObjectProperty> properties, List<ScriptNode> scripts, TokenPosition position)
            : base(position)
        {
            Name = name;
            Inherits = inherits;
            Fields = fields;
            Properties = properties;
            Scripts = scripts;
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
