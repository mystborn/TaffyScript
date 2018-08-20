---
layout: default
title: call_instance_script
---

# call_instance_script

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Reflection]({{site.baseurl}}/docs/TaffyScript/Reflection/).[call_instance_script]({{site.baseurl}}/docs/TaffyScript/Reflection/call_instance_script/)

Calls the specified script declared by the target with the given arguments.

```cs
call_instance_script(target, script_name, ..args)
```

## Arguments

<table>
  <col width="15%">
  <col width="15%">
  <thead>
    <tr>
      <th>Argument</th>
      <th>Type</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>target</td>
      <td>instance</td>
      <td>The target of the script. The function will act as if this instance called it, even if this instance doesn't define the script.</td>
    </tr>
    <tr>
      <td>script_name</td>
      <td>string</td>
      <td>The name of the instance script to call.</td>
    </tr>
    <tr>
      <td>..args</td>
      <td>objects</td>
      <td>The arguments to pass to the script.</td>
    </tr>
  </tbody>
</table>

**Returns:** object
