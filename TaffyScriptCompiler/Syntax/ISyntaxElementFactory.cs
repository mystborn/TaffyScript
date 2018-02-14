using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler.Syntax
{
    public interface ISyntaxElementFactory
    {
        ISyntaxNode CreateNode(SyntaxType type, TokenPosition position);
        ISyntaxNode CreateNode(SyntaxType type, string value, TokenPosition position);
        ISyntaxToken CreateToken(SyntaxType type, string value, TokenPosition position);
        ISyntaxToken CreateConstant(ConstantType type, string value, TokenPosition position);
    }
}
