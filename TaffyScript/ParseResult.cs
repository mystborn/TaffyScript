using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Represents the result from parsing operations.
    /// </summary>
    /// <property name="success" type="bool" access="get">
    ///     <summary>Determines if the parse operation was successful.</summary>
    /// </property>
    /// <property name="result" type="object" access="get">
    ///     <summary>If the parse operation was successful, holds the result.</summary>
    /// </property>
    [TaffyScriptObject]
    public class ParseResult : TsInstance
    {
        private TsObject _success;

        public override string ObjectType => "TaffyScript.ParseResult";

        public bool Success { get; }
        public TsObject Result { get; }

        public ParseResult(TsObject[] args)
        {
            _success = args[0];
            Success = (bool)_success;
            Result = args[1];
        }

        public ParseResult(bool success, TsObject result)
        {
            _success = success;
            Success = success;
            Result = result;
        }

        public override TsObject Call(string scriptName, TsObject[] args)
        {
            if (_members.TryGetValue(scriptName, out var member))
            {
                if (member.Type != VariableType.Delegate)
                    throw new InvalidTsTypeException($"Tried to call member {ObjectType}.{scriptName} that wasn't a script");
                return ((TsDelegate)member).Invoke(args);
            }
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public override TsObject GetMember(string name)
        {
            switch(name)
            {
                case "success":
                    return _success;
                case "result":
                    return Result;
                default:
                    if (_members.TryGetValue(name, out var member))
                        return member;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public override void SetMember(string name, TsObject value)
        {
            switch(name)
            {
                case "success":
                case "result":
                    throw new MemberAccessException($"Tried to set read only member {ObjectType}.{name}");
                default:
                    _members[name] = value;
                    break;
            }
        }

        public override bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            if(_members.TryGetValue(delegateName, out var member) && member.Type == VariableType.Delegate)
            {
                del = (TsDelegate)member;
                return true;
            }
            del = null;
            return false;
        }
    }
}
