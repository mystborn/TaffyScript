using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public abstract class ObjectWrapper
    {
        private Dictionary<string, TsObject> _members = null;
        protected Dictionary<string, TsObject> Members
        {
            get
            {
                if (_members is null)
                    _members = new Dictionary<string, TsObject>();
                return _members;
            }
        }
    }
}
