using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public struct InstanceEnumerator : IEnumerator<TsObject>
    {
        private IEnumerator<TsObject> _backingEnumerator;

        object IEnumerator.Current => _backingEnumerator.Current;

        public TsObject Current => _backingEnumerator.Current;

        public InstanceEnumerator(TsObject obj)
        {
            if (obj.Type == VariableType.Real)
            {
                var val = ((TsValue<float>)obj.Value).StrongValue;
                if (val == TsObject.All)
                    _backingEnumerator = TsInstance.Instances().GetEnumerator();
                else
                    _backingEnumerator = new List<TsObject>() { obj }.GetEnumerator();
            }
            else if (obj.Type == VariableType.String)
                _backingEnumerator = TsInstance.Instances(((TsValue<string>)obj.Value).StrongValue).GetEnumerator();
            else
                throw new InvalidOperationException("Can only enumerate on a string or real");
        }

        public InstanceEnumerator(float value)
        {
            if (value == TsObject.All)
                _backingEnumerator = TsInstance.Instances().GetEnumerator();
            else
                _backingEnumerator = new List<TsObject>() { new TsObject(value) }.GetEnumerator();
        }

        public bool MoveNext()
        {
            bool result;
            do
            {
                result = _backingEnumerator.MoveNext();
            }
            while (result == true && !TsInstance.InstanceExists(((TsValue<float>)_backingEnumerator.Current.Value).StrongValue));
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
