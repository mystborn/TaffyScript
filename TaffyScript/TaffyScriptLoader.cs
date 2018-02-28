using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TaffyScript
{
    public static class TaffyScriptLoader
    {
        public static void Load(string path)
        {
            var asm = Assembly.LoadFile(path);
            var name = asm.GetName().Name;
            var init = asm.GetType($"{name}.{name}_Initializer");
            init.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
        }
    }
}
