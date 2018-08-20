using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Numbers
{
    [TaffyScriptBaseType]
    public class NumberScripts
    {
        /// <summary>
        /// Gets the random number generator used by TaffyScript.
        /// </summary>
        public static Random Rng { get; private set; }

        /// <summary>
        /// Gets the seed used by <see cref="Rng"/>.
        /// </summary>
        public static int RandomSeed { get; private set; }

        static NumberScripts()
        {
            RandomSeed = (int)DateTimeOffset.Now.Ticks;
            Rng = new Random(RandomSeed);
        }

        [TaffyScriptMethod]
        public static TsObject abs(TsObject[] args)
        {
            return Math.Abs((float)args[0]);
        }

        [TaffyScriptMethod]
        public static TsObject ceil(TsObject[] args)
        {
            return Math.Ceiling((double)args[0]);
        }

        [TaffyScriptMethod]
        public static TsObject choose(TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("There must be at least one argument passed to Choose.");
            return args[Rng.Next(args.Length)];
        }

        [TaffyScriptMethod]
        public static TsObject clamp(TsObject[] args)
        {
            float val = (float)args[0], min = (float)args[1], max = (float)args[2];
            return val < min ? min : (val > max ? max : val);
        }

        [TaffyScriptMethod]
        public static TsObject exp(TsObject[] args)
        {
            return Math.Exp((double)args[0]);
        }

        [TaffyScriptMethod]
        public static TsObject floor(TsObject[] args)
        {
            return Math.Floor((double)args[0]);
        }

        [TaffyScriptMethod]
        public static TsObject ln(TsObject[] args)
        {
            return Math.Log((double)args[0]);
        }

        [TaffyScriptMethod]
        public static TsObject log10(TsObject[] args)
        {
            return Math.Log10((double)args[0]);
        }

        [TaffyScriptMethod]
        public static TsObject logn(TsObject[] args)
        {
            return Math.Log((double)args[0], (double)args[1]);
        }

        [TaffyScriptMethod]
        public static TsObject max(TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentOutOfRangeException("args", "You must pass in at least one value to Max");
            var max = args[0].GetNumber();
            for (var i = 1; i < args.Length; i++)
            {
                var num = args[i].GetNumber();
                if (num > max)
                    max = num;
            }
            return max;
        }

        [TaffyScriptMethod]
        public static TsObject min(TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentOutOfRangeException("args", "You must pass in at least one value to Max");
            var min = args[0].GetNumber();
            for (var i = 1; i < args.Length; i++)
            {
                var num = args[i].GetNumber();
                if (num < min)
                    min = num;
            }
            return min;
        }

        [TaffyScriptMethod]
        public static TsObject pow(TsObject[] args)
        {
            return Math.Pow((double)args[0], (double)args[1]);
        }

        [TaffyScriptMethod]
        public static TsObject random(TsObject[] args)
        {
            return (float)Rng.NextDouble() * (float)args[0];
        }

        [TaffyScriptMethod]
        public static TsObject random_get_seed(TsObject[] args)
        {
            return RandomSeed;
        }

        [TaffyScriptMethod]
        public static TsObject random_range(TsObject[] args)
        {
            var min = (float)args[0];
            return (float)Rng.NextDouble() * ((float)args[1] - min) + min;
        }

        [TaffyScriptMethod]
        public static TsObject random_set_seed(TsObject[] args)
        {
            var seed = (int)args[0];
            Rng = new Random(seed);
            RandomSeed = seed;
            return TsObject.Empty;
        }

        [TaffyScriptMethod]
        public static TsObject randomise(TsObject[] args)
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

        [TaffyScriptMethod]
        public static TsObject round(TsObject[] args)
        {
            return Math.Round((double)args[0]);
        }

        [TaffyScriptMethod]
        public static TsObject sign(TsObject[] args)
        {
            return Math.Sign((float)args[0]);
        }

        [TaffyScriptMethod]
        public static TsObject sqr(TsObject[] args)
        {
            var n = (float)args[0];
            return n * n;
        }

        [TaffyScriptMethod]
        public static TsObject sqrt(TsObject[] args)
        {
            return Math.Sqrt((double)args[0]);
        }
    }
}
