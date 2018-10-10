---
layout: tutorial
title: Writing TaffyScript Assemblies in C#
---

When using the TaffyScript.Speech or TaffyScript.MonoGame assemblies, you don't have to import anything despite them having been written in C#. This can be extremely useful when writing libraries for TaffyScript without having to an extra assembly just for the imports. This tutorial describes how to do that.

## Setting Up

First, the project will need to reference TaffyScript.dll. The file can easily be got from Nuget.

The next thing you need to do is make sure the TaffyScript compiler knows that this is a TaffyScript assembly. To do so, inside of your project there will be a file called `AssemblyInfo.cs`. In Visual Studio, this is inside of properties.

![AssemblyInfo.cs in Visual Studio]({{site.baseurl}}/assets/images/tutorial/AssemblyInfoInVisualStudio.png "AssemblyInfo.cs in Visual Studio")

Inside of the file, we're going to reference an attribute defined inside of TaffyScript.dll, so add this using statement to the top:

```cs
using TaffyScript;
```

Then, anywhere inside of the file, add the following:

```cs
[assembly: TaffyScriptLibrary()]
```

This will add the attribute `TaffyScriptLibarary` to the assembly. When the TaffyScript compiler loads an assembly, it looks for this attribute to determine what to do with it.

## Defining Global Scripts

In C# there is no such thing as a global method. They have to be bound to a class. TaffyScript creates this class behind the scenes. Here we have to make one for ourselves. It can have any name, as the name will never be used. What's important is that the scripts defined in the class will be available globally from the _namespace_ that the class is in.

In addition, the class will need a special attribute: `TaffyScriptBaseType`. This lets the compiler know that the class defines TaffyScript scripts.

Now, any public static method that takes `TsObject[]` and returns a `TsObject` will be directly callable from TaffyScript.

```cs
// C#
using TaffyScript;

namespace Math.Extensions 
{
    [TaffyScriptBaseType]
    public static MathScripts
    {
        /// <summary>
        /// Takes two numbers, adds them, and returns the result.
        /// </summary>
        public static TsObject add(TsObject[] args)
        {
            return (float)args[0] + (float)args[1];
        }

        /// <summary>
        /// Takes two numbers, subtracts them, and returns the result.
        /// </summary>
        public static TsObject minus(TsObject[] args) {
            return (float)args[0] - (float)args[1];
        }
    }
}

// TaffyScript
using Math.Extensions;

script main() {
    var result = add(10, 5);
    print(result);

    result = minus(10, 5);
    print(result);
}

// Outputs:
// 15
// 5
```

## Defining TaffyScript Objects

To define an object that will be usable from TaffyScript in C#, simply create a class that implements ITsInstance. This will let TaffyScript interact with the class, but it won't let TaffyScript create an instance of it. In order to allow that to happen, the class also needs the `TaffyScriptObject` attribute as well as a constructor that takes `TsObject[]` as it's only parameter.

### Implementing ITsInstance

All TaffyScript object implement ITsInstance. This section will outline what is expected of an external class implementing it.

#### ObjectType

```cs
string ObjectType { get; }
```

Each class will need to have a property `ObjectType`. This is the TaffyScript name of the type. In 99% of circumstances, this should return the name of the class, including the namespace.

#### GetMember

```cs
TsObject GetMember(string name);
```

GetMember will take the name of a member and return the value or throw a `MissingMemberException`. Typically implemented using a switch statement, it should first search for fields or properties with the desired name, then it should search for a method with the same name and return a delegate for it. If the type is dynamic, it can then lookup the name in a `Dictionary<string, TsObject>`. Finally, it should throw the exception.

#### SetMember

```cs
void SetMember(string name, TsObject value);
```

SetMember should take the name of the member as well as the value to set it to. It will update the member if it can be set or throw a `MissingMemberException` if it doesn't exist or if it's readonly.

#### TryGetDelegate

```cs
bool TryGetDelegate(string scriptName, out TsDelegate del);
```

This should get a delegate to a script defined by the type. It returns true if the script is found, setting `del` to the script. It returns false if the script wasn't found, setting `del` to `null`.

#### GetDelegate

```cs
TsDelegate GetDelegate(string scriptName);
```

Very similar to `TryGetDelegate`, this will return a delegate to a script with the matching name. This should throw a `MissingMethodException` if there is no script that matches the name.

#### Call

```cs
TsObject Call(string scriptName, TsObject[] args);
```

This will call the script with the matching name and return the result. If the script doesn't exist, it should throw a `MissingMethodException`.

#### Indexer

```cs
TsObject this[string memberName] { get; set; }
```

This should be used as a shorthand to `GetMember` and `SetMember`. Typically this will just forward the call to those to methods.

#### Casting

```cs
public static implicit operator TsObject(TypeName type);
public static explicit operator TypeName(TsObject obj);
```

