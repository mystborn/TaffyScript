---
layout: tutorial
title: Operators
---

TaffyScript supports a number of operators:

## Arithmetic

| Operator | Operation |
| --- | --- |
| + | Addition |
| - | Subtraction
| * | Multiplication |
| / | Division |
| % | Modulus |
| ++ | Increment |
| -- | Decrement |

## Comparison

| Operator | Operation |
| --- | --- |
| == | Equal |
| = | Equal (When used within an expression) |
| != | Not Equal |
| <> | Not Equal |
| < | Less Than |
| <= | Less Than or Equal |
| > | Greater Than |
| >= | Greater Than or Equal |

## Boolean
| Operator | Operation |
| --- | --- |
| and | And |
| or | Or |
| && | And |
| \|\| | Or |
| ! | Not |

## Bitwise
| Operator | Operation |
| --- | --- |
| ~ | Complement |
| & | Bitwise And |
| \| | Bitwise Or |
| ^ | Xor |
| << | Shift Left |
| >> | Shift Right |

*Note: Shifting right extends the sign. In other words, shifting a negative number right keeps the sign.*

## Assignment

Unlike many other languages, assignments in TaffyScript are statements, not expressions. In other words, they stand alone and produce no value. If the equal sign `=` is used in a place that meets these requirements, or in a comma separated list of local declarations, it assigns a value. Otherwise, it will do an equality check.

```cs
var i = 0; // Assigns 0 to the variable i

var array = [0, 1];
array[0] = 5; //Assigns 5 to the first index of the array.
array[0] = ["moo"]; // Assigns a new array to the first index of the array.
array[0][0] = "oink";

if(i = 0) { // Here the equals sign performs an equality check.
    // ... 
}

var check = i = array[1] // This assigns check to the result of an equality comparison between i and the second element of array.
```

However, you can assign multiple local variables on the same line using a comma in between each assignment.

```cs
var i = 10, j = 20, k = 30;
```