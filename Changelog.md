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
