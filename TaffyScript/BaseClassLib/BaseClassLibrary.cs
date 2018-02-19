using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public static partial class Bcl
    {
        public static Random Rng = new Random(123456789);
        public static int RandomSeed { get; private set; } = 123456789;

        [WeakMethod]
        public static TsObject ToString(TsObject[] args)
        {
            if (args[0].Type != VariableType.String)
                return new TsObject(args[0].ToString());
            else
                return args[0];
        }

        [WeakMethod]
        public static TsObject AngleDifference(TsObject[] args)
        {
            var result = args[0].GetNum() - args[1].GetNum();
            if (result > 180)
            {
                do
                {
                    result -= 180;
                }
                while (result > 180);
            }
            else if (result < -180)
            {
                do
                {
                    result += 180;
                }
                while (result > 180);
            }
            return new TsObject(result);
        }

        [WeakMethod]
        public static TsObject ArcCos(TsObject[] args)
        {
            return new TsObject((float)Math.Acos(args[0].GetNum()));
        }

        [WeakMethod]
        public static TsObject ArcSin(TsObject[] args)
        {
            return new TsObject((float)Math.Asin(args[0].GetNum()));
        }

        [WeakMethod]
        public static TsObject ArcTan(TsObject[] args)
        {
            return new TsObject((float)Math.Atan(args[0].GetNum()));
        }

        [WeakMethod]
        public static TsObject ArcTan2(TsObject[] args)
        {
            return new TsObject((float)Math.Atan2(args[0].GetNum(), args[1].GetNum()));
        }

        [WeakMethod]
        public static TsObject ArrayCopy(TsObject[] args)
        {
            var destIndex = args[1].GetNumAsInt();
            var srcIndex = args[3].GetNumAsInt();
            var length = args[4].GetNumAsInt();
            if (args[0].Type == VariableType.Array1)
            {
                var destWrapper = args[0].Value as TsValueArray<TsObject[]> ?? throw new ArgumentException("Can only copy 1D arrays", "dest");
                var srcWrapper = args[2].Value as TsValueArray<TsObject[]> ?? throw new ArgumentException("Can only copy 1D arrays", "src");
                var dest = destWrapper.StrongValue;
                var src = srcWrapper.StrongValue;
                if (destIndex + length >= dest.Length)
                {
                    var temp = new TsObject[destIndex + length];
                    Buffer.BlockCopy(dest, 0, temp, 0, dest.Length);
                    dest = temp;
                    destWrapper.StrongValue = dest;
                }
                Buffer.BlockCopy(src, srcIndex, dest, destIndex, length);
            }
            else
                throw new ArgumentException("Can only copy 1D arrays", "dest");
            return TsObject.Empty();
        }

        [WeakMethod]
        public static TsObject ArrayCreate(TsObject[] args)
        {
            var size = args[0].GetNumAsInt();
            var value = TsObject.Empty();
            if (args.Length > 1)
                value = args[1];
            var result = new TsObject[size];
            for (var i = 0; i < size; ++i)
                result[i] = value;

            return new TsObject(result);
        }

        [WeakMethod]
        public static TsObject ArrayEquals(TsObject[] args)
        {
            var var1 = (args[0].Value as TsValueArray<TsObject[]> ?? throw new ArgumentException("Can only compare 1D arrays", "var1")).StrongValue;
            var var2 = (args[1].Value as TsValueArray<TsObject[]> ?? throw new ArgumentException("Can only compare 1D arrays", "var2")).StrongValue;
            if (var1.Length != var2.Length)
                return new TsObject(false);

            for (var i = 0; i < var1.Length; ++i)
            {
                if (var1[i] != var2[i])
                    return new TsObject(false);
            }
            return new TsObject(true);
        }

        [WeakMethod]
        public static TsObject ArrayHeight2D(TsObject[] args)
        {
            var arrayIndex = (args[0].Value as TsValueArray<TsObject[][]> ?? throw new ArgumentException($"Expected 2D array, got {args[0].Type}.", "array_index")).StrongValue;
            return new TsObject(arrayIndex.Length);
        }

        [WeakMethod]
        public static TsObject ArrayLength1D(TsObject[] args)
        {
            var arrayIndex = (args[0].Value as TsValueArray<TsObject[]> ?? throw new ArgumentException($"Expected 1D array, got {args[0].Type}.", "array_index")).StrongValue;
            return new TsObject(arrayIndex.Length);
        }

        [WeakMethod]
        public static TsObject ArrayLength2D(TsObject[] args)
        {
            var arrayIndex = (args[0].Value as TsValueArray<TsObject[][]> ?? throw new ArgumentException($"Expected 2D array, got {args[0].Type}.", "array_index")).StrongValue;
            if (args[1].Type != VariableType.Real)
                throw new ArgumentException($"Expected real value, got {args[1].Type}");
            var n = (int)args[1].GetNumUnchecked();
            if (arrayIndex[n] == null)
                return new TsObject(0);
            else
                return new TsObject(arrayIndex[n].Length);
        }

        [WeakMethod]
        public static TsObject Base64Encode(TsObject[] args)
        {
            if (args[0].Type != VariableType.String)
                throw new ArgumentException($"Expected string, got {args[0].Type}.", "string");
            var str = args[0].GetStringUnchecked();
            return new TsObject(Convert.ToBase64String(Encoding.Unicode.GetBytes(str)));
        }

        [WeakMethod]
        public static TsObject Base64Decode(TsObject[] args)
        {
            if (args[0].Type != VariableType.String)
                throw new ArgumentException($"Expected string, got {args[0].Type}.", "string");
            var str = args[0].GetStringUnchecked();
            return new TsObject(Encoding.Unicode.GetString(Convert.FromBase64String(str)));
        }


        public static float Ceil(float x)
        {
            return (float)Math.Ceiling(x);
        }

        [WeakMethod]
        public static TsObject Choose(TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("There must be at least one argument passed to Choose.");
            return (args[Rng.Next(args.Length)]);
        }

        public static float Clamp(float val, float min, float max)
        {
            return val < min ? min : (val > max ? max : val);
        }

        public static bool CodeIsCompiled()
        {
            return true;
        }

        public static float DotProduct(float x1, float y1, float x2, float y2)
        {
            return (x1 * x2) + (y1 + y2);
        }

        [WeakMethod]
        public static TsObject EventInherited(TsObject[] args)
        {
            var inst = TsObject.Id.Peek().GetInstance();
            if (TsInstance.TryGetEvent(inst.Parent, TsInstance.EventType.Peek(), out var ev))
                ev(inst);

            return TsObject.Empty();
        }

        public static string EnvironmentGetVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

        public static void EventPerform(string name)
        {
            var inst = TsObject.Id.Peek().GetInstance();
            if (TsInstance.TryGetEvent(inst.ObjectType, name, out var ev))
                ev(inst);
        }

        public static void EventPerformObject(string type, string eventName)
        {
            var inst = TsObject.Id.Peek().GetInstance();
            if (TsInstance.TryGetEvent(type, eventName, out var ev))
                ev(inst);
        }

        public static float Exp(float n)
        {
            return (float)Math.Exp(n);
        }

        public static float Floor(float n)
        {
            return (float)Math.Floor(n);
        }

        public static float Ln(float n)
        {
            return (float)Math.Log(n);
        }

        public static float Log10(float n)
        {
            return (float)Math.Log10(n);
        }

        public static float Log2(float val)
        {
            return (float)Math.Log(val, 2);
        }

        public static float LogN(float n, float val)
        {
            return (float)Math.Log(val, n);
        }

        [WeakMethod]
        public static TsObject Max(TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentOutOfRangeException("args", "You must pass in at least one value to Max");
            var max = args[0].GetNum();
            for(var i = 1; i < args.Length; i++)
            {
                var num = args[i].GetNum();
                if (num > max)
                    max = num;
            }
            return new TsObject(max);
        }

        [WeakMethod]
        public static TsObject Min(TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentOutOfRangeException("args", "You must pass in at least one value to Max");
            var min = args[0].GetNum();
            for (var i = 1; i < args.Length; i++)
            {
                var num = args[i].GetNum();
                if (num < min)
                    min = num;
            }
            return new TsObject(min);
        }

        public static float Random(float n)
        {
            return (float)Rng.NextDouble() * n;
        }

        public static int RandomGetSeed()
        {
            return RandomSeed;
        }

        public static float RandomRange(float min, float max)
        {
            return (float)Rng.NextDouble() * (max - min) + min;
        }

        public static void RandomSetSeed(int seed)
        {
            Rng = new Random(seed);
            RandomSeed = seed;
        }

        public static int Randomise()
        {
            int seed;
            unchecked
            {
                seed = (int)DateTimeOffset.Now.Ticks;
            }
            RandomSetSeed(seed);
            return seed;
        }

        public static float Real(string str)
        {
            return float.Parse(str);
        }

        public static float Round(float n)
        {
            return (float)Math.Round(n);
        }

        [WeakMethod]
        public static TsObject ScriptExecute(TsObject[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("You must pass at least a script name to script_execute.");
            var name = args[0].GetString();
            if (!TsInstance.Functions.TryGetValue(name, out var function))
                throw new ArgumentException($"Tried to execute a non-existant function: {name}");
            var parameters = new TsObject[args.Length - 1];
            if (parameters.Length != 0)
                Array.Copy(args, 1, parameters, 0, parameters.Length);
            return function(parameters);
        }

        public static bool ScriptExists(string name)
        {
            return TsInstance.Functions.ContainsKey(name);
        }

        public static void ShowError(string message, bool throws)
        {
            var error = new UserDefinedException(message);
            if (throws)
                throw error;
            else
                Console.WriteLine(error);
        }

        public static float Sign(float n)
        {
            return Math.Sign(n);
        }

        public static float Square(float n)
        {
            return n * n;
        }

        public static float Sqrt(float n)
        {
            return (float)Math.Sqrt(n);
        }

        public static int StringByteAt(string str, int index)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            if (index >= bytes.Length)
                return -1;

            return bytes[index];
        }

        public static int StringByteLength(string str)
        {
            return Encoding.UTF8.GetByteCount(str);
        }

        public static string StringCharAt(string str, int index)
        {
            if (index >= str.Length)
                return "";
            return str[index].ToString();
        }

        public static string StringCopy(string str, int index, int count)
        {
            return str.Substring(index, count);
        }

        public static int StringCount(string subString, string str)
        {
            // Code found here:
            // https://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-within-a-string
            return (str.Length - str.Replace(subString, "").Length) / subString.Length;
        }

        public static string StringDelete(string str, int index, int count)
        {
            return str.Remove(index, count);
        }

        public static string StringDigits(string str)
        {
            //Test with regex to see if that's faster.
            //return System.Text.RegularExpressions.Regex.Replace(str, @"[^\d]", "");
            var sb = new StringBuilder();
            for(var i = 0; i < str.Length; i++)
            {
                //Good ol fashioned C trick.
                if (str[i] >= '0' && str[i] <= '9')
                    sb.Append(str[i]);
            }
            return sb.ToString();
        }

        public static string StringInsert(string subString, string str, int index)
        {
            return str.Insert(index, subString);
        }

        public static int StringLength(string str)
        {
            return str.Length;
        }

        public static string StringLetters(string str)
        {
            //Test with regex to see if that's faster.
            //return System.Text.RegularExpressions.Regex.Replace(str, @"[^a-zA-Z]", "");
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                if ((str[i] >= 'a' && str[i] <= 'z') || (str[i] >= 'A' && str[i] <= 'Z'))
                    sb.Append(str[i]);
            }
            return sb.ToString();
        }

        public static string StringLettersDigits(string str)
        {
            //Test with regex to see if that's faster.
            //return System.Text.RegularExpressions.Regex.Replace(str, @"[^a-zA-Z\d]", "");
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                if ((str[i] >= 'a' && str[i] <= 'z') || (str[i] >= 'A' && str[i] <= 'Z') || (str[i] >= '0' && str[i] <= '9'))
                    sb.Append(str[i]);
            }
            return sb.ToString();
        }

        public static string StringLower(string str)
        {
            return str.ToLower();
        }

        public static int StringOrdAt(string str, int index)
        {
            return str[index];
        }

        public static int StringPos(string subString, string str)
        {
            return str.IndexOf(subString);
        }

        public static string StringRepeat(string str, int count)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
                sb.Append(str);

            return sb.ToString();
        }

        public static string StringReplace(string str, string subString, string newString)
        {
            var index = str.IndexOf(subString);
            if(index != -1)
                return str.Substring(0, index) + newString + str.Substring(index + subString.Length);

            return str;
        }

        public static string StringReplaceAll(string str, string subString, string newString)
        {
            return str.Replace(subString, newString);
        }

        public static string StringSetByte(string str, int pos, int value)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            bytes[pos] = (byte)value;
            return Encoding.UTF8.GetString(bytes);
        }

        public static string StringUpper(string str)
        {
            return str.ToUpper();
        }
    }
}