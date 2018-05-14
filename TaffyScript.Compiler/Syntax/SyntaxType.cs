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
        With,
        Do,
        For,
        Return,
        Block,
        Import,
        Enum,
        Script,
        ArgumentAccess,
        Object,
        Usings,
        Namespace,
        New,
        ImportObject,
        Lambda,

        //Token
        Constant,
        Variable,
        End,
        Break,
        Continue,
        Exit,
        ReadOnlyValue
    }
}
