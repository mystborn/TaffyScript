using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class TsGrid : ITsInstance
    {
        private Grid<TsObject> _source;

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "ds_grid";
        public Grid<TsObject> Source => _source;

        public TsGrid(TsObject[] args)
        {
            _source = new Grid<TsObject>((int)args[0], (int)args[1]);
        }

        public TsGrid(Grid<TsObject> grid)
        {
            _source = new Grid<TsObject>(grid.Width, grid.Height);
            _source.SetGridRegion(grid, 0, 0, grid.Width - 1, grid.Height - 1, 0, 0);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "get":
                    return _source[(int)args[0], (int)args[1]];
                case "set":
                    _source[(int)args[0], (int)args[1]] = args[2];
                    break;
                case "add_disk":
                    _source.OverDisk((int)args[0], (int)args[1], (int)args[2], (w, h, g) => g[w, h] += args[3]);
                    break;
                case "add_grid_region":
                    return add_grid_region(null, args);
                case "add_region":
                    _source.OverRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], (w, h, g) => g[w, h] += args[4]);
                    break;
                case "clear":
                    _source.Clear(args[0]);
                    break;
                case "copy":
                    return new TsGrid(_source);
                case "get_disk_max":
                    return get_disk_max(null, args);
                case "get_disk_mean":
                    return get_disk_mean(null, args);
                case "get_disk_min":
                    return get_disk_min(null, args);
                case "get_disk_sum":
                    return get_disk_sum(null, args);
                case "get_region_max":
                    return get_region_max(null, args);
                case "get_region_mean":
                    return get_region_mean(null, args);
                case "get_region_min":
                    return get_region_min(null, args);
                case "get_region_sum":
                    return get_region_sum(null, args);
                case "multiply_disk":
                    _source.OverDisk((int)args[0], (int)args[1], (int)args[2], (w, h, g) => g[w, h] *= args[3]);
                    break;
                case "multiply_grid_region":
                    return multiply_grid_region(null, args);
                case "multiply_region":
                    _source.OverRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], (w, h, g) => g[w, h] *= args[4]);
                    break;
                case "resize":
                    _source.Resize((int)args[0], (int)args[1]);
                    break;
                case "set_disk":
                    _source.OverDisk((int)args[0], (int)args[1], (int)args[2], (w, h, g) => g[w, h] *= args[3]);
                    break;
                case "set_grid_region":
                    return set_grid_region(null, args);
                case "set_region":
                    _source.OverRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], (w, h, g) => g[w, h] = args[4]);
                    break;
                case "shuffle":
                    _source.Shuffle();
                    break;
                case "sort":
                    _source.Sort((int)args[0], (bool)args[1]);
                    break;
                case "value_exists_in_disk":
                    return _source.InDisk((int)args[0], (int)args[1], (int)args[2], args[3]);
                case "value_position_in_disk":
                    if (_source.InDisk((int)args[0], (int)args[1], (int)args[2], args[3], out var result, (w, h) => new TsObject[] { w, h }))
                        return result;
                    break;
                case "value_exists_in_region":
                    return _source.InRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], args[4]);
                case "value_position_in_region":
                    if (_source.InRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], args[4], out result, (w, h) => new TsObject[] { w, h }))
                        return result;
                    break;
                default:
                    throw new MemberAccessException($"The type {ObjectType} does not define a script called {scriptName}");
            }
            return TsObject.Empty();
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MemberAccessException($"The type {ObjectType} does not define a script called {scriptName}");
        }

        public TsObject GetMember(string name)
        {
            switch (name)
            {
                case "width":
                    return _source.Width;
                case "height":
                    return _source.Height;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MemberAccessException($"Couldn't find member with the name {name}");
            }
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MemberAccessException($"Member {name} on type {ObjectType} is readonly");
        }

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            switch (delegateName)
            {

                case "get":
                    del = new TsDelegate(get, "get", this);
                    return true;
                case "set":
                    del = new TsDelegate(set, "set", this);
                    return true;
                case "add_disk":
                    del = new TsDelegate(add_disk, "add_disk", this);
                    return true;
                case "add_grid_region":
                    del = new TsDelegate(add_grid_region, "add_grid_region", this);
                    return true;
                case "add_region":
                    del = new TsDelegate(add_region, "add_region", this);
                    return true;
                case "clear":
                    del = new TsDelegate(clear, "clear", this);
                    return true;
                case "copy":
                    del = new TsDelegate(copy, "copy", this);
                    return true;
                case "get_disk_max":
                    del = new TsDelegate(get_disk_max, "get_disk_max", this);
                    return true;
                case "get_disk_mean":
                    del = new TsDelegate(get_disk_mean, "get_disk_mean", this);
                    return true;
                case "get_disk_min":
                    del = new TsDelegate(get_disk_min, "get_disk_min", this);
                    return true;
                case "get_disk_sum":
                    del = new TsDelegate(get_disk_sum, "get_disk_sum", this);
                    return true;
                case "get_region_max":
                    del = new TsDelegate(get_region_max, "get_region_max", this);
                    return true;
                case "get_region_mean":
                    del = new TsDelegate(get_region_mean, "get_region_mean", this);
                    return true;
                case "get_region_min":
                    del = new TsDelegate(get_region_min, "get_region_min", this);
                    return true;
                case "get_region_sum":
                    del = new TsDelegate(get_region_sum, "get_region_sum", this);
                    return true;
                case "multiply_disk":
                    del = new TsDelegate(multiply_disk, "multiply_disk", this);
                    return true;
                case "multiply_grid_region":
                    del = new TsDelegate(multiply_grid_region, "multiply_grid_region", this);
                    return true;
                case "multiply_region":
                    del = new TsDelegate(multiply_region, "multiply_region", this);
                    return true;
                case "resize":
                    del = new TsDelegate(resize, "resize", this);
                    return true;
                case "set_disk":
                    del = new TsDelegate(set_disk, "set_disk", this);
                    return true;
                case "set_grid_region":
                    del = new TsDelegate(set_grid_region, "set_grid_region", this);
                    return true;
                case "set_region":
                    del = new TsDelegate(set_region, "set_region", this);
                    return true;
                case "shuffle":
                    del = new TsDelegate(shuffle, "shuffle", this);
                    return true;
                case "sort":
                    del = new TsDelegate(sort, "sort", this);
                    return true;
                case "value_exists_in_disk":
                    del = new TsDelegate(value_exists_in_disk, "value_exists_in_disk", this);
                    return true;
                case "value_position_in_disk":
                    del = new TsDelegate(value_position_in_disk, "value_position_in_disk", this);
                    return true;
                case "value_exists_in_region":
                    del = new TsDelegate(value_exists_in_region, "value_exists_in_region", this);
                    return true;
                case "value_position_in_region":
                    del = new TsDelegate(value_position_in_region, "value_position_in_region", this);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public static explicit operator TsGrid(TsObject obj)
        {
            return (TsGrid)obj.Value.WeakValue;
        }

        public static implicit operator TsObject(TsGrid grid)
        {
            return new TsObject(grid);
        }

#pragma warning disable IDE1006 // Naming Styles

        public TsObject get(ITsInstance inst, params TsObject[] args)
        {
            return _source[(int)args[0], (int)args[1]];
        }

        public TsObject set(ITsInstance inst, params TsObject[] args)
        {
            _source[(int)args[0], (int)args[1]] = args[2];
            return TsObject.Empty();
        }

        public TsObject add_disk(ITsInstance inst, params TsObject[] args)
        {
            _source.OverDisk((int)args[0], (int)args[1], (int)args[2], (w, h, g) => g[w, h] += args[3]);
            return TsObject.Empty();
        }

        public TsObject add_grid_region(ITsInstance inst, params TsObject[] args)
        {
            var src = ((TsGrid)args[0]).Source;
            var x1 = (int)args[1];
            var y1 = (int)args[2];
            var x2 = (int)args[2];
            var y2 = (int)args[3];
            var xPos = (int)args[4];
            var yPos = (int)args[5];
            var xLength = x2 - x1;
            var yLength = y2 - y1;

            if (x1 < 0)
                throw new ArgumentOutOfRangeException(nameof(x1));
            if (x2 >= src.Width)
                throw new ArgumentOutOfRangeException(nameof(x2));
            if (y1 < 0)
                throw new ArgumentOutOfRangeException(nameof(y1));
            if (y2 >= src.Height)
                throw new ArgumentOutOfRangeException(nameof(y2));
            if (xPos < 0 || xPos + xLength >= _source.Width)
                throw new ArgumentOutOfRangeException(nameof(xPos));
            if (yPos < 0 || yPos + yLength >= _source.Height)
                throw new ArgumentOutOfRangeException(nameof(yPos));

            for (var w = 0; w < xLength; w++)
            {
                for (var h = 0; h < yLength; h++)
                {
                    _source[xPos + w, yPos + h] += src[x1 + w, y1 + h];
                }
            }

            return TsObject.Empty();
        }

        public TsObject add_region(ITsInstance inst, params TsObject[] args)
        {
            _source.OverRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], (w, h, g) => g[w, h] += args[4]);
            return TsObject.Empty();
        }

        public TsObject clear(ITsInstance inst, params TsObject[] args)
        {
            _source.Clear(args[0]);
            return TsObject.Empty();
        }

        public TsObject copy(ITsInstance inst, params TsObject[] args)
        {
            return new TsGrid(_source);
        }

        public TsObject get_disk_max(ITsInstance inst, params TsObject[] args)
        {
            TsObject i = TsObject.Empty();
            var set = false;
            _source.OverDisk((int)args[0], (int)args[1], (int)args[2], (w, h, g) =>
            {
                if (!set)
                {
                    i = g[w, h];
                    set = true;
                }
                else
                {
                    var current = g[w, h];
                    if (current > i)
                        i = current;
                }
            });
            return i;
        }

        public TsObject get_disk_mean(ITsInstance inst, params TsObject[] args)
        {
            var mean = new TsObject(0);
            var iter = 0;
            _source.OverDisk((int)args[0], (int)args[1], (int)args[2], (w, h, g) =>
            {
                iter++;
                mean += g[w, h];
            });
            return mean / iter;
        }

        public TsObject get_disk_min(ITsInstance inst, params TsObject[] args)
        {
            bool set = false;
            TsObject i = TsObject.Empty();
            _source.OverDisk((int)args[0], (int)args[1], (int)args[2], (w, h, g) =>
            {
                if (!set)
                {
                    i = g[w, h];
                    set = true;
                }
                else
                {
                    var current = g[w, h];
                    if (current < i)
                        i = current;
                }
            });
            return i;
        }

        public TsObject get_disk_sum(ITsInstance inst, params TsObject[] args)
        {
            var mean = new TsObject(0);
            _source.OverDisk((int)args[0], (int)args[1], (int)args[2], (w, h, g) =>
            {
                mean += g[w, h];
            });
            return mean;
        }

        public TsObject get_region_max(ITsInstance inst, params TsObject[] args)
        {
            TsObject i = TsObject.Empty();
            var set = false;
            _source.OverRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], (w, h, g) =>
            {
                if (!set)
                {
                    i = g[w, h];
                    set = true;
                }
                else
                {
                    var current = g[w, h];
                    if (current > i)
                        i = current;
                }
            });
            return i;
        }

        public TsObject get_region_mean(ITsInstance inst, params TsObject[] args)
        {
            var mean = new TsObject(0);
            var iter = 0;
            _source.OverRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], (w, h, g) =>
            {
                iter++;
                mean += g[w, h];
            });
            return mean / iter;
        }

        public TsObject get_region_min(ITsInstance inst, params TsObject[] args)
        {
            bool set = false;
            TsObject i = TsObject.Empty();
            _source.OverRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], (w, h, g) =>
            {
                if (!set)
                {
                    i = g[w, h];
                    set = true;
                }
                else
                {
                    var current = g[w, h];
                    if (current < i)
                        i = current;
                }
            });
            return i;
        }

        public TsObject get_region_sum(ITsInstance inst, params TsObject[] args)
        {
            var sum = new TsObject(0);
            _source.OverRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], (w, h, g) =>
            {
                sum += g[w, h];
            });
            return sum;
        }

        public TsObject multiply_disk(ITsInstance inst, params TsObject[] args)
        {
            _source.OverDisk((int)args[0], (int)args[1], (int)args[2], (w, h, g) => g[w, h] *= args[3]);
            return TsObject.Empty();
        }

        public TsObject multiply_grid_region(ITsInstance inst, params TsObject[] args)
        {
            var src = ((TsGrid)args[0]).Source;
            var x1 = (int)args[1];
            var y1 = (int)args[2];
            var x2 = (int)args[2];
            var y2 = (int)args[3];
            var xPos = (int)args[4];
            var yPos = (int)args[5];
            var xLength = x2 - x1;
            var yLength = y2 - y1;

            if (x1 < 0)
                throw new ArgumentOutOfRangeException(nameof(x1));
            if (x2 >= src.Width)
                throw new ArgumentOutOfRangeException(nameof(x2));
            if (y1 < 0)
                throw new ArgumentOutOfRangeException(nameof(y1));
            if (y2 >= src.Height)
                throw new ArgumentOutOfRangeException(nameof(y2));
            if (xPos < 0 || xPos + xLength >= _source.Width)
                throw new ArgumentOutOfRangeException(nameof(xPos));
            if (yPos < 0 || yPos + yLength >= _source.Height)
                throw new ArgumentOutOfRangeException(nameof(yPos));

            for (var w = 0; w < xLength; w++)
            {
                for (var h = 0; h < yLength; h++)
                {
                    _source[xPos + w, yPos + h] *= src[x1 + w, y1 + h];
                }
            }

            return TsObject.Empty();
        }

        public TsObject multiply_region(ITsInstance inst, params TsObject[] args)
        {
            _source.OverRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], (w, h, g) => g[w, h] *= args[4]);
            return TsObject.Empty();
        }

        public TsObject set_disk(ITsInstance inst, params TsObject[] args)
        {
            _source.OverDisk((int)args[0], (int)args[1], (int)args[2], (w, h, g) => g[w, h] *= args[3]);
            return TsObject.Empty();
        }

        public TsObject set_grid_region(ITsInstance inst, params TsObject[] args)
        {
            var src = ((TsGrid)args[0]).Source;
            var x1 = (int)args[1];
            var y1 = (int)args[2];
            var x2 = (int)args[2];
            var y2 = (int)args[3];
            var xPos = (int)args[4];
            var yPos = (int)args[5];
            var xLength = x2 - x1;
            var yLength = y2 - y1;

            if (x1 < 0)
                throw new ArgumentOutOfRangeException(nameof(x1));
            if (x2 >= src.Width)
                throw new ArgumentOutOfRangeException(nameof(x2));
            if (y1 < 0)
                throw new ArgumentOutOfRangeException(nameof(y1));
            if (y2 >= src.Height)
                throw new ArgumentOutOfRangeException(nameof(y2));
            if (xPos < 0 || xPos + xLength >= _source.Width)
                throw new ArgumentOutOfRangeException(nameof(xPos));
            if (yPos < 0 || yPos + yLength >= _source.Height)
                throw new ArgumentOutOfRangeException(nameof(yPos));

            for (var w = 0; w < xLength; w++)
            {
                for (var h = 0; h < yLength; h++)
                {
                    _source[xPos + w, yPos + h] = src[x1 + w, y1 + h];
                }
            }

            return TsObject.Empty();
        }

        public TsObject set_region(ITsInstance inst, params TsObject[] args)
        {
            _source.OverRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], (w, h, g) => g[w, h] = args[4]);
            return TsObject.Empty();
        }

        public TsObject resize(ITsInstance inst, params TsObject[] args)
        {
            _source.Resize((int)args[0], (int)args[1]);
            return TsObject.Empty();
        }

        public TsObject shuffle(ITsInstance inst, params TsObject[] args)
        {
            _source.Shuffle();
            return TsObject.Empty();
        }

        public TsObject sort(ITsInstance inst, params TsObject[] args)
        {
            _source.Sort((int)args[0], (bool)args[1]);
            return TsObject.Empty();
        }

        public TsObject value_exists_in_disk(ITsInstance inst, params TsObject[] args)
        {
            return _source.InDisk((int)args[0], (int)args[1], (int)args[2], args[3]);
        }

        public TsObject value_position_in_disk(ITsInstance inst, params TsObject[] args)
        {
            if (_source.InDisk((int)args[0], (int)args[1], (int)args[2], args[3], out var result, (w, h) => new TsObject[] { w, h }))
                return result;
            return TsObject.Empty();
        }

        public TsObject value_exists_in_region(ITsInstance inst, params TsObject[] args)
        {
            return _source.InRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], args[4]);
        }

        public TsObject value_position_in_region(ITsInstance inst, params TsObject[] args)
        {
            if (_source.InRegion((int)args[0], (int)args[1], (int)args[2], (int)args[3], args[4], out var result, (w, h) => new TsObject[] { w, h }))
                return result;
            return TsObject.Empty();
        }
    }
}
