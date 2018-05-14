using System;

namespace TaffyScript.Compiler
{
    public class InvalidSymbolException : Exception
    {
        public InvalidSymbolException()
            : base()
        {
        }

        public InvalidSymbolException(string message)
            : base(message)
        {
        }

        public InvalidSymbolException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
