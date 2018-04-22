using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;

namespace TaffyScript
{
    /// <summary>
    /// Basic Grid implementation attempting to keep the same api as the ds_grid from Gamemaker.
    /// </summary>
    public class DsGrid
    {
        private static readonly ClassBinder<Grid<TsObject>> _grids = new ClassBinder<Grid<TsObject>>();

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
            _grids[index][x, y] += value;
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
            _grids[index].OverDisk(xm, ym, r, (w, h, g) => g[w, h] += value);
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
            var dest = _grids[index];
            var src = _grids[source];
            var xLength = x2 - x1;
            var yLength = y2 - y1;

            if (x1 < 0)
                throw new ArgumentOutOfRangeException("x1");
            else if (x2 >= src.Width)
                throw new ArgumentOutOfRangeException("x2");
            else if (y1 < 0)
                throw new ArgumentOutOfRangeException("y1");
            else if (y2 >= src.Height)
                throw new ArgumentOutOfRangeException("y2");
            else if (xpos < 0 || xpos + xLength >= dest.Width)
                throw new ArgumentOutOfRangeException("xpos");
            else if (ypos < 0 || ypos + yLength >= dest.Height)
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
            _grids[index].OverRegion(x1, y1, x2, y2, (w, h, g) => g[w, h] += value);
        }

        /// <summary>
        /// Sets all of the values in a grid to the specified value.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="value">The new value.</param>
        public static void DsGridClear(int index, TsObject value)
        {
            var grid = _grids[index];
            for(var w = 0; w < grid.Width; w++)
            {
                for(var h = 0; h < grid.Height; h++)
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
            var dst = _grids[destination];
            var src = _grids[source];
            var xLength = Math.Min(dst.Width, src.Width) - 1;
            var yLength = Math.Min(dst.Height, src.Height) - 1;
            dst.SetGridRegion(src, 0, 0, xLength, yLength, 0, 0);
        }

        /// <summary>
        /// Creates a grid with the specified width and height.
        /// </summary>
        /// <param name="w">Grid width</param>
        /// <param name="h">Grid height</param>
        /// <returns>Grid id</returns>
        public static int DsGridCreate(int w, int h)
        {   
            return _grids.Add(new Grid<TsObject>(w, h));
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

            _grids.Remove(index);
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
            return _grids[index][x, y];
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
            _grids[index].OverDisk(xm, ym, r, (w, h, g) =>
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
            _grids[index].OverDisk(xm, ym, r, (w, h, g) =>
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
            _grids[index].OverDisk(xm, ym, r, (w, h, g) =>
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
            _grids[index].OverDisk(xm, ym, r, (w, h, g) =>
            {
                mean += g[w, h];
            });
            return mean;
        }

        public static Grid<TsObject> DsGridGetGrid(int id)
        {
            return _grids[id];
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
            _grids[index].OverRegion(x1, y1, x2, y2, (w, h, g) =>
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
            _grids[index].OverRegion(x1, y1, x2, y2, (w, h, g) =>
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
            _grids[index].OverRegion(x1, y1, x2, y2, (w, h, g) =>
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
            _grids[index].OverRegion(x1, y1, x2, y2, (w, h, g) =>
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
            return _grids[index].Height;
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
            _grids[index][x, y] *= value;
        }

        /// <summary>
        /// Multiplies a value with every object within a disk.
        /// </summary>
        /// <param name="index">Grid Id</param>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="r">The radius of the disk</param>
        /// <param name="obj">The value to multiply.</param>
        public static void DsGridMultiplyDisk(int index, int xm, int ym, int r, TsObject obj)
        {
            _grids[index].OverDisk(xm, ym, r, (w, h, g) => g[w, h] *= obj);
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
            var dest = _grids[index];
            var src = _grids[source];
            var xLength = x2 - x1;
            var yLength = y2 - y1;

            if (x1 < 0)
                throw new ArgumentOutOfRangeException("x1");
            else if (x2 >= src.Width)
                throw new ArgumentOutOfRangeException("x2");
            else if (y1 < 0)
                throw new ArgumentOutOfRangeException("y1");
            else if (y2 >= src.Height)
                throw new ArgumentOutOfRangeException("y2");
            else if (xpos < 0 || xpos + xLength >= dest.Width)
                throw new ArgumentOutOfRangeException("xpos");
            else if (ypos < 0 || ypos + yLength >= dest.Height)
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
            _grids[index].OverRegion(x1, y1, x2, y2, (w, h, g) => g[w, h] *= value);
        }

        /// <summary>
        /// Resizes a grid.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="w">The new width</param>
        /// <param name="h">The new height</param>
        public static void DsGridResize(int index, int w, int h)
        {
            _grids[index].Resize(w, h);
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
            _grids[index][x, y] = value;
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
            _grids[index].OverDisk(xm, ym, r, (w, h, g) => g[w, h] = value);
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
            var dest = _grids[index];
            var src = _grids[source];
            dest.SetGridRegion(src, x1, y1, x2, y2, xpos, ypos);
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
            _grids[index].OverRegion(x1, y1, x2, y2, (w, h, g) => g[w, h] = value);
        }

        /// <summary>
        /// Shuffles all of the values within a grid.
        /// </summary>
        /// <param name="index">Grid id</param>
        public static void DsGridShuffle(int index)
        {
            _grids[index].Shuffle();
        }

        /// <summary>
        /// Sorts a column within a grid.
        /// </summary>
        /// <param name="index">Grid id</param>
        /// <param name="column">Column index</param>
        /// <param name="ascending">Whether the values should be sorted in ascending or descending order.</param>
        public static void DsGridSort(int index, int column, bool ascending)
        {
            _grids[index].Sort(column, ascending);
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
            return _grids[index].InDisk(xm, ym, r, val);
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
            var grid = _grids[index];
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
            var grid = _grids[index];
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
            return _grids[index].InRegion(x1, y1, x2, y2, val);
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
            var grid = _grids[index];
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
            var grid = _grids[index];
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
            return _grids[index].Width;
        }
    }
}
