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
        public string Text { get; }

        public Token(TokenType type, string text, TokenPosition position)
        {
            Type = type;
            Text = text;
            Position = position;
        }

        public override string ToString()
        {
            return $"Token: Type {Type} | Text {Text} | Position {Position}";
        }
    }
}
