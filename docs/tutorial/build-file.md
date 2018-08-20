---
layout: tutorial
title: The Build File
---

The build file tells the compiler how to compile your program. It should always be located in the base directory of your project, with the name `build.cfg`. The build file doesn't specify which files to include, but it can specify some files to exclude. When starting a new project you can have the compiler auto-generate one using the flag `/build` that will have all of the default values. Here is an example build file:

```xml
<Build>
  <Output>bin\ProjectName</Output>
  <References>
    <Reference>path/to/assembly.dll</Reference>
  </References>
  <Excludes>
    <Exclude>path/to/file.tfs</Exclude>
  </Excludes>
  <Mode>Debug</Mode>
  <Target>x86</Target>
  <EntryPoint>Test.main</EntryPoint>
  <Product>Test Project</Product>
  <Version>0.1.0</Version>
  <Company>Fake Company Name</Company>
  <Copyright>Copyright © 2018</Copyright>
  <Trademark />
  <Description>Description of the product</Description>
</Build>
```

The only part of the file that is strictly necessary is the Output property. The rest can be excluded and their default value will be used.

## Output
This should be the name of the output assembly. This is the only required field. If you'd like the assembly to be output into a separate path, you can prefix the name with the path. The assembly name should _not_ end with .dll. Example: `bin\LibraryName` which will output the assembly in the `bin` folder with the name `LibraryName`.exe or .dll depending on whether or not the entry point was found.

## References
This should be a list of _Reference_ elements.

## Reference
This should be a path to an assembly to include with your project. This can be an absolute or relative path.

## Excludes
This should be a list of _Exclude_ elements.

## Exclude
This should be the path to a .tfs file you'd like to exclude from the compile process. This can be an absolute or relative path.

## Mode
This determines the compile mode. The supported values are Debug and Release. The output assembly won't be particularly different, but Release mode allows the JIT to perform optimizations, while the Debug mode allows you to hook a debugger up to the assembly and step through the code to some degree. This defaults to `Debug`.

## Target
This allows you to determine the target cpu architecture to compile for. The two options are `x86` for 32-bit and `x64` for 64 bit. This defaults to `x32`.

## EntryPoint
This should point to the name of the entry point script. If the script is in a namespace, make sure that is included. Example: `MyNamespace.EntryPoint`. This defaults to `main`.


# Assembly Info

These values can have a string value, but will always default to `""`. They get stored in the assembly, but have no effect on the compilation process.

* Product
* Version
* Company
* Copyright
* Trademark
* Description