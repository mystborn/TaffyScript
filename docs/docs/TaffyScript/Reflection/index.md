---
layout: default
title: Reflection
---

# TaffyScript.Reflection

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Reflection]({{site.baseurl}}/docs/TaffyScript/Reflection/)

Provides scripts to dynamically retrieve information about loaded assemblies.

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
      <td><a href="{{page.url}}call_global_script">call_global_script(name, ..args)</a></td>
      <td>Calls the specified global script with the given arguments.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}call_instance_script">call_instance_script(target, script_name, ..args)</a></td>
      <td>Calls the specified script declared by the target with the given arguments.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}instance_create">instance_create(type, ..args)</a></td>
      <td>Creates a new instance of a specified type.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}instance_get_name">instance_get_name(inst)</a></td>
      <td>Gets the type of the specified instance.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}instance_get_parent">instance_get_parent(inst)</a></td>
      <td>Gets the parent type of an instance. Returns an empty string if the instance has no parent.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}is_array">is_array(value)</a></td>
      <td>Determines if a value is an array.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}is_null">is_null(value)</a></td>
      <td>Determines if a value is a script.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}is_number">is_number(value)</a></td>
      <td>Determines if a value is null.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}is_script">is_script(value)</a></td>
      <td>Determines if a value is a script.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}is_string">is_string(value)</a></td>
      <td>Determines if a value is a string.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}object_is_ancestor">object_is_ancestor(parent, type)</a></td>
      <td>Determines if a type is a parent of another.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}script_exists">script_exists(name)</a></td>
      <td>Determines if a global script with the given name exists.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}variable_global_exists">variable_global_exists(name)</a></td>
      <td>Determines if a global variable with the given name exists.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}variable_global_get">variable_global_get(name)</a></td>
      <td>Gets the global variable with the given name.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}variable_global_get_names">variable_global_get_names()</a></td>
      <td>Gets an array of all of the currently defined global variable names.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}variable_global_set">variable_global_set(name, value)</a></td>
      <td>Sets a global variable with a specified name to the given value.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}variable_instance_get">variable_instance_get(inst, name)</a></td>
      <td>Gets the variable with the given name from the specified instance.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}variable_instance_set">variable_instance_set(inst, name, value)</a></td>
      <td>Sets the variable with the given name on the specified instance to the given value.</td>
    </tr>
  </tbody>
</table>
