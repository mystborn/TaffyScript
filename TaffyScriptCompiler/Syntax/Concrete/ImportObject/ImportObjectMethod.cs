using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScriptCompiler.Syntax
{
    public class ImportObjectMethod
    {
        private List<string> _args;
        private List<IConstantToken<string>> _generics;

        public string ExternalName { get; }
        public string ImportName { get; set; }
        public TokenPosition Position { get; }

        public List<string> ArgumentTypes
        {
            get
            {
                if (_args == null)
                    _args = new List<string>();
                return _args;
            }
        }

        public List<IConstantToken<string>> Generics
        {
            get
            {
                if (_generics == null)
                    _generics = new List<IConstantToken<string>>();
                return _generics;
            }
        }

        public ImportObjectMethod(string name, TokenPosition position)
        {
            ImportName = ExternalName = name;
            Position = position;
        }

        public ImportObjectMethod(string externalName, string importName, TokenPosition position)
        {
            ExternalName = externalName;
            ImportName = importName;
            Position = position;
        }
    }
}
