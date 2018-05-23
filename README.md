# TaffyScript
## What is TaffyScript?
Taffy is a dynamic programming language based heavily off of Gamemaker language, 
which is the scripting language used inside of the game engine Gamemaker. For more info, check out the [wiki](https://github.com/mystborn/TaffyScript/wiki).

## Nuget
Both the TaffyScript implementation and it's base class library are available via nuget:
* https://www.nuget.org/packages/TaffyScript/
* https://www.nuget.org/packages/TaffyScript.BCL/

You can use these to use TaffyScript projects inside of .NET projects

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

## Example Code
```cs
script main {
    print("Hello, World!");
    var user = new obj_user("Script");
    print_user(user);
    user.name = "Taffy";
    print_user(user);
    print("Closing...");
}

script print_user(user) {
    print("User: " + user.name);
}

object obj_user {
    script create(name) {
        self.name = "";
    }

    script greet {
        print("Hello, " + name);
    }
}

// Output:
// Hello World!
// User: Script
// User: Taffy
// Closing...
```

## Want to contribute?
I have a very, _very_, rough draft of my plans for the language on [this](https://trello.com/b/suLDsBDJ/taffyscript) Trello board. If you want to pick anything up from that, it would certainly be a help. If you have any further questions, you can ask me on Discord (I prefer this way, as I'm almost always online). My username is mystborn#0264. Alternativle, you can shoot me an email at ckramer017@gmail.com, but I don't check it as often as I should.
