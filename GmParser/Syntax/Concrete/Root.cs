using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser.Syntax
{
    public class RootNode : SyntaxNode
    {
        public RootNode(SyntaxType type, string value) : base(type, value)
        {
        }

        public override void Accept(ISyntaxElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
