---
layout: default
title: XmlWriter.write_attributes
---

# XmlWriter.write_attributes

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Xml]({{site.baseurl}}/docs/TaffyScript/Xml/).[XmlWriter]({{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/).[write_attributes]({{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_attributes/)

_Writes out all of the attributes found at the current position in the specified XmlReader._

```cs
XmlWriter.write_attributes(reader, defattr)
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
      <td>reader</td>
      <td>[XmlReader]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReader)</td>
      <td>The reader fomr which to copt the attributes.</td>
    </tr>
    <tr>
      <td>defattr</td>
      <td>bool</td>
      <td>Determines whether the copy the default attributes from the XmlReader.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
