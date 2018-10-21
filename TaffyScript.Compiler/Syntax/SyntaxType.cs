using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public enum SyntaxType
    {
        //Node
        Root,
        Assign,
        Locals,
        Conditional,
        Logical,
        Bitwise,
        Equality,
        Relational,
        Shift,
        Additive,
        Multiplicative,
        Prefix,
        FunctionCall,
        MemberAccess,
        ArrayAccess,
        Postfix,
        ArrayLiteral,
        Declare,
        If,
        Switch,
        Case,
        Default,
        While,
        Repeat,
        Do,
        For,
        Return,
        Block,
        ImportScript,
        Enum,
        Script,
        ArgumentAccess,
        Object,
        Usings,
        Namespace,
        New,
        ImportObject,
        Lambda,
        Base,

        //Token
        Constant,
        Variable,
        End,
        Break,
        Continue,
        ReadOnly
    }
}
