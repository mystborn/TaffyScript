using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser
{
    public class SyntaxToken : ISyntaxNode
    {
        public SyntaxNode Parent { get; set; }
        public string Value { get; }
        public string Type { get; }
        public bool IsToken => true;

        public SyntaxToken(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public SyntaxToken(SyntaxNode parent, string type, string value)
        {
            Parent = parent;
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Type}: {Value}";
        }
    }
}
