using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalReader = System.IO.StringReader;

namespace TaffyScript.IO
{
    /// <summary>
    /// Implements a TextReader that reads from a string.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stringreader?view=netframework-4.7</source>
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

        /// <summary>
        /// Initializes a new StringReader that reads from the specified string.
        /// </summary>
        /// <arg name="str" type="string">The string to read from.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stringreader.-ctor?view=netframework-4.7</source>
        public StringReader(TsObject[] args)
        {
            Source = new InternalReader((string)args[0]);
        }

        public static implicit operator TsObject(StringReader reader) => new TsInstanceWrapper(reader);
        public static explicit operator StringReader(TsObject obj) => (StringReader)obj.WeakValue;
    }
}
