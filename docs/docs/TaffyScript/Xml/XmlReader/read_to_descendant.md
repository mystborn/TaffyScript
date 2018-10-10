---
layout: default
title: XmlReader.read_to_descendant
---

# XmlReader.read_to_descendant

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Xml]({{site.baseurl}}/docs/TaffyScript/Xml/).[XmlReader]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/).[read_to_descendant]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_to_descendant/)

_Advances the reader to the next matching descendant element._

```cs
XmlReader.read_to_descendant(name, [ns])
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
      <td>name</td>
      <td>string</td>
      <td>The name of the element to move to.</td>
    </tr>
    <tr>
      <td>[ns]</td>
      <td>string</td>
      <td>The namespace URI of the element to move to.</td>
    </tr>
  </tbody>
</table>

**Returns:** bool
