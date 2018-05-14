using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public interface IErrorLogger
    {
        List<string> Errors { get; }
        List<string> Warnings { get; }
        void Error(string message, TokenPosition position);
        void Warning(string message, TokenPosition position);
    }
}
