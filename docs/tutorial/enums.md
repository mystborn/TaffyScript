---
layout: tutorial
title: Enums
---

In TaffyScript, an enum is a set of named number constants. To declare an enum, write `enum` followed by the enum name. Inside of a block, you write each constant name separated by a comma. Each name gets assigned a value starting at 0, and then iterating by one. For example:
```cs
enum HorizontalAlignment {
    Left, // = 0
    Center, // = 1
    Right // = 2
}
```

If you wish to assign a value to the name you can do so, and each successive name will increment based on that value:
```cs
enum State {
    Walking = 5,  // = 5
    Running,      // = 6
    Crawling,     // = 7
    Laughing = 0, // = 0
    Sleeping      // = 1
}
```

As such, two names in the same enum can have the same value.

To use an enum, write the enum name, a dot, and then the enum value name like so:
```cs
var value = State.Walking;
```

That will set value to 5.

You can directly use enums defined in external .NET libraries:
```cs
//In a c# project
namespace CSharpExample
{
    public enum External 
    {
        First,
        Second
    }
}

//In a TS project
using CSharpExample;

script main {
    print(External.First);
}
```
It will automatically convert the enum value into a number. This means you can't import methods that take an enum parameter, though you might be able to in future versions.