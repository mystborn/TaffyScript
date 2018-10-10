---
layout: tutorial
title: Interop Types
---

When interacting with C#, sometimes you'll want to pass and receive values from TaffyScript code. The C# type system is quite a bit larger than the one that TaffyScript implements, so only certain values can pass through the language barrier. So any place that uses C# are constrained to the following types:
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

Note that you cannot call a C# method that returns a plain object from TaffyScript. However methods that have no return type (e.g. `void`) are allowed. When called in TaffyScript, they will return `null`.

More types may be added into the future, but these type will always be allowed. Potentially allowed types in the future might be enums referenced by name and TaffyScript types referred to by name.