This part isn't strictly necessary, but it is useful. All compiler generated types define an implicit cast to `TsObject` and an explicit cast from `TsObject`. Typically the implicit operator returns a `TsInstanceWrapper` constructed with `this` as the value. The explicit operator casts `obj.WeakValue` to `TypeName` and returns the result. If you wanted to be safer, you could cast the result of `obj.GetInstance` instead, but an exception will be thrown on failure either way, so I find it best to not waste time on successful casts.

### Example

The following is an example implementation of a person class that can be used from TaffyScript.


```cs
using System;
using TaffyScript;

namespace Example {

    [TaffyScriptObject]
    public class Person : ITsInstance 
    {
        public string ObjectType => "Example.Person";

        public this[string memberName] 
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string Name { get; }

        // The height of the person in inches.
        public float Height { get; set; }

        public Person(TsObject[] args) 
        {
            Name = (string)args[0];
            Height = (float)args[1];
        }

        public TsObject GetMember(string name)
        {
            switch(name) 
            {
                case "name":
                    return Name;
                case "Height":
                    return Height;
                default:
                    if(TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            switch(name) 
            {
                case "height":
                    Height = (float)value;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName)
            {
                case "grow":
                    del = new TsDelegate(grow, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        publid TsDelegate GetDelegate(string scriptName)
        {
            if(TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public TsObject Call(string scriptName, TsObject[] args)
        {
            switch(scriptName)
            {
                case "grow":
                    return grow(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsObject grow(TsObject[] args)
        {
            Height += (float)args[0];
            return TsObject.Empty;
        }

        public static TsObject get_hamilton(TsObject[] args)
        {
            return new Person("Alexander Hamilton", 67);
        }

        public static implicit operator TsObject(Person person)
        {
            return new TsInstanceWrapper(person);
        }

        public static explicit operator Person(TsObject obj)
        {
            return (Person)obj.WeakValue;
        }
    }
}
```

Even though the actual properties in the class are PascalCase, because of the switch statements they are accessed in TaffyScript as snake_case.

```cs
using Example;

script main() {
    // He registered his height as 6 ft 3.5 in when ordering his coffin,
    // but his real height was likely somewhat smaller
    // according to wikipedia.
    var person = new Person("George Washington", 75.5);
    print(person.name);
}
```

Any static methods with the TaffyScript signature will be callable from TaffyScript through the type. So in the `Person` class, it defines the `get_hamilton` method that can be called as a script in TaffyScript like so:

```cs
using Example;
script main() {
    var hamilton = Person.get_hamilton();
    print(person.name);
}
```

These classes can be inherited, but if this is the desired behaviour, make sure to mark any method that might be overridden as virtual, otherwise the compiler will emit an error.

## SpecialImports

Sometimes the above just isn't enough, or the implementation is undesirable. For example, if you wanted to define a script in the global namespace from C#, you'd have to pollute the global namespace, which is generally not recommended and can cause many annoying name conflicts. Other times the name of a C# class can't be the same as the desired TaffyScript type. In all of these situations, the TaffyScript compiler can transform the names and namespaces through a special file called `SpecialImports.resource`.

To create this file, inside of your solution create a file with the name `SpecialImports.resource`. That part may seem obvious. However, this file shouldn't be bundled _along_ with the assembly, it should be embedded inside of it. To do that, inside of the Properties pane in Visual Studio, inside of the Advanced section, change it's Build Action from whatever it's set to to `Embedded Resource`.

![Special Imports Example]({{site.baseurl}}/assets/images/tutorial/SpecialImportsExample.png "Special Imports Example")

This file is a list of items to import. These items can be an object that implements ITsInstance and has a constructor that only takes `TsObject[]`, or it can be a static method that takes `TsObject[]` as it's only argument and returns a `TsObject`. Each item should have it's own line.

Each import is split into four parts, each seperated with a `:`.

1. Determines which type of import it is.
2. The namespace to import the type or script into. This is blank if the desired namespace is the global namespace.
3. The imported name of the script or object.
4. The name of the type or method to import.

### Import Type

This determines which type of import the line is. Currently there are only two types of imports, but more may be added in the future.

| Value | Type |
| --- | --- |
| 0 | Script |
| 1 | Object |

### Import Namespace

This part determines which namespace the imported type or script is added to. If this is left blank, it is imported into the global namespace. Importing into the global namespace is one of the main reasons to use the `SpecialImports.resource` file.

### Import Name

This part determines the name of the imported script or object. This can be used to create a type or script that conflicts with a c# type or script, or even conflicts with a C# keyword. For example, the BCL defines the [typeof]({{site.baseurl}}/docs/typeof) script.

### C\# Name

This is the name of the C# type or method to import. This can be namespace qualified if the name can be found in two different namespace. If a script is being imported, this **must** include the name of the type that defines the method.

The following is an excerpt from the `SpecialImports.resource` file found inside of the `TaffyScript.dll` assembly.

```txt
1::Grid:TsGrid
1::List:TaffyScript.Collections.TsList
0::real:GlobalScripts.real
```

As you can see, the `List` type used in TaffyScript is called `TaffyScript.Collections.TsList` in C#, but gets transformed by the compiler when read from this file.