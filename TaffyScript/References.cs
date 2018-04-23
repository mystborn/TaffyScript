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
        private static FastList<WeakReference<TsInstance>> _instances = new FastList<WeakReference<TsInstance>>();
        internal static FastList<WeakReference<TsInstance>> Instances => _instances;

        public static void Register(TsInstance inst)
        {
            _instances.Add(new WeakReference<TsInstance>(inst));
        }

        public static IEnumerator<TsInstance> EnumerateType(string type)
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

        public static IEnumerator<TsInstance> EnumerateAll()
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