# TaffyScript
## What is TaffyScript?
Taffy is a dynamic programming language based heavily off of Gamemaker language, 
which is the scripting language used inside of the game engine Gamemaker. For more info, check out the [wiki](https://github.com/mystborn/TaffyScript/wiki).

_Psst. I've added something to the contributing section of this page. Please check it out._

## Example Code
```cs
script main {
    show_debug_message("Hello, World!");
    var user = new obj_user();
    user.name = "Taffy";
    user.destroy();
    show_debug_message("Closing...");
}

object obj_user {
    event create {
        name = "";
    }

    event destroy {
        show_debug_message("Goodbye, " + name);
    }
}

// Output:
// Hello World!
// Goodbye, Taffy
// Closing...
```

## Features
* Built in C#
* Compiles into a valid .NET assembly
* Dynamic Typing
* Does not use DLR
* Accessable from existing .NET projects
* Import external .NET methods.
* Basic Reflection

## Sounds Great! How Can I Get it?
You can now download a precompiled binary from the [releases](https://github.com/mystborn/TaffyScript/releases) page! Please note that TaffyScript is windows only.

## Nuget
Both the TaffyScript implementation and it's base class library are available via nuget:
* https://www.nuget.org/packages/TaffyScript/
* https://www.nuget.org/packages/TaffyScript.BCL/

You can use these to use TaffyScript projects inside of .NET project

## Want to contribute?
The easiest way to contibute is to look at issues with the `opinion wanted` label. These are issues asking about feature requests that I want some user input on. Otherwise, check out the contributing.ms file in the root of the repo. If you have any further questions, please don't hesitate to ask!
