using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace TaffyScript.Compiler.Backend
{
    public class ObjectGenerator
    {
        private static FieldInfo baseMemberField = typeof(TsInstanceTemp).GetField("_members", BindingFlags.NonPublic | BindingFlags.Instance);
        private static Type _baseType = typeof(TsInstanceTemp);
        private static ConstructorInfo _baseConstructor = typeof(TsInstanceTemp).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
        private static ConstructorInfo _definitionConstructor = typeof(ObjectDefinition).GetConstructor(new[] { typeof(string),
                                                                                                                typeof(string),
                                                                                                                typeof(Dictionary<string, TsDelegate>),
                                                                                                                typeof(Func<TsObject[], ITsInstance>) });
        private static MethodInfo _processDefintion = typeof(TsReflection).GetMethod("ProcessObjectDefinition");

        //Todo: Come up with better storage mechanism for _tryGetDelegateStore.
        //      If the compiler is used on multiple assemblies without restart,
        //      the store will need to be cleared.
        //      What happens if the compiling happens in parallel?
        private static Dictionary<Type, MethodInfo> _tryGetDelegateStore = new Dictionary<Type, MethodInfo>();

        private FieldBuilder _scripts;
        private TypeBuilder _type;
        private Type _parentType;
        private MethodBuilder _create;

        private ObjectGenerator(TypeBuilder type, Type parent, FieldBuilder scripts)
        {
            _type = type;
            _parentType = parent;
            _scripts = scripts;
        }

        public static ObjectGenerator GenerateObjectDefinition(TypeBuilder type, Type parentType, TokenPosition position, IErrorLogger logger)
        {
            FieldInfo members;
            MethodInfo parentTryGetDelegate = null;
            parentType = parentType ?? _baseType;
            type.SetParent(parentType);

            if (parentType != null && parentType != _baseType && !_tryGetDelegateStore.TryGetValue(parentType, out parentTryGetDelegate))
            {
                if(parentType is TypeBuilder builder && !builder.IsCreated())
                {
                    var tryGetDelegateMethod = builder.DefineMethod("TryGetDelegate",
                                                                    MethodAttributes.Public |
                                                                        MethodAttributes.HideBySig |
                                                                        MethodAttributes.Virtual,
                                                                    typeof(bool),
                                                                    new[] { typeof(string), typeof(TsDelegate).MakeByRefType() });
                    tryGetDelegateMethod.DefineParameter(2, ParameterAttributes.Out, "del");
                    parentTryGetDelegate = tryGetDelegateMethod;
                }
                else
                {
                    parentTryGetDelegate = parentType.GetMethod("TryGetDelegate", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (parentTryGetDelegate.IsFinal)
                    {
                        logger.Error($"Could not inherit from {parentType.FullName}: TryGetDelegate was sealed", position);
                        return null;
                    }
                }

                _tryGetDelegateStore.Add(parentType, parentTryGetDelegate);
            }

            if (parentType != null && !typeof(TsInstanceTemp).IsAssignableFrom(parentType))
            {
                members = parentType.GetField("_members", BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance);
                if (members == null || members.FieldType != typeof(Dictionary<string, TsObject>))
                    members = type.DefineField("_members", typeof(Dictionary<string, TsObject>), FieldAttributes.Family);
            }
            else
                members = baseMemberField;

            var scripts = type.DefineField("_scripts", typeof(Dictionary<string, TsDelegate>), FieldAttributes.Private | FieldAttributes.Static);

            GenerateTryGetDelegateMethod(type, members, scripts, parentTryGetDelegate);
            GenerateObjectTypeProperty(type);

            return new ObjectGenerator(type, parentType, scripts);
        }

        public void FinalizeType(ConstructorBuilder constructor, ConstructorInfo parentConstructor, List<MethodInfo> scripts, ILEmitter moduleInitializer)
        {
            parentConstructor = parentConstructor ?? _baseConstructor;
            var staticCtor = _type.DefineTypeInitializer();
            var ctor = new ILEmitter(staticCtor, Type.EmptyTypes);
            ctor.New(typeof(Dictionary<string, TsDelegate>).GetConstructor(Type.EmptyTypes));
            var hadCreate = false;
            for(var i = 0; i < scripts.Count; i++)
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
                    GenerateConstructor(constructor, parentConstructor, script);
                    hadCreate = true;
                }
            }

            if (!hadCreate)
                GenerateConstructor(constructor, parentConstructor, null);

            ctor.StFld(_scripts)
                .Ret();

            GenerateInitializeMethod(moduleInitializer);

            var attrib = new CustomAttributeBuilder(typeof(WeakObjectAttribute).GetConstructor(Type.EmptyTypes), new Type[] { });
            _type.SetCustomAttribute(attrib);
        }

        private void GenerateInitializeMethod(ILEmitter moduleInitializer)
        {
            var init = _type.DefineMethod("Initialize",
                                          MethodAttributes.Assembly |
                                              MethodAttributes.HideBySig |
                                              MethodAttributes.Static,
                                          typeof(ObjectDefinition),
                                          Type.EmptyTypes);

            var emit = new ILEmitter(init, Type.EmptyTypes);
            emit.LdStr(_type.FullName);

            if (_parentType == null || _parentType == _baseType)
                emit.LdNull();
            else
                emit.LdStr(_parentType.FullName);

            emit.LdFld(_scripts)
                .LdNull()
                .LdFtn(_create)
                .New(typeof(Func<TsObject[], ITsInstance>).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                .New(_definitionConstructor)
                .Ret();

            moduleInitializer.Call(init, 0, typeof(ObjectDefinition))
                             .Call(_processDefintion);
        }

        private void GenerateConstructor(ConstructorBuilder ctorMethod, ConstructorInfo parentConstructor, MethodInfo createScript)
        {
            bool defaultCtor = true;
            if (parentConstructor != _baseConstructor)
                defaultCtor = false;

            var ctor = new ILEmitter(ctorMethod, new[] { _type, typeof(TsObject[]) });
            ctor.LdArg(0);
            if (defaultCtor == false)
                ctor.LdArg(1);
            ctor.CallBase(parentConstructor, defaultCtor ? 0 : 1);
            if(createScript != null)
            {
                ctor.LdArg(0)
                    .LdArg(1)
                    .Call(createScript, 2, typeof(TsObject))
                    .Pop();
            }
            ctor.Ret();

            GenerateCreateMethod(ctorMethod);
        }

        private static PropertyBuilder GenerateObjectTypeProperty(TypeBuilder type)
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
            return objectType;
        }

        private static MethodBuilder GenerateTryGetDelegateMethod(TypeBuilder type, FieldInfo membersField, FieldInfo scriptsField, MethodInfo parentMethod)
        {
            MethodBuilder tryGetDelegateMethod;
            if (!_tryGetDelegateStore.TryGetValue(type, out var temp))
            {

                tryGetDelegateMethod = type.DefineMethod("TryGetDelegate",
                                                 MethodAttributes.Public |
                                                     MethodAttributes.HideBySig |
                                                     MethodAttributes.Virtual,
                                                 typeof(bool),
                                                 new[] { typeof(string), typeof(TsDelegate).MakeByRefType() });
                tryGetDelegateMethod.DefineParameter(2, ParameterAttributes.Out, "del");
                _tryGetDelegateStore.Add(type, tryGetDelegateMethod);
            }
            else
                tryGetDelegateMethod = (MethodBuilder)temp;

            var mthd = new ILEmitter(tryGetDelegateMethod, new[] { type, typeof(string), typeof(TsDelegate).MakeByRefType() });
            var member = mthd.DeclareLocal(typeof(TsObject), "member");
            var memberLookupFailed = mthd.DefineLabel();
            var memberRightType = mthd.DefineLabel();
            var scriptLookupFailed = mthd.DefineLabel();
            mthd.LdArg(0)
                .LdFld(membersField)
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
                .LdFld(scriptsField)
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
                .LdArg(0)
                .LdFld(membersField)
                .LdArg(1)
                .LdArg(2)
                .LdIndRef()
                .New(TsTypes.Constructors[typeof(TsDelegate)])
                .Call(typeof(Dictionary<string, TsObject>).GetMethod("Add"))
                .LdBool(true)
                .Ret()
                .MarkLabel(scriptLookupFailed);

            if(parentMethod != null)
            {
                mthd.LdArg(0)
                    .LdArg(1)
                    .LdArg(2)
                    .Call(parentMethod, 3, null)
                    .Ret();
            }
            else
            {
                mthd.LdArg(2)
                    .LdNull()
                    .StIndRef()
                    .LdBool(false)
                    .Ret();
            }

            return tryGetDelegateMethod;
        }

        private void GenerateCreateMethod(ConstructorInfo ctor)
        {
            _create = _type.DefineMethod("Create",
                                        MethodAttributes.Private |
                                            MethodAttributes.Static |
                                            MethodAttributes.HideBySig,
                                        typeof(ITsInstance),
                                        new[] { typeof(TsObject[]) });
            var emit = new ILEmitter(_create, new[] { typeof(TsObject[]) });
            emit.LdArg(0)
                .New(ctor, 1)
                .Ret();
        }
    }
}
