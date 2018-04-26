using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Exceptions
{
    public class MemberAccessException : Exception
    {
        public MemberAccessException()
            : base()
        {
        }

        public MemberAccessException(string message)
            : base(message)
        {
        }

        public MemberAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
