using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalWriter = System.IO.StringWriter;

namespace TaffyScript.IO
{
    [TaffyScriptObject]
    public class StringWriter : TextWriter
    {
        public override string ObjectType => "TaffyScript.IO.StringWriter";
        public override System.IO.TextWriter Writer => Source;

        public InternalWriter Source { get; }

        public StringWriter(InternalWriter writer)
        {
            Source = writer;
        }

        public StringWriter(TsObject[] args)
        {
            if (args is null)
                Source = new InternalWriter();

            switch(args.Length)
            {
                case 0:
                    Source = new InternalWriter();
                    break;
                case 1:
                    Source = new InternalWriter(((TaffyScript.Strings.StringBuilder)args[0]).Source);
                    break;
            }
        }
    }
}
