using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public struct InstanceEnumerator : IEnumerator<GmObject>
    {
        private IEnumerator<GmObject> _backingEnumerator;

        object IEnumerator.Current => _backingEnumerator.Current;

        public GmObject Current => _backingEnumerator.Current;

        public InstanceEnumerator(GmObject obj)
        {
            if (obj.Type == VariableType.Real)
            {
                var val = ((GmValue<float>)obj.Value).StrongValue;
                if (val == GmObject.All)
                    _backingEnumerator = GmInstance.Instances().GetEnumerator();
                else
                    _backingEnumerator = new List<GmObject>() { obj }.GetEnumerator();
            }
            else if (obj.Type == VariableType.String)
                _backingEnumerator = GmInstance.Instances(((GmValue<string>)obj.Value).StrongValue).GetEnumerator();
            else
                throw new InvalidOperationException("Can only enumerate on a string or real");
        }

        public InstanceEnumerator(float value)
        {
            if (value == GmObject.All)
                _backingEnumerator = GmInstance.Instances().GetEnumerator();
            else
                _backingEnumerator = new List<GmObject>() { new GmObject(value) }.GetEnumerator();
        }

        public bool MoveNext()
        {
            bool result;
            do
            {
                result = _backingEnumerator.MoveNext();
            }
            while (result == true && !GmInstance.InstanceExists(((GmValue<float>)_backingEnumerator.Current.Value).StrongValue));
            return result;
        }

        public void Reset()
        {
            _backingEnumerator.Reset();
        }

        public void Dispose()
        {
            _backingEnumerator.Dispose();
        }
    }
}
