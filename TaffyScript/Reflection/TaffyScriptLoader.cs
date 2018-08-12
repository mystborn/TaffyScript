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
        /// <summary>
        /// Loads a TaffyScript from the specified path and initializes it.
        /// </summary>
        /// <param name="path"></param>
        public static void Load(string path)
        {
            var asm = Assembly.LoadFile(path);

            if (asm.GetCustomAttribute<TaffyScriptLibraryAttribute>() == null)
                throw new InvalidOperationException("Tried to load a non TaffyScript library.");

            var name = asm.GetName().Name;
            var init = asm.GetType($"{name}.{name}_Initializer");
            init.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
        }
    }
}
