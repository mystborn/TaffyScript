using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public class Grid
    {
        private GmObject[,] _source;

        public GmObject this[int x, int y]
        {
            get => _source[x, y];
            set => _source[x, y] = value;
        }

        public Grid(int width, int height)
        {
            _source = new GmObject[width, height];
        }
    }
}
