---
layout: default
title: XmlWriterSettings
---

# XmlWriterSettings

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Xml]({{site.baseurl}}/docs/TaffyScript/Xml/).[XmlWriterSettings]({{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/)

_Specifies a set of features to support on the XmlWriter created with these settings._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/async/">async</a></td>
      <td>bool</td>
      <td>Determines if asynchronous methods can be used on a particular SmlWriter instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/check_characters/">check_characters</a></td>
      <td>bool</td>
      <td>Determines if the XmlWriter should ensure that all characters conform to the XML specification.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/close_output/">close_output</a></td>
      <td>bool</td>
      <td>Determines if the XmlWriter should also close the underlying device when it's closed.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/conformance_level/">conformance_level</a></td>
      <td></td>
      <td>Determines the level of conformance the XmlWriter checks the output for.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/do_not_excape_uri_attributes/">do_not_excape_uri_attributes</a></td>
      <td>bool</td>
      <td>Determines whether the XmlWriter does not escape URI attributes.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/encoding/">encoding</a></td>
      <td>string</td>
      <td>Determines the type of text encoding to use.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/indent/">indent</a></td>
      <td>bool</td>
      <td>Determines whether to indent elements.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/indent_chars/">indent_chars</a></td>
      <td>string</td>
      <td>Determines the string to be used when indenting.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/namespace_handling/">namespace_handling</a></td>
      <td></td>
      <td>Determines if the XmlWriter should remove duplicate namespace declarations when writing Xml content.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/new_line_chars/">new_line_chars</a></td>
      <td>string</td>
      <td>Determines the string to be used for line breaks.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/new_line_handling/">new_line_handling</a></td>
      <td></td>
      <td>Determines whether to normalize line breaks in the output.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/new_line_on_attributes/">new_line_on_attributes</a></td>
      <td>bool</td>
      <td>Determines whether to write attributes on a new line.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/omit_xml_declaration/">omit_xml_declaration</a></td>
      <td>bool</td>
      <td>Determines whether to omit an XML declaration.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/output_method/">output_method</a></td>
      <td></td>
      <td>Gets the method used to serialize XmlWriter output.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/write_end_document_on_close/">write_end_document_on_close</a></td>
      <td>bool</td>
      <td>Determines whether the XmlWriter will add closing tags to all unclosed elements when closed.</td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/clone">clone()</a></td>
      <td>Creates a copy of this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings/reset">reset()</a></td>
      <td>Resets the settings to their default values.</td>
    </tr>
  </tbody>
</table>
