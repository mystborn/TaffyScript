using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public class InvalidTsTypeException : Exception
    {
        public InvalidTsTypeException(string message)
            : base(message)
        {
        }

        public InvalidTsTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
