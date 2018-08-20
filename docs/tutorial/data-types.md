---
layout: tutorial
title: Data Types
---

In TaffyScript, there are very few built in type. The ones that exist are as follows:
* Number (float)
* String
* Array
* Instance
* Script
* Null

## Comments

This isn't a data type, but it seemed like a good place to outline it. There are two types of comments in TaffyScript.

A single line comment starts after `//`

```cs
// A comment

var i = 0; // A comment on a line with code
```

A multi-line comment starts after `/*` and ends after `*/`
```cs
/*
This is 
a multi-line
comment.
*/
```

## Number

Numbers in TaffyScript are single precision floating point numbers, or floats.

Number constants can be any numerical value with or without a decimal point (i.e. 13354 or 60.6). In addition, you can use the following two hex styles:
* Prefix with 0x - `0xFFFF00`
* Prefix with ? - `?FFFF00`

## String

String constants can be any number of characters between single or double quotes. There is no difference between the two styles. In addition you can add a special character by using a forward slash followed by the value. The following special characters are supported

| Symbol | Description |
| --- | --- |
| \\n | Adds a new line character. |
| \\r | Adds a carriage return character. |
| \\t | Adds a tab character. |
| \\" | Adds a double quote character. |
| \\' | Adds a single quote character. |
| \\\\ | Adds a forward slash character. |
| \\u[xxxx] | Adds a character whose value is equal to the hex value following the special character. You can escape the hex value at any point by putting an incompatible character or by using a closing forward slash. |


```java
"This is a string"
'This is also a string'
"This string is split \n into two lines"
"This string has a special hex character \u007E"
"This string has an escaped hex character \u7e\ "
```

You can concatenate two strings together using the `+` operator, but you _can't_ add a string and any other data type. Instead, you should convert the other type to a string using the `string` script and then add the two values together.

```cs
var left = "Hello ";
var right = "World";
var result = left + right;
print(result);
// Outputs:
// Hello World

left = "3 + 3 = ";
right = 3 + 3;
result = left + string(right);
print(result);
// Outputs:
// 3 + 3 = 6
```

## Array

Arrays can be created using the `new` keyword with the length as an argument:

```cs
var array = new Array(10); //Creates an array with 10 elements.
```

Arrays can also be created using an array literal:

```cs
var array = [0, 1, 2];
```

Or by the following script:

```cs
var array = array_create(3); // Creates an array with 3 null elements
```

You can access an element in the array using the `[]` accessor like so:

```cs
var array = [0, 1, 2];
var first_element = array[0];
var last_element = array[2];
```

Arrays in TaffyScript are all single dimensional. However, you can access jagged arrays as if they were multi-dimensional arrays:

```cs
var array = [["moo"]]; // Creates a jagged array.
var access1 = array[0][0]; // Typical jagged array access.
var access2 = array[0, 0]; // Access the jagged array as if it were a multi-dimensional array.
```

## Bool

There is no data type for true and false values. Instead, any numeric value that is greater than 0 is true and everything else is false. However, you can use the keyword `true` and `false` which equate to `1` and `0` respectively.

## Null

A null value represents the lack of a value. Pretty similar to null in c#. You can use the `null` literal to represent it in code.

## Instance

An instance is an instance of an object. You can create an instance using the `new` keyword followed by the object name like so:

```cs
script main() {
    var inst = new User();
}

object User { }
```

## Script Instance

A script instance (usually just referred to as a script) is a callable type. They can be created by assigning a variable to an instance or global script. They can also be created inside of script definitions. These are typically referred to as lambdas or inline scripts.

```cs
script test() {
    print("hello");
}

object Cow {
    script moo() {
        print("moo");
    }
}

script main() {
    var hello = test; // Assign hello to an instance of test
    hello(); // Calls hello, which prints "hello"

    var cow = new Cow();
    var moo = cow.moo; //Assign the variable moo to the cows moo script.
    moo(); // Calls moo, which print "moo".

    var lambda = script { print("lambda"); } // Assign the variable lambda to an inline script.
    lambda(); // Calls lambda, which prints "lambda"

    lambda = script(output) { print(output); } // Assign the variable lambda to an inline script that takes an argument.
    lambda("oink"); // Calls lambda with an argument, which prints "oink"
}
```

Script instances can also capture variables from the environment around them.

```cs
script main() {
    var i = 0;
    var add = script() { print(i++); }
    add(); // prints 0
    add(); // prints 1
}
```