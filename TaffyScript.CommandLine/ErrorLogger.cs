using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Compiler;

namespace TaffyScript.CommandLine
{
    public class ErrorLogger : IErrorLogger
    {
        public List<string> Errors { get; } = new List<string>();
        public List<string> Warnings { get; } = new List<string>();

        public void Error(string message)
        {
            Errors.Add(message);
        }

        public void Error(string message, TokenPosition position)
        {
            if(position != null)
            {
                var sb = new StringBuilder(message);
                if(position.File != null)
                {
                    sb.Append(" in file ");
                    sb.Append(position.File);
                }
                sb.Append(" at line ");
                sb.Append(position.Line);
                sb.Append(", column ");
                sb.Append(position.Column);
                message = sb.ToString();
            }
            Errors.Add(message);
        }

        public void Warning(string message)
        {
            Warnings.Add(message);
        }

        public void Warning(string message, TokenPosition position)
        {
            if (position != null)
            {
                var sb = new StringBuilder(message);
                if (position.File != null)
                {
                    sb.Append(" in file ");
                    sb.Append(position.File);
                }
                sb.Append(" at line ");
                sb.Append(position.Line);
                sb.Append(", column ");
                sb.Append(position.Column);
                message = sb.ToString();
            }
            Warnings.Add(message);
        }
    }
}
