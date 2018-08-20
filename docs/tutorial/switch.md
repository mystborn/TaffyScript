---
layout: tutorial
title: Switch Statement
---

A switch statement allows you to choose an action based on a value. A switch statement has the following form:

```cs
switch(value) {
    case 0:
        //code if value is 0
        break;
    case 1:
        //code if value is 1
        break;
    default:
        //code if value is anything other than 0 or 1
        break;
}
```

As you can see, each action is represented by a case that will be executed if the value matches the case. You can also have a default case that will be executed if the value doesn't meet any other case. The case value can technically be any expression, but the compiler will work best if they are constants (i.e. numbers, strings, enum values). If the case is an expression, it will get chosen if value is equal to it.

## Break

When the break statement is reached, it will end the action. If there is code under the break statement inside of the case block, it will not be executed.

## Fallthrough

In TaffyScript, if a break statement is not found, the code will continue on to the next case block. This is called fallthrough. This behaviour can be desirable in some situations, but you must be mindful to use `break` in situations where it isn't.