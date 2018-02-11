using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmParser.FrontEnd;

namespace GmParser
{
    public class InvalidTokenException : Exception
    {
        Token Token { get; }

        public InvalidTokenException(Token token)
            : base("Encountered an invalid token.")
        {
            Token = token;
        }

        public InvalidTokenException(Token token, string message)
            : base(message)
        {
            Token = token;
        }

        public InvalidTokenException(Token token, string message, Exception innerException)
            : base(message, innerException)
        {
            Token = token;
        }
    }
}
