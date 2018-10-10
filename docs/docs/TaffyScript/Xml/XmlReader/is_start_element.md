---
layout: default
title: XmlReader.is_start_element
---

# XmlReader.is_start_element

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Xml]({{site.baseurl}}/docs/TaffyScript/Xml/).[XmlReader]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/).[is_start_element]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/is_start_element/)

_Tests if the current node is a start tag, optionally testing if the name matches the specified string._

```cs
XmlReader.is_start_element([name], [ns])
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
      <td>[name]</td>
      <td>string</td>
      <td>The string to match against the LocalName property of the element.</td>
    </tr>
    <tr>
      <td>[ns]</td>
      <td>string</td>
      <td>The string to match against the NamespaceURI property of the element.</td>
    </tr>
  </tbody>
</table>

**Returns:** bool
