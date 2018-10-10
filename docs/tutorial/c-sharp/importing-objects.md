---
layout: tutorial
title: Importing Objects
---

In addition to individual methods, you can import an entire type. Once imported, the type can be used like any other TaffyScript type. Imports take the following form:

```cs
import object[(<ImportArguments>)] <TypeName> [as <ImportName>] [{ <Members> }]
```

| Key | Description |
| --- | --- |
| ImportArguments | A set of optional arguments used by the compiler during the import. |
| TypeName | The name of the c# class to import.
| ImportName | An optional name used to refer to the imported type in TaffyScript. |
| Members | A list of members to import. |

If the Members part is included, it is called an explicit import, because the members to import are explicitly laid out. If that is excluded, it's called an implicit import because the members are determined by the compiler and the ImportArguments.

## Import Arguments

The import arguments are a set of comma separated values that take the following form:

```cs
<ArgumentName> = <ArgumentValue>
```

The following arguments are supported: casing, typing, and include_std.

#### casing

The casing option determines how the C# method names get translated to the TaffyScript representation. It is only used with implicit imports. It supports the following values:

| Option | Description |
| --- | --- |
| pascal_case | Converts all of the member names to PascalCase |
| snake_case | Converts all of the member names to snake_case (sometimes referred to as underscore_case) |
| camel_case | Converts all of the member names to camelCase |
| native_case (Default) | Keeps the casing the same as it was defined in C# |

#### typing

This determines if the imported types can still have members added to it at runtime. So if we import a type that does **not** have a name member, this option determines whether the following code works or throws an error:

```cs
var obj = new ImportedObject();
obj.name = "Alexander Hamilton";
```

| Option | Description |
| --- | --- |
| weak (default) | Values can be added at runtime. |
| strong | Values cannot be added at runtime. |

#### include_std

This option determines whether or not the methods inherited from `object` will be imported (i.e. ToString, GetHashCode, Equals).

| Option | Description |
| --- | --- |
| true | Imports the standard methods. |
| false | Ignores the standard methods. |

Object imports take two forms: explicit and implicit.

## Members

If the object import has a body, it determines which members to import. It can import any field, property, constructor, and method that conforms to the [interop types]({{site.baseurl}}/tutorial/c-sharp/interop-types). In between each member, you can place any number of optional semicolons in order to make the code look cleaner if desired.

### Fields and Properties
Both fields and properties are imported in the same way. They take the following form:

```cs
<MemberName> [as <ImportName>]
```

Where ImportName is the optional name used to refer to that member in TaffyScript.

### Constructor
You can only import one constructor override, but you must choose one. It takes the following form:

```cs
new([<ParameterTypes>])
```

### Methods
These look pretty similar to the [global script imports]({{site.baseurl}}/tutorial/c-sharp/importing-methods).

```cs
<MethodName>([<ParameterTypes>]) [as <ImportName>]
```

Where ParameterTypes are the types used to call the method and the optional import name is how the TaffyScript script will be referred to.

## Importing List<TsObject>

The following two section showcase how this construct could be used to implement the [List]({{site.baseurl}}/docs/List) type in the Base Class Library.

### Explicit Import

```cs
import object(typing=strong) List<TsObject> as List {

    // Import fields and properties
    Count as count;

    // Import the constructor
    new();

    // Import the desired methods
    Add(object) as add;
    Clear() as clear;
    Insert(int, object) as insert;
    RemoveAt(int) as delete;

    // Most indexers get implemented with the following method names
    // The only exception I've personally come across is the
    // string indexer, which is called get_Chars for some reason.
    // If your import fails for some reason, check if that's the reason.
    get_Item(int) as get;
    set_Item(int, object) as set;
}
```

### Implicit Import
```cs
import object(typing=strong, case=snake_case, include_std=false) List<TsObject> as List
```

When importing a normal type, the compiler generates a wrapper class that is used by TaffyScript. However, there is a way to avoid this. If the imported class inherits from ITsInstance and has a constructor that looks like this:

```cs
TypeName(TsObject[] args)
{

}
```

then the type will be used directly. You can still rename it using the `as` keyword when importing, but the other options should be avoided.

```cs
import object <TypeName> [as <ImportName>]
```