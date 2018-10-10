---
layout: tutorial
title: Importing Methods
---

One of the best ways to extend TaffyScript is to import methods. Only static methods can be imported, and they get imported as global scripts. Method imports take the following form:

```cs
import <DeclaringType>.<MethodName>[(ParameterTypes)] [as <ImportName>]
```

It starts with the `import` keyword, followed by the type that declares the method. The type name can be be qualified (i.e. include its namespace), but it doesn't have to. After a dot, add the name of the method to import. In between a pair of parentheses, write the argument types, if any. Optionally, you can write `as` and then an alternate name for the resultant script. The argument types must be one of the [interop types]({{site.baseurl}}/tutorial/c-sharp/interop-types).

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

script main() {
    var result = add(-5, 30);
    print(result);
}

// Output:
// 25
```

Sometimes you need a method that takes a variable number of arguments. To do this, you'll need to include the TaffyScript library in your project. You can get it from nuget. Second, the method signature _must_ look like this.

```cs
[TaffyScriptMethod]
public static TsObject MethodName(TsObject[] args)
{
    // Insert code here...
}
```

For example, this is a method that takes any number of arguments and writes them all.

```cs
// C#

public class Printer 
{
    [TaffyScriptMethod]
    public static TsObject PrintAll(TsObject[] args) 
    {
        for(var i = 0; i < args.Length; i++)
        {
            Console.WriteLine(args[i]);
        }
    }
}

// TaffyScript

import Printer.WriteAll(array) as print_all;
script main() {
    print_all("Hello", "World", 12, [0, 1, 2]);
}

//Outputs:
// Hello
// World
// 12
// [0, 1, 2]
```

Methods with that signature and attribute are desirable because they can be called directly from TaffyScript. If either is missing, the compiler generates a wrapper method that unpacks the arguments and calls the imported script.