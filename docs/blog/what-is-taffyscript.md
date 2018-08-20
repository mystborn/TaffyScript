---
layout: blog-post
title: What is TaffyScript
date: 7/19/2018
tags: TaffyScript
---

# What is TaffyScript, and Why?

I get these two questions all the time, and with good reason. Why would anybody use some random dudes programming language over the plethora of other choices?

Well, TaffyScript is a general purpose\*, object oriented, dynamic, strongly typed programming language. That means that it's [duck typed](https://en.wikipedia.org/wiki/Duck_typing), but you can't add a string and a number together. It is an unofficial [.NET](https://www.microsoft.com/net/learn/languages) language. So despite having a relatively small Base Class Library, it has access to the rest of the .NET ecosystem to close the gaps. Additionally, is can easily be added to existing or new C# applications. 

TaffyScript was designed to be easy to pick up but powerful enough for advances users. With the ability to import .NET classes and methods from external libraries, the possiblities are virtually endless. Other typical high level features such as single object inheritance, lambdas, reflection, etc. are also supported.

TaffyScript does not use the [DLR](https://en.wikipedia.org/wiki/Dynamic_Language_Runtime), and I get asked why all the time. I'm not really sure why. I decided not to use the DLR because in early tests I found the current object model to have a faster runtime.

\* Even though TaffyScript can be used as a general purpose language, it is designed to be embedded into larger .NET projects. In particular, for implementing game logic, and for the easy addition of mod support. 

---

TaffyScript was originally a project to learn about programming a compiler. I implemented it in C# because I've had a lot of experience with the System.Reflection.Emit assembly, so it seemed like the perfect choice.

Unfortunately, I'm not a very creative person. I really didn't know what the programming language should look like. So I decided to try and port an existing one to the .NET framework. I've dabbled in many programming languages (c#, c++, c, python, javascript, nim, java, haxe, GML, etc), but I was only experienced in two of them. I certainly wasn't about to try and create a new C# compiler. So that left me with one real option: [GML](https://docs.yoyogames.com/source/dadiospice/002_reference/001_gml%20language%20overview/).

I actually created a pretty darn good clone of the language with minor changes in the initial release. Too good in many instances. Objects had to be created using a script (what a method is called in TaffyScript), and they had to be manually destroyed, lest you risk a memory leak. Instance scripts couldn't have return values. You couldn't chain array accessors. These are just a few of the quirks that my language shares with GML.

Even though TaffyScript was essentially an experience, I had grown attached to the project. It had been a blast to make, and I had learned a lot along the way. I decided to continue working on it. So I started to remove strange GML features and replace them with more modern constructs. At some point, I had to decide how close I wanted to stick to GML. Basically, I needed to decide if I wanted instances to be garbage collected like everything else, or if they had to be destroyed manually. If I kept the old version where they had to be destroyed, it allowed a cool/unique, but abusable feature that I've only seen in GML. You could use a `with` statement to perform a block of code on each instance of a specific type. After getting some [advice from reddit](https://www.reddit.com/r/ProgrammingLanguages/comments/8e8vc7/should_instances_be_garbage_collected/), I decided to remove that feature.

After that, I started making drastic changes and improvements to the language. It barely resembles it's parent now. Hopefully it continues to grow into its own as time goes on.