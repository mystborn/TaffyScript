using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Numbers
{
    /// <summary>
    /// Contains static mathematical constants.
    /// </summary>
    [TaffyScriptObject]
    public class MathConstants : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.Numbers.MathConstants";

        /// <summary>
        /// Represents the ratio of the circumference of a circle to its diameter.
        /// </summary>
        public static TsObject pi => Math.PI;

        /// <summary>
        /// Represents the natural logarithmic base.
        /// </summary>
        public static TsObject e => Math.E;

        private MathConstants() { }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            throw new NotImplementedException();
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            throw new NotImplementedException();
        }

        public TsObject GetMember(string name)
        {
            throw new NotImplementedException();
        }

        public void SetMember(string name, TsObject value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            throw new NotImplementedException();
        }
    }
}
