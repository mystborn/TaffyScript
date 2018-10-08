using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.IO
{
    public class WrappedStream : TsStream
    {
        public override string ObjectType => "TaffyScript.IO.WrappedStream";
        public override Stream Stream { get; }

        public WrappedStream(Stream stream)
        {
            Stream = stream;
        }

        public static implicit operator TsObject(WrappedStream stream) => new TsInstanceWrapper(stream);
        public static explicit operator WrappedStream(TsObject obj) => (WrappedStream)obj.WeakValue;
    }
}
