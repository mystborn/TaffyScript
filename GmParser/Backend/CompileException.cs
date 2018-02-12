using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Backend
{
    public class CompileException : Exception
    {
        public CompileException(string message)
            : base(message)
        {
        }
    }
}
