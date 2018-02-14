using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler.Backend
{
    public class NameConflictException : Exception
    {
        public NameConflictException(string message)
            : base(message)
        {

        }
    }
}
