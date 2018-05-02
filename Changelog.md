# Release 1.7.0
### Temporarily Removed `with`; Permanently Changed it's Functionality
TaffyScript instances are no longer bound to an id. Therefore, the way with originally worked is no longer valid. Removing id's from instances will make them work much faster (depending on the operations, sometimes up to a 300% increase), and allow them to be garbage collected. This means you no longer have to call `.destroy()` on every instance. It also makes the next major change much easier to implement.

### Import C# Objects
You can now import a c# object to be usable from within TaffyScript. When imported, a special wrapper class is created that allows the object to be used as any other TS object. There are two import modes: explicit and automatic. In the eplicit mode you layout what methods, fields, properties, and constructor to import. In the automatic mode, all methods/properties/fields that have valid TS types are imported, as well as the first valid constructor. Until the wiki page is created you can check out [this](https://github.com/mystborn/TaffyScript/blob/dev-experimental/Samples/TestSuite/Tests/object_import.tfs) page which shows some example imports.

### Object Indexers
_This functionality is still under consideration and is highly subject to change._

You can now define indexers on objects. This functionality was created to support accessors on c# objects. To define an accessor, simply define a combination of `get` and `set` events on an object. Then you can call those methods using array access syntax. The values inside the brackets will be passed to the respective method.

### Deprecated ds_* Scripts
These scripts were removed in favor of imported c# objects. The new c# objects are ds_list, ds_map, and ds_grid. More information will be added to the wiki soon.

### Full Changelog
* Made Binders more efficient
* Fixed excluding files
* Made all `ISyntaxElement`s have a `Text` property. Means less casting and more clear code during compilation phase.
* Better errors when using an addition operator.
* Added an `ITsInstance` interface that all instances must derive from
* Made `TsInstance` inherit from `ITsInstance`.
* Changed TsScript first argument to be `ITsInstance`.
* Changed `TsDelegate` `Target` to be of type `ITsInstance`. 
* All WeakMethod first arguments have been changed to use `ITsInstance` instead of `TsInstance`.
* Removed `InstanceEnumerator` as it no longer functioned.
* Temporarily removed variable_instance_* scripts.
* Made `TsObject` wrap `ITsInstance`.
* Added some timer related methods to the UnitTest library.
* Added MemberAccessException.
* Implemented types for ds_list, ds_map, and ds_grid.
* Added the ability to import c# objects.
* Temporarily removed `with` block.
* Added the ability to natively import any type that inherits from `ITsInstance` in a fashion similar to importing `WeakMethod`s.
* Allow objects to have indexer syntax with get and set scripts.
* Fixed bug when resizing a 2d array.
* Fully deprecated ds_* scripts
* Removed indexer accessor tokens

# Release 1.6.1.0
_General fixes and updates related to the script rework. Many bug fixes and performance enhancements._

* Where possible, BCL now imports directly from the Math class.
* Updated BCL implementation to conform to script rework.
   * Updated BCL generator import signatures
* Updated Moddable sample to use new language feautres.
* Continuing to add more comprehensive test coverage.
* Updated Threading Sample to conform to script rework.
* Added warning when importing an invalid WeakMethod.
* Improved default arguments to include readonly constants.
* Improved (Class/Struct)Binders.
* Ds* classes now use ClassBinder
* Fixed bug when calling imported method before actually importing it.
* Fixed bug with default args with certain constant values.
* Changed Token.Type from a string to an enum to improve performance and enforce static types.
* Improved Tokenizer creation speed/memory footprint.
* Added another project to the TestSuite that allows methods to run in a try block.
* Created a Grid<T> class.
* Better performance when getting/setting array values.
* Implicitly cast TsInstance and TsDelegate to TsObject, explicit vice versa.
* Better errors when an invalid addition/subtraction operation is detected.
* Better performance with data structures when reading and writing in a single operation (++, --, +=, etc).
* Compiler no longer breaks when trying to call an import method it couldn't find. Still adds an error to the output.
* Fixed bug when dividing/modulo two constants.
* Fixed bug with ternary operator when evaluating bools
* Fixed bug with argument_count when there are zero args.
* Compiler now emits better sequence points.

# Release 1.6.0.0
This release is the largest to date, after the initial of course.
### Script Rework
The internal workings of scripts has been completely reworked. This new implementation will allow many improvements to be made going forward, as well as some already in place. Most notably, object events can take arguments and have a return value. This change opens the door to many missing OOP features. It also allows for first class functions to a degree. You can assign functions to a variable and then call the variable.

### Full Notes
* Many code generation fixes.
* Sample code improvements
    * Better test coverage (still small, but growing)
    * Better threading code
    * Changed unit test lib to use new features
* Updated BCL to conform to new standards
* ds_map now works properly.
* Renamed TsValue and TsArrayValue to TsImmutableValue and TsMutableValue respectively to better match their purpose.
* Removed InstanceEvents and Scripts properties from TsInstance and replaced them with the following: InstanceScripts and GlobalScripts.
* Removed Id stack from TsObject
* Changed Script delegate to TsScript.
* Changed TsScript method sig to TsObject TsScript(TsInstance, TsObject[])
    * Whenever a script is called, the executing object passes itself as the first argument.
* TsDelegate class that wraps a TsScript for easier/safer invocation. Access Script property to call directly.
* TsInstance now has a Destroyed event that will be triggered on destruction.
* Added methods to convert TsObject to and from TsDelegate.
* A bunch of internal changes to fix bugs/speed up compile times
* New native type: delegate
    * Scripts can be assigned to variables. These vars will have the delegate type.
    * Delegate variables can be invoked like normal scripts.
    * You can return scripts.
    * You can assign both global and instance scripts to a variable
        * If a variable is assigned an instance script, that instance will always be passed in as the target of the script.
        * Invoking the script after the instance has been destroyed will throw an exception.
* Object events can now take arguments
* Object events can now have a return value.

# Release 1.5.1.1
_This release does not come with a new prebuilt compiler. You must build from source to use the new changes._
* Made some QoL changes to the config file generation.
* Fixed a few errors when parsing strings with special characters.
* Fixed a few namespace related errors.
* Some small codegen optimizations.
* Grossly improved the threading library in the Samples folder.

# Release 1.5.1.0
* Added Threading example
* Fixed bug when importing methods with the WeakMethod attibute.
* Fixed namespace bug

# Release 1.5.0.0
* Directly call events from instance: inst.event_name()
* New keyword to create instances: inst = new object_name()
* Public constructor for TsInstance
* Function call optimization for 0 arguments
* Fixed not equal operator when used with a variable and constant
* Destroy instance using destroy event: inst.destroy()
* Include script arguments in script declaration
* Optional script arguments
* Updated certain function in the BaseClassLibrary to use stronger types
* Added quality of life instance methods to TsInstance
* Added the ability to import methods with any numeric type other than decimals in its signature.
* Changed the name of some methods in TsObject to provide a more consistent naming scheme
    * GetNum -> GetFloat
    * GetNumUnchecked -> GetFloatUnchecked
    * GetNumAsInt -> GetInt
