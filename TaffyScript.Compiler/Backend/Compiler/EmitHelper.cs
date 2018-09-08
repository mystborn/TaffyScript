using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using TaffyScript.Compiler.Syntax;
using Number = System.Single;

namespace TaffyScript.Compiler.Backend
{
    public static class EmitHelper
    {
        public static void ConvertTopToObject(ILEmitter emit)
        {
            var top = emit.GetTop();
            if (!typeof(TsObject).IsAssignableFrom(top))
            {
                if (typeof(ITsInstance).IsAssignableFrom(top))
                    top = typeof(ITsInstance);
                emit.New(TsTypes.Constructors[top]);
            }
        }

        public static bool EmitConstant(ILEmitter emit, ISyntaxElement element)
        {
            switch (element)
            {
                case ReadOnlyToken readOnly:
                    if (readOnly.Name != "null")
                        return false;
                    emit.Call(TsTypes.Empty);
                    break;
                case IConstantToken<string> stringToken:
                    emit.LdStr(stringToken.Value);
                    break;
                case IConstantToken<bool> boolToken:
                    emit.LdFloat(boolToken.Value ? 1 : 0);
                    break;
                case IConstantToken<Number> numberToken:
                    emit.LdFloat(numberToken.Value);
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}
