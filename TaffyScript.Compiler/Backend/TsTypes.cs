using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using TaffyScript;

namespace TaffyScript.Compiler
{
    public class TsTypes
    {
        private static HashSet<string> _standardMethods = null;

        public static Dictionary<Type, MethodInfo> ObjectCasts { get; }
        public static Dictionary<Type, ConstructorInfo> Constructors { get; }
        public static Dictionary<string, Type> BasicTypes { get; }
        public static MethodInfo Empty { get; }
        public static HashSet<string> StandardMethods
        {
            get
            {
                if(_standardMethods is null)
                {
                    _standardMethods = new HashSet<string>()
                    {
                        "ToString",
                        "GetHashCode",
                        "Equals"
                    };
                }
                return _standardMethods;
            }
        }
        public static Type[] ArgumentTypes = new[] { typeof(TsObject[]) };

        static TsTypes()
        {
            var objType = typeof(TsObject);
            ObjectCasts = new Dictionary<Type, MethodInfo>()
            {
                { typeof(bool), objType.GetMethod("GetBool") },
                { typeof(char), objType.GetMethod("GetChar") },
                { typeof(byte), objType.GetMethod("GetByte") },
                { typeof(sbyte), objType.GetMethod("GetSByte") },
                { typeof(short), objType.GetMethod("GetShort") },
                { typeof(ushort), objType.GetMethod("GetUShort") },
                { typeof(int), objType.GetMethod("GetInt") },
                { typeof(uint), objType.GetMethod("GetUInt") },
                { typeof(long), objType.GetMethod("GetLong") },
                { typeof(ulong), objType.GetMethod("GetULong") },
                { typeof(float), objType.GetMethod("GetFloat") },
                { typeof(double), objType.GetMethod("GetDouble") },
                { typeof(string), objType.GetMethod("GetString") },
                { typeof(ITsInstance), objType.GetMethod("GetInstance") },
                { typeof(TsDelegate), objType.GetMethod("GetDelegate") },
                { typeof(TsObject[]), objType.GetMethod("GetArray", Type.EmptyTypes) }
            };

            Constructors = new Dictionary<Type, ConstructorInfo>()
            {
                { typeof(bool), typeof(TsNumber).GetConstructor(new[] { typeof(bool) }) },
                { typeof(byte), typeof(TsNumber).GetConstructor(new[] { typeof(byte) }) },
                { typeof(sbyte), typeof(TsNumber).GetConstructor(new[] { typeof(sbyte) }) },
                { typeof(short), typeof(TsNumber).GetConstructor(new[] { typeof(short) }) },
                { typeof(ushort), typeof(TsNumber).GetConstructor(new[] { typeof(ushort) }) },
                { typeof(int), typeof(TsNumber).GetConstructor(new[] { typeof(int) }) },
                { typeof(uint), typeof(TsNumber).GetConstructor(new[] { typeof(uint) }) },
                { typeof(long), typeof(TsNumber).GetConstructor(new[] { typeof(long) }) },
                { typeof(ulong), typeof(TsNumber).GetConstructor(new[] { typeof(ulong) }) },
                { typeof(float), typeof(TsNumber).GetConstructor(new[] { typeof(float) }) },
                { typeof(double), typeof(TsNumber).GetConstructor(new[] { typeof(double) }) },
                { typeof(char), typeof(TsString).GetConstructor(new[] { typeof(char) }) },
                { typeof(string), typeof(TsString).GetConstructor(new[] { typeof(string) }) },
                { typeof(ITsInstance), typeof(TsInstanceWrapper).GetConstructor(new[] { typeof(ITsInstance) }) },
                { typeof(TsObject[]), typeof(TsArray).GetConstructor(new[] { typeof(TsObject[]) }) }
            };

            BasicTypes = new Dictionary<string, Type>()
            {
                { "bool", typeof(bool) },
                { "char", typeof(char) },
                { "byte", typeof(byte) },
                { "sbyte", typeof(sbyte) },
                { "short", typeof(short) },
                { "ushort", typeof(ushort) },
                { "int", typeof(int) },
                { "uint", typeof(uint) },
                { "long", typeof(long) },
                { "ulong", typeof(ulong) },
                { "float", typeof(float) },
                { "double", typeof(double) },
                { "string", typeof(string) },
                { "instance", typeof(ITsInstance) },
                { "delegate", typeof(TsDelegate) },
                { "array", typeof(TsObject[]) },
                { "object", typeof(TsObject) }
            };

            Empty = objType.GetMethod("get_Empty");
        }
    }
}
