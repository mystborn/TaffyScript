using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Number = System.Single;

namespace TaffyScript
{
    public class TsString : TsObject, ITsInstance
    {
        public string Value { get; }
        public override VariableType Type => VariableType.String;
        public override object WeakValue => Value;
        public string ObjectType => "string";

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public TsString(string value)
        {
            Value = value;
        }

        public TsString(char value)
        {
            Value = value.ToString();
        }

        public override string GetString() => Value;
        public override ITsInstance GetInstance() => this;

        public override TsObject[] GetArray() => throw new InvalidTsTypeException($"Variable is supposed to be of type Array, is {Type} instead.");
        public override TsDelegate GetDelegate() => throw new InvalidTsTypeException($"Variable is supposed to be of type Script, is {Type} instead.");
        public override Number GetNumber() => throw new InvalidTsTypeException($"Variable is supposed to be of type Number, is {Type} instead.");

        public override bool Equals(object obj)
        {
            if (obj is TsString str)
                return Value == str.Value;
            else if (obj is string val)
                return Value == val;
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public TsObject GetMember(string name)
        {
            switch(name)
            {
                case "length":
                    return name.Length;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "contains":
                    del = new TsDelegate(contains, scriptName, this);
                    return true;
                case "copy":
                    del = new TsDelegate(copy, scriptName, this);
                    return true;
                case "count":
                    del = new TsDelegate(count, scriptName, this);
                    return true;
                case "delete":
                    del = new TsDelegate(delete, scriptName, this);
                    return true;
                case "digits":
                    del = new TsDelegate(digits, scriptName, this);
                    return true;
                case "duplicate":
                    del = new TsDelegate(duplicate, scriptName, this);
                    return true;
                case "ends_with":
                    del = new TsDelegate(ends_with, scriptName, this);
                    return true;
                case "get":
                    del = new TsDelegate(get, scriptName, null);
                    return true;
                case "index_of":
                    del = new TsDelegate(index_of, scriptName, this);
                    return true;
                case "insert":
                    del = new TsDelegate(insert, scriptName, this);
                    return true;
                case "last_index_of":
                    del = new TsDelegate(last_index_of, scriptName, this);
                    return true;
                case "letters":
                    del = new TsDelegate(letters, scriptName, this);
                    return true;
                case "letters_digits":
                    del = new TsDelegate(letters_digits, scriptName, this);
                    return true;
                case "lower":
                    del = new TsDelegate(lower, scriptName, this);
                    return true;
                case "ord":
                    del = new TsDelegate(ord, scriptName, this);
                    return true;
                case "replace":
                    del = new TsDelegate(replace, scriptName, this);
                    return true;
                case "replace_all":
                    del = new TsDelegate(replace_all, scriptName, this);
                    return true;
                case "starts_with":
                    del = new TsDelegate(starts_with, scriptName, this);
                    return true;
                case "trim":
                    del = new TsDelegate(trim, scriptName, this);
                    return true;
                case "trim_end":
                    del = new TsDelegate(trim_end, scriptName, this);
                    return true;
                case "trim_start":
                    del = new TsDelegate(trim_start, scriptName, this);
                    return true;
                case "upper":
                    del = new TsDelegate(upper, scriptName, this);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "contains":
                    return contains(null, args);
                case "copy":
                    return copy(null, args);
                case "count":
                    return count(null, args);
                case "delete":
                    return delete(null, args);
                case "digits":
                    return digits(null, args);
                case "duplicate":
                    return duplicate(null, args);
                case "ends_with":
                    return ends_with(null, args);
                case "get":
                    return get(null, args);
                case "index_of":
                    return index_of(null, args);
                case "insert":
                    return insert(null, args);
                case "last_index_of":
                    return last_index_of(null, args);
                case "letters":
                    return letters(null, args);
                case "letters_digits":
                    return letters_digits(null, args);
                case "lower":
                    return lower(null, args);
                case "ord":
                    return ord(null, args);
                case "replace":
                    return replace(null, args);
                case "replace_all":
                    return replace_all(null, args);
                case "starts_with":
                    return starts_with(null, args);
                case "trim":
                    return trim(null, args);
                case "trim_end":
                    return trim_end(null, args);
                case "trim_start":
                    return trim_start(null, args);
                case "upper":
                    return upper(null, args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsObject contains(ITsInstance inst, TsObject[] args)
        {
            return Value.Contains((string)args[0]);
        }

        public TsObject copy(ITsInstance inst, TsObject[] args)
        {
            if (args is null)
                return string.Copy(Value);

            switch(args.Length)
            {
                case 0:
                    return string.Copy(Value);
                case 1:
                    return Value.Substring((int)args[0]);
                default:
                    return Value.Substring((int)args[0], (int)args[1]);
            }
        }

        public TsObject count(ITsInstance inst, TsObject[] args)
        {
            var subString = (string)args[0];

            // Code found here:
            // https://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-within-a-string
            return (Value.Length - Value.Replace(subString, "").Length) / subString.Length;
        }

        public TsObject delete(ITsInstance isnt, TsObject[] args)
        {
            return Value.Remove((int)args[0], (int)args[1]);
        }

        public TsObject digits(ITsInstance inst, TsObject[] args)
        {
            // Test with regex to see if that's faster.
            // return System.Text.RegularExpressions.Regex.Replace(Value, @"[^\d]", "");
            var sb = new StringBuilder();
            for (var i = 0; i < Value.Length; i++)
            {
                //Good ol fashioned C trick.
                if (Value[i] >= '0' && Value[i] <= '9')
                    sb.Append(Value[i]);
            }
            return sb.ToString();
        }

        public TsObject duplicate(ITsInstance inst, TsObject[] args)
        {
            var count = (int)args[0];

            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
                sb.Append(Value);

            return sb.ToString();
        }

        public TsObject ends_with(ITsInstance inst, TsObject[] args)
        {
            return Value.EndsWith((string)args[0]);
        }

        public TsObject get(ITsInstance inst, TsObject[] args)
        {
            return Value[(int)args[0]];
        }

        public TsObject index_of(ITsInstance inst, TsObject[] args)
        {
            return args.Length == 1 ? Value.IndexOf((string)args[0]) : Value.IndexOf((string)args[0], (int)args[1]);
        }

        public TsObject insert(ITsInstance inst, TsObject[] args)
        {
            return Value.Insert((int)args[0], (string)args[1]);
        }

        public TsObject last_index_of(ITsInstance inst, TsObject[] args)
        {
            return args.Length == 1 ? Value.LastIndexOf((string)args[0]) : Value.LastIndexOf((string)args[0], (int)args[1]);
        }

        public TsObject letters(ITsInstance inst, TsObject[] args)
        {
            // Test with regex to see if that's faster.
            // return System.Text.RegularExpressions.Regex.Replace(Value, @"[^a-zA-Z]", "");
            var sb = new StringBuilder();
            for (var i = 0; i < Value.Length; i++)
            {
                if ((Value[i] >= 'a' && Value[i] <= 'z') || (Value[i] >= 'A' && Value[i] <= 'Z'))
                    sb.Append(Value[i]);
            }
            return sb.ToString();
        }

        public TsObject letters_digits(ITsInstance inst, TsObject[] args)
        {
            //Test with regex to see if that's faster.
            //return System.Text.RegularExpressions.Regex.Replace(Value, @"[^a-zA-Z\d]", "");
            var sb = new StringBuilder();
            for (var i = 0; i < Value.Length; i++)
            {
                if ((Value[i] >= 'a' && Value[i] <= 'z') || (Value[i] >= 'A' && Value[i] <= 'Z') || (Value[i] >= '0' && Value[i] <= '9'))
                    sb.Append(Value[i]);
            }
            return sb.ToString();
        }

        public TsObject lower(ITsInstance inst, TsObject[] args)
        {
            return Value.ToLower();
        }

        public TsObject ord(ITsInstance inst, TsObject[] args)
        {
            return (Number)Value[(int)args[0]];
        }

        public TsObject replace(ITsInstance inst, TsObject[] args)
        {
            var subString = (string)args[0];
            var newString = (string)args[1];

            var index = Value.IndexOf(subString);
            return index != -1 ? Value.Substring(0, index) + newString + Value.Substring(index + subString.Length) : Value;
        }

        public TsObject replace_all(ITsInstance inst, TsObject[] args)
        {
            return Value.Replace((string)args[0], (string)args[1]);
        }

        public TsObject starts_with(ITsInstance inst, TsObject[] args)
        {
            return Value.StartsWith((string)args[0]);
        }

        public TsObject trim(ITsInstance inst, TsObject[] args)
        {
            if (args is null)
                return Value.Trim();
            var characters = new char[args.Length];
            for (var i = 0; i < args.Length; i++)
                characters[i] = args[i].GetString()[0];

            return Value.Trim(characters);
        }

        public TsObject trim_end(ITsInstance inst, TsObject[] args)
        {
            if (args is null)
                return Value.TrimEnd();
            var characters = new char[args.Length];
            for (var i = 0; i < args.Length; i++)
                characters[i] = args[i].GetString()[0];

            return Value.TrimEnd(characters);
        }

        public TsObject trim_start(ITsInstance inst, TsObject[] args)
        {
            if (args is null)
                return Value.TrimStart();
            var characters = new char[args.Length];
            for (var i = 0; i < args.Length; i++)
                characters[i] = args[i].GetString()[0];

            return Value.TrimStart(characters);
        }

        public TsObject upper(ITsInstance inst, TsObject[] args)
        {
            return Value.ToUpper();
        }
    }
}
