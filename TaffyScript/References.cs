#if KeepRef

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;

namespace TaffyScript
{
    public static class References
    {
        private static FastList<WeakReference<ITsInstance>> _instances = new FastList<WeakReference<ITsInstance>>(100000);
        internal static FastList<WeakReference<ITsInstance>> Instances => _instances;

        public static void Register(TsInstance inst)
        {
            var wr = new WeakReference<ITsInstance>(inst);
            inst.Destroyed += (i) => wr.SetTarget(null);
            _instances.Add(wr);
        }

        public static IEnumerator<ITsInstance> GetEnumerator(TsObject val)
        {
            if (val.Type == VariableType.String)
                return EnumerateType(val.GetStringUnchecked());
            else if (val.Type == VariableType.Instance)
                return new List<ITsInstance>() { val.GetInstance() }.GetEnumerator();
            else if (val.Type == VariableType.Real && val.GetFloatUnchecked() == TsObject.All)
                return EnumerateAll();
            else
                throw new ArgumentException($"Could not enumerate over the value: {val}", nameof(val));
        }

        public static IEnumerator<ITsInstance> EnumerateType(string type)
        {
            //Enumerate backwards so that the list can safely remove.
            for(var i = _instances.Count - 1; i > -1; --i)
            {
                if (_instances.Buffer[i].TryGetTarget(out var inst))
                {
                    if(inst.ObjectType == type)
                        yield return inst;
                }
                else
                    _instances.RemoveAt(i);
            }
        }

        public static IEnumerator<ITsInstance> EnumerateAll()
        {
            //Enumerate backwards so that the list can safely remove.
            for (var i = _instances.Count - 1; i > -1; --i)
            {
                if (_instances.Buffer[i].TryGetTarget(out var inst))
                {
                    yield return inst;
                }
                else
                    _instances.RemoveAt(i);
            }
        }
    }
}
#endif