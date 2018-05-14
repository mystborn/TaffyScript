using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using TaffyScript;

namespace TaffyScriptCompiler
{
    public class TsTypes
    {
        public static Dictionary<Type, MethodInfo> ObjectCasts { get; }
        public static Dictionary<Type, ConstructorInfo> Constructors { get; }
        public static Dictionary<string, Type> BasicTypes { get; }
        public static MethodInfo Empty { get; }

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
                { typeof(TsInstance), objType.GetMethod("GetInstance") },
                { typeof(TsObject[]), objType.GetMethod("GetArray1D") },
                { typeof(TsObject[][]), objType.GetMethod("GetArray2D") }
            };

            Constructors = new Dictionary<Type, ConstructorInfo>()
            {
                { typeof(bool), objType.GetConstructor(new[] { typeof(bool) }) },
                { typeof(char), objType.GetConstructor(new[] { typeof(char) }) },
                { typeof(byte), objType.GetConstructor(new[] { typeof(byte) }) },
                { typeof(sbyte), objType.GetConstructor(new[] { typeof(sbyte) }) },
                { typeof(short), objType.GetConstructor(new[] { typeof(short) }) },
                { typeof(ushort), objType.GetConstructor(new[] { typeof(ushort) }) },
                { typeof(int), objType.GetConstructor(new[] { typeof(int) }) },
                { typeof(uint), objType.GetConstructor(new[] { typeof(uint) }) },
                { typeof(long), objType.GetConstructor(new[] { typeof(long) }) },
                { typeof(ulong), objType.GetConstructor(new[] { typeof(ulong) }) },
                { typeof(float), objType.GetConstructor(new[] { typeof(float) }) },
                { typeof(double), objType.GetConstructor(new[] { typeof(double) }) },
                { typeof(string), objType.GetConstructor(new[] { typeof(string) }) },
                { typeof(TsInstance), objType.GetConstructor(new[] { typeof(TsInstance) }) },
                { typeof(TsObject[]), objType.GetConstructor(new[] { typeof(TsObject[]) }) },
                { typeof(TsObject[][]), objType.GetConstructor(new[] { typeof(TsObject[][]) }) }
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
                { "instance", typeof(TsInstance) },
                { "array1d", typeof(TsObject[]) },
                { "array2d", typeof(TsObject[][]) },
                { "object", typeof(TsObject) }
            };

            Empty = objType.GetMethod("Empty");
        }
    }
}
