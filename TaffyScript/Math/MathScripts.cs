using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maths = System.Math;

namespace TaffyScript.Math
{
    [WeakBaseType]
    public class MathScripts
    {
        /// <summary>
        /// Gets the random number generator used by TaffyScript.
        /// </summary>
        public static Random Rng { get; private set; } = new Random(123456789);

        /// <summary>
        /// Gets the seed used by <see cref="Rng"/>.
        /// </summary>
        public static int RandomSeed { get; private set; } = 123456789;

        [WeakMethod]
        public static TsObject abs(ITsInstance inst, TsObject[] args)
        {
            return Maths.Abs((float)args[0]);
        }

        [WeakMethod]
        public static TsObject ceil(ITsInstance inst, TsObject[] args)
        {
            return Maths.Ceiling((double)args[0]);
        }

        [WeakMethod]
        public static TsObject choose(ITsInstance target, TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("There must be at least one argument passed to Choose.");
            return args[Rng.Next(args.Length)];
        }

        [WeakMethod]
        public static TsObject clamp(ITsInstance inst, TsObject[] args)
        {
            float val = (float)args[0], min = (float)args[1], max = (float)args[2];
            return val < min ? min : (val > max ? max : val);
        }

        [WeakMethod]
        public static TsObject exp(ITsInstance inst, TsObject[] args)
        {
            return Maths.Exp((double)args[0]);
        }

        [WeakMethod]
        public static TsObject floor(ITsInstance inst, TsObject[] args)
        {
            return Maths.Floor((double)args[0]);
        }

        [WeakMethod]
        public static TsObject ln(ITsInstance inst, TsObject[] args)
        {
            return Maths.Log((double)args[0]);
        }

        [WeakMethod]
        public static TsObject log10(ITsInstance inst, TsObject[] args)
        {
            return Maths.Log10((double)args[0]);
        }

        [WeakMethod]
        public static TsObject logn(ITsInstance inst, TsObject[] args)
        {
            return Maths.Log((double)args[0], (double)args[1]);
        }

        [WeakMethod]
        public static TsObject max(ITsInstance target, TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentOutOfRangeException("args", "You must pass in at least one value to Max");
            var max = args[0].GetFloat();
            for (var i = 1; i < args.Length; i++)
            {
                var num = args[i].GetFloat();
                if (num > max)
                    max = num;
            }
            return max;
        }

        [WeakMethod]
        public static TsObject min(ITsInstance target, TsObject[] args)
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
            return min;
        }

        [WeakMethod]
        public static TsObject random(ITsInstance inst, TsObject[] args)
        {
            return (float)Rng.NextDouble() * (float)args[0];
        }

        [WeakMethod]
        public static TsObject random_get_seed(ITsInstance inst, TsObject[] args)
        {
            return RandomSeed;
        }

        [WeakMethod]
        public static TsObject random_range(ITsInstance inst, TsObject[] args)
        {
            var min = (float)args[0];
            return (float)Rng.NextDouble() * ((float)args[1] - min) + min;
        }

        [WeakMethod]
        public static TsObject random_set_seed(ITsInstance inst, TsObject[] args)
        {
            var seed = (int)args[0];
            Rng = new Random(seed);
            RandomSeed = seed;
            return TsObject.Empty();
        }

        [WeakMethod]
        public static TsObject randomise(ITsInstance inst, TsObject[] args)
        {
            int seed;
            unchecked
            {
                seed = (int)DateTimeOffset.Now.Ticks;
            }
            Rng = new Random(seed);
            RandomSeed = seed;
            return seed;
        }

        [WeakMethod]
        public static TsObject round(ITsInstance inst, TsObject[] args)
        {
            return Maths.Round((double)args[0]);
        }

        [WeakMethod]
        public static TsObject sign(ITsInstance inst, TsObject[] args)
        {
            return Maths.Sign((float)args[0]);
        }

        [WeakMethod]
        public static TsObject sqr(ITsInstance inst, TsObject[] args)
        {
            var n = (float)args[0];
            return n * n;
        }

        [WeakMethod]
        public static TsObject sqrt(ITsInstance inst, TsObject[] args)
        {
            return Maths.Sqrt((double)args[0]);
        }
    }
}
