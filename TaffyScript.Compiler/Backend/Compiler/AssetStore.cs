using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using TaffyScript.Reflection;
using TaffyScript.Strings;
using TaffyScript.Compiler.Syntax;

namespace TaffyScript.Compiler.Backend
{
    public class AssetStore
    {
        private static FieldInfo _baseMemberField = typeof(TsInstance).GetField("_members", BindingFlags.NonPublic | BindingFlags.Instance);
        private static Type _baseType = typeof(TsInstance);
        private static ConstructorInfo _baseConstructor = typeof(TsInstance).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
        private static ConstructorInfo _definitionConstructor = typeof(ObjectDefinition).GetConstructor(new[] { typeof(string),
                                                                                                                typeof(string),
                                                                                                                typeof(Func<TsObject[], ITsInstance>) });

        private Dictionary<ISymbol, ObjectInfo> _definedTypes = new Dictionary<ISymbol, ObjectInfo>();
        private Dictionary<ISymbol, EnumInfo> _definedEnums = new Dictionary<ISymbol, EnumInfo>();

        private SymbolTable _table;
        private SymbolResolver _resolver;
        private IErrorLogger _logger;
        private ModuleBuilder _module;
        private MsilWeakCodeGen _codeGen;
        private ILEmitter _moduleInitializer;

        public Dictionary<ISymbol, MethodInfo> Globals { get; } = new Dictionary<ISymbol, MethodInfo>();

        public AssetStore(MsilWeakCodeGen codeGen, SymbolTable table, SymbolResolver resolver, IErrorLogger logger)
        {
            _codeGen = codeGen;
            _table = table;
            _logger = logger;
            _resolver = resolver;
        }

        public void InitializeModule(ModuleBuilder module, ILEmitter moduleInitializer)
        {
            _module = module;
            _moduleInitializer = moduleInitializer;
        }

        public ObjectInfo AddExternalType(ISymbol symbol, Type type)
        {
            var result = new ObjectInfo(type, null, null, null, null);
            _definedTypes.Add(symbol, result);
            return result;
        }

        public void AddObjectInfo(ISymbol symbol, ObjectInfo info)
        {
            _definedTypes.Add(symbol, info);
        }

        public void AddEnumInfo(ISymbol symbol, EnumInfo info)
        {
            _definedEnums.Add(symbol, info);
        }

        public Type GetDefinedType(ISymbol typeSymbol, TokenPosition position)
        {
            return GetObjectInfo(typeSymbol, position).Type;
        }

        public EnumInfo GetEnumInfo(ISymbol enumSymbol, TokenPosition position)
        {
            if(enumSymbol.Type != SymbolType.Enum)
            {
                _logger.Error($"Tried to get enum from symbol '{_resolver.GetAssetFullName(enumSymbol)}' which is '{enumSymbol.Type}'");
                return null;
            }

            var node = enumSymbol as SymbolNode;
            if(!_definedEnums.TryGetValue(enumSymbol, out var info))
            {
                var typeName = _resolver.GetAssetFullName(enumSymbol);
                var type = _module.DefineEnum(typeName, TypeAttributes.Public, typeof(long));
                info = new EnumInfo(type);

                foreach(var kvp in node.Children)
                {
                    var value = kvp.Value as EnumLeaf;
                    if (value is null)
                        continue;

                    info.Values[value.Name] = value.Value;
                    type.DefineLiteral(value.Name, value.Value);
                }

                type.CreateType();
                _definedEnums.Add(enumSymbol, info);
            }

            return info;
        }

        public FieldInfo GetInstanceField(ISymbol typeSymbol, string fieldName, TokenPosition position)
        {
            var info = GetObjectInfo(typeSymbol, position);

            if(!info.Fields.TryGetValue(fieldName, out var field))
            {
                if (typeSymbol is ObjectSymbol obj)
                    field = GetInstanceField(obj.Inherits, fieldName);
                else
                    field = info.Type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                if (field is null)
                {
                    _logger.Error($"Could not find field named {fieldName} on type {info.Type.FullName}", position);
                    return null;
                }

                info.Fields.Add(fieldName, field);
            }

            return field;
        }

