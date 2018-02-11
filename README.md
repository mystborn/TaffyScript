# Taffy
## What is Taffy?
Taffy is a dynamic programming language based heavily off of Gamemaker language, 
which is the scripting language used inside of the game engine Gamemaker.

## What Makes Taffy Special?
Taffy is built in C# and compiles down to a valid .NET assembly. Despite being a dynamically typed language, it doesn't use the DLR at all, 
allowing it to have speed both in execution and development.
In addition, it can be easily added to current .NET projects as an embedded scripting language.
Furthermore, it supports a wide variety of Reflection-esque features, making it particularly useful as a plug-in language.
Finally, it can easily reference methods defined in any .NET language, giving it access to a wide variety of features, such as the phenomenal Base Class Library.

## Sounds Great! How Can I Get it?
Currently the only way to acquire Taffy is to build it from the source, however that will be changing soon.

## Example Code
```cs
script main {
    var user = instance_create(obj_user);
    user.name = "Taffy";
    instance_destroy(user);
    show_debug_message("Closing...");
}

object obj_user {
    event create {
        name = "";
    }

    event destroy {
        show_debug_message("goodbye, " + name);
    }
}
```

## What's Left?
* Base class library (High)
* Better error handling during compilation. (High)
* Namespace/module system (Mid)
* C style import (Low)
