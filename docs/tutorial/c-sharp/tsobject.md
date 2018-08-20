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

You can access the value through the `WeakValue` property. 