---
layout: tutorial
title: Variables
---

In TaffyScript there are 3 types of variables: local, global, and instance.

## Local

To declare a local variable, just write `var` and then the variable name. For example:
```cs
var name;
name = "Chris";
var age = 20;
```

You can declare multiple local variables on the same line by separating each name by a comma:

```cs
var name, age;
var eye_color = "blue", hobby = "programming";
var computer_on = true, drink, dinner;
```

As you can see, we can mix assigning and just declaring a variable on the same line.

A local variable can only be accessed within the current scope. After the scope exits, the variable is gone forever.

## Scoping

TaffyScript is technically lexically scoped, but only because very few things actually define a new scope. In most programming languages, a set of curly braces `{ }` defines a new scope, but _not_ in TaffyScript. The only construct that defines a new scope is a script. This includes lambdas. What does all this mean? Let's look at some examples.

```cs
var i = 0;
if(true) {
    i = 10;
}
print(i);
// Outputs: 10
```

This example works like most other programming languages.

```cs
var i = 0;
if(true) {
    var i = 10;
}
print(i);
// Outputs: 10
```

In most other languages, the second use of var would [shadow](https://en.wikipedia.org/wiki/Variable_shadowing) the previous variable for the duration of the code in between the braces. Here it overwrites the old i.

```cs
var i = 0;
var lambda = script { var i = 10; print(i); }
lambda();
print(i);
// Outputs:
// 10
// 0
```

Here you can see how a script creates a new scope.

## Instance

When declaring instance variables, you must assign a value to the variable. Each declaration must be a separate statement, and therefore can be separated by semicolons or any amount of whitespace. You can only assign instance variables inside of an instance script.

```cs
object Example {
    script create {
        name = "Chris";
        age = 20;
    }
}
```

These variables can only be accessed through the object instance.

Alternatively, you can also explicitly reference the instance.

```cs
object Example {
    script create {
        self.name = "Chris";
        self.age = 20;
    }
}
```

That syntax is useful if an argument corresponds with a variable name.

## Global

In TaffyScript, you can access global variables by prefixing the variable name with `global.`. For example:

```cs
global.name = "Chris";
print(global.name);
```

These variables can be accessed from anywhere. It's usually bad practice to use global variables, so use them sparingly.