If you'd like to contribute to TafyScript, there are plenty of opportunities to do so.

The most important ways to contribute don't even require any digging through the source code. These include:
* Art! If you're creatively inclined, I'd love to see any art that could be applied to the language.
* Web Design. This is pretty unrealistic, but if you'd like to help in the biggest way possible, eventually I'd like to move the documentation over to a github pages site. Unfortunately, I'm artistically challenged, so if you'd like to work with me to get a basic foundation laid, you'd be providing the most valuable support.
* Documentation. The project isn't quite ready for this step, but it's close. The base class library consists mainly of functions ported over from Gamemaker, but there are subtle differences that must be documented, and the docs should be entirely decoupled from GM to begin with.
* Plugins. If you have experience writing plugins for text editors like Sublime Text or VSCode, you would be providing a huge service by creating one for TaffyScript.

For the people willing to sift through the code and make changes, first of all thank you. Second, here's a basic list of things that should be implemented:
* In MsilWeakCodeGen, when accessing a data structure, the index gets saved to a local variable. If it's a constant value, it should use that instead of a local. (See ListAccessSet, GridAccessSet, MapAccessSet).
* In MsilWeakCodeGen, when visiting prefix and postfix nodes, only load the result to the eval stack if it's parent node is NOT a block.
* In MsilWeakCodeGen, when accessing a member variable, if the left hand side is an object type, set the value on the first object of that type if one exists. This is done in order to keep GM compatibilty.
* Contribute to the BCL.
* Bug fixes. Bug fixes are always appreciated  :)
* Comments and Grammar. Right now my comments are pretty sparse, and I know that my grammar isn't always the best. If you think any part needs a comment, go ahead and add it. If you come across a comment that can be modified to be more clear or that has improper grammar, change it. 


For those willing to invest a fair bit of time into the project, here are some large changes that are currently planned:
* Namespaces. This will help alleviate the potential of running into naming conflicts in larger projects or when using libraries.
* Strong Functions. Currently all methods have the same signature `TsObject MethodName(TsObject[] args)`. However, on methods that don't require a variable number of arguments, the arguments should be layer out properly i.e. `TsObject MethodName(TsObject arg1, TsObject arg2)`
* Macros. This is pretty low on the list, but these are variables assigned to a constant value. These will have to be preprocessed, and will generally slow compile time down. One solution is to only allow macros to be declared all in one special file, but no idea is set in stone yet.
* Lazy or Faster Initialization. This library uses a special method to load all of it's scripts and objects into a usable form on startup, but on larger projects this could cause considerable slowdown.
* Nuget integration. This is top priority for now. Due to TaffyScripts tight coupling with c#, Nuget is the obvious route to take for a package manager. It should work relatively well out of the box, but certain core libraries need to actually be added to nuget, and then some custom tooling could be nice.
