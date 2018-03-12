Changes from the master branch:
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
