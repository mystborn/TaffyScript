using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Represents an object type.
    /// </summary>
    /// <property name="base_type" type="[Type]({{site.baseurl}}/docs/TaffyScript/Type)" access="get">
    ///     <summary>Gets the type this type inherits from.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.basetype?view=netframework-4.7</source>
    /// </property>
    /// <property name="full_name" type="string" access="get">
    ///     <summary>Gets the fully qualified name of this type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.fullname?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_abstract" type="bool" access="get">
    ///     <summary>Determines if this type is abstract.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isabstract?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_ansi_class" type="bool" access="get">
    ///     <summary>Determines if the string format attribute AnsiClass is selected for this type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isansiclass?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_array" type="bool" access="get">
    ///     <summary>Determines if this type is an array.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isarray?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_auto_class" type="bool" access="get">
    ///     <summary>Determines if the string format attribute AutoClass is selected for this type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isautoclass?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_auto_layout" type="bool" access="get">
    ///     <summary>Determines if the fields for this type are laid out automatically by the CLR.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isautolayout?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_by_ref" type="bool" access="get">
    ///     <summary>Determines if this type is passed by reference.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isbyref?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_class" type="bool" access="get">
    ///     <summary>Determines if this type is a class or script.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isclass?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_com_object" type="bool" access="get">
    ///     <summary>Determines if this type is COM object.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.iscomobject?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_constructed_generic_type" type="bool" access="get">
    ///     <summary>Determines if this type is a constructed generic type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isconstructedgenerictype?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_contextful" type="bool" access="get">
    ///     <summary>Determines if this type can be hosted in a context.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.iscontextful?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_enum" type="bool" access="get">
    ///     <summary>Determines if this type is an enum.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isenum?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_explicit_layout" type="bool" access="get">
    ///     <summary>Determines if the fields of this type are laid out at explicitly specified offsets.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isexplicitlayout?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_generic_parameter" type="bool" access="get">
    ///     <summary>Determines if this type represents a type parameter in the definition of a generic type or method.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isgenericparameter?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_generic_type" type="bool" access="get">
    ///     <summary>Determines if this type is a generic type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isgenerictype?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_generic_type_definition" type="bool" access="get">
    ///     <summary>Determines if this type represents a generic type definition.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isgenerictypedefinition?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_import" type="bool" access="get">
    ///     <summary>Determines if this type has a ComImport attribute, indicating that it was imported from a COM type library.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isimport?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_interface" type="bool" access="get">
    ///     <summary>Determines if this type is an interface.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isinterface?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_layout_sequential" type="bool" access="get">
    ///     <summary>Determines if the fields of this type are laid out sequentially in the order they were defined.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.islayoutsequential?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_marshal_by_ref" type="bool" access="get">
    ///     <summary>Determines if this type is marshaled by reference.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.ismarshalbyref?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_nested" type="bool" access="get">
    ///     <summary>Determines if this type is defined inside of another type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isnested?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_nested_assembly" type="bool" access="get">
    ///     <summary>Determines if this type is nested and only visible within its own assembly.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isnestedassembly?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_nested_family" type="bool" access="get">
    ///     <summary>Determines if this type is nested and is only visible within its own family</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isnestedfamily?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_nested_family_and_assembly" type="bool" access="get">
    ///     <summary>Determines if this type is nested and is only visible to both its own family and its own assembly.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isnestedfamandassem?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_nested_family_or_assembly" type="bool" access="get">
    ///     <summary>Determines if this type is nested and is only visible to its own family or its own assembly.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isnestedfamorassem?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_nested_private" type="bool" access="get">
    ///     <summary>Determines if this type is nested and declared private.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isnestedprivate?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_nested_public" type="bool" access="get">
    ///     <summary>Determines if this type is nested and declared public.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isnestedpublic?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_not_public" type="bool" access="get">
    ///     <summary>Determines if this type is not declared public.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isnotpublic?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_pointer" type="bool" access="get">
    ///     <summary>Determines if this type is a pointer.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.ispointer?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_primitive" type="bool" access="get">
    ///     <summary>Determines if this type is a primitive type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isprimitive?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_public" type="bool" access="get">
    ///     <summary>Determines if this type is declared public.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.ispublic?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_sealed" type="bool" access="get">
    ///     <summary>Determines if this type is declared sealed.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.issealed?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_security_critical" type="bool" access="get">
    ///     <summary>Determines if this type is security-critical or security-safe-critical at the current trust level.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.issecuritycritical?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_security_safe_critical" type="bool" access="get">
    ///     <summary>Determines if this type is security-safe-critical at the current trust level.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.issecuritysafecritical?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_security_transparent" type="bool" access="get">
    ///     <summary>Determines if this type is transparent at the current trust level.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.issecuritytransparent?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_serializable" type="bool" access="get">
    ///     <summary>Determines if this type is serializable.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isserializable?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_special_name" type="bool" access="get">
    ///     <summary>Determines if this type has a name that requires special handling.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isspecialname?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_unicode_class" type="bool" access="get">
    ///     <summary>Determines if the string format UnicodeClass is selected for this type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isunicodeclass?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_value_type" type="bool" access="get">
    ///     <summary>Determines if this type is a value type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isvaluetype?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_visible" type="bool" access="get">
    ///     <summary>Determines if this type can be accessed by code outside the assembly.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.isvisible?view=netframework-4.7</source>
    /// </property>
    /// <property name="member_type" type="[MemberTypes](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.membertypes?view=netframework-4.7)" access="get">
    ///     <summary>Gets a value indicating that this member is a type or a nested type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.membertype?view=netframework-4.7</source>
    /// </property>
    /// <property name="name" type="string" access="get">
    ///     <summary>Gets the name of this type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.name?view=netframework-4.7</source>
    /// </property>
    /// <property name="namespace" type="string" access="get">
    ///     <summary>Gets the namespace of this type.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.type.namespace?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject("TaffyScript.Type")]
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

        /// <summary>
        /// If this type is an array, gets the number of array dimensions.
        /// </summary>
        /// <returns>number</returns>
        public TsObject get_array_rank(TsObject[] args)
        {
            return Source.GetArrayRank();
        }

        /// <summary>
        /// If this type is an enum, gets the name of the constant that has the specified value.
        /// </summary>
        /// <arg name="value" type="number">The value to get the name of.</arg>
        /// <returns>string</returns>
        public TsObject get_enum_name(TsObject[] args)
        {
            // Todo: See if the value needs to be converted to the underlying enum type.
            return Source.GetEnumName((long)args[0]);
        }

        /// <summary>
        /// If this type is an enum, gets the names of its members as an array.
        /// </summary>
        /// <returns>array</returns>
        public TsObject get_enum_names(TsObject[] args)
        {
            var names = Source.GetEnumNames();
            var result = new TsObject[names.Length];
            for (var i = 0; i < names.Length; i++)
                result[i] = names[i];

            return result;
        }

        /// <summary>
        /// If this type is an enum, gets the underlying CLR type.
        /// </summary>
        /// <returns>[Type]({{site.baseurl}}/docs/TaffyScript/Type)</returns>
        public TsObject get_enum_underlying_type(TsObject[] args)
        {
            return new TsType(Source.GetEnumUnderlyingType());
        }

        /// <summary>
        /// If this type is an enum, gets an array of the values of the constants defined by the enum.
        /// </summary>
        /// <returns>array</returns>
        public TsObject get_enum_values(TsObject[] args)
        {
            var values = Source.GetEnumValues();
            var result = new TsObject[values.Length];
            for (var i = 0; i < values.Length; i++)
                result[i] = Convert.ToSingle(values.GetValue(i));

            return result;
        }

        /// <summary>
        /// Determines if an instance of the specified type can be cast to an instance of this type.
        /// </summary>
        /// <arg name="type" type="[Type]({{site.baseurl}}/docs/TaffyScript/Type)">The instance to check.</arg>
        /// <returns>bool</returns>
        public TsObject is_assignable_from(TsObject[] args)
        {
            return Source.IsAssignableFrom(((TsType)args[0]).Source);
        }

        /// <summary>
        /// If this type is an enum, determines if the specified value is defined by the enum.
        /// </summary>
        /// <arg name="value" type="number">The value to check.</arg>
        /// <returns>bool</returns>
        public TsObject is_enum_defined(TsObject[] args)
        {
            return Source.IsEnumDefined((long)args[0]);
        }

        /// <summary>
        /// Determines if the specified object is an instance of this type.
        /// </summary>
        /// <arg name="obj" type="object">The object to check.</arg>
        /// <returns>bool</returns>
        public TsObject is_instance_of_type(TsObject[] args)
        {
            return Source.IsInstanceOfType(args[0].WeakValue);
        }

        /// <summary>
        /// Determines if this type derives from the specified class.
        /// </summary>
        /// <arg name="type" type="[Type]({{site.baseurl}}/docs/TaffyScript/Type)">The type to check.</arg>
        /// <returns>bool</returns>
        public TsObject is_subclass_of(TsObject[] args)
        {
            return Source.IsSubclassOf(((TsType)args[0]).Source);
        }

        /// <summary>
        /// Gets the name of this type.
        /// </summary>
        /// <returns>string</returns>
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
