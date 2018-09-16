using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public class TsType : ITsInstance
    {
        private string _name;
        private string _fullName;

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.Type";

        public string Name
        {
            get
            {
                if (_fullName is null)
                    _fullName = Source.FullName;
                return _fullName;
            }
        }

        public Type Source { get; }

        public TsType(Type type)
        {
            Source = type;
        }

        public TsType(Type type, string name)
        {
            _fullName = name;
            Source = type;
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "get_array_rank":
                    return get_array_rank(args);
                case "get_enum_name":
                    return get_enum_name(args);
                case "get_enum_names":
                    return get_enum_names(args);
                case "get_enum_underlying_type":
                    return get_enum_underlying_type(args);
                case "get_enum_values":
                    return get_enum_values(args);
                case "is_assignable_from":
                    return is_assignable_from(args);
                case "is_enum_defined":
                    return is_enum_defined(args);
                case "is_instance_of_type":
                    return is_instance_of_type(args);
                case "is_subclass_of":
                    return is_subclass_of(args);
                case "to_string":
                    return to_string(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public TsObject GetMember(string name)
        {
            switch(name)
            {
                case "base_type":
                    return new TsType(Source.BaseType);
                case "full_name":
                    return Source.FullName;
                case "is_abstract":
                    return Source.IsAbstract;
                case "is_ansi_class":
                    return Source.IsAnsiClass;
                case "is_array":
                    return Source.IsArray;
                case "is_auto_class":
                    return Source.IsAutoClass;
                case "is_auto_layout":
                    return Source.IsAutoLayout;
                case "is_by_ref":
                    return Source.IsByRef;
                case "is_class":
                    return Source.IsClass;
                case "is_com_object":
                    return Source.IsCOMObject;
                case "is_constructed_generic_type":
                    return Source.IsConstructedGenericType;
                case "is_contextful":
                    return Source.IsContextful;
                case "is_enum":
                    return Source.IsEnum;
                case "is_explicit_layout":
                    return Source.IsExplicitLayout;
                case "is_generic_parameter":
                    return Source.IsGenericParameter;
                case "is_generic_type":
                    return Source.IsGenericType;
                case "is_generic_type_definition":
                    return Source.IsGenericTypeDefinition;
                case "is_import":
                    return Source.IsImport;
                case "is_interface":
                    return Source.IsInterface;
                case "is_layout_sequential":
                    return Source.IsLayoutSequential;
                case "is_marshal_by_ref":
                    return Source.IsMarshalByRef;
                case "is_nested":
                    return Source.IsNested;
                case "is_nested_assembly":
                    return Source.IsNestedAssembly;
                case "is_nested_family":
                    return Source.IsNestedFamily;
                case "is_nested_family_and_assembly":
                    return Source.IsNestedFamANDAssem;
                case "is_nested_family_or_assembly":
                    return Source.IsNestedFamORAssem;
                case "is_nested_private":
                    return Source.IsNestedPrivate;
                case "is_nested_public":
                    return Source.IsNestedPublic;
                case "is_not_public":
                    return Source.IsNotPublic;
                case "is_pointer":
                    return Source.IsPointer;
                case "is_primitive":
                    return Source.IsPrimitive;
                case "is_public":
                    return Source.IsPublic;
                case "is_sealed":
                    return Source.IsSealed;
                case "is_security_critical":
                    return Source.IsSecurityCritical;
                case "is_security_safe_critical":
                    return Source.IsSecuritySafeCritical;
                case "is_security_transparent":
                    return Source.IsSecurityTransparent;
                case "is_serializable":
                    return Source.IsSerializable;
                case "is_special_name":
                    return Source.IsSpecialName;
                case "is_unicode_class":
                    return Source.IsUnicodeClass;
                case "is_value_type":
                    return Source.IsValueType;
                case "is_visible":
                    return Source.IsVisible;
                case "member_type":
                    return (float)Source.MemberType;
                case "name":
                    if(_name is null)
                    {
                        var index = Name.LastIndexOf('.') + 1;
                        if (index != 0)
                            _name = Name.Substring(index);
                        else
                            _name = Name;
                    }
                    return _name;
                case "namespace":
                    return Source.Namespace;
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
            switch(scriptName)
            {
                case "get_array_rank":
                    del = new TsDelegate(get_array_rank, scriptName);
                    break;
                case "get_enum_name":
                    del = new TsDelegate(get_enum_name, scriptName);
                    break;
                case "get_enum_names":
                    del = new TsDelegate(get_enum_names, scriptName);
                    break;
                case "get_enum_underlying_type":
                    del = new TsDelegate(get_enum_underlying_type, scriptName);
                    break;
                case "get_enum_values":
                    del = new TsDelegate(get_enum_values, scriptName);
                    break;
                case "is_assignable_from":
                    del = new TsDelegate(is_assignable_from, scriptName);
                    break;
                case "is_enum_defined":
                    del = new TsDelegate(is_enum_defined, scriptName);
                    break;
                case "is_instance_of_type":
                    del = new TsDelegate(is_instance_of_type, scriptName);
                    break;
                case "is_subclass_of":
                    del = new TsDelegate(is_subclass_of, scriptName);
                    break;
                case "to_string":
                    del = new TsDelegate(to_string, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        public TsObject get_array_rank(TsObject[] args)
        {
            return Source.GetArrayRank();
        }

        public TsObject get_enum_name(TsObject[] args)
        {
            // Todo: See if the value needs to be converted to the underlying enum type.
            return Source.GetEnumName((long)args[0]);
        }

        public TsObject get_enum_names(TsObject[] args)
        {
            var names = Source.GetEnumNames();
            var result = new TsObject[names.Length];
            for (var i = 0; i < names.Length; i++)
                result[i] = names[i];

            return result;
        }

        public TsObject get_enum_underlying_type(TsObject[] args)
        {
            return new TsType(Source.GetEnumUnderlyingType());
        }

        public TsObject get_enum_values(TsObject[] args)
        {
            var values = Source.GetEnumValues();
            var result = new TsObject[values.Length];
            for (var i = 0; i < values.Length; i++)
                result[i] = Convert.ToSingle(values.GetValue(i));

            return result;
        }

        public TsObject is_assignable_from(TsObject[] args)
        {
            return Source.IsAssignableFrom(((TsType)args[0]).Source);
        }

        public TsObject is_enum_defined(TsObject[] args)
        {
            return Source.IsEnumDefined((long)args[0]);
        }

        public TsObject is_instance_of_type(TsObject[] args)
        {
            return Source.IsInstanceOfType(args[0].WeakValue);
        }

        public TsObject is_subclass_of(TsObject[] args)
        {
            return Source.IsSubclassOf(((TsType)args[0]).Source);
        }

        public TsObject to_string(TsObject[] args)
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is TsType tsType)
                return Source == tsType.Source;
            else if (obj is Type type)
                return Source == type;
            return false;
        }

        public static implicit operator TsObject(TsType type)
        {
            return new TsInstanceWrapper(type);
        }

        public static explicit operator TsType(TsObject obj)
        {
            return (TsType)obj.WeakValue;
        }
    }
}
