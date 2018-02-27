using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace TaffyScriptIde
{
    public class Ide
    {
        public static void Main()
        {
            Application.Init();

            var window = new IdeWindow("TaffyScript");

            Application.Run();
        }
    }
}
