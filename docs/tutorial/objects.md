---
layout: tutorial
title: Objects
---

Objects are user defined data types. They contain variables and define scripts. They work extremely similar to classes in other languages.

## Defining Objects

Objects are defined using the `object` keyword, followed by the object name.

```cs
object Person {
    // Object members go here...
}
```

## Creating Instances

An instance is an instantiated version of an object. In other words, it's a variable of an object. Instances are created using the `new` keyword.

```cs
var person = new Person();
```

Inside of the parentheses, you can pass any desired arguments to the types constructor.

## Object Scripts

An object script is a script that is bound to a type. They are defined inside of the body of an object.

To define an object script, start with the `script` keyword, followed by the script name. Optionally, there can be a list of arguments in between parentheses. The arguments can have default values by putting a `=` followed by a constant value. You cannot have an argument with a default value _before_ an argument without. The script can directly access the argument array when needed. They can return a value using the `return` keyword.

```cs
object Person {
    script say_hello {
        print("Hello");
    }

    script greet(name) {
        print("Hello, " + name);
    }
}
```

## Script Access

An object script can access any other script defined inside of its type as if they were global scripts.

```cs
object Cow {
    script get_noise() {
        return "moo";
    }

    script moo() {
        // Accessing a script defined inside of the object.
        var noise = get_noise();
        print(noise);
    }
}
```

Outside of the type, you can access the script through an instance using the member access operator `.`.

```cs
object Cow {
    script moo() {
        print("moo");
    }
}

script main() {
    // Create an instance of Cow.
    var cow = new Cow();

    // Call the get_noise script through the instance.
    cow.get_noise();
}
```

## Static Methods

An object script can be prefixed with the modifier `static`. This will bind the script to the object, but it will be available globally. In other words, static scripts don't need an instance of a type to be called.

```cs
object Math {
    static script add(left, right) {
        return left + right;
    }
}
```

A static script is called through the type that defines it like so:

```cs
object Math {
    static script add(left, right) {
        return left + right;
    }
}

script main() {
    var result = Math.add(10, 5);
    print(result);
}
```

An object can call a static script inside of it's body without prefixing the type name.

```cs
object Cow {
    static script get_noise() {
        return "moo";
    }

    script moo() {
        print(get_noise());
    }
}
```

## Instance Variables

If a variable is assigned inside of an object script that isn't a local variable (i.e. it wasn't created with `var`), it will be assigned to that instance. Inside of the type, these variables can be accessed just like any other variable.

```cs
object Person {
    script create() {
        // Assign an instance variable.
        name = "Alexander Hamilton"

        //Access the instance variable.
        print(name);
    }
}
```

You can get and set the variables outside of the type using the member access operator `.`.

```cs
object Person {

} 

script main() {
    var person = new Person();
    person.name = "Alexander Hamilton";
    print(person.name);
}
```

## Self

When inside of an object script, you can explicitly refer to the calling instance using the keyword `self`. This can be particularly useful when trying to assign an instance variable with the same name as a local variable or script argument.

```cs
object Person {
    script set_name(name) {
        self.name = name;
    }
}
```

# Special Scripts

There are a handful of special script names in TaffyScript. These are used to modify the behaviour of objects.

## create

If an object defines a script with the name `create`, it will be called when an instance is created. Any arguments used when creating a new instance will be passed to the create script. This can also be referred to as the types constructor.

```cs
object Person {
    script create(name) {
        self.name = name;
    }
}

script main() {
    // Creates a new person and calls Person.create
    var person = new Person("Alexander Hamilton");
    print(person.name);
}
```

It should be noted that the create script cannot return a value, and it cannot be called explicitly.

## get

If an object defines a script with the name `get`, the get script will be called when using the array access operator `[]` on the instance. The values inside of the operator will be passed as arguments to the get script. This is the reason you can access elements of a [List]({{site.baseurl}}/docs/List) or values in a [Map]({{site.baseurl}}/docs/Map).

```cs
object Echo {
    script get(value) {
        return value;
    }
}

script main() {
    var echo = new Echo();
    var result = echo["moo"];
    print(result);
}
```

## set

The `set` script works similarly to the `get` script. When using the array accessor `[]` on an instance, the set script will be called. The value on the right hand side of the equals sign will be passed as the last argument to the set script.

```cs
object Names {
    script create() {
        map = new Map();
    }

    script get(first_name) {
        return map[first_name];
    }

    script set(first_name, last_name) {
        map[first_name] = last_name;
    }
}

script main() {
    var names = new Names();
    names["Alexander"] = "Hamilton";
    print(names["Alexander"]);
}
```

## to_string

When converting an object to a string, the `to_string` script will be called.

```cs
object Point {
    script create(x, y) {
        self.x = x;
        self.y = y;
    }

    script to_string() {
        return "X: " + string(x) + ", Y: " + string(y);
    }
}

script main() {
    var point = new Point(32, 64);
    print(point);
}
```

# Inheritance

TaffyScript supports single object inheritance. When an object inherits from another, it gains all of its parents scripts, except for it's constructor, which will have to be called explicitly.

To inherit from a class, after writing the object name in an object definition, follow it with a colon `:` followed by the parent types name.

```cs
object Parent {

}

object Child : Parent {

}
```

A child has direct access to its parents scripts, including the static ones.

```cs
object Cow {
    static script get_noise() {
        return "moo";
    }

    script moo() {
        print(get_noise());
    }
}

object Calf : Cow {
    script create() {
        // Call the parent method.
        moo();

        // Call the parent static method.
        var noise = get_noise();
        print(noise);
    }
}
```

A child can override a parents script with their own implementation. This is done by defining a script with the same name inside of the childs definition. The child script can call the parent's using the `base` keyword. The childs script can have a different number of arguments from its parents.

```cs
object Animal {
    script make_noise(noise) {
        print(noise);
    }
}

object Cow : Animal {
    script make_noise() {
        base("moo");
    }
}
```