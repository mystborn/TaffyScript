﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.FrontEnd;

namespace TaffyScript
{
    public class InvalidTokenException : Exception
    {
        Token Token { get; }

        public InvalidTokenException(Token token)
            : base($"Encountered an invalid token: {token.Value} " + GetPosition(token.Position))
        {
            Token = token;
        }

        public InvalidTokenException(Token token, string message)
            : base(message + GetPosition(token.Position))
        {
            Token = token;
        }

        public InvalidTokenException(Token token, string message, Exception innerException)
            : base(message + GetPosition(token.Position), innerException)
        {
            Token = token;
        }

        private static string GetPosition(TokenPosition pos)
        {
            var sb = new StringBuilder();
            if (pos.File != null)
            {
                sb.Append("in file ");
                sb.Append(pos.File);
                sb.Append(" ");
            }
            sb.Append("at line ");
            sb.Append(pos.Line);
            sb.Append(", column ");
            sb.Append(pos.Column);
            return sb.ToString();
        }
    }
}