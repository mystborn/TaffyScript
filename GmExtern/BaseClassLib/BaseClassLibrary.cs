using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public static partial class Bcl
    {
        public static Random rng = new Random(123456789);

        [WeakMethod]
        public static GmObject ToString(GmObject[] args)
        {
            if (args[0].Type != VariableType.String)
                return new GmObject(args[0].ToString());
            else
                return args[0];
        }

        [WeakMethod]
        public static GmObject AngleDifference(GmObject[] args)
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
            return new GmObject(result);
        }

        [WeakMethod]
        public static GmObject ArcCos(GmObject[] args)
        {
            return new GmObject((float)Math.Acos(args[0].GetNum()));
        }

        [WeakMethod]
        public static GmObject ArcSin(GmObject[] args)
        {
            return new GmObject((float)Math.Asin(args[0].GetNum()));
        }

        [WeakMethod]
        public static GmObject ArcTan(GmObject[] args)
        {
            return new GmObject((float)Math.Atan(args[0].GetNum()));
        }

        [WeakMethod]
        public static GmObject ArcTan2(GmObject[] args)
        {
            return new GmObject((float)Math.Atan2(args[0].GetNum(), args[1].GetNum()));
        }

        [WeakMethod]
        public static GmObject ArrayCopy(GmObject[] args)
        {
            var destIndex = args[1].GetNumAsInt();
            var srcIndex = args[3].GetNumAsInt();
            var length = args[4].GetNumAsInt();
            if (args[0].Type == VariableType.Array1)
            {
                var destWrapper = args[0].Value as GmValueArray<GmObject[]> ?? throw new ArgumentException("Can only copy 1D arrays", "dest");
                var srcWrapper = args[2].Value as GmValueArray<GmObject[]> ?? throw new ArgumentException("Can only copy 1D arrays", "src");
                var dest = destWrapper.StrongValue;
                var src = srcWrapper.StrongValue;
                if (destIndex + length >= dest.Length)
                {
                    var temp = new GmObject[destIndex + length];
                    Buffer.BlockCopy(dest, 0, temp, 0, dest.Length);
                    dest = temp;
                    destWrapper.StrongValue = dest;
                }
                Buffer.BlockCopy(src, srcIndex, dest, destIndex, length);
            }
            else
                throw new ArgumentException("Can only copy 1D arrays", "dest");
            return GmObject.Empty();
        }

        [WeakMethod]
        public static GmObject ArrayCreate(GmObject[] args)
        {
            var size = args[0].GetNumAsInt();
            var value = GmObject.Empty();
            if(args.Length > 1)
                value = args[1];
            var result = new GmObject[size];
            for(var i = 0; i < size; ++i)
                result[i] = value;

            return new GmObject(result);
        }

        [WeakMethod]
        public static GmObject ArrayEquals(GmObject[] args)
        {
            var var1 = (args[0].Value as GmValueArray<GmObject[]> ?? throw new ArgumentException("Can only compare 1D arrays", "var1")).StrongValue;
            var var2 = (args[1].Value as GmValueArray<GmObject[]> ?? throw new ArgumentException("Can only compare 1D arrays", "var2")).StrongValue;
            if (var1.Length != var2.Length)
                return new GmObject(false);

            for(var i = 0; i < var1.Length; ++i)
            {
                if (var1[i] != var2[i])
                    return new GmObject(false);
            }
            return new GmObject(true);
        }

        [WeakMethod]
        public static GmObject ArrayHeight2D(GmObject[] args)
        {
            var arrayIndex = (args[0].Value as GmValueArray<GmObject[][]> ?? throw new ArgumentException($"Expected 2D array, got {args[0].Type}.", "array_index")).StrongValue;
            return new GmObject(arrayIndex.Length);
        }

        [WeakMethod]
        public static GmObject ArrayLength1D(GmObject[] args)
        {
            var arrayIndex = (args[0].Value as GmValueArray<GmObject[]> ?? throw new ArgumentException($"Expected 1D array, got {args[0].Type}.", "array_index")).StrongValue;
            return new GmObject(arrayIndex.Length);
        }

        [WeakMethod]
        public static GmObject ArrayLength2D(GmObject[] args)
        {
            var arrayIndex = (args[0].Value as GmValueArray<GmObject[][]> ?? throw new ArgumentException($"Expected 2D array, got {args[0].Type}.", "array_index")).StrongValue;
            if (args[1].Type != VariableType.Real)
                throw new ArgumentException($"Expected real value, got {args[1].Type}");
            var n = (int)args[1].GetNumUnchecked();
            if (arrayIndex[n] == null)
                return new GmObject(0);
            else
                return new GmObject(arrayIndex[n].Length);
        }

        [WeakMethod]
        public static GmObject Base64Encode(GmObject[] args)
        {
            if (args[0].Type != VariableType.String)
                throw new ArgumentException($"Expected string, got {args[0].Type}.", "string");
            var str = args[0].GetStringUnchecked();
            return new GmObject(Convert.ToBase64String(Encoding.Unicode.GetBytes(str)));
        }

        [WeakMethod]
        public static GmObject Base64Decode(GmObject[] args)
        {
            if (args[0].Type != VariableType.String)
                throw new ArgumentException($"Expected string, got {args[0].Type}.", "string");
            var str = args[0].GetStringUnchecked();
            return new GmObject(Encoding.Unicode.GetString(Convert.FromBase64String(str)));
        }


        public static float Ceil(float x)
        {
            return (float)Math.Ceiling(x);
        }

        [WeakMethod]
        public static GmObject Choose(GmObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("There must be at least one argument passed to Choose.");
            return (args[rng.Next(args.Length)]);
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
    }
}
