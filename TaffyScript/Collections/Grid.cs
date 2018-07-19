using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    public class Grid<T>
    {
        private T[,] _source;

        /// <summary>
        /// Get or set the value at the given location within a Grid.
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <returns>The value at the location.</returns>
        public T this[int x, int y]
        {
            get => _source[x, y];
            set => _source[x, y] = value;
        }

        public int Width => _source.GetLength(0);
        public int Height => _source.GetLength(1);

        public Grid(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            _source = new T[width, height];
        }

        public void SetGridRegion(Grid<T> src, int x1, int y1, int x2, int y2, int xpos, int ypos)
        {
            var xLength = x2 - x1;
            var yLength = y2 - y1;

            if (x1 < 0)
                throw new ArgumentOutOfRangeException("x1");
            else if (x2 >= src.Width)
                throw new ArgumentOutOfRangeException("x2");
            else if (y1 < 0)
                throw new ArgumentOutOfRangeException("y1");
            else if (y2 >= src._source.GetLength(1))
                throw new ArgumentOutOfRangeException("y2");
            else if (xpos < 0 || xpos + xLength >= Width)
                throw new ArgumentOutOfRangeException("xpos");
            else if (ypos < 0 || ypos + yLength >= Height)
                throw new ArgumentOutOfRangeException("ypos");

            var srcOffset = y1 * src.Width + x1;
            var dstOffset = ypos * Width + xpos;
            for (var h = y1; h <= y2; h++)
            {
                Array.Copy(src._source, srcOffset, _source, dstOffset, xLength);
                srcOffset += src.Width;
                dstOffset += Width;
            }
        }

        public void Clear(T value)
        {
            for (var h = 0; h < Height; h++)
                for (var w = 0; w < Width; w++)
                    _source[w, h] = value;
        } 

        public void Shuffle()
        {
            Extensions.Shuffle(_source);
        }

        public void Sort(int column, bool ascending)
        {
            if (column < 0 || column >= Width)
                throw new ArgumentOutOfRangeException(nameof(column));
            
            var temp = new T[Height];
            for (var i = 0; i < Height; i++)
                temp[i] = _source[column, i];

            Array.Sort(temp);
            for (var i = 0; i < Height; i++)
                _source[column, i] = temp[i];
        }

        /// <summary>
        /// Performs an action on each value in a region.
        /// </summary>
        /// <param name="x1">The left position of the region</param>
        /// <param name="y1">The top position of the region</param>
        /// <param name="x2">The right position of the region</param>
        /// <param name="y2">The bottom position of the region</param>
        /// <param name="action">The action to perform (Takes in x-position, y-position, and the grid calling the action)</param>
        public void OverRegion(int x1, int y1, int x2, int y2, Action<int, int, Grid<T>> action)
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
        /// <param name="radius">The radius of the disk</param>
        /// <param name="action">The action to perform (Takes in x-position, y-position, and the grid calling the action)</param>
        public void OverDisk(int xm, int ym, int radius, Action<int, int, Grid<T>> action)
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
                for (var h = -radius; h <= radius; h++)
                    if ((w * w) + (h * h) <= r2)
                        action(w + xm, h + ym, this);
        }

        /// <summary>
        /// Determines if a value is within a disk.
        /// </summary>
        /// <param name="xm">The x position of the center of the disk</param>
        /// <param name="ym">The y position of the center of the disk</param>
        /// <param name="radius">The radius of the disk</param>
        /// <param name="val">The value to find</param>
        /// <returns></returns>
        public bool InDisk(int xm, int ym, int radius, T val)
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
                        if (_source[w + xm, h + ym].Equals(val))
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
        /// <param name="radius">The radius of the disk</param>
        /// <param name="val">The value to find</param>
        /// <param name="result">The result of setter</param>
        /// <param name="setter">The action to perform if the value is found.</param>
        /// <returns></returns>
        public bool InDisk<TResult>(int xm, int ym, int radius, TsObject val, out TResult result, Func<int, int, TResult> setter)
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
            result = default(TResult);

            for (var w = -radius; w <= radius; w++)
            {
                for (var h = -radius; h <= radius; h++)
                {
                    if ((w * w) + (h * h) <= r2)
                    {
                        var ex = w + xm;
                        var ey = h + ym;
                        if (_source[ex, ey].Equals(val))
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
                    if (_source[w, h].Equals(val))
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
        public bool InRegion<TResult>(int x1, int y1, int x2, int y2, TsObject val, out TResult result, Func<int, int, TResult> setter)
        {
            if (x1 < 0)
                throw new ArgumentOutOfRangeException("x1");
            else if (x2 >= _source.GetLength(0))
                throw new ArgumentOutOfRangeException("x2");
            else if (y1 < 0)
                throw new ArgumentOutOfRangeException("y1");
            else if (y2 >= _source.GetLength(1))
                throw new ArgumentOutOfRangeException("y2");

            result = default(TResult);
            for (var w = x1; w <= x2; w++)
            {
                for (var h = y1; h <= y2; h++)
                {
                    if (_source[w, h].Equals(val))
                    {
                        result = setter(w, h);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Resizes the grid to the new width and height.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Resize(int width, int height)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(width));
            else if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            var temp = new TsObject[width, height];
            var zero = new TsObject(0);

            for (var h = 0; h < height; ++h)
                for (var w = 0; w < width; ++w)
                    temp[w, h] = 0;

            var srcWidth = _source.GetLength(0);
            var copyWidth = System.Math.Min(srcWidth, width);
            var copyHeight = System.Math.Min(_source.GetLength(1), height);

            var srcOffset = 0;
            var dstOffset = 0;

            for (var h = 0; h < copyHeight; ++h)
            {
                Array.Copy(_source, srcOffset, temp, dstOffset, copyWidth);
                srcOffset += srcWidth;
                dstOffset += width;
            }
        }
    }
}
