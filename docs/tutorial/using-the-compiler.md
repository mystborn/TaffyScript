---
layout: tutorial
title: Using the Compiler
---

Compiling a project is extremely easy. In a console, use this command:
```sh
path/to/tsc.exe [path/to/project/dir] [options]
```

Where the project dir is a TaffyScript project directory with a [build.cfg]({{site.baseurl}}/tutorial/build-file/) file in it.

This will search all directories and files in the project directory all for files that end in `.tfs`, which will all be compiled into one assembly. If the project path is omitted, it will look for the project file in the current directory.

## Commands

You can trigger a command by prefixing it with `/` or `--`. Currently there are only a few commands:

| Command | Action |
| r | Runs the output assembly if it's an exe. |
| build | Creates a default build.cfg file. |
| t | Times how long it takes to compile the project. |