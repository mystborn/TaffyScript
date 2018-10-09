using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Numbers
{
    /// <summary>
    /// Provides commonly used, general math scripts.
    /// </summary>
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

        /// <summary>
        /// Gets the absolute value of a number.
        /// </summary>
        /// <arg name="value" type="number">The number to get the absolute value of.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject abs(TsObject[] args)
        {
            return Math.Abs((float)args[0]);
        }

        /// <summary>
        /// Rounds a number to the closest integer that is greater than or equal to the value.
        /// </summary>
        /// <param name="value" type="number">The value to round.</param>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject ceil(TsObject[] args)
        {
            return Math.Ceiling((double)args[0]);
        }

        /// <summary>
        /// Randomly chooses one of the arguments.
        /// </summary>
        /// <arg name="..args" type="objects">Any number of objects to choose from.</arg>
        /// <returns>object</returns>
        [TaffyScriptMethod]
        public static TsObject choose(TsObject[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("There must be at least one argument passed to Choose.");
            return args[Rng.Next(args.Length)];
        }

        /// <summary>
        /// Clamps a value between a minimum and maximum
        /// </summary>
        /// <arg name="value" type="number">The value to clamp.</arg>
        /// <arg name="min" type="number">The minimum possible value.</arg>
        /// <arg name="max" type="number">The maximum possible value.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject clamp(TsObject[] args)
        {
            float val = (float)args[0], min = (float)args[1], max = (float)args[2];
            return val < min ? min : (val > max ? max : val);
        }

        /// <summary>
        /// Raises euler's number to specified power.
        /// </summary>
        /// <param name="args">The exponent to raise eulers number by.</param>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject exp(TsObject[] args)
        {
            return Math.Exp((double)args[0]);
        }

        /// <summary>
        /// Rounds a number to the closest integer that is less than or equal to the value.
        /// </summary>
        /// <param name="value" type="number">The value to round.</param>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject floor(TsObject[] args)
        {
            return Math.Floor((double)args[0]);
        }

        /// <summary>
        /// Gets a random integer.
        /// </summary>
        /// <arg name="[max_or_min]" type="number">If there are two arguments, this is the inclusive minimum possible result. Otherwise, this is the exclusive maximum possible result.</arg>
        /// <arg name="[max]" type="number">The exclusive maximum result.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject irandom(TsObject[] args)
        {
            if (args is null)
                return Rng.Next();

            switch (args.Length)
            {
                case 0:
                    return Rng.Next();
                case 1:
                    return Rng.Next((int)args[0]);
                default:
                    return Rng.Next((int)args[0], (int)args[1]);
            }
        }

        /// <summary>
        /// Gets the natural logarithm of a value.
        /// </summary>
        /// <arg name="value" type="number">The value to get the natural log of.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject ln(TsObject[] args)
        {
            return Math.Log((double)args[0]);
        }

        /// <summary>
        /// Gets the logarithm in base 10 of a value.
        /// </summary>
        /// <arg name="value" type="number">The value to get the logarithm of.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject log10(TsObject[] args)
        {
            return Math.Log10((double)args[0]);
        }

        /// <summary>
        /// Gets the logarithm in the specified base of a value.
        /// </summary>
        /// <arg name="value" type="number">The value to get the logarithm of.</arg>
        /// <arg name="log_base" type="number">The base of the logarithm.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject logn(TsObject[] args)
        {
            return Math.Log((double)args[0], (double)args[1]);
        }

        /// <summary>
        /// Gets the argument with the highest value.
        /// </summary>
        /// <arg name="..args" type="numbers">Any amount of numbers.</arg>
        /// <returns>number</returns>
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

        /// <summary>
        /// Gets the argument with the lowest value.
        /// </summary>
        /// <arg name="..args" type="numbers">Any amount of numbers.</arg>
        /// <returns>number</returns>
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

        /// <summary>
        /// Raises a value to the specifies power.
        /// </summary>
        /// <arg name="value" type="number">The value to raise.</arg>
        /// <arg name="pow" type="number">The power to raise the value to.</arg>
        /// <returns></returns>
        [TaffyScriptMethod]
        public static TsObject pow(TsObject[] args)
        {
            return Math.Pow((double)args[0], (double)args[1]);
        }

        /// <summary>
        /// Gets a random number. If there are no arguments, returns a number between 0 and 1. If there is one argument, returns a number between 0 and argument0. If there are two arguments, returns a number between argument0 and argument1.
        /// </summary>
        /// <arg name="[max_or_min]" type="number">If there are two arguments, this is the inclusive minimum possible result. Otherwise, this is the exclusive maximum possible result.</arg>
        /// <arg name="[max]" type="number">The exclusive maximum result.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject random(TsObject[] args)
        {
            if (args is null)
                return (float)Rng.NextDouble();

            switch(args.Length)
            {
                case 0:
                    return (float)Rng.NextDouble();
                case 1:
                    return (float)Rng.NextDouble() * (float)args[0];
                default:
                    var min = (float)args[0];
                    return (float)Rng.NextDouble() * ((float)args[1] - min) + min;
            }
            
        }

        /// <summary>
        /// Gets the seed used to initialize the random number generator.
        /// </summary>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject random_get_seed(TsObject[] args)
        {
            return RandomSeed;
        }

        /// <summary>
        /// Initializes the random number generator using the given seed.
        /// </summary>
        /// <arg name="seed" type="number">The number used to initialize the rng.</arg>
        /// <returns>null</returns>
        [TaffyScriptMethod]
        public static TsObject random_set_seed(TsObject[] args)
        {
            var seed = (int)args[0];
            Rng = new Random(seed);
            RandomSeed = seed;
            return TsObject.Empty;
        }

        /// <summary>
        /// Initializes the random number generator with a random seed.
        /// </summary>
        /// <returns>number</returns>
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

        /// <summary>
        /// Rounds a value to the nearest integer.
        /// </summary>
        /// <arg name="value" type="number">The value to round.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject round(TsObject[] args)
        {
            return Math.Round((double)args[0]);
        }

        /// <summary>
        /// Gets a value representing the sign of the specified number. Returns -1 for a negativae value, 0 for zero, and 1 for a positive number.
        /// </summary>
        /// <arg name="value" type="number">The value to get the sign of.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject sign(TsObject[] args)
        {
            return Math.Sign((float)args[0]);
        }

        /// <summary>
        /// Squares a number.
        /// </summary>
        /// <arg name="value" type="number">The value to square.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject sqr(TsObject[] args)
        {
            var n = (float)args[0];
            return n * n;
        }

        /// <summary>
        /// Gets the square root of a number.
        /// </summary>
        /// <arg name="value" type="number">The value to get the square root of.</arg>
        /// <returns>number</returns>
        [TaffyScriptMethod]
        public static TsObject sqrt(TsObject[] args)
        {
            return Math.Sqrt((double)args[0]);
        }
    }
}
