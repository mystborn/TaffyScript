using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace TaffyScript.Compiler.Backend
{
    public class AssetStore
    {
        private static FieldInfo _baseMemberField = typeof(TsInstance).GetField("_members", BindingFlags.NonPublic | BindingFlags.Instance);
        private static Type _baseType = typeof(TsInstance);
        private static ConstructorInfo _baseConstructor = typeof(TsInstance).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
        private static ConstructorInfo _definitionConstructor = typeof(ObjectDefinition).GetConstructor(new[] { typeof(string),
                                                                                                                typeof(string),
                                                                                                                typeof(Dictionary<string, TsDelegate>),
                                                                                                                typeof(Func<TsObject[], ITsInstance>) });
        private static MethodInfo _processDefintion = typeof(TsReflection).GetMethod("ProcessObjectDefinition");

        private Dictionary<ISymbol, ObjectInfo> _definedTypes = new Dictionary<ISymbol, ObjectInfo>();

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

        public void AddExternalType(ISymbol symbol, Type type)
        {
            _definedTypes.Add(symbol, new ObjectInfo(type, null, null, null, null, null));
        }

        public void AddObjectInfo(ISymbol symbol, ObjectInfo info)
        {
            _definedTypes.Add(symbol, info);
        }

        public Type GetDefinedType(ISymbol typeSymbol, TokenPosition position)
        {
            return GetObjectInfo(typeSymbol, position).Type;
        }

        public MethodInfo GetInstanceMethod(ISymbol typeSymbol, string methodName, TokenPosition position)
        {
            var info = GetObjectInfo(typeSymbol, position);

            if (!info.Methods.TryGetValue(methodName, out var method))
            {
                if (typeSymbol is ObjectSymbol obj)
                {
                    if (obj.Children.ContainsKey(methodName))
                    {
                        var builder = info.Type as TypeBuilder;
                        method = builder.DefineMethod(methodName, MethodAttributes.Public | MethodAttributes.Static, typeof(TsObject), TsTypes.ArgumentTypes);
                    }
                    else if (obj.Inherits != null)
                    {
                        method = GetInstanceMethod(obj.Inherits, methodName, position);
                    }
                    else
                    {
                        _logger.Error("Tried to call script that does not exist", position);
                        return null;
                    }
                }
                else
                {
                    method = info.Type.GetMethod(methodName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static, null, TsTypes.ArgumentTypes, null);
                    if (method is null)
                    {
                        _logger.Error("Tried to call script that doesn't exist", position);
                        return null;
                    }
                }
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
                {
                    info = GenerateType(os, position);
                }
                else if (symbol is ImportObjectLeaf leaf)
                {
                    leaf.ImportObject.Accept(_codeGen);
                }
            }

            if (info is null)
                _logger.Error($"Tried to get type that doesn't exist: {_resolver.GetAssetFullName(symbol)}", position);

            return info;
        }

        public void FinalizeType(ObjectInfo info, ISymbol parent, List<MethodInfo> scripts, TokenPosition position)
        {
            var parentConstructor = parent == null ? _baseConstructor : GetConstructor(parent, position);
            var type = (TypeBuilder)info.Type;
            var staticCtor = type.DefineTypeInitializer();
            var ctor = new ILEmitter(staticCtor, Type.EmptyTypes);
            ctor.New(typeof(Dictionary<string, TsDelegate>).GetConstructor(Type.EmptyTypes));
            var hadCreate = false;
            for (var i = 0; i < scripts.Count; i++)
            {
                var script = scripts[i];
                var name = script.Name;

                ctor.Dup()
                    .LdStr(name)
                    .LdNull()
                    .LdFtn(script)
                    .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                    .LdStr(name)
                    .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                    .Call(typeof(Dictionary<string, TsDelegate>).GetMethod("Add"));

                if (name == "create")
                {
                    GenerateConstructor(info, parentConstructor, script);
                    hadCreate = true;
                }
            }

            if (!hadCreate)
                GenerateConstructor(info, parentConstructor, null);

            ctor.StFld(info.Scripts)
                .Ret();

            GenerateInitializeMethod(info);

            var attrib = new CustomAttributeBuilder(typeof(WeakObjectAttribute).GetConstructor(Type.EmptyTypes), new Type[] { });
            type.SetCustomAttribute(attrib);
        }

        #region Native TaffyScript Object Generation

        private ObjectInfo GenerateType(ObjectSymbol os, TokenPosition position)
        {
            var typeName = _resolver.GetAssetFullName(os);
            var builder = _module.DefineType(typeName, TypeAttributes.Public);
            builder.AddInterfaceImplementation(typeof(ITsInstance));
            ObjectInfo parent = null;
            FieldInfo members;
            if (os.Inherits != null)
            {
                parent = GetObjectInfo(os.Inherits, position);
                builder.SetParent(parent.Type);
                // Todo: Verify that the parent is valid.
                //       It shouldn't be possible to inherit from a type that
                //       has a sealed TryGetDelegate method.
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

            var scripts = builder.DefineField("_scripts", typeof(Dictionary<string, TsDelegate>), FieldAttributes.Private | FieldAttributes.Static);
            var tryGetDelegate = GenerateTryGetDelegate(builder, members, scripts, GetTryGetDelegate(parent));
            var constructor = builder.DefineConstructor(MethodAttributes.Public |
                                                            MethodAttributes.HideBySig |
                                                            MethodAttributes.SpecialName |
                                                            MethodAttributes.RTSpecialName,
                                                        CallingConventions.HasThis,
                                                        new[] { typeof(TsObject[]) });
            GenerateObjectType(builder);
            var info = new ObjectInfo(builder, parent?.Type ?? _baseType, tryGetDelegate, constructor, members, scripts);
            _definedTypes.Add(os, info);

            return info;
        }

        private MethodInfo GetTryGetDelegate(ObjectInfo info)
        {
            if (info == null)
                return null;

            if (info.TryGetDelegate != null)
                return info.TryGetDelegate;

            info.TryGetDelegate = info.Type.GetMethod("TryGetDelegate", new[] { typeof(string), typeof(TsDelegate).MakeByRefType() });
            if (!info.TryGetDelegate.IsVirtual)
                _logger.Error("Tried to inherit from an object with a non-virtual TryGetDelegate method");

            return info.TryGetDelegate;
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

        private void GenerateInitializeMethod(ObjectInfo info)
        {
            var builder = (TypeBuilder)info.Type;
            var init = builder.DefineMethod("Initialize",
                                            MethodAttributes.Assembly |
                                                MethodAttributes.HideBySig |
                                                MethodAttributes.Static,
                                            typeof(ObjectDefinition),
                                            Type.EmptyTypes);

            var emit = new ILEmitter(init, Type.EmptyTypes);
            emit.LdStr(builder.FullName);

            if (info.Parent == null || info.Parent == _baseType)
                emit.LdNull();
            else
                emit.LdStr(info.Parent.FullName);

            emit.LdFld(info.Scripts)
                .LdNull()
                .LdFtn(info.Create)
                .New(typeof(Func<TsObject[], ITsInstance>).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                .New(_definitionConstructor)
                .Ret();

            _moduleInitializer.Call(init, 0, typeof(ObjectDefinition))
                              .Call(_processDefintion);
        }

        private void GenerateConstructor(ObjectInfo info, ConstructorInfo parentConstructor, MethodInfo createScript)
        {
            var ctorMethod = (ConstructorBuilder)info.Constructor;

            bool defaultCtor = true;
            if (parentConstructor != _baseConstructor)
                defaultCtor = false;

            var ctor = new ILEmitter(ctorMethod, new[] { info.Type, typeof(TsObject[]) });
            ctor.LdArg(0);
            if (defaultCtor == false)
                ctor.LdNull();
            ctor.CallBase(parentConstructor, defaultCtor ? 0 : 1);
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
                                        MethodAttributes.Private |
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

        private MethodInfo GenerateTryGetDelegate(TypeBuilder type, FieldInfo members, FieldInfo scripts, MethodInfo parentMethod)
        {

            MethodBuilder tryGetDelegateMethod;
            tryGetDelegateMethod = type.DefineMethod("TryGetDelegate",
                                                     MethodAttributes.Public |
                                                         MethodAttributes.HideBySig |
                                                         MethodAttributes.Virtual,
                                                     typeof(bool),
                                                     new[] { typeof(string), typeof(TsDelegate).MakeByRefType() });
            tryGetDelegateMethod.DefineParameter(2, ParameterAttributes.Out, "del");

            var mthd = new ILEmitter(tryGetDelegateMethod, new[] { type, typeof(string), typeof(TsDelegate).MakeByRefType() });
            var member = mthd.DeclareLocal(typeof(TsObject), "member");
            var memberLookupFailed = mthd.DefineLabel();
            var memberRightType = mthd.DefineLabel();
            var scriptLookupFailed = mthd.DefineLabel();
            mthd.LdFld(scripts)
                .LdArg(1)
                .LdArg(2)
                .Call(typeof(Dictionary<string, TsDelegate>).GetMethod("TryGetValue"))
                .BrFalseS(scriptLookupFailed)
                .LdArg(2)
                .Dup()
                .LdIndRef()
                .LdArg(0)
                .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsDelegate), typeof(ITsInstance) }))
                .StIndRef()
                .LdBool(true)
                .Ret()
                .MarkLabel(scriptLookupFailed);

            if(parentMethod != null)
            {
                mthd.LdArg(0)
                    .LdArg(1)
                    .LdArg(2)
                    .CallE(parentMethod, 3, null)
                    .Ret();
            }
            else
            {
                mthd.LdArg(0)
                    .LdFld(members)
                    .LdArg(1)
                    .LdLocalA(member)
                    .Call(typeof(Dictionary<string, TsObject>).GetMethod("TryGetValue"))
                    .BrFalseS(memberLookupFailed)
                    .LdLocalA(member)
                    .Call(typeof(TsObject).GetMethod("get_Type"))
                    .LdInt(5)
                    .BeqS(memberRightType)
                    .LdArg(2)
                    .LdNull()
                    .StIndRef()
                    .LdBool(false)
                    .Ret()
                    .MarkLabel(memberRightType)
                    .LdArg(2)
                    .LdLocalA(member)
                    .Call(typeof(TsObject).GetMethod("GetDelegate"))
                    .StIndRef()
                    .LdBool(true)
                    .Ret()
                    .MarkLabel(memberLookupFailed)
                    .LdArg(2)
                    .LdNull()
                    .StIndRef()
                    .LdBool(false)
                    .Ret();
            }

            return tryGetDelegateMethod;
        }

        #endregion
    }

    public class ObjectInfo
    {
        public Type Parent { get; }
        public Type Type { get; }
        public FieldInfo Members { get; set; }
        public FieldInfo Scripts { get; }
        public MethodInfo TryGetDelegate { get; set; }
        public ConstructorInfo Constructor { get; set; }
        public MethodInfo Create { get; set; }
        public Dictionary<string, MethodInfo> Methods { get; } = new Dictionary<string, MethodInfo>();

        public ObjectInfo(Type type, Type parent, MethodInfo tryGetDelegate, ConstructorInfo constructor, FieldInfo members, FieldInfo scripts)
        {
            Type = type;
            Parent = parent;
            TryGetDelegate = tryGetDelegate;
            Members = members;
            Constructor = constructor;
            Members = members;
            Scripts = scripts;
        }
    }
}
