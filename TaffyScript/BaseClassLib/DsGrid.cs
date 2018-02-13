using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public class DsGrid
    {
        private readonly static List<DsGrid> _grids = new List<DsGrid>();
        private readonly static Queue<int> _gridSlots = new Queue<int>();

        private TsObject[,] _source;

        public TsObject this[int x, int y]
        {
            get => _source[x, y];
            set => _source[x, y] = value;
        }

        private DsGrid(int width, int height)
        {
            _source = new TsObject[width, height];
        }

        public static void DsGridAdd(int index, int x, int y, TsObject value)
        {
            GetGrid(index)[x, y] += value;
        }

        public static void DsGridAddDisk(int index, int xm, int ym, int r, TsObject obj)
        {
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) => g[w, h] += obj);
        }

        public static void DsGridAddGridRegion(int index, int source, int x1, int y1, int x2, int y2, int xpos, int ypos)
        {
            var dest = GetGrid(index);
            var src = GetGrid(source);
            var xLength = x2 - x1;
            var yLength = y2 - y1;

            if (x1 < 0)
                throw new ArgumentOutOfRangeException("x1");
            else if (x2 >= src._source.GetLength(0))
                throw new ArgumentOutOfRangeException("x2");
            else if (y1 < 0)
                throw new ArgumentOutOfRangeException("y1");
            else if (y2 >= src._source.GetLength(1))
                throw new ArgumentOutOfRangeException("y2");
            else if (xpos < 0 || xpos + xLength >= dest._source.GetLength(0))
                throw new ArgumentOutOfRangeException("xpos");
            else if (ypos < 0 || ypos + yLength >= dest._source.GetLength(1))
                throw new ArgumentOutOfRangeException("ypos");

            for(var w = 0; w < xLength; w++)
            {
                for(var h = 0; h < yLength; h++)
                {
                    dest[xpos + w, ypos + h] += src[x1 + w, x2 + h];
                }
            }
        }

        public static void DsGridAddRegion(int index, int x1, int y1, int x2, int y2, TsObject value)
        {
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) => g[w, h] += value);
        }

        public static void DsGridClear(int index, TsObject value)
        {
            var grid = GetGrid(index)._source;
            for(var w = 0; w < grid.GetLength(0); w++)
            {
                for(var h = 0; h < grid.GetLength(1); h++)
                {
                    grid[w, h] = value;
                }
            }
        }

        public static void DsGridCopy(int destination, int source)
        {
            var dst = GetGrid(destination);
            var src = GetGrid(source);
            var xLength = Math.Max(dst._source.GetLength(0), src._source.GetLength(0));
            var yLength = Math.Min(dst._source.GetLength(1), src._source.GetLength(1));
            DsGridSetGridRegion(dst, src, 0, 0, xLength - 1, yLength - 1, 0, 0);
        }

        public static int DsGridCreate(int w, int h)
        {
            if (w < 0)
                throw new ArgumentOutOfRangeException("w");
            else if (h < 0)
                throw new ArgumentOutOfRangeException("h");

            var grid = new DsGrid(w, h);
            int index;
            if (_gridSlots.Count == 0)
            {
                index = _grids.Count;
                _grids.Add(grid);
            }
            else
            {
                index = _gridSlots.Dequeue();
                _grids[index] = grid;
            }

            return index;
        }

        public static void DsGridDestroy(int index)
        {
            if (index < 0 || index >= _grids.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (_grids[index] == null)
                throw new DataStructureDestroyedException("grid", index);

            _grids[index] = null;
            _gridSlots.Enqueue(index);
        }

        public static TsObject DsGridGet(int index, int x, int y)
        {
            return GetGrid(index)[x, y];
        }

        public static TsObject DsGridGetDiskMax(int index, int xm, int ym, int r)
        {
            bool set = false;
            TsObject i = TsObject.Empty();
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) =>
            {
                if (!set)
                    i = g[w, h];
                else
                {
                    var current = g[w, h];
                    if (current > i)
                        i = current;
                }
            });
            return i;
        }

        public static TsObject DsGridGetDiskMean(int index, int xm, int ym, int r)
        {
            var mean = new TsObject(0);
            var iter = 0;
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) =>
            {
                iter++;
                mean += g[w, h];
            });
            return mean / iter;
        }

        public static TsObject DsGridGetDiskMin(int index, int xm, int ym, int r)
        {
            bool set = false;
            TsObject i = TsObject.Empty();
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) =>
            {
                if (!set)
                    i = g[w, h];
                else
                {
                    var current = g[w, h];
                    if (current < i)
                        i = current;
                }
            });
            return i;
        }

        public static TsObject DsGridGetDiskSum(int index, int xm, int ym, int r)
        {
            var mean = new TsObject(0);
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) =>
            {
                mean += g[w, h];
            });
            return mean;
        }

        public static TsObject DsGridGetMax(int index, int x1, int y1, int x2, int y2)
        {
            bool set = false;
            TsObject i = TsObject.Empty();
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) =>
            {
                if (!set)
                    i = g[w, h];
                else
                {
                    var current = g[w, h];
                    if (current > i)
                        i = current;
                }
            });
            return i;
        }

        public static TsObject DsGridGetMean(int index, int x1, int y1, int x2, int y2)
        {
            var mean = new TsObject(0);
            var iter = 0;
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) =>
            {
                iter++;
                mean += g[w, h];
            });
            return mean / iter;
        }

        public static TsObject DsGridGetMin(int index, int x1, int y1, int x2, int y2)
        {
            bool set = false;
            TsObject i = TsObject.Empty();
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) =>
            {
                if (!set)
                    i = g[w, h];
                else
                {
                    var current = g[w, h];
                    if (current < i)
                        i = current;
                }
            });
            return i;
        }

        public static TsObject DsGridGetSum(int index, int x1, int y1, int x2, int y2)
        {
            var mean = new TsObject(0);
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) =>
            {
                mean += g[w, h];
            });
            return mean;
        }

        public static int DsGridHeight(int index)
        {
            return GetGrid(index)._source.GetLength(1);
        }

        public static void DsGridMultiply(int index, int x, int y, TsObject value)
        {
            GetGrid(index)[x, y] *= value;
        }

        public static void DsGridMultiplyDisk(int index, int xm, int ym, int r, TsObject obj)
        {
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) => g[w, h] *= obj);
        }

        public static void DsGridMultiplyGridRegion(int index, int source, int x1, int y1, int x2, int y2, int xpos, int ypos)
        {
            var dest = GetGrid(index);
            var src = GetGrid(source);
            var xLength = x2 - x1;
            var yLength = y2 - y1;

            if (x1 < 0)
                throw new ArgumentOutOfRangeException("x1");
            else if (x2 >= src._source.GetLength(0))
                throw new ArgumentOutOfRangeException("x2");
            else if (y1 < 0)
                throw new ArgumentOutOfRangeException("y1");
            else if (y2 >= src._source.GetLength(1))
                throw new ArgumentOutOfRangeException("y2");
            else if (xpos < 0 || xpos + xLength >= dest._source.GetLength(0))
                throw new ArgumentOutOfRangeException("xpos");
            else if (ypos < 0 || ypos + yLength >= dest._source.GetLength(1))
                throw new ArgumentOutOfRangeException("ypos");

            for (var w = 0; w < xLength; w++)
            {
                for (var h = 0; h < yLength; h++)
                {
                    dest[xpos + w, ypos + h] *= src[x1 + w, x2 + h];
                }
            }
        }

        public static void DsGridMultiplyRegion(int index, int x1, int y1, int x2, int y2, TsObject value)
        {
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) => g[w, h] *= value);
        }

        public static void DsGridResize(int index, int w, int h)
        {
            if (w < 0)
                throw new ArgumentOutOfRangeException(nameof(w));
            else if (h < 0)
                throw new ArgumentOutOfRangeException(nameof(h));

            var temp = new DsGrid(w, h);
            var src = GetGrid(index);
            var xLength = Math.Max(w, src._source.GetLength(0));
            var yLength = Math.Min(h, src._source.GetLength(1));
            DsGridSetGridRegion(temp, src, 0, 0,xLength - 1, yLength - 1, 0, 0);
            var zero = new TsObject(0);
            for(var width = xLength; width < w; width++)
            {
                for(var height = yLength; height < h; height++)
                {
                    temp._source[width, height] = zero;
                }
            }
            _grids[index] = temp;
        }

        public static void DsGridSet(int index, int x, int y, TsObject value)
        {
            GetGrid(index)[x, y] = value;
        }

        public static void DsGridSetDisk(int index, int xm, int ym, int r, TsObject obj)
        {
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) => g[w, h] = obj);
        }

        public static void DsGridSetGridRegion(int index, int source, int x1, int y1, int x2, int y2, int xpos, int ypos)
        {
            var dest = GetGrid(index);
            var src = GetGrid(source);
            DsGridSetGridRegion(dest, src, x1, y1, x2, y2, xpos, ypos);
        }

        public static void DsGridSetRegion(int index, int x1, int y1, int x2, int y2, TsObject value)
        {
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) => g[w, h] = value);
        }

        public static void DsGridShuffle(int index)
        {
            Extensions.Shuffle(GetGrid(index)._source);
        }

        public static void DsGridSort(int index, int column, bool ascending)
        {
            var src = GetGrid(index)._source;
            if (column < 0 || column >= src.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(column));

            var height = src.GetLength(1);
            var temp = new TsObject[height];
            for (var i = 0; i < height; i++)
                temp[i] = src[column, i];

            Array.Sort(temp);
            for (var i = 0; i < height; i++)
                src[column, i] = temp[i];
        }

        public static bool DsGridValueDiskExists(int index, int xm, int ym, int r, TsObject val)
        {
            return GetGrid(index).InDisk(xm, ym, r, val);
        }

        public static int DsGridValueDiskX(int index, int xm, int ym, int r, TsObject val)
        {
            var grid = GetGrid(index);
            if(grid.InDisk(xm, ym, r, val, out var result, (w, h) => w))
                return result;
            return 0;
        }

        public static int DsGridValueDiskY(int index, int xm, int ym, int r, TsObject val)
        {
            var grid = GetGrid(index);
            if (grid.InDisk(xm, ym, r, val, out var result, (w, h) => h))
                return result;
            return 0;
        }

        public static bool DsGridValueExists(int index, int x1, int y1, int x2, int y2, TsObject val)
        {
            return GetGrid(index).InRegion(x1, y1, x2, y2, val);
        }

        public static int DsGridValueX(int index, int x1, int y1, int x2, int y2, TsObject val)
        {
            var grid = GetGrid(index);
            if (grid.InRegion(x1, y1, x2, y2, val, out var result, (w, h) => w))
                return result;
            return 0;
        }

        public static int DsGridValueY(int index, int x1, int y1, int x2, int y2, TsObject val)
        {
            var grid = GetGrid(index);
            if (grid.InRegion(x1, y1, x2, y2, val, out var result, (w, h) => h))
                return result;
            return 0;
        }

        public static int DsGridWidth(int index)
        {
            return GetGrid(index)._source.GetLength(0);
        }

        public static void DsGridSetGridRegion(DsGrid dst, DsGrid src, int x1, int y1, int x2, int y2, int xpos, int ypos)
        {
            var xLength = x2 - x1;
            var yLength = y2 - y1;
            var srcXLength = src._source.GetLength(0);
            var dstXLength = dst._source.GetLength(0);

            if (x1 < 0)
                throw new ArgumentOutOfRangeException("x1");
            else if (x2 >= srcXLength)
                throw new ArgumentOutOfRangeException("x2");
            else if (y1 < 0)
                throw new ArgumentOutOfRangeException("y1");
            else if (y2 >= src._source.GetLength(1))
                throw new ArgumentOutOfRangeException("y2");
            else if (xpos < 0 || xpos + xLength >= dstXLength)
                throw new ArgumentOutOfRangeException("xpos");
            else if (ypos < 0 || ypos + yLength >= dst._source.GetLength(1))
                throw new ArgumentOutOfRangeException("ypos");

            var srcOffset = y1 * src._source.GetLength(0) + x1;
            var dstOffset = ypos * dst._source.GetLength(0) + xpos;
            for (var h = y1; h <= y2; h++)
            {
                Array.Copy(src._source, srcOffset, dst._source, dstOffset, xLength);
                srcOffset += srcXLength;
                dstOffset += dstXLength;
            }
        }

        public static DsGrid GetGrid(int index)
        {
            if (index < 0 || index >= _grids.Count || _grids[index] == null)
                throw new ArgumentOutOfRangeException("index");
            return _grids[index];
        }

        public void OverRegion(int x1, int y1, int x2, int y2, Action<int, int, DsGrid> action)
        {
            if (x1 < 0)
                throw new ArgumentOutOfRangeException("x1");
            else if (x2 >= _source.GetLength(0))
                throw new ArgumentOutOfRangeException("x2");
            else if (y1 < 0)
                throw new ArgumentOutOfRangeException("y1");
            else if (y2 >= _source.GetLength(1))
                throw new ArgumentOutOfRangeException("y2");

            for (var w = x1; w <= x2; w++)
                for (var h = y1; h <= y2; h++)
                    action(w, h, this);
        }

        public void OverDisk(int x, int y, int radius, Action<int, int, DsGrid> action)
        {
            var xEnd = x + radius;
            var yEnd = x + radius;
            var xStart = x - radius;
            var yStart = y - radius;
            if (xStart < 0 || xEnd >= _source.GetLength(0))
                throw new ArgumentOutOfRangeException("xm");
            else if (yStart < 0 || yEnd >= _source.GetLength(1))
                throw new ArgumentOutOfRangeException("ym");

            var r2 = radius * radius;

            for(var w = -radius; w <= radius; w++)
                for(var h = -radius; h <= radius; h++)
                    if ((w * w) + (h * h) <= r2)
                        action(w + x, h + y, this);
        }

        public bool InDisk(int xm, int ym, int radius, TsObject val)
        {
            var xEnd = xm + radius;
            var yEnd = xm + radius;
            var xStart = xm - radius;
            var yStart = ym - radius;
            if (xStart < 0 || xEnd >= _source.GetLength(0))
                throw new ArgumentOutOfRangeException("xm");
            else if (yStart < 0 || yEnd >= _source.GetLength(1))
                throw new ArgumentOutOfRangeException("ym");

            var r2 = radius * radius;

            for (var w = -radius; w <= radius; w++)
            {
                for (var h = -radius; h <= radius; h++)
                {
                    if ((w * w) + (h * h) <= r2)
                    {
                        if (_source[w + xm, h + ym] == val)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool InDisk<T>(int xm, int ym, int radius, TsObject val, out T result, Func<int, int, T> setter)
        {
            var xEnd = xm + radius;
            var yEnd = xm + radius;
            var xStart = xm - radius;
            var yStart = ym - radius;
            if (xStart < 0 || xEnd >= _source.GetLength(0))
                throw new ArgumentOutOfRangeException("xm");
            else if (yStart < 0 || yEnd >= _source.GetLength(1))
                throw new ArgumentOutOfRangeException("ym");

            var r2 = radius * radius;
            result = default(T);

            for (var w = -radius; w <= radius; w++)
            {
                for (var h = -radius; h <= radius; h++)
                {
                    if ((w * w) + (h * h) <= r2)
                    {
                        var ex = w + xm;
                        var ey = h + ym;
                        if (_source[ex, ey] == val)
                        {
                            result = setter(ex, ey);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool InRegion(int x1, int y1, int x2, int y2, TsObject val)
        {
            if (x1 < 0)
                throw new ArgumentOutOfRangeException("x1");
            else if (x2 >= _source.GetLength(0))
                throw new ArgumentOutOfRangeException("x2");
            else if (y1 < 0)
                throw new ArgumentOutOfRangeException("y1");
            else if (y2 >= _source.GetLength(1))
                throw new ArgumentOutOfRangeException("y2");

            for (var w = x1; w <= x2; w++)
                for (var h = y1; h <= y2; h++)
                    if (_source[w, h] == val)
                        return true;

            return false;
        }

        public bool InRegion<T>(int x1, int y1, int x2, int y2, TsObject val, out T result, Func<int, int, T> setter)
        {
            if (x1 < 0)
                throw new ArgumentOutOfRangeException("x1");
            else if (x2 >= _source.GetLength(0))
                throw new ArgumentOutOfRangeException("x2");
            else if (y1 < 0)
                throw new ArgumentOutOfRangeException("y1");
            else if (y2 >= _source.GetLength(1))
                throw new ArgumentOutOfRangeException("y2");

            result = default(T);
            for (var w = x1; w <= x2; w++)
            {
                for (var h = y1; h <= y2; h++)
                {
                    if (_source[w, h] == val)
                    {
                        result = setter(w, h);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
