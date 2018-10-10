---
layout: default
title: XmlWriter
---

# XmlWriter

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Xml]({{site.baseurl}}/docs/TaffyScript/Xml/).[XmlWriter]({{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/)

_Represents a writer that provides a fast, non-cached, forward-only way to generate streams or files that contain XML data._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/settings/">settings</a></td>
      <td>[XmlWriterSettings]({{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings)</td>
      <td>Gets the settings used to create this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_state/">write_state</a></td>
      <td>[WriteState](https://docs.microsoft.com/en-us/dotnet/api/system.xml.writestate?view=netframework-4.7)</td>
      <td>Gets the state of this writer.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/xml_lang/">xml_lang</a></td>
      <td>string</td>
      <td>Gets the current xmk:lang scope.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/xml_space/">xml_space</a></td>
      <td>[XmlSpace](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlspace?view=netframework-4.7)</td>
      <td>Gets the current xml:space scope.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/dispose">dispose()</a></td>
      <td>Releases all dynamic resources used by this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/flush">flush()</a></td>
      <td>Flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/lookup_prefix">lookup_prefix(ns)</a></td>
      <td>Returns the closest prefix defined in the current namespace for the namespace URI.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_attributes">write_attributes(reader, defattr)</a></td>
      <td>Writes out all of the attributes found at the current position in the specified XmlReader.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_attribute_string">write_attribute_string(name, value, [ns], [prefix])</a></td>
      <td>Writes an attribute witht the specified value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_cdata">write_cdata(text)</a></td>
      <td>Writes out a CDATA block containing the specified text.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_comment">write_comment(text)</a></td>
      <td>Writes out a comment containing the specified text.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_doc_type">write_doc_type(name, pubid, sysid, subset)</a></td>
      <td>Writes the DOCTYPE declaration with the specified name and optional attributes.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_element_string">write_element_string(name, value, [ns], [prefix])</a></td>
      <td>Writes an element containing a string value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_end_attribute">write_end_attribute()</a></td>
      <td>Closes the previous write_start_attribute call.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_end_document">write_end_document()</a></td>
      <td>Closes any open elements and attributes and puts the writer back in the Start state.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_end_element">write_end_element()</a></td>
      <td>Closes one element andpops the corresponding namespace scope.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_entity_ref">write_entity_ref(name)</a></td>
      <td>Writes out an entity reference.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_full_end_element">write_full_end_element()</a></td>
      <td>Closes one element and pops the corresponding namespace scope.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_name">write_name(name)</a></td>
      <td>Writes out the specified name, ensuring is is valid.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_nm_token">write_nm_token(name_token)</a></td>
      <td>Writes out the specified name token, ensuring it is valid.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_processing_instruction">write_processing_instruction(name, text)</a></td>
      <td>Writes out a processing instruction.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_qualified_name">write_qualified_name(name, ns)</a></td>
      <td>Writes out the namespace qualified name.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_raw">write_raw(data)</a></td>
      <td>Writes raw markup manually.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_start_attribute">write_start_attribute(name, [ns], [prefix])</a></td>
      <td>Writes the start of an attribute.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_start_document">write_start_document([standalone])</a></td>
      <td>Writes the XML declaration.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_start_element">write_start_element(name, [ns], [prefix])</a></td>
      <td>Writes the specified start tag.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_string">write_string(text)</a></td>
      <td>Writes the given text content.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_value_bool">write_value_bool(value)</a></td>
      <td>Writes a bool value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_value_date_time">write_value_date_time(value)</a></td>
      <td>Writes a DateTime value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_value_decimal">write_value_decimal(value)</a></td>
      <td>Writes a decimal value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_value_double">write_value_double(value)</a></td>
      <td>Writes a double value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_value_float">write_value_float(value)</a></td>
      <td>Writes a float value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_value_int">write_value_int(value)</a></td>
      <td>Writes an int value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_value_long">write_value_long(value)</a></td>
      <td>Writes a long value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_value_string">write_value_string(value)</a></td>
      <td>Writes a string value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriter/write_whitespace">write_whitespace(ws)</a></td>
      <td>Writes the given whitespace.</td>
    </tr>
  </tbody>
</table>
