---
layout: default
title: XmlReader
---

# XmlReader

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Xml]({{site.baseurl}}/docs/TaffyScript/Xml/).[XmlReader]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/)

_Represents a reader that provides fast, noncached, forward-only access to XML data._

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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/attribute_count/">attribute_count</a></td>
      <td>number</td>
      <td>Gets number of attributes on the current node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/base_uri/">base_uri</a></td>
      <td>string</td>
      <td>Gets the base URI of the current node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/can_read_binary_content/">can_read_binary_content</a></td>
      <td>bool</td>
      <td>Determines if this instance implements the binary content read methods.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/can_read_value_chunk/">can_read_value_chunk</a></td>
      <td>bool</td>
      <td>Determines if this instance can read a chunk of values.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/can_resolve_entity/">can_resolve_entity</a></td>
      <td>bool</td>
      <td>Determines if this instance can parse and resolve entities.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/depth/">depth</a></td>
      <td>number</td>
      <td>Gets the depth of the current node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/eof/">eof</a></td>
      <td>bool</td>
      <td>Determines if the reader is positioned at the end of the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/has_attributes/">has_attributes</a></td>
      <td>bool</td>
      <td>Determines if the current node has any attributes.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/has_value/">has_value</a></td>
      <td>bool</td>
      <td>Determines if the current node can have a value.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/is_default/">is_default</a></td>
      <td>bool</td>
      <td>Determines if the current node is an attribute that was generated from the default value defined in the DTD or schema.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/is_empty_element/">is_empty_element</a></td>
      <td>bool</td>
      <td>Determines if the current node is an empty element.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/local_name/">local_name</a></td>
      <td>string</td>
      <td>Gets the local name of the current node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/name/">name</a></td>
      <td>string</td>
      <td>Gets the qualified name of the current node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/namespace_uri/">namespace_uri</a></td>
      <td>string</td>
      <td>Gets the namespace URI of the current node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/node_type/">node_type</a></td>
      <td>[XmlNodeType](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlnodetype?view=netframework-4.7)</td>
      <td>Gets the type of the current node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/prefix/">prefix</a></td>
      <td>string</td>
      <td>Gets the namespace prefix associated with the current node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/quote_char/">quote_char</a></td>
      <td>string</td>
      <td>Gets the quotation mark character used to enclose the value of an attribute node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_state/">read_state</a></td>
      <td>[ReadState](https://docs.microsoft.com/en-us/dotnet/api/system.xml.readstate?view=netframework-4.7)</td>
      <td>Gets the state of the reader.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/settings/">settings</a></td>
      <td>[XmlReaderSettings]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReaderSettings)</td>
      <td>Gets the XmlReaderSettings used to create this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/value/">value</a></td>
      <td>string</td>
      <td>Gets the text value of the current node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/value_type/">value_type</a></td>
      <td>string</td>
      <td>Gets the CLR type of the current node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/xml_lang/">xml_lang</a></td>
      <td>string</td>
      <td>Gets the current xml:lang scope.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/xml_space/">xml_space</a></td>
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
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/dispose">dispose()</a></td>
      <td>Releases all dynamic resources used by this instance.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/get">get(name_or_index, [ns])</a></td>
      <td>Gets the value of an attibute.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/get_attribute">get_attribute(name_or_index, [ns])</a></td>
      <td>Gets the value of an attibute.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/is_name">is_name(str)</a></td>
      <td>Determines if the specified string is a valid XML name.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/is_name_token">is_name_token(str)</a></td>
      <td>Determines if the specified string is a valid XML name token.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/is_start_element">is_start_element([name], [ns])</a></td>
      <td>Tests if the current node is a start tag, optionally testing if the name matches the specified string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/lookup_namespace">lookup_namespace(prefix)</a></td>
      <td>Resolves a namespace prefix in the current scope.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/move_to_attribute">move_to_attribute(name_or_index, [ns])</a></td>
      <td>Moves to the specified attribute.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/move_to_content">move_to_content()</a></td>
      <td>If the current element is not a content node, skips ahead to the next conent node or end of file.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/move_to_element">move_to_element()</a></td>
      <td>Moves to the element that contains the current attribute node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/move_to_first_attribute">move_to_first_attribute()</a></td>
      <td>Moves to the first attribute.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/move_to_next_attribute">move_to_next_attribute()</a></td>
      <td>Moves to the next attribute.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read">read()</a></td>
      <td>Reads the next node from the stream.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_attribute_value">read_attribute_value()</a></td>
      <td>Parses the attribute value into a node.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_content_as_bool">read_content_as_bool()</a></td>
      <td>Reads the text at the current position as a bool.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_content_as_date_time">read_content_as_date_time()</a></td>
      <td>Reads the text at the current position as a DateTime.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_content_as_double">read_content_as_double()</a></td>
      <td>Reads the text at the current position as a double.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_content_as_float">read_content_as_float()</a></td>
      <td>Reads the text at the current position as a float.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_content_as_int">read_content_as_int()</a></td>
      <td>Reads the text at the current position as an int.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_content_as_long">read_content_as_long()</a></td>
      <td>Reads the text at the current position as a long.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_content_as_string">read_content_as_string()</a></td>
      <td>Reads the text at the current position as a string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_element_content_as_bool">read_element_content_as_bool()</a></td>
      <td>Reads the current element as a bool.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_element_content_as_date_time">read_element_content_as_date_time()</a></td>
      <td>Reads the current element as a DateTime.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_element_content_as_double">read_element_content_as_double()</a></td>
      <td>Reads the current element as a double.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_element_content_as_float">read_element_content_as_float()</a></td>
      <td>Reads the current element as a float.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_element_content_as_int">read_element_content_as_int()</a></td>
      <td>Reads the current element as an int.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_element_content_as_long">read_element_content_as_long()</a></td>
      <td>Reads the current element as a long.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_element_content_as_string">read_element_content_as_string()</a></td>
      <td>Reads the current element as a string.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_end_element">read_end_element()</a></td>
      <td>Reads an end tag.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_inner_xml">read_inner_xml()</a></td>
      <td>Reads all of the content, including markup, representing this nodes children.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_outer_xml">read_outer_xml()</a></td>
      <td>Reads all of the content, including markup, representing this node and its children.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_start_element">read_start_element()</a></td>
      <td>Reads an element.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_subtree">read_subtree()</a></td>
      <td>Returns a new XmlReader that can be used to read the current node and its descendants.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_to_descendant">read_to_descendant(name, [ns])</a></td>
      <td>Advances the reader to the next matching descendant element.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_to_following">read_to_following(name, [ns])</a></td>
      <td>Reads until the named element is found.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/read_to_next_sibling">read_to_next_sibling(name, [ns])</a></td>
      <td>Advances the reader to the next matching sibling element.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/resolve_entity">resolve_entity()</a></td>
      <td>Resolves the entity reference for EntityReference nodes.</td>
    </tr>
    <tr>
      <td><a href="{{site.baseurl}}/docs/TaffyScript/Xml/XmlReader/skip">skip()</a></td>
      <td>Skips the children of the current node.</td>
    </tr>
  </tbody>
</table>
