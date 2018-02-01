using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public class InvalidGmTypeException : Exception
    {
        public InvalidGmTypeException(string message)
            : base(message)
        {
        }

        public InvalidGmTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
