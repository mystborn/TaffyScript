---
layout: tutorial
title: If/Else Statements
---

An if statement allows you to perform an action if a certain condition is met. An if statement takes the following form:

```cs
if(condition) <body>
```

If the condition is met, the body is executed. If you want something to happen if the condition _isn't_ met, then you can add an `else` to end of the if block like so:
```cs
if(condition) <body> else <body>
```

For example:

```cs
if(i == 0)
    zero = true;
else
    zero = false;

if(name == "Chris") {
    print("Hello, Chris");
} else if(name == "Bob") {
    print("Hello, Bob");
} else {
    print("Who are you?");
}
```