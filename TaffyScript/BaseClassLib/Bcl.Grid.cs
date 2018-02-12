using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public static partial class Bcl
    {
        private readonly static Dictionary<float, Grid> _grids = new Dictionary<float, Grid>();
        private readonly static Queue<float> _gridSlots = new Queue<float>();

        public static void DsGridAdd(GmObject obj)
        {

        }

        public static float DsGridCreate(float w, float h)
        {
            float index;
            if (_gridSlots.Count == 0)
                index = _grids.Count;
            else
                index = _gridSlots.Dequeue();

            var grid = new Grid((int)w, (int)h);
            _grids[index] = grid;

            return index;
        }
    }
}
