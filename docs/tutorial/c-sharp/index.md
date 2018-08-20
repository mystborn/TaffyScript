---
layout: tutorial
title: C# Integration
---

Up until now, the focus of the tutorial has been almost entirely on TaffScript code. However, TaffyScript is realtively limited in terms of what it can do. The Base Class Library is growing, but is still missing many crucial parts. To make up for it, it can easily integrate with external .NET libraries. I'll be referring to all external libraries as c# libraries, but they can be any .NET language.

The relationship goes both ways. TaffyScript can fill in its own gaps with C#, but C# can use TaffyScript libraries. On of the main reasons the language was created was to have a language that could be used to implement game logic that could be embedded into a MonoGame project, and TaffyScript fulfills this purpose well. In addition, due to it's dynamic nature and basic reflection support, it's easy to load in TaffyScript libraries at runtime, which makes adding mod support extremely easy.

The first couple of tutorials will define how to integrate with c#. After that, there will be some examples from the BCL and other official libraries.

## Tutorials

* Importing Scripts
* Importing Objects
* Using TaffyScript in C#


## Examples

* Implementing array_copy
* Wrapping List<T>
* Subscribing to an event in TaffyScript.Speech