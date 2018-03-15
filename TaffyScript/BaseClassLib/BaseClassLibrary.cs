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
        
        public static TsObject ToString(TsObject obj)
        {
            if (obj.Type != VariableType.String)
                return new TsObject(obj.ToString());
            else
                return obj;
        }
        
        public static TsObject AngleDifference(float ang1, float ang2)
        {
            var result = ang1 - ang2;
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

        public static TsObject ArcCos(float x)
        {
            return new TsObject((float)Math.Acos(x));
        }
        
        public static TsObject ArcSin(float x)
        {
            return new TsObject((float)Math.Asin(x));
        }
        
        public static TsObject ArcTan(float x)
        {
            return new TsObject((float)Math.Atan(x));
        }
        
        public static TsObject ArcTan2(float y, float x)
        {
            return new TsObject((float)Math.Atan2(y, x));
        }
        
        public static TsObject ArrayCopy(TsObject dest, int destIndex, TsObject src, int srcIndex, int length)
        {
            var destWrapper = dest.Value as TsValueArray<TsObject[]> ?? throw new ArgumentException("Can only copy 1D arrays", "dest");
            var srcWrapper = src.Value as TsValueArray<TsObject[]> ?? throw new ArgumentException("Can only copy 1D arrays", "src");
            var destValue = destWrapper.StrongValue;
            var srcValue = srcWrapper.StrongValue;
            if (destIndex + length >= destValue.Length)
            {
                var temp = new TsObject[destIndex + length + 1];
                Buffer.BlockCopy(destValue, 0, temp, 0, destValue.Length);
                destValue = temp;
                destWrapper.StrongValue = destValue;
            }
            Buffer.BlockCopy(srcValue, srcIndex, destValue, destIndex, length);
            return TsObject.Empty();
        }

        [WeakMethod]
        public static TsObject ArrayCreate(TsObject[] args)
        {
            var size = args[0].GetInt();
            var value = TsObject.Empty();
            if (args.Length > 1)
                value = args[1];
            var result = new TsObject[size];
            for (var i = 0; i < size; ++i)
                result[i] = value;

            return new TsObject(result);
        }
        
        public static TsObject ArrayEquals(TsObject[] var1, TsObject[] var2)
        {
            if (var1.Length != var2.Length)
                return new TsObject(false);

            for (var i = 0; i < var1.Length; ++i)
            {
                if (var1[i] != var2[i])
                    return new TsObject(false);
            }
            return new TsObject(true);
        }
        
        public static TsObject ArrayHeight2D(TsObject[][] array)
        {
            return new TsObject(array.Length);
        }
        
        public static TsObject ArrayLength1D(TsObject[] array)
        {
            return new TsObject(array.Length);
        }
        
        public static TsObject ArrayLength2D(TsObject[][] array, int n)
        {
            if (array[n] == null)
                return new TsObject(0);
            else
                return new TsObject(array[n].Length);
        }
        
        public static TsObject Base64Encode(string str)
        {
            return new TsObject(Convert.ToBase64String(Encoding.Unicode.GetBytes(str)));
        }
        
        public static TsObject Base64Decode(string str)
        {
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

        public static void EventInherited()
        {
            var inst = TsObject.Id.Peek().GetInstance();
            if (TsInstance.TryGetEvent(inst.Parent, TsInstance.EventType.Peek(), out var ev))
                ev(inst);
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
            var max = args[0].GetFloat();
            for(var i = 1; i < args.Length; i++)
            {
                var num = args[i].GetFloat();
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
            var min = args[0].GetFloat();
            for (var i = 1; i < args.Length; i++)
            {
                var num = args[i].GetFloat();
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
            if (!TsInstance.Scripts.TryGetValue(name, out var function))
                throw new ArgumentException($"Tried to execute a non-existant function: {name}");
            var parameters = new TsObject[args.Length - 1];
            if (parameters.Length != 0)
                Array.Copy(args, 1, parameters, 0, parameters.Length);
            return function(parameters);
        }

        public static bool ScriptExists(string name)
        {
            return TsInstance.Scripts.ContainsKey(name);
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