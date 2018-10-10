---
layout: default
title: XmlWriter.write_doc_type
---

# XmlWriter.write_doc_type

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Xml]({{site.baseurl}}/docs/TaffyScript/Xml/).[XmlWriter]({{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/).[write_doc_type]({{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_doc_type/)

_Writes the DOCTYPE declaration with the specified name and optional attributes._

```cs
XmlWriter.write_doc_type(name, pubid, sysid, subset)
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
      <td>The name of the DOCTYPE. Cannot be null or empty.</td>
    </tr>
    <tr>
      <td>pubid</td>
      <td>string</td>
      <td>If non-null, writes PUBLIC "pubid" "sysid" where pubid and sysid are replaced with the value of the given arguments.</td>
    </tr>
    <tr>
      <td>sysid</td>
      <td>string</td>
      <td>If pubid is null and this isn't, writes SYSTEM "sysid" where sysid is replaced with the value of this argument.</td>
    </tr>
    <tr>
      <td>subset</td>
      <td>string</td>
      <td>If non-null, writes [subset] where the subset is replaced witht the value of this argument.</td>
    </tr>
  </tbody>
</table>

**Returns:** null
