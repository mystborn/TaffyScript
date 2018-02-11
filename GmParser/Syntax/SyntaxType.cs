using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser
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
        ListAccess,
        GridAccess,
        MapAccess,
        ArrayAccess,
        ExplicitArrayAccess,
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
        Event,

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
