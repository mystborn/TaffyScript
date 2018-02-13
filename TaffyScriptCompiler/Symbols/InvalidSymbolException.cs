using System;

namespace TaffyScript
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
