---
layout: tutorial
title: Including Libraries
---

Including libraries is extremely simple. You specify it's path via the [[build file|The Build File]], and it will be included in the library. If the file isn't found in the path specified by the build file, the compiler will look in one other location for the file. Wherever the compiler is installed, it will look in a folder called `Libraries` to see if it can find the file. This allows you to place all of the most commonly used assemblies in one location for ease of use. It essentially acts as a Global Assembly Cache for TaffyScript.