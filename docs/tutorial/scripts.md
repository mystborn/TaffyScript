---
layout: tutorial
title: Scripts
---

Scripts are the bread and butter of TaffyScript. A script is a reusable piece of code. A script is just another name for a function or method.

## Defining a Script

To define a script, start with `script` then the script name. Then inside of a block, write the script code.
```cs
script example {
    print("I'm in a script!");
}
```

## Arguments

If you want your script to have arguments, you can add them to the script signature after the script name in between parentheses. Each argument needs to seperated by a comma.

```cs
script example(arg1, arg2) {
    print(arg1);
}
```

Script arguments can also have a default value. Any number of arguments can have a default, but they must come after all of the arguments without one.

A default value must be one of the following:
* bool
* string
* number
* null

To define a default value, simply add an equals sign `=` after the argument name, followed by the desired default value.

```cs
script example(arg1, arg2 = "moo") {
    print(arg2);
}
```

While the above methods of accessing arguments is nice, sometimes it is not sufficient. You can access the argument array directly, either by writing argument followed by a number, or by using traditional array syntax:
```cs
script example() {
    print(argument0);
    print(argument[1]);
}
```
You can determine how many arguments were passed to a script using the keyword `argument_count`:

```cs
script print_argument_count() {
    print(argument_count);
}
```

## Calling a script

You can call a script by writing it's name, followed by `(`, writing a comma separated argument list, then ending with `)`. For example:
```cs
script_without_args();
script_with_args("hello", "world", 10);
```

## Return Values
You can exit from a script early using the `return` keyword. Optionally, it can be followed by a value to return. By default, all scripts return the value `null`. If you want to make sure the script returned something, you can use the script `is_null(value)` to check.

```cs
script script_with_return() {
    return 10;
}
```

## Examples

This is an example of a script that does something similar to string.Join in c#.

```cs
script join(separator) {
    var result = string(argument1); //The result start with the second argument by itself, then appends the rest of the arguments with the separator string in between.
    for(var i = 2; i < argument_count; i++) {
        result += separator + string(argument[i]);
    }
    return result;
}

script main() {
    print(join(", ", "moo", 0, 32.5, "cow", "etc"));
    
    // Output: moo, 0, 32.5, cow, etc
}
```