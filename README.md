# TaffyScript
## What is TaffyScript?
TaffyScript is a dynamic programming language designed to be embedded into .NET applications. The primary purpose of the language is to be used for implementing game logic, and for the easy addition of mod support. However, it can be used as a general purpose language if desired. Originally it was based off of the [GameMaker Language](https://docs.yoyogames.com/source/dadiospice/002_reference/001_gml%20language%20overview/), but it no longer bears much resemblance. For more info, check out the [wiki](https://github.com/mystborn/TaffyScript/wiki).

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
* Import external .NET methods and types
* Basic Reflection

## Sounds Great! How Can I Get it?
You can now download a precompiled binary from the [releases](https://github.com/mystborn/TaffyScript/releases) page! Please note that TaffyScript is windows only. 

## Example Code
```cs
script main {
    print("Hello, World!");
    var user = new User("Script");
    print_user(user);
    user.name = "Taffy";
    print_user(user);
    print("Closing...");
}

script print_user(user) {
    print("User: " + user.name);
}

object User {
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
Occasionally I'll add issues with the `opinion wanted` label. These are issues asking about new features or language changes that I want some additional feedback on. The easiest way to contribute is to comment on those.

Alternatively, I have a very, _very_, rough draft of my plans for the language on [this](https://trello.com/b/suLDsBDJ/taffyscript) Trello board. If you want to pick anything up from that, it would certainly be a big help. If you have any further questions, you can ask me on Discord (I prefer this way, as I'm almost always online). My username is mystborn#0264. Alternatively, you can shoot me an email at ckramer017@gmail.com, but I don't check it as often as I should.

## General Roadmap
The following are some general things I'd like to add to the language at some point in the future. A slightly more detailed roadmap can be found on the [trello](https://trello.com/b/suLDsBDJ/taffyscript) page.

* Documentation Website (High Priority)
    * Just a general github pages site detailing how to use the language as well as major library documentation
* Tests
    * Currently the tests make sure certain things work, but a larger set of tests is needed to make sure things _break in the right way_.
* Runtime Interpreter
* Base Class Library
    * The BCL is extremely barebones currently. For example, it doesn't have any file handling, date/time handling, or networking scripts.