        private FieldInfo GetInstanceField(ISymbol typeSymbol, string fieldName)
        {
            if (typeSymbol is null)
                return null;

            var info = GetObjectInfo(typeSymbol, null);

            if (!info.Fields.TryGetValue(fieldName, out var field))
            {
                if (typeSymbol is ObjectSymbol obj)
                    field = GetInstanceField(obj.Inherits, fieldName);
                else
                    field = info.Type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                if (field is null)
                    return null;

                info.Fields.Add(fieldName, field);
            }

            return field;
        }

        public PropertyInfo GetProperty(ISymbol typeSymbol, string propertyName, TokenPosition position)
        {
            var info = GetObjectInfo(typeSymbol, position);

            if(!info.Properties.TryGetValue(propertyName, out var property))
            {
                if (typeSymbol is ObjectSymbol obj)
                    property = GetProperty(obj.Inherits, propertyName);
                else
                    property = info.Type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                if(property is null)
                {
                    _logger.Error($"Could not find field named '{propertyName}' on type '{info.Type.FullName}'", position);
                    return null;
                }

                info.Properties.Add(propertyName, property);
            }

            return property;
        }

        private PropertyInfo GetProperty(ISymbol typeSymbol, string propertyName)
        {
            if (typeSymbol is null)
                return null;

            var info = GetObjectInfo(typeSymbol, null);

            if (!info.Properties.TryGetValue(propertyName, out var property))
            {
                if (typeSymbol is ObjectSymbol obj)
                    property = GetProperty(obj.Inherits, propertyName);
                else
                    property = info.Type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                if (property is null)
                    return null;

                info.Properties.Add(propertyName, property);
            }

            return property;
        }

        public MethodInfo GetInstanceMethod(ISymbol typeSymbol, string methodName, TokenPosition position)
        {
            var info = GetObjectInfo(typeSymbol, position);

            if (!info.Methods.TryGetValue(methodName, out var method))
            {
                if (typeSymbol is ObjectSymbol obj)
                    method = GetInstanceMethod(obj.Inherits, methodName);
                else
                    method = info.Type.GetMethod(methodName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance, null, TsTypes.ArgumentTypes, null);

                if (method is null)
                {
                    _logger.Error("Tried to call script that doesn't exist", position);
                    return null;
                }

                info.Methods.Add(methodName, method);
            }

            return method;
        }

        private MethodInfo GetInstanceMethod(ISymbol typeSymbol, string methodName)
        {
            if (typeSymbol is null)
                return null;

            var info = GetObjectInfo(typeSymbol, null);

            if (!info.Methods.TryGetValue(methodName, out var method))
            {
                if (typeSymbol is ObjectSymbol obj)
                    method = GetInstanceMethod(obj.Inherits, methodName);
                else
                    method = info.Type.GetMethod(methodName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance, null, TsTypes.ArgumentTypes, null);

                if (method is null)
                    return null;

                info.Methods.Add(methodName, method);
            }

            return method;
        }

        public ConstructorInfo GetConstructor(ISymbol typeSymbol, TokenPosition position)
        {
            var info = GetObjectInfo(typeSymbol, position);
            if (info.Constructor is null)
                info.Constructor = info.Type.GetConstructor(new[] { typeof(TsObject[]) });

            return info.Constructor;
        }

        public ObjectInfo GetObjectInfo(ISymbol symbol, TokenPosition position)
        {
            if (!_definedTypes.TryGetValue(symbol, out var info))
            {
                if (symbol is ObjectSymbol os)
                    info = GenerateType(os, position);
                else if (symbol is ImportObjectSymbol leaf)
                    leaf.ImportObject.Accept(_codeGen);
            }

            if (info is null)
                _logger.Error($"Tried to get type that doesn't exist: {_resolver.GetAssetFullName(symbol)}", position);

            return info;
        }

