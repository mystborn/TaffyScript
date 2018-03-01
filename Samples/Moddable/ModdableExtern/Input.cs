using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace ModdableExtern
{
    public static class Input
    {
        private static KeyboardState _keyboardCurrent;
        private static KeyboardState _keyboardPrevious;

        public static bool KeyCheck(int key)
        {
            if (!Enum.IsDefined(typeof(Keys), key))
                throw new ArgumentException("The given key was not valid", "key");
            return KeyCheck((Keys)key);
        }

        public static bool KeyCheck(Keys key)
        {
            return _keyboardCurrent.IsKeyDown(key);
        }

        public static bool KeyCheckPressed(int key)
        {
            if (!Enum.IsDefined(typeof(Keys), key))
                throw new ArgumentException("The given key was not valid", "key");
            return KeyCheckPressed((Keys)key);
        }

        public static bool KeyCheckPressed(Keys key)
        {
            return _keyboardCurrent.IsKeyDown(key) && !_keyboardPrevious.IsKeyDown(key);
        }

        public static void Update()
        {
            _keyboardPrevious = _keyboardCurrent;
            _keyboardCurrent = Keyboard.GetState();
        }
    }
}