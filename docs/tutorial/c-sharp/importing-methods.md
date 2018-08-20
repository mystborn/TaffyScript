---
layout: tutorial
title: Importing Methods
---

One of the best ways to extend TaffyScript is to import methods. Only static methods can be imported, and they get imported as global scripts. Method imports take the following form:

```cs
import <DeclaringType>.<MethodName>([ParameterTypes]) [as <ImportName>]
```

It starts with the `import` keyword, followed by the type that declares the method. The type name can be be qualified (i.e. include its namespace), but it doesn't have to. After a dot, add the name of the method to import. In between a pair of parentheses, write the argument types, if any. Optionally, you can write `as` and then an alternate name for the resultant script.

There's one other constraint on imported methods. All arguments must be one of the following types:
* bool
* byte
* sbyte
* short
* ushort
* int
* uint
* long
* ulong
* float
* double
* char
* string
* array (TsObject[])
* object (either a TsObject or a plain old object)
* instance (an ITsInstance)

```cs
import Console.WriteLine(object) as print;
print("moo");
```

In the above, we import the WriteLine method defined by Console. It uses the overload that takes any object. We change the name from WriteLine to print, then we call the imported script. Here's an example that includes the c# code as well:

```cs
// C#
namespace Example 
{
    public static class MathFunctions 
    {
        public static float Add(float left, float right)
        {
            return left + right;
        }
    }
}

// TaffyScript

import MathFunctions.Add(float, float) as add;
var result = add(-5, 30);
print(result);
// Output:
// 25
```

Sometimes you need a method that takes a variable number of arguments. To do this, you'll need to include the TaffyScript library in your project. You can get it from nuget. Second, the method signature _must_ look like this.

```cs
[WeakMethod]
public static TsObject MethodName(TsObject[] args)
{
    // Insert code here...
}
```