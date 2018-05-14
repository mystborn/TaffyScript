using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.FrontEnd
{
    public class Token
    {
        /// <summary>
        /// The location within the stream that this token was found.
        /// </summary>
        public TokenPosition Position { get; }

        /// <summary>
        /// The type of this token.
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// The value of this token.
        /// </summary>
        public string Value { get; }

        public Token(TokenType type, string value, TokenPosition position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public override string ToString()
        {
            return $"Token: Type {Type} | Value {Value} | Position {Position}";
        }
    }
}
