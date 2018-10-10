---
layout: tutorial
title: Understanding TsObject
---

TsObject is the type that all TaffyScript variables are. It is essentially a [tagged union](https://en.wikipedia.org/wiki/Tagged_union). 

You can get the type of the tagged union from it's `Type` property, which will correspond to one of the `VariableType` enum values. The possible values are:

| Name | Value |
| --- | --- |
| Null | TsObject.Empty |
| Real | float |
| String | string |
| Array | TsObject[] |
| Delegate | TsDelegate |
| Instance | ITsInstance |

The value can be accessed generically through it's `WeakValue` property, but this will cause boxing if its `Type` is `Real`. The better way to get the value is through one of the various `Get*` methods. There is a method defined for each type that can pass through the [C# <-> TaffyScript boundary]({{site.baseurl}}/tutorial/c-sharp/interop-types). The best way to get a type is to just cast the TsObject to the desired type. There is an explicit cast operator defined for each of the aforementioned boundary types. In addition each of those types can implicitly be converted to `TsObject`.

TsObject is an abstract class that gets implemented by a different child for each version of it's `Type` property. For the most part, this doesn't need to be thought about. Unfortunately, C# doesn't allow users to define a cast operator that includes an interface for some reason. Instead each TaffyScript type defines the casts on their end. If for some reason the type doesn't define the casts, you'll need to wrap it explicitly using the `TsInstanceWrapper` class.