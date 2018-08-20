---
layout: tutorial
title: Hello, World
---

As with most programming language tutorials, we'll start with an extremely simple program before moving on to the meat of the language. 

```cs
script main() {
    print("Hello, TaffyScript!");
}

// Outputs: Hello, TaffyScript!
```

This is a basic Hello, World program. In order to compile it, you'll need to create a folder with a [build file]({{site.baseurl}}/tutorial/the-build-file) in it. For now, just copy this into a file and save it as build.cfg:
```xml
<Build>
  <Output>bin\HelloWorld</Output>
  <Mode>Debug</Mode>
</Build>
```

Then create a file with the above code in it. Save the file with the extension `.tfs`. Then from the command line you can compile it like so:

```sh
path/to/tsc.exe path/to/project/folder /r
```

You can omit the project folder if you're inside of that directory in the command line. The `/r` will tell the compiler to run the output assembly.