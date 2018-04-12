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