        public void FinalizeType(ObjectInfo info, ISymbol parent, List<MethodInfo> scripts, TokenPosition position)
        {
            var parentConstructor = parent == null ? _baseConstructor : GetConstructor(parent, position);
            var type = (TypeBuilder)info.Type;

            GenerateConstructor(info, parentConstructor, scripts.FirstOrDefault(m => m.Name == "create"));

            _moduleInitializer.LdStr(info.Type.FullName);
            if (info.Parent is null)
                _moduleInitializer.LdNull();
            else
                _moduleInitializer.LdStr(info.Parent?.Type.FullName);

            _moduleInitializer.LdNull()
                              .LdFtn(info.Create)
                              .New(typeof(Func<TsObject[], ITsInstance>).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                              .New(_definitionConstructor)
                              .Call(typeof(TsReflection).GetMethod("ProcessObjectDefinition"));

            var attrib = new CustomAttributeBuilder(typeof(TaffyScriptObjectAttribute).GetConstructor(Type.EmptyTypes), Type.EmptyTypes);
            type.SetCustomAttribute(attrib);
        }

        #region Native TaffyScript Object Generation

        private ObjectInfo GenerateType(ObjectSymbol os, TokenPosition position)
        {
            var typeName = _resolver.GetAssetFullName(os);
            var builder = _module.DefineType(typeName, TypeAttributes.Public);

            var compilerGenerated = new CustomAttributeBuilder(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes), new object[] { });
            builder.SetCustomAttribute(compilerGenerated);


            builder.AddInterfaceImplementation(typeof(ITsInstance));
            ObjectInfo parent = null;
            FieldInfo members;
            if (os.Inherits != null)
            {
                parent = GetObjectInfo(os.Inherits, position);
                if (CanInheritFromType(parent.Type))
                    builder.SetParent(parent.Type);
                else
                {
                    _logger.Error($"Cannot inherit from type '{parent.Type}': Has non-virtual or sealed ITsInstance method", position);
                    parent = null;
                    builder.SetParent(typeof(TsInstance));
                }
            }
            else
                builder.SetParent(typeof(TsInstance));

            if (parent != null && !typeof(TsInstance).IsAssignableFrom(parent.Type))
            {
                if(parent.Members is null)
                {
                    //Todo: Memoize the result if it's not found.
                    var inherits = parent.Type;
                    while (inherits is TypeBuilder parentBuilder && !parentBuilder.IsCreated())
                        inherits = inherits.BaseType;
                    members = inherits.GetField("_members", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    parent.Members = members;
                }
                else
                    members = parent.Members;

                if (members == null || members.FieldType != typeof(Dictionary<string, TsObject>))
                    members = builder.DefineField("_members", typeof(Dictionary<string, TsObject>), FieldAttributes.Family);
            }
            else
                members = _baseMemberField;
            
            var tryGetDelegate = builder.DefineMethod("TryGetDelegate",
                                                      MethodAttributes.Public |
                                                          MethodAttributes.HideBySig |
                                                          MethodAttributes.Virtual,
                                                      typeof(bool),
                                                      new[] { typeof(string), typeof(TsDelegate).MakeByRefType() });

            tryGetDelegate.DefineParameter(2, ParameterAttributes.Out, "del");

            var constructor = builder.DefineConstructor(MethodAttributes.Public |
                                                            MethodAttributes.HideBySig |
                                                            MethodAttributes.SpecialName |
                                                            MethodAttributes.RTSpecialName,
                                                        CallingConventions.HasThis,
                                                        new[] { typeof(TsObject[]) });

            var parentConstructor = parent is null ? _baseConstructor : GetConstructor(os.Inherits, position);

            var ctor = new ILEmitter(constructor, new[] { typeof(TsObject[]) });
            ctor.LdArg(0)
                .Dup();

            if (parentConstructor != _baseConstructor)
                ctor.LdNull();
            ctor.CallBase(parentConstructor, parentConstructor == _baseConstructor ? 0 : 1);

            if (members.DeclaringType == builder)
            {
                ctor.Dup()
                    .New(typeof(Dictionary<string, TsObject>).GetConstructor(Type.EmptyTypes))
                    .StFld(members);
            }

            GenerateObjectType(builder);
            var info = new ObjectInfo(builder, parent, tryGetDelegate, constructor, members);
            _definedTypes.Add(os, info);
            DefineMembers(builder, os, info, tryGetDelegate);

            return info;
        }

        private bool CanInheritFromType(Type type)
        {
            if (type is TypeBuilder)
                return true;

            if (!typeof(ITsInstance).IsAssignableFrom(type))
                return false;

            var objectType = type.GetProperty("ObjectType");
            if (objectType is null || !objectType.CanRead || !objectType.GetMethod.IsPublic || !objectType.GetMethod.IsVirtual || objectType.GetMethod.IsFinal)
                return false;

            var get = type.GetMethod("GetMember", new[] { typeof(string) });
            if (get is null || !get.IsPublic || !get.IsVirtual || get.IsFinal)
                return false;

            var set = type.GetMethod("SetMember", new[] { typeof(string), typeof(TsObject) });
            if (set is null || !set.IsPublic || !set.IsVirtual || set.IsFinal)
                return false;

            var call = type.GetMethod("Call", new[] { typeof(string), typeof(TsObject[]) });
            if (call is null || !call.IsPublic || !call.IsVirtual || call.IsFinal)
                return false;

            var tryGet = type.GetMethod("TryGetDelegate", new[] { typeof(string), typeof(TsDelegate).MakeByRefType() });
            if (tryGet is null || !tryGet.IsPublic || !tryGet.IsVirtual || tryGet.IsFinal)
                return false;

            return true;
        }

        private void DefineMembers(TypeBuilder builder, ObjectSymbol os, ObjectInfo info, MethodBuilder tryGetDelegate)
        {
            var memberHash = new HashSet<int>();
            var memberBruteForce = new List<KeyValuePair<string, MemberInfo>>();
            var memberTree = new RedBlackTree<int, KeyValuePair<string, MemberInfo>>();

            foreach(var member in os.Children.Values)
            {
                switch(member.Type)
                {
                    case SymbolType.Script:
                        MethodBuilder method;
                        if (member.Scope == SymbolScope.Member)
                        {
                            if(!MemberIsValid(info, member, out var parentMember) && parentMember.MemberType != MemberTypes.Method)
                            {
                                _logger.Error("Tried to define script that would overwrite parent member");
                                continue;
                            }

                            var parentMethod = GetInstanceMethod(os.Inherits, member.Name);

                            if (parentMethod != null && (parentMethod.IsFinal || !(parentMethod.IsVirtual || parentMethod.IsAbstract)))
                            {
                                _logger.Error("Tried to override non-virtual, non-abstract method");
                                continue;
                            }

                            MethodAttributes flags = (parentMethod is null ? MethodAttributes.NewSlot : 0)
                                | MethodAttributes.Virtual
                                | MethodAttributes.Public
                                | MethodAttributes.HideBySig;

                            method = builder.DefineMethod(member.Name, flags, typeof(TsObject), TsTypes.ArgumentTypes);

                            if(member.Name == "to_string")
                                GenerateToStringMethod(builder, method);

                            // The create script should not be directly callable.
                            if(member.Name != "create")
                            {
                                var key = Fnv.Fnv32(member.Name);
                                if (memberHash.Add(key))
                                    memberTree.Insert(key, new KeyValuePair<string, MemberInfo>(member.Name, method));
                                else
                                    memberBruteForce.Add(new KeyValuePair<string, MemberInfo>(member.Name, method));
                            }
                        }
                        else
                        {
                            MethodAttributes flags = MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig;
                            method = builder.DefineMethod(member.Name, flags, typeof(TsObject), TsTypes.ArgumentTypes);
                            var staticAttribute = new CustomAttributeBuilder(typeof(TaffyScriptStaticAttribute).GetConstructor(Type.EmptyTypes), new object[] { });
                            method.SetCustomAttribute(staticAttribute);
                        }
                        info.Methods[member.Name] = method;
                        break;
                    case SymbolType.Field:
                        FieldBuilder field;
                        if(member.Scope == SymbolScope.Member)
                        {
                            if(!MemberIsValid(info, member, out var parentMember))
                            {
                                if (parentMember.MemberType == MemberTypes.Field || parentMember.MemberType == MemberTypes.Property)
                                    _logger.Warning($"'{info.Type.FullName}' defines field '{member.Name}' that shadows field defined by parent '{parentMember.DeclaringType}'");
                                else
                                {
                                    _logger.Error($"'{info.Type.FullName}' defines field '{member.Name}' that overrides '{parentMember.MemberType}' by parent '{parentMember.DeclaringType}'");
                                    continue;
                                }
                            }
                            field = builder.DefineField(member.Name, typeof(TsObject), FieldAttributes.Public);
                            var key = Fnv.Fnv32(member.Name);
                            if (memberHash.Add(key))
                                memberTree.Insert(key, new KeyValuePair<string, MemberInfo>(member.Name, field));
                            else
                                memberBruteForce.Add(new KeyValuePair<string, MemberInfo>(member.Name, field));
                        }
                        else
                        {
                            field = builder.DefineField(member.Name, typeof(TsObject), FieldAttributes.Public | FieldAttributes.Static);
                            var staticAttribute = new CustomAttributeBuilder(typeof(TaffyScriptStaticAttribute).GetConstructor(Type.EmptyTypes), new object[] { });
                            field.SetCustomAttribute(staticAttribute);
                        }
                        info.Fields[member.Name] = field;
                        break;
                }
            }

            if (info.Parent != null)
            {
                var parentSymbol = os.Inherits as SymbolNode;

                foreach (var kvp in info.Parent.Methods.Where(kvp => !info.Methods.ContainsKey(kvp.Key)))
                {
                    info.Methods.Add(kvp.Key, kvp.Value);
                    var scriptSymbol = parentSymbol.Children[kvp.Key];
                    os.Children.Add(kvp.Key, scriptSymbol);

                    if (!kvp.Value.IsStatic)
                    {
                        var key = Fnv.Fnv32(kvp.Key);
                        if (memberHash.Add(key))
                            memberTree.Insert(key, new KeyValuePair<string, MemberInfo>(kvp.Key, kvp.Value));
                        else
                            memberBruteForce.Add(new KeyValuePair<string, MemberInfo>(kvp.Key, kvp.Value));
                    }
                }

                foreach (var kvp in info.Parent.Fields.Where(kvp => !info.Fields.ContainsKey(kvp.Key)))
                {
                    info.Fields.Add(kvp.Key, kvp.Value);
                    os.Children.Add(kvp.Key, parentSymbol.Children[kvp.Key]);

                    if (!kvp.Value.IsStatic)
                    {
                        var key = Fnv.Fnv32(kvp.Key);
                        if (memberHash.Add(key))
                            memberTree.Insert(key, new KeyValuePair<string, MemberInfo>(kvp.Key, kvp.Value));
                        else
                            memberBruteForce.Add(new KeyValuePair<string, MemberInfo>(kvp.Key, kvp.Value));
                    }
                }

                foreach(var kvp in info.Parent.Properties.Where(kvp => !info.Properties.ContainsKey(kvp.Key)))
                {
                    info.Properties.Add(kvp.Key, kvp.Value);
                    os.Children.Add(kvp.Key, parentSymbol.Children[kvp.Key]);

                    if ((kvp.Value.CanRead && !kvp.Value.GetMethod.IsStatic) || (kvp.Value.CanWrite && !kvp.Value.GetMethod.IsStatic))
                    {
                        var key = Fnv.Fnv32(kvp.Key);
                        if (memberHash.Add(key))
                            memberTree.Insert(key, new KeyValuePair<string, MemberInfo>(kvp.Key, kvp.Value));
                        else
                            memberBruteForce.Add(new KeyValuePair<string, MemberInfo>(kvp.Key, kvp.Value));
                    }
                }
            }

            var callMethod = builder.DefineMethod("Call",
                                                  MethodAttributes.Public |
                                                          MethodAttributes.HideBySig |
                                                          MethodAttributes.Virtual,
                                                  typeof(TsObject),
                                                  new[] { typeof(string), typeof(TsObject[]) });

            var getMember = builder.DefineMethod("GetMember",
                                                 MethodAttributes.Public |
                                                          MethodAttributes.HideBySig |
                                                          MethodAttributes.Virtual,
                                                 typeof(TsObject),
                                                 new[] { typeof(string) });

            var setMember = builder.DefineMethod("SetMember",
                                                 MethodAttributes.Public |
                                                          MethodAttributes.HideBySig |
                                                          MethodAttributes.Virtual,
                                                 typeof(void),
                                                 new[] { typeof(string), typeof(TsObject) });

            TsInstanceInterfaceMethodGenerator.ImplementInterfaceMethods(memberBruteForce,
                                                                         memberTree,
                                                                         tryGetDelegate,
                                                                         callMethod,
                                                                         getMember,
                                                                         setMember,
                                                                         null,
                                                                         info.Members,
                                                                         true,
                                                                         info.Type.FullName,
                                                                         info.Parent);
        }

        private bool MemberIsValid(ObjectInfo info, ISymbol member, out MemberInfo parentMember)
        {
            parentMember = default;
            if (info.Parent is null || member.Scope == SymbolScope.Global)
                return true;
            var parent = info.Parent;
            while(parent != null)
            {
                if (parent.Type is TypeBuilder builder && !builder.IsCreated())
                {
                    if (parent.Methods.TryGetValue(member.Name, out var method))
                    {
                        parentMember = method;
                        return false;
                    }

                    if (parent.Fields.TryGetValue(member.Name, out var field))
                    {
                        parentMember = field;
                        return false;
                    }

                    if(parent.Properties.TryGetValue(member.Name, out var property))
                    {
                        parentMember = property;
                        return false;
                    }
                }
                else
                {
                    var members = parent.Type.GetMember(member.Name,
                                                        MemberTypes.Field |
                                                            MemberTypes.Property |
                                                            MemberTypes.Method,
                                                        BindingFlags.Public |
                                                            BindingFlags.Instance |
                                                            BindingFlags.NonPublic |
                                                            BindingFlags.FlattenHierarchy);

                    parentMember = members.FirstOrDefault(InheritedMember);

                    return parentMember is null ? true : false;
                }

                parent = parent.Parent;
            }

            return true;

            bool InheritedMember(MemberInfo memberInfo)
            {
                switch (memberInfo)
                {
                    case MethodInfo method:
                        return method.IsPublic || method.IsFamily;
                    case FieldInfo field:
                        return field.IsPublic || field.IsFamily;
                    case PropertyInfo property:
                        return (property.GetMethod != null && (property.GetMethod.IsPublic || property.GetMethod.IsFamily)) ||
                               (property.SetMethod != null && (property.SetMethod.IsPublic || property.SetMethod.IsFamily));
                    default:
                        return false;
                }
            }
        }

        private void GenerateObjectType(TypeBuilder type)
        {
            var objectType = type.DefineProperty("ObjectType", PropertyAttributes.None, typeof(string), Type.EmptyTypes);
            var get = type.DefineMethod("get_ObjectType",
                                        MethodAttributes.Public |
                                            MethodAttributes.HideBySig |
                                            MethodAttributes.SpecialName |
                                            MethodAttributes.Virtual,
                                        typeof(string),
                                        Type.EmptyTypes);
            var emit = new ILEmitter(get, new[] { type });
            emit.LdStr(type.FullName)
                .Ret();

            objectType.SetGetMethod(get);
        }

        private void GenerateConstructor(ObjectInfo info, ConstructorInfo parentConstructor, MethodInfo createScript)
        {
            var ctorMethod = (ConstructorBuilder)info.Constructor;

            var ctor = new ILEmitter(ctorMethod, new[] { info.Type, typeof(TsObject[]) });
            ctor.PushType(info.Type).Pop();

            if (createScript != null)
            {
                var notValid = ctor.DefineLabel();
                ctor.LdArg(1)
                    .LdNull()
                    .BeqS(notValid)
                    .LdArg(0)
                    .LdArg(1)
                    .Call(createScript, 2, typeof(TsObject))
                    .Pop()
                    .MarkLabel(notValid);
            }
            ctor.Ret();

            GenerateCreateMethod(info);
        }

        private void GenerateCreateMethod(ObjectInfo info)
        {
            var builder = (TypeBuilder)info.Type;
            var create = builder.DefineMethod("Create",
                                        MethodAttributes.Assembly |
                                            MethodAttributes.Static |
                                            MethodAttributes.HideBySig,
                                        typeof(ITsInstance),
                                        new[] { typeof(TsObject[]) });
            var emit = new ILEmitter(create, new[] { typeof(TsObject[]) });
            emit.LdArg(0)
                .New(info.Constructor, 1)
                .Ret();

            info.Create = create;
        }

        private void GenerateToStringMethod(TypeBuilder type, MethodInfo toString)
        {
            var methodBuilder = type.DefineMethod("ToString",
                                                  MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                                                  CallingConventions.HasThis,
                                                  typeof(string),
                                                  Type.EmptyTypes);
            var emit = new ILEmitter(methodBuilder, Type.EmptyTypes);
            emit.LdArg(0)
                .LdNull()
                .Call(toString, 1, typeof(TsObject))
                .Call(TsTypes.ObjectCasts[typeof(string)])
                .Ret();
        }

        #endregion
    }

