using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Basic Grid implementation attempting to keep the same api as the ds_grid from Gamemaker.
    /// </summary>
    public class DsGrid
    {
        private readonly static List<DsGrid> _grids = new List<DsGrid>();
        private readonly static Queue<int> _gridSlots = new Queue<int>();

        private TsObject[,] _source;

        /// <summary>
        /// Get or set the value at the given location within a Grid.
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <returns>The value at the location.</returns>
        public TsObject this[int x, int y]
        {
            get => _source[x, y];
            set => _source[x, y] = value;
        }

        private DsGrid(int width, int height)
        {
            _source = new TsObject[width, height];
        }

        /// <summary>
        /// Adds a value to the object at the given location.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="value">The value to add.</param>
        public static void DsGridAdd(int index, int x, int y, TsObject value)
        {
            GetGrid(index)[x, y] += value;
        }

        /// <summary>
        /// Adds a value to every object within a disk.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <param name="value">The value to add.</param>
        public static void DsGridAddDisk(int index, int xm, int ym, int r, TsObject value)
        {
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) => g[w, h] += value);
        }

        /// <summary>
        /// Adds all of the values in a source grid to the values in a destination grid.
        /// </summary>
        /// <param name="index">The id of the destination grid</param>
        /// <param name="source">The id of the source grid</param>
        /// <param name="x1">The left position of the source region to copy</param>
        /// <param name="y1">The top position of the source region to copy</param>
        /// <param name="x2">The right position of the source region to copy</param>
        /// <param name="y2">The bottom position of the source region to copy</param>
        /// <param name="xpos">The x position in the destination grid to add the source region to.</param>
        /// <param name="ypos">The y position in the destination grid to add the source region to.</param>
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

        /// <summary>
        /// Adds a value to every object within a region.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <param name="value">The value to add.</param>
        public static void DsGridAddRegion(int index, int x1, int y1, int x2, int y2, TsObject value)
        {
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) => g[w, h] += value);
        }

        /// <summary>
        /// Sets all of the values in a grid to the specified value.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="value">The new value.</param>
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

        /// <summary>
        /// Copies one grid into another.
        /// </summary>
        /// <param name="destination">The destination grid id</param>
        /// <param name="source">The source grid id</param>
        public static void DsGridCopy(int destination, int source)
        {
            var dst = GetGrid(destination);
            var src = GetGrid(source);
            var xLength = Math.Max(dst._source.GetLength(0), src._source.GetLength(0));
            var yLength = Math.Min(dst._source.GetLength(1), src._source.GetLength(1));
            DsGridSetGridRegion(dst, src, 0, 0, xLength - 1, yLength - 1, 0, 0);
        }

        /// <summary>
        /// Creates a grid with the specified width and height.
        /// </summary>
        /// <param name="w">Grid width</param>
        /// <param name="h">Grid height</param>
        /// <returns>Grid id</returns>
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

        /// <summary>
        /// Destroys a previously created grid.
        /// </summary>
        /// <param name="index">Grid id</param>
        public static void DsGridDestroy(int index)
        {
            if (index < 0 || index >= _grids.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (_grids[index] == null)
                throw new DataStructureDestroyedException("grid", index);

            _grids[index] = null;
            _gridSlots.Enqueue(index);
        }

        /// <summary>
        /// Gets the value in the grid at the specified position.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <returns>The retrieved value.</returns>
        public static TsObject DsGridGet(int index, int x, int y)
        {
            return GetGrid(index)[x, y];
        }

        /// <summary>
        /// Gets the maximum value within a disk in the grid.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="xm"></param>
        /// <param name="ym"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static TsObject DsGridGetDiskMax(int index, int xm, int ym, int r)
        {
            TsObject i = TsObject.Empty();
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) =>
            {
                var set = false;
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

        /// <summary>
        /// Gets the average value within a disk in the grid.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the minimum value within a disk in the grid.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the sum of all values within a disk in the grid.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <returns></returns>
        public static TsObject DsGridGetDiskSum(int index, int xm, int ym, int r)
        {
            var mean = new TsObject(0);
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) =>
            {
                mean += g[w, h];
            });
            return mean;
        }

        /// <summary>
        /// Gets the maximum value within a region.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the average value within a region.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the minimum value within a region.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the sum of all values within a region.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <returns></returns>
        public static TsObject DsGridGetSum(int index, int x1, int y1, int x2, int y2)
        {
            var mean = new TsObject(0);
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) =>
            {
                mean += g[w, h];
            });
            return mean;
        }

        /// <summary>
        /// Gets the height of a grid.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <returns>Height</returns>
        public static int DsGridHeight(int index)
        {
            return GetGrid(index)._source.GetLength(1);
        }

        /// <summary>
        /// Multiplies a value with the object at the given location.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="value">The value to multiply.</param>
        public static void DsGridMultiply(int index, int x, int y, TsObject value)
        {
            GetGrid(index)[x, y] *= value;
        }

        /// <summary>
        /// Multiplies a value with every object within a disk.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <param name="value">The value to multiply.</param>
        public static void DsGridMultiplyDisk(int index, int xm, int ym, int r, TsObject obj)
        {
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) => g[w, h] *= obj);
        }

        /// <summary>
        /// Multiplies all of the values in a source grid with the values in a destination grid.
        /// </summary>
        /// <param name="index">The id of the destination grid</param>
        /// <param name="source">The id of the source grid</param>
        /// <param name="x1">The left position of the source region to copy</param>
        /// <param name="y1">The top position of the source region to copy</param>
        /// <param name="x2">The right position of the source region to copy</param>
        /// <param name="y2">The bottom position of the source region to copy</param>
        /// <param name="xpos">The x position in the destination grid to add the source region to.</param>
        /// <param name="ypos">The y position in the destination grid to add the source region to.</param>
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

        /// <summary>
        /// Multiplies a value with every object within a region.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <param name="value">The value to multiply.</param>
        public static void DsGridMultiplyRegion(int index, int x1, int y1, int x2, int y2, TsObject value)
        {
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) => g[w, h] *= value);
        }

        /// <summary>
        /// Resizes a grid.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="w">The new width</param>
        /// <param name="h">The new height</param>
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

        /// <summary>
        /// Sets the value at the given location.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="value">The value to set</param>
        public static void DsGridSet(int index, int x, int y, TsObject value)
        {
            GetGrid(index)[x, y] = value;
        }

        /// <summary>
        /// Sets every location within a disk to a value.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <param name="value">The value to set</param>
        public static void DsGridSetDisk(int index, int xm, int ym, int r, TsObject value)
        {
            GetGrid(index).OverDisk(xm, ym, r, (w, h, g) => g[w, h] = value);
        }

        /// <summary>
        /// Copies the contents in a source grid to a destination grid.
        /// </summary>
        /// <param name="index">The id of the destination grid</param>
        /// <param name="source">The id of the source grid</param>
        /// <param name="x1">The left position of the source region to copy</param>
        /// <param name="y1">The top position of the source region to copy</param>
        /// <param name="x2">The right position of the source region to copy</param>
        /// <param name="y2">The bottom position of the source region to copy</param>
        /// <param name="xpos">The x position in the destination grid to add the source region to.</param>
        /// <param name="ypos">The y position in the destination grid to add the source region to.</param>
        public static void DsGridSetGridRegion(int index, int source, int x1, int y1, int x2, int y2, int xpos, int ypos)
        {
            var dest = GetGrid(index);
            var src = GetGrid(source);
            DsGridSetGridRegion(dest, src, x1, y1, x2, y2, xpos, ypos);
        }

        /// <summary>
        /// Sets every location within a region to a value.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <param name="value">The value to set.</param>
        public static void DsGridSetRegion(int index, int x1, int y1, int x2, int y2, TsObject value)
        {
            GetGrid(index).OverRegion(x1, y1, x2, y2, (w, h, g) => g[w, h] = value);
        }

        /// <summary>
        /// Shuffles all of the values within a grid.
        /// </summary>
        /// <param name="index">Grid id</param>
        public static void DsGridShuffle(int index)
        {
            Extensions.Shuffle(GetGrid(index)._source);
        }

        /// <summary>
        /// Sorts a column within a grid.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="column">Column index</param>
        /// <param name="ascending">Whether the values should be sorted in ascending or descending order.</param>
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

        /// <summary>
        /// Determines if a value exists within a disk.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <param name="val">The value to find</param>
        /// <returns></returns>
        public static bool DsGridValueDiskExists(int index, int xm, int ym, int r, TsObject val)
        {
            return GetGrid(index).InDisk(xm, ym, r, val);
        }

        /// <summary>
        /// Gets the x position of a value within a disk.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <param name="val">The value to find</param>
        /// <returns></returns>
        public static int DsGridValueDiskX(int index, int xm, int ym, int r, TsObject val)
        {
            var grid = GetGrid(index);
            if(grid.InDisk(xm, ym, r, val, out var result, (w, h) => w))
                return result;
            return 0;
        }

        /// <summary>
        /// Gets the y position of a value within a disk.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <param name="val">The value to find</param>
        /// <returns></returns>
        public static int DsGridValueDiskY(int index, int xm, int ym, int r, TsObject val)
        {
            var grid = GetGrid(index);
            if (grid.InDisk(xm, ym, r, val, out var result, (w, h) => h))
                return result;
            return 0;
        }

        /// <summary>
        /// Determines if a value exists within a region.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool DsGridValueExists(int index, int x1, int y1, int x2, int y2, TsObject val)
        {
            return GetGrid(index).InRegion(x1, y1, x2, y2, val);
        }

        /// <summary>
        /// Gets the x position of a value within a region.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <param name="val">The value to find</param>
        /// <returns></returns>
        public static int DsGridValueX(int index, int x1, int y1, int x2, int y2, TsObject val)
        {
            var grid = GetGrid(index);
            if (grid.InRegion(x1, y1, x2, y2, val, out var result, (w, h) => w))
                return result;
            return 0;
        }

        /// <summary>
        /// Gets the y position of a value within a region.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <param name="val">The value to find</param>
        /// <returns></returns>
        public static int DsGridValueY(int index, int x1, int y1, int x2, int y2, TsObject val)
        {
            var grid = GetGrid(index);
            if (grid.InRegion(x1, y1, x2, y2, val, out var result, (w, h) => h))
                return result;
            return 0;
        }

        /// <summary>
        /// Gets the width of a grid.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <returns></returns>
        public static int DsGridWidth(int index)
        {
            return GetGrid(index)._source.GetLength(0);
        }

        /// <summary>
        /// Copies the contents in a source grid to a destination grid.
        /// </summary>
        /// <param name="index">The destination grid</param>
        /// <param name="source">The source grid</param>
        /// <param name="x1">The left position of the source region to copy</param>
        /// <param name="y1">The top position of the source region to copy</param>
        /// <param name="x2">The right position of the source region to copy</param>
        /// <param name="y2">The bottom position of the source region to copy</param>
        /// <param name="xpos">The x position in the destination grid to add the source region to.</param>
        /// <param name="ypos">The y position in the destination grid to add the source region to.</param>
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

        /// <summary>
        /// Performs an action on each value in a region.
        /// </summary>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <param name="action">The action to perform (Takes in x-position, y-position, and the grid calling the action)</param>
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

        /// <summary>
        /// Performs an action on each value within a region.
        /// </summary>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <param name="action">The action to perform (Takes in x-position, y-position, and the grid calling the action)</param>
        public void OverDisk(int xm, int ym, int radius, Action<int, int, DsGrid> action)
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

            for(var w = -radius; w <= radius; w++)
                for(var h = -radius; h <= radius; h++)
                    if ((w * w) + (h * h) <= r2)
                        action(w + xm, h + ym, this);
        }

        /// <summary>
        /// Determines if a value is within a disk.
        /// </summary>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <param name="val">The value to find</param>
        /// <returns></returns>
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

        /// <summary>
        /// Determines if a value is within a disk, performing an event if it is found.
        /// </summary>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <param name="val">The value to find</param>
        /// <param name="result">The result of setter</param>
        /// <param name="setter">The action to perform if the value is found.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Determines if a value is within a region.
        /// </summary>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <param name="val">The value to find</param>
        /// <returns></returns>
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

        /// <summary>
        /// Determines if a value is within a region, performing an action if it is.
        /// </summary>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <param name="val">The value to find</param>
        /// <param name="result">The result of setter</param>
        /// <param name="setter">The action to perform if the value is found</param>
        /// <returns></returns>
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
