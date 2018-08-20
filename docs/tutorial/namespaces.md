---
layout: tutorial
title: Namespaces
---

A namespace is a convenient way to separate code into named sections, particularly to avoid naming conflicts. The concept is almost the exact same as the one used in c# and other similar languages.

## Defining a Namespace

To define a namespace, write `namespace` followed by the name of the namespace. Then, inside of a block, you can declare objects, scripts, and other namespaces that will be defined inside of the parent namespace. For example:

```cs
namespace Example {
    script namepsace_script {
        print("Called from a namespace");
    }

    object NamespaceObject {
        script create {
            print("Created from a namespace");
        }
    }

    //You can also have nested namespaces
    namespace Inner {
        //To access an element from here, it's path would be Example.Inner.element_name
    }
}
```

## Accessing Namespace Members

### Direct Access
To access a namespace member directly, you can write the namespace name, a dot, then the member name like so: `Example.NamespaceObject`. 

### Using
To access all of the members of a namespace, you can add a using statement to the top of your file. Note that it MUST be the first thing in a file. For example:

```cs
using Example;

script global_script {
    namespace_script();
}
```

vs

```cs
script global_script {
    Example.namespace_script();
}
```