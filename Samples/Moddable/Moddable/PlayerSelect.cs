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

        public string Name => _source["name"].GetString();
        public string PlayerObjectType => _source["player"].GetString();

        public PlayerSelect(string typeName)
        {
            _source = TsInstance.InstanceCreate(typeName).GetInstance();
        }

        public void Destroy()
        {
            TsInstance.InstanceDestroy(_source.Id);
        }
    }
}
