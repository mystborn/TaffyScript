using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using TaffyScript.Strings;

namespace TaffyScript.Compiler.Backend
{
    public static class TsInstanceInterfaceMethodGenerator
    {
        public static void ImplementInterfaceMethods(List<KeyValuePair<string, MemberInfo>> memberBruteForce,
                                                          RedBlackTree<int, KeyValuePair<string, MemberInfo>> memberTree,
                                                          List<MethodInfo> methodBruteForce,
                                                          RedBlackTree<int, MethodInfo> methodTree,
                                                          MethodBuilder tryGetDelegate,
                                                          MethodBuilder callMethod,
                                                          MethodBuilder getMember,
                                                          MethodBuilder setMember,
                                                          FieldInfo source,
                                                          FieldInfo members,
                                                          bool isWeaklyTyped,
                                                          string typeName,
                                                          ObjectInfo parentInfo)
        {
            // Todo: This value is currently an arbitrary value with no testing.
            //       Test to see when it's optimal to go from brute force to binary search.
            const int MinimumValuesNeededForBinarySearch = 3;

            var call = new ILEmitter(callMethod, new[] { typeof(string), typeof(TsObject[]) });
            var getm = new ILEmitter(getMember, new[] { typeof(string) });
            var setm = new ILEmitter(setMember, new[] { typeof(string), typeof(TsObject) });
            var tryd = new ILEmitter(tryGetDelegate, new[] { typeof(string), typeof(TsDelegate).MakeByRefType() });

            var callError = call.DefineLabel();
            var getError = getm.DefineLabel();
            var setError = setm.DefineLabel();
            var tryError = tryd.DefineLabel();
            var readError = getm.DefineLabel();
            var writeError = setm.DefineLabel();
            var readErrors = false;
            var writeErrors = false;

            var needsParentMethods = NeedsParentMethods(parentInfo, out var parentTry, out var parentCall, out var parentGet, out var parentSet);

            if (memberTree.Count < MinimumValuesNeededForBinarySearch)
            {
                memberBruteForce.AddRange(memberTree.InOrder().Select(kvp => kvp.Value));
                GenerateGetAndSetBruteForce(memberBruteForce, source, getm, setm, readError, writeError, ref readErrors, ref writeErrors);
            }
            else
            {
                GenerateGetAndSetBruteForce(memberBruteForce, source, getm, setm, readError, writeError, ref readErrors, ref writeErrors);

                var getHash = getm.DeclareLocal(typeof(int), "hash");
                var setHash = setm.DeclareLocal(typeof(int), "hash");

                InitializeBinarySearch(getm, getHash);
                InitializeBinarySearch(setm, setHash);

                GenerateGetAndSetBinarySearch(memberTree.Root,
                                                          source,
                                                          getm,
                                                          setm,
                                                          getHash,
                                                          setHash,
                                                          getError,
                                                          setError,
                                                          readError,
                                                          writeError,
                                                          ref readErrors,
                                                          ref writeErrors);
            }

            if (methodTree.Count < MinimumValuesNeededForBinarySearch)
            {
                methodBruteForce.AddRange(methodTree.InOrder().Select(kvp => kvp.Value));
                GenerateCallBruteForce(methodBruteForce, call, tryd);
            }
            else
            {
                GenerateCallBruteForce(methodBruteForce, call, tryd);

                var callHash = call.DeclareLocal(typeof(int), "hash");
                var tryHash = tryd.DeclareLocal(typeof(int), "hash");

                InitializeBinarySearch(call, callHash);
                InitializeBinarySearch(tryd, tryHash);

                GenerateCallBinarySearch(methodTree.Root, call, tryd, callHash, tryHash, callError, tryError);
            }

            getm.MarkLabel(getError);
            setm.MarkLabel(setError);
            call.MarkLabel(callError);
            tryd.MarkLabel(tryError);
            if (isWeaklyTyped)
            {
                var getDynamicError = getm.DefineLabel();
                var callDynamicError = call.DefineLabel();
                var tryDynamicError = tryd.DefineLabel();

                var getMemberVariable = getm.DeclareLocal(typeof(TsObject), "member");
                var callMemberVariable = call.DeclareLocal(typeof(TsObject), "member");
                var tryMemberVariable = tryd.DeclareLocal(typeof(TsObject), "member");

                getm.LdArg(0)
                    .LdFld(members)
                    .LdArg(1)
                    .LdLocalA(getMemberVariable)
                    .Call(typeof(Dictionary<string, TsObject>).GetMethod("TryGetValue"))
                    .BrFalse(getDynamicError)
                    .LdLocal(getMemberVariable)
                    .Ret()
                    .MarkLabel(getDynamicError);

                if (needsParentMethods)
                {
                    getm.LdArg(0)
                        .LdArg(1)
                        .CallE(parentGet)
                        .Ret();
                }
                else
                { 
                    getm.LdStr(typeName)
                    .LdArg(1)
                    .New(typeof(MissingMemberException).GetConstructor(new[] { typeof(string), typeof(string) }))
                    .Throw();
                }

                if (needsParentMethods)
                {
                    setm.LdArg(0)
                        .LdArg(1)
                        .LdArg(2)
                        .CallE(parentSet)
                        .Ret();
                }
                else
                {
                    setm.LdArg(0)
                        .LdFld(members)
                        .LdArg(1)
                        .LdArg(2)
                        .Call(typeof(Dictionary<string, TsObject>).GetMethod("set_Item"))
                        .Ret();
                }

                call.LdArg(0)
                    .LdFld(members)
                    .LdArg(1)
                    .LdLocalA(callMemberVariable)
                    .Call(typeof(Dictionary<string, TsObject>).GetMethod("TryGetValue"))
                    .BrFalse(callDynamicError)
                    .LdLocal(callMemberVariable)
                    .Call(typeof(TsObject).GetMethod("get_Type"))
                    .LdInt((int)VariableType.Delegate)
                    .Bne(callDynamicError)
                    .LdLocal(callMemberVariable)
                    .LdArg(2)
                    .Call(typeof(TsObject).GetMethod("DelegateInvoke"))
                    .Ret()
                    .MarkLabel(callDynamicError);

                if (needsParentMethods)
                {
                    call.LdArg(0)
                        .LdArg(1)
                        .LdArg(2)
                        .CallE(parentCall)
                        .Ret();
                }
                else
                {
                    call.LdStr(typeName)
                        .LdArg(1)
                        .New(typeof(MissingMethodException).GetConstructor(new[] { typeof(string), typeof(string) }))
                        .Throw();
                }

                tryd.LdArg(0)
                    .LdFld(members)
                    .LdArg(1)
                    .LdLocalA(tryMemberVariable)
                    .Call(typeof(Dictionary<string, TsObject>).GetMethod("TryGetValue"))
                    .BrFalse(tryDynamicError)
                    .LdLocal(tryMemberVariable)
                    .Call(typeof(TsObject).GetMethod("get_Type"))
                    .LdInt((int)VariableType.Delegate)
                    .Bne(tryDynamicError)
                    .LdArg(2)
                    .LdLocal(tryMemberVariable)
                    .CastClass(typeof(TsDelegate))
                    .StIndRef()
                    .LdBool(true)
                    .Ret()
                    .MarkLabel(tryDynamicError);

                if (needsParentMethods)
                {
                    tryd.LdArg(0)
                        .LdArg(1)
                        .LdArg(2)
                        .CallE(parentTry)
                        .Ret();
                }
                else
                {
                    tryd.LdArg(2)
                        .LdNull()
                        .StIndRef()
                        .LdBool(false)
                        .Ret();
                }
            }
            else
            {
                if (needsParentMethods)
                {
                    getm.LdArg(0)
                        .LdArg(1)
                        .CallE(parentGet)
                        .Ret();

                    setm.LdArg(0)
                        .LdArg(1)
                        .LdArg(2)
                        .CallE(parentSet)
                        .Ret();

                    call.LdArg(0)
                        .LdArg(1)
                        .LdArg(2)
                        .CallE(parentCall)
                        .Ret();

                    tryd.LdArg(0)
                        .LdArg(1)
                        .LdArg(2)
                        .CallE(parentTry)
                        .Ret();
                }
                else
                {
                    getm.LdStr(typeName)
                        .LdArg(1)
                        .New(typeof(MissingMemberException).GetConstructor(new[] { typeof(string), typeof(string) }))
                        .Throw();

                    setm.LdStr(typeName)
                        .LdArg(1)
                        .New(typeof(MissingMemberException).GetConstructor(new[] { typeof(string), typeof(string) }))
                        .Throw();

                    call.LdStr(typeName)
                        .LdArg(1)
                        .New(typeof(MissingMethodException).GetConstructor(new[] { typeof(string), typeof(string) }))
                        .Throw();

                    tryd.LdArg(2)
                        .LdNull()
                        .StIndRef()
                        .LdBool(false)
                        .Ret();
                }
            }

            if (readErrors)
            {
                getm.MarkLabel(readError)
                    .LdStr($"Member '{{0}}' defined by type {typeName} is write-only")
                    .LdArg(1)
                    .Call(typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object) }))
                    .New(typeof(MemberAccessException).GetConstructor(new[] { typeof(string) }))
                    .Throw();
            }

            if (writeErrors)
            {
                setm.MarkLabel(writeError)
                    .LdStr($"Member '{{0}}' defined by type {typeName} is read-only")
                    .LdArg(1)
                    .Call(typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object) }))
                    .New(typeof(MemberAccessException).GetConstructor(new[] { typeof(string) }))
                    .Throw();
            }
        }

        private static bool NeedsParentMethods(ObjectInfo parentInfo, out MethodInfo parentTry, out MethodInfo parentCall, out MethodInfo parentGet, out MethodInfo parentSet)
        {
            while(parentInfo != null)
            {
                if (parentInfo.Type is TypeBuilder)
                    parentInfo = parentInfo.Parent;
                else
                    break;
            }

            if(parentInfo is null)
            {
                parentTry = default;
                parentCall = default;
                parentGet = default;
                parentSet = default;
                return false;
            }

            var type = parentInfo.Type;
            while (type != null && type.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>() != null)
                type = type.BaseType;

            if (type is null || !typeof(ITsInstance).IsAssignableFrom(type))
            {
                parentTry = default;
                parentCall = default;
                parentGet = default;
                parentSet = default;
                return false;
            }

            parentTry = type.GetMethod("TryGetDelegate", new[] { typeof(string), typeof(TsDelegate).MakeByRefType() });
            parentCall = type.GetMethod("Call", new[] { typeof(string), typeof(TsObject[]) });
            parentGet = type.GetMethod("GetMember", new[] { typeof(string) });
            parentSet = type.GetMethod("SetMember", new[] { typeof(string), typeof(TsObject) });
            return true;
        }

        private static void GenerateGetAndSetBruteForce(List<KeyValuePair<string, MemberInfo>> members,
                                                             FieldInfo source,
                                                             ILEmitter getm,
                                                             ILEmitter setm,
                                                             Label readError,
                                                             Label writeError,
                                                             ref bool readErrors,
                                                             ref bool writeErrors)
        {
            foreach (var kvp in members)
            {
                var getNext = getm.DefineLabel();
                var setNext = setm.DefineLabel();
                switch (kvp.Value)
                {
                    case FieldInfo field:
                        getm.LdStr(kvp.Key)
                            .LdArg(1)
                            .Call(typeof(string).GetMethod("op_Equality"))
                            .BrFalseS(getNext)
                            .LdArg(0);

                        if (source != null)
                            getm.LdFld(source);

                        getm.LdFld(field);
                        EmitHelper.ConvertTopToObject(getm);
                        getm.Ret()
                            .MarkLabel(getNext);

                        setm.LdStr(kvp.Key)
                            .LdArg(1)
                            .Call(typeof(string).GetMethod("op_Equality"))
                            .BrFalseS(setNext)
                            .LdArg(0);

                        if (source != null)
                            setm.LdFld(source);

                        setm.LdArg(2);

                        if (!field.FieldType.IsAssignableFrom(setm.GetTop()))
                            setm.Call(TsTypes.ObjectCasts[field.FieldType]);

                        setm.StFld(field)
                            .Ret()
                            .MarkLabel(setNext);
                        break;
                    case PropertyInfo property:
                        getm.LdStr(kvp.Key)
                            .LdArg(1)
                            .Call(typeof(string).GetMethod("op_Equality"))
                            .BrFalseS(getNext);

                        if (property.CanRead && property.GetMethod.IsPublic)
                        {
                            getm.LdArg(0);

                            if (source != null)
                                getm.LdFld(source);
                            
                            getm.Call(property.GetMethod);
                            EmitHelper.ConvertTopToObject(getm);
                            getm.Ret();
                        }
                        else
                        {
                            getm.Br(readError);
                            readErrors = true;
                        }

                        getm.MarkLabel(getNext);

                        setm.LdStr(kvp.Key)
                            .LdArg(1)
                            .Call(typeof(string).GetMethod("op_Equality"))
                            .BrFalseS(setNext);

                        if (property.CanWrite && property.SetMethod.IsPublic)
                        {
                            setm.LdArg(0);

                            if (source != null)
                                setm.LdFld(source);

                            setm.LdArg(2);

                            if (!property.PropertyType.IsAssignableFrom(setm.GetTop()))
                                setm.Call(TsTypes.ObjectCasts[property.PropertyType]);

                            setm.Call(property.SetMethod)
                                .Ret();
                        }
                        else
                        {
                            setm.Br(writeError);
                            writeErrors = true;
                        }
                        setm.MarkLabel(setNext);
                        break;
                    case MethodInfo method:
                        getm.LdStr(kvp.Key)
                            .LdArg(1)
                            .Call(typeof(string).GetMethod("op_Equality"))
                            .BrFalseS(getNext)
                            .LdArg(0)
                            .LdFtn(method)
                            .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                            .LdArg(1)
                            .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                            .Ret()
                            .MarkLabel(getNext);

                        setm.LdStr(kvp.Key)
                            .LdArg(1)
                            .Call(typeof(string).GetMethod("op_Equality"))
                            .BrFalseS(getNext)
                            .Br(writeError)
                            .MarkLabel(setNext);

                        writeErrors = true;
                        break;
                }
            }
        }

        private static void GenerateGetAndSetBinarySearch(RedBlackTree<int, KeyValuePair<string, MemberInfo>>.RedBlackNode node,
                                                               FieldInfo source,
                                                               ILEmitter getm,
                                                               ILEmitter setm,
                                                               LocalBuilder getHash,
                                                               LocalBuilder setHash,
                                                               Label getError,
                                                               Label setError,
                                                               Label readError,
                                                               Label writeError,
                                                               ref bool readErrors,
                                                               ref bool writeErrors)
        {
            var getGreater = getm.DefineLabel();
            var getEqual = getm.DefineLabel();
            var setGreater = setm.DefineLabel();
            var setEqual = setm.DefineLabel();

            getm.LdLocal(getHash)
                .LdInt(node.Key)
                .Bge(getGreater);

            setm.LdLocal(setHash)
                .LdInt(node.Key)
                .Bge(setGreater);

            if (node.Left != RedBlackTree<int, KeyValuePair<string, MemberInfo>>.Leaf)
                GenerateGetAndSetBinarySearch(node.Left, source, getm, setm, getHash, setHash, getError, setError, readError, writeError, ref readErrors, ref writeErrors);
            else
            {
                getm.Br(getError);
                setm.Br(setError);
            }

            getm.MarkLabel(getGreater)
                .LdLocal(getHash)
                .LdInt(node.Key)
                .Beq(getEqual);

            setm.MarkLabel(setGreater)
                .LdLocal(setHash)
                .LdInt(node.Key)
                .Beq(setEqual);

            if (node.Right != RedBlackTree<int, KeyValuePair<string, MemberInfo>>.Leaf)
                GenerateGetAndSetBinarySearch(node.Right, source, getm, setm, getHash, setHash, getError, setError, readError, writeError, ref readErrors, ref writeErrors);
            else
            {
                getm.Br(getError);
                setm.Br(setError);
            }

            getm.MarkLabel(getEqual);
            setm.MarkLabel(setEqual);

            getm.LdArg(1)
                .LdStr(node.Value.Key)
                .Call(typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) }))
                .BrFalse(getError);

            setm.LdArg(1)
                .LdStr(node.Value.Key)
                .Call(typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) }))
                .BrFalse(setError);

            switch (node.Value.Value)
            {
                case FieldInfo field:
                    getm.LdArg(0);

                    if (source != null)
                        getm.LdFld(source);

                    getm.LdFld(field);
                    EmitHelper.ConvertTopToObject(getm);
                    getm.Ret();

                    setm.LdArg(0);

                    if (source != null)
                        setm.LdFld(source);

                    setm.LdArg(2);
                    if (!field.FieldType.IsAssignableFrom(setm.GetTop()))
                        setm.Call(TsTypes.ObjectCasts[field.FieldType]);

                    setm.StFld(field)
                        .Ret();
                    break;
                case PropertyInfo property:
                    if (property.CanRead && property.GetMethod.IsPublic)
                    {
                        getm.LdArg(0);

                        if (source != null)
                            getm.LdFld(source);

                        getm.Call(property.GetMethod);
                        EmitHelper.ConvertTopToObject(getm);
                        getm.Ret();
                    }
                    else
                    {
                        getm.Br(readError);
                        readErrors = true;
                    }

                    if (property.CanWrite && property.SetMethod.IsPublic)
                    {
                        setm.LdArg(0);

                        if (source != null)
                            setm.LdFld(source);

                        setm.LdArg(2);

                        if (!property.PropertyType.IsAssignableFrom(setm.GetTop()))
                            setm.Call(TsTypes.ObjectCasts[property.PropertyType]);

                        setm.Call(property.SetMethod)
                            .Ret();
                    }
                    else
                    {
                        setm.Br(writeError);
                        writeErrors = true;
                    }
                    break;
                case MethodInfo method:
                    getm.LdArg(0)
                        .LdFtn(method)
                        .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                        .LdArg(1)
                        .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                        .Ret();

                    setm.Br(writeError);
                    writeErrors = true;
                    break;
            }
        }

        private static void GenerateCallBruteForce(List<MethodInfo> methods,
                                                        ILEmitter call,
                                                        ILEmitter tryd)
        {
            foreach (var method in methods)
            {
                var callNext = call.DefineLabel();
                var tryNext = tryd.DefineLabel();

                call.LdStr(method.Name)
                    .LdArg(1)
                    .Call(typeof(string).GetMethod("op_Equality"))
                    .BrFalseS(callNext)
                    .LdArg(0)
                    .LdArg(2)
                    .Call(method, 1, typeof(TsObject))
                    .Ret()
                    .MarkLabel(callNext);

                tryd.LdStr(method.Name)
                    .LdArg(1)
                    .Call(typeof(string).GetMethod("op_Equality"))
                    .BrFalseS(callNext)
                    .LdArg(2)
                    .LdArg(0)
                    .LdFtn(method)
                    .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                    .LdStr(method.Name)
                    .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                    .StIndRef()
                    .LdBool(true)
                    .Ret()
                    .MarkLabel(tryNext);
            }
        }

        private static void GenerateCallBinarySearch(RedBlackTree<int, MethodInfo>.RedBlackNode node,
                                                          ILEmitter call,
                                                          ILEmitter tryd,
                                                          LocalBuilder callHash,
                                                          LocalBuilder tryHash,
                                                          Label callError,
                                                          Label tryError)
        {
            var callGreater = call.DefineLabel();
            var callEqual = call.DefineLabel();
            var tryGreater = tryd.DefineLabel();
            var tryEqual = tryd.DefineLabel();

            call.LdLocal(callHash)
                .LdInt(node.Key)
                .Bge(callGreater);

            tryd.LdLocal(tryHash)
                .LdInt(node.Key)
                .Bge(tryGreater);

            if (node.Left != RedBlackTree<int, MethodInfo>.Leaf)
                GenerateCallBinarySearch(node.Left, call, tryd, callHash, tryHash, callError, tryError);
            else
            {
                call.Br(callError);
                tryd.Br(tryError);
            }

            call.MarkLabel(callGreater)
                .LdLocal(callHash)
                .LdInt(node.Key)
                .Beq(callEqual);

            tryd.MarkLabel(tryGreater)
                .LdLocal(tryHash)
                .LdInt(node.Key)
                .Beq(tryEqual);

            if (node.Right != RedBlackTree<int, MethodInfo>.Leaf)
                GenerateCallBinarySearch(node.Right, call, tryd, callHash, tryHash, callError, tryError);
            else
            {
                call.Br(callError);
                tryd.Br(tryError);
            }

            call.MarkLabel(callEqual)
                .LdArg(1)
                .LdStr(node.Value.Name)
                .Call(typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) }))
                .BrFalse(callError)
                .LdArg(0)
                .LdArg(2)
                .Call(node.Value, 1, typeof(TsObject))
                .Ret();

            tryd.MarkLabel(tryEqual)
                .LdArg(1)
                .LdStr(node.Value.Name)
                .Call(typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) }))
                .BrFalse(tryError)
                .LdArg(2)
                .LdArg(0)
                .LdFtn(node.Value)
                .New(typeof(TsScript).GetConstructor(new[] { typeof(object), typeof(IntPtr) }))
                .LdStr(node.Value.Name)
                .New(typeof(TsDelegate).GetConstructor(new[] { typeof(TsScript), typeof(string) }))
                .StIndRef()
                .LdBool(true)
                .Ret();
        }

        private static void InitializeBinarySearch(ILEmitter mthd, LocalBuilder hashLocal)
        {
            var hashMethod = typeof(Fnv).GetMethod("Fnv32");
            mthd.LdArg(1)
                .Call(hashMethod)
                .StLocal(hashLocal);
        }
    }
}
