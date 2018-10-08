using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalReader = System.IO.StringReader;

namespace TaffyScript.IO
{
    [TaffyScriptObject]
    public class StringReader : TextReader
    {
        public override string ObjectType => "TaffyScript.IO.StringReader";
        public override System.IO.TextReader Reader => Source;

        public InternalReader Source { get; }

        public StringReader(InternalReader source)
        {
            Source = source;
        }

        public StringReader(TsObject[] args)
        {
            Source = new InternalReader((string)args[0]);
        }

        public static implicit operator TsObject(StringReader reader) => new TsInstanceWrapper(reader);
        public static explicit operator StringReader(TsObject obj) => (StringReader)obj.WeakValue;
    }
}
