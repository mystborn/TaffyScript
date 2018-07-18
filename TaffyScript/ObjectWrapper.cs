using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public abstract class ObjectWrapper
    {
        protected Dictionary<string, TsObject> _members = new Dictionary<string, TsObject>();
    }
}
