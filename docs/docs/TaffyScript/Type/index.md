---
layout: default
title: Type
---

# Type

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Type]({{site.baseurl}}/docs/TaffyScript/Type/)

_Represents an object type._

## Properties

<table>
  <col width="15%">
  <col width="15%">
  <thead>
    <tr>
      <th>Name</th>
      <th>Type</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/base_type/">base_type</a></td>
      <td>[Type]({{site.baseurl}}/docs/TaffyScript/Type)</td>
      <td>Gets the type this type inherits from.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/full_name/">full_name</a></td>
      <td>string</td>
      <td>Gets the fully qualified name of this type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_abstract/">is_abstract</a></td>
      <td>bool</td>
      <td>Determines if this type is abstract.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_ansi_class/">is_ansi_class</a></td>
      <td>bool</td>
      <td>Determines if the string format attribute AnsiClass is selected for this type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_array/">is_array</a></td>
      <td>bool</td>
      <td>Determines if this type is an array.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_auto_class/">is_auto_class</a></td>
      <td>bool</td>
      <td>Determines if the string format attribute AutoClass is selected for this type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_auto_layout/">is_auto_layout</a></td>
      <td>bool</td>
      <td>Determines if the fields for this type are laid out automatically by the CLR.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_by_ref/">is_by_ref</a></td>
      <td>bool</td>
      <td>Determines if this type is passed by reference.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_class/">is_class</a></td>
      <td>bool</td>
      <td>Determines if this type is a class or script.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_com_object/">is_com_object</a></td>
      <td>bool</td>
      <td>Determines if this type is COM object.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_constructed_generic_type/">is_constructed_generic_type</a></td>
      <td>bool</td>
      <td>Determines if this type is a constructed generic type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_contextful/">is_contextful</a></td>
      <td>bool</td>
      <td>Determines if this type can be hosted in a context.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_enum/">is_enum</a></td>
      <td>bool</td>
      <td>Determines if this type is an enum.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_explicit_layout/">is_explicit_layout</a></td>
      <td>bool</td>
      <td>Determines if the fields of this type are laid out at explicitly specified offsets.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_generic_parameter/">is_generic_parameter</a></td>
      <td>bool</td>
      <td>Determines if this type represents a type parameter in the definition of a generic type or method.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_generic_type/">is_generic_type</a></td>
      <td>bool</td>
      <td>Determines if this type is a generic type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_generic_type_definition/">is_generic_type_definition</a></td>
      <td>bool</td>
      <td>Determines if this type represents a generic type definition.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_import/">is_import</a></td>
      <td>bool</td>
      <td>Determines if this type has a ComImport attribute, indicating that it was imported from a COM type library.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_interface/">is_interface</a></td>
      <td>bool</td>
      <td>Determines if this type is an interface.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_layout_sequential/">is_layout_sequential</a></td>
      <td>bool</td>
      <td>Determines if the fields of this type are laid out sequentially in the order they were defined.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_marshal_by_ref/">is_marshal_by_ref</a></td>
      <td>bool</td>
      <td>Determines if this type is marshaled by reference.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_nested/">is_nested</a></td>
      <td>bool</td>
      <td>Determines if this type is defined inside of another type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_nested_assembly/">is_nested_assembly</a></td>
      <td>bool</td>
      <td>Determines if this type is nested and only visible within its own assembly.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_nested_family/">is_nested_family</a></td>
      <td>bool</td>
      <td>Determines if this type is nested and is only visible within its own family</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_nested_family_and_assembly/">is_nested_family_and_assembly</a></td>
      <td>bool</td>
      <td>Determines if this type is nested and is only visible to both its own family and its own assembly.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_nested_family_or_assembly/">is_nested_family_or_assembly</a></td>
      <td>bool</td>
      <td>Determines if this type is nested and is only visible to its own family or its own assembly.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_nested_private/">is_nested_private</a></td>
      <td>bool</td>
      <td>Determines if this type is nested and declared private.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_nested_public/">is_nested_public</a></td>
      <td>bool</td>
      <td>Determines if this type is nested and declared public.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_not_public/">is_not_public</a></td>
      <td>bool</td>
      <td>Determines if this type is not declared public.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_pointer/">is_pointer</a></td>
      <td>bool</td>
      <td>Determines if this type is a pointer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_primitive/">is_primitive</a></td>
      <td>bool</td>
      <td>Determines if this type is a primitive type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_public/">is_public</a></td>
      <td>bool</td>
      <td>Determines if this type is declared public.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_sealed/">is_sealed</a></td>
      <td>bool</td>
      <td>Determines if this type is declared sealed.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_security_critical/">is_security_critical</a></td>
      <td>bool</td>
      <td>Determines if this type is security-critical or security-safe-critical at the current trust level.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_security_safe_critical/">is_security_safe_critical</a></td>
      <td>bool</td>
      <td>Determines if this type is security-safe-critical at the current trust level.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_security_transparent/">is_security_transparent</a></td>
      <td>bool</td>
      <td>Determines if this type is transparent at the current trust level.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_serializable/">is_serializable</a></td>
      <td>bool</td>
      <td>Determines if this type is serializable.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_special_name/">is_special_name</a></td>
      <td>bool</td>
      <td>Determines if this type has a name that requires special handling.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_unicode_class/">is_unicode_class</a></td>
      <td>bool</td>
      <td>Determines if the string format UnicodeClass is selected for this type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_value_type/">is_value_type</a></td>
      <td>bool</td>
      <td>Determines if this type is a value type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_visible/">is_visible</a></td>
      <td>bool</td>
      <td>Determines if this type can be accessed by code outside the assembly.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/member_type/">member_type</a></td>
      <td>[MemberTypes](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.membertypes?view=netframework-4.7)</td>
      <td>Gets a value indicating that this member is a type or a nested type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/name/">name</a></td>
      <td>string</td>
      <td>Gets the name of this type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/namespace/">namespace</a></td>
      <td>string</td>
      <td>Gets the namespace of this type.</td>
    </tr>
  </tbody>
</table>

## Scripts

<table>
  <col width="20%">
  <thead>
    <tr>
      <th>Signature</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/get_array_rank">get_array_rank()</a></td>
      <td>If this type is an array, gets the number of array dimensions.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/get_enum_name">get_enum_name(value)</a></td>
      <td>If this type is an enum, gets the name of the constant that has the specified value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/get_enum_names">get_enum_names()</a></td>
      <td>If this type is an enum, gets the names of its members as an array.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/get_enum_underlying_type">get_enum_underlying_type()</a></td>
      <td>If this type is an enum, gets the underlying CLR type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/get_enum_values">get_enum_values()</a></td>
      <td>If this type is an enum, gets an array of the values of the constants defined by the enum.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_assignable_from">is_assignable_from(type)</a></td>
      <td>Determines if an instance of the specified type can be cast to an instance of this type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_enum_defined">is_enum_defined(value)</a></td>
      <td>If this type is an enum, determines if the specified value is defined by the enum.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_instance_of_type">is_instance_of_type(obj)</a></td>
      <td>Determines if the specified object is an instance of this type.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/is_subclass_of">is_subclass_of(type)</a></td>
      <td>Determines if this type derives from the specified class.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Type/to_string">to_string()</a></td>
      <td>Gets the name of this type.</td>
    </tr>
  </tbody>
</table>
