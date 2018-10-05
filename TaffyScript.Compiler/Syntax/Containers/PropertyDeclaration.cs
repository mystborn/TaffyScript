using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler.Syntax
{
    public class ObjectProperty
    {
        public string Name { get; }
        public bool IsStatic { get; }
        public TokenPosition Position { get; }
        public ScriptNode GetScript { get; }
        public ScriptNode SetScript { get; }
        public bool CanRead => GetScript != null;
        public bool CanWrite => SetScript != null;

        public ObjectProperty(string name, bool isStatic, TokenPosition position, ScriptNode getScript, ScriptNode setScript)
        {
            Name = name;
            IsStatic = isStatic;
            Position = position;
            GetScript = getScript;
            SetScript = setScript;
        }
    }
}
