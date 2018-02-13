using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public static partial class Bcl
    {
        public static Random Rng = new Random(123456789);

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
            if (inst.Parent != null)
                if (TsInstance.Events.TryGetValue(inst.Parent, out var events) && events.TryGetValue(TsInstance.EventType.Peek(), out var ev))
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
            if(TsInstance.Events.TryGetValue(inst.ObjectType, out var events))
            {
                if(events.TryGetValue(name, out var toTrigger))
                {
                    toTrigger(inst);
                }
            }
            else if(TsInstance.Events.TryGetValue(inst.Parent, out events))
            {
                if(events.TryGetValue(name, out var toTrigger))
                {
                    toTrigger(inst);
                }
            }
        }

        public static void EventPerformObject(string type, string eventName)
        {
            var inst = TsObject.Id.Peek().GetInstance();
            if(TsInstance.Events.TryGetValue(type, out var events))
            {
                if (events.TryGetValue(eventName, out var ev))
                    ev(inst);
            }
        } 
    }
}