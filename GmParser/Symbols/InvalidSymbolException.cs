using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
