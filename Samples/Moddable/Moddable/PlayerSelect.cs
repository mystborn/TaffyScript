using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript;

namespace Moddable
{
    public class PlayerSelect
    {
        private TsInstance _source;

        public string Name => (string)_source["name"];
        public string PlayerObjectType => (string)_source["player"];

        public PlayerSelect(string typeName)
        {
            _source = new TsInstance(typeName);
        }

        public void Destroy()
        {
            _source.Destroy();
            _source = null;
        }
    }
}
