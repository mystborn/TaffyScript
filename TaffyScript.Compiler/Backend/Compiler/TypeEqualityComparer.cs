using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace TaffyScript.Compiler.Backend
{
    // These specialized classes generate a hash code based off of a
    // Types name. These are necessary because a TypeBuilders 
    // hashcode changes when CreateType is called. In order to
    // lookup the type in a dictionary, this custom implementation
    // is needed.

    public class TypeEqualityComparer : IEqualityComparer<Type>
    {
        private static TypeEqualityComparer _single = new TypeEqualityComparer();

        public static TypeEqualityComparer Single => _single;

        public bool Equals(Type x, Type y)
        {
            return x == y;
        }

        public int GetHashCode(Type obj)
        {
            return obj.FullName.GetHashCode();
        }
    }

    public class TypeBuilderEqualityComparer : IEqualityComparer<TypeBuilder>
    {
        private static TypeBuilderEqualityComparer _single = new TypeBuilderEqualityComparer();

        public static TypeBuilderEqualityComparer Single => _single;

        public bool Equals(TypeBuilder x, TypeBuilder y)
        {
            return x == y;
        }

        public int GetHashCode(TypeBuilder obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}