    public class ObjectInfo
    {
        public ObjectInfo Parent { get; }
        public Type Type { get; }
        public FieldInfo Members { get; set; }
        public MethodInfo TryGetDelegate { get; set; }
        public ConstructorInfo Constructor { get; set; }
        public MethodInfo Create { get; set; }
        public Dictionary<string, MethodInfo> Methods { get; } = new Dictionary<string, MethodInfo>();
        public Dictionary<string, FieldInfo> Fields { get; } = new Dictionary<string, FieldInfo>();
        public Dictionary<string, PropertyInfo> Properties { get; } = new Dictionary<string, PropertyInfo>();

        public ObjectInfo(Type type, ObjectInfo parent, MethodInfo tryGetDelegate, ConstructorInfo constructor, FieldInfo members)
        {
            Type = type;
            Parent = parent;
            TryGetDelegate = tryGetDelegate;
            Constructor = constructor;
            Members = members;
        }

        public ObjectInfo(Type type, 
                          ObjectInfo parent, 
                          MethodInfo tryGetDelegate, 
                          ConstructorInfo constructor, 
                          FieldInfo members, 
                          Dictionary<string, MethodInfo> methods,
                          Dictionary<string, FieldInfo> fields,
                          Dictionary<string, PropertyInfo> properties)
        {
            Type = type;
            Parent = parent;
            TryGetDelegate = tryGetDelegate;
            Constructor = constructor;
            Members = members;
            Methods = methods;
            Fields = fields;
            Properties = properties;
        }
    }

    public class EnumInfo
    {
        public Type Type { get; }
        public Dictionary<string, long> Values { get; } = new Dictionary<string, long>();

        public EnumInfo(Type enumType)
        {
            Type = enumType;
        }
    }
}
