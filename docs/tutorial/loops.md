---
layout: tutorial
title: Loops
---

*Note: on all of the examples, I put the loop body in curly braces. If you have a simple (one-line) statement, you don't have to do that. For more complex bodies, it is still necessary.*

# Basic Loops

## Repeat
A repeat loop has the following form:

```cs
repeat(count) <body>
```

`count` can be a constant or an expression that results in a number. The code in the repeat body will execute the specified number of times.

```cs
var i = 0;
repeat(3) {
    print(i++);
}
// Outputs:
// 0
// 1
// 2
```

## While

A while loop has the following form:

```cs
while(condition) <body>
```

The code in the while body will continue to execute while the condition is true or a break statement is hit.

```cs
var i = 0;
while(i < 3) {
    print(i++);
}
// Outputs:
// 0
// 1
// 2
```

## Do Until

A do-until loop has the following form

```cs
do <body> until <condition>
```

The code in the do body will continue to execute until the `until` condition is true. Unlike a while loop, the do-until loop will always execute at least once.

```cs
var i = 0;
do {
    print(i++);
}
until(i == 3);
// Outputs:
// 0
// 1
// 2
```

## For

A for loop has the following form:

```cs
for(<init>; <condition>; <increment>) <body>
```

This loop is obviously a bit more complicated. First, the `init` segment is executed. Then, while the `condition` is true, the body is executed. After each execution of the body, the `increment` code is run.

```cs
for(var i = 0; i < 3; i++) {
    print(i);
}
// Outputs:
// 0
// 1
// 2
```

`init`, `condition`, and `increment` are all optional. If `init` or `increment` are missing, those steps simply aren't executed. If the `condition` is missing, It will just continue to run forever until something breaks out of the loop.

```cs
for(var i = 0;;i++) {
    print(i);

    if(i >= 3)
        break;
}
// Outputs:
// 0
// 1
// 2
// 3
```

## Continue
If the code hits a continue statement in any loop, it stops executing the current iteration of the body and move directly on to the next. For example, the following code uses continue to only output even numbers:
```cs
for(var i = 0; i < 10; i++) {
    if(i % 2 == 1)
        continue;
    print(i);
}
```

## Break
If the code hits a break statement in any loop, it will stop all execution of the current loop.

```cs
var i = 0;
while(true) {
    print(i++);

    if(i == 3)
        break;
}
// Ouptuts:
// 0
// 1
// 2
```