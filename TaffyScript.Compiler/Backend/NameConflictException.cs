using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Backend
{
    public class NameConflictException : Exception
    {
        public NameConflictException(string message)
            : base(message)
        {

        }
    }
}
