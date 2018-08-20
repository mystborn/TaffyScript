---
layout: default
title: XmlReaderSettings
---

# XmlReaderSettings

[\[global\]]({{site.baseurl}}/docs/).[TaffyScript]({{site.baseurl}}/docs/TaffyScript/).[Xml]({{site.baseurl}}/docs/TaffyScript/Xml/).[XmlReaderSettings]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReaderSettings/)

_Specifies a set of features to be supported by an XmlReader._

## Fields

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
      <td><a href="{{page.url}}check_characters/">check_characters</a></td>
      <td>bool</td>
      <td>Gets or sets whether to do character checking.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}close_input/">close_input</a></td>
      <td>bool</td>
      <td>Gets or sets whether the underlying stream should be closed when the reader is closed.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}conformance_level/">conformance_level</a></td>
      <td><a href="https://docs.microsoft.com/en-us/dotnet/api/system.xml.conformancelevel?view=netframework-4.7.2">ConformanceLevel</a> (number)</td>
      <td>Gets or sets the level of conformance which the XmlReader will comply.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}dtd_processing/">dtd_processing</a></td>
      <td><a href="https://docs.microsoft.com/en-us/dotnet/api/system.xml.dtdprocessing?view=netframework-4.7.2">DtdProcessing</a> (number)</td>
      <td>Gets or sets a value that determines the processing of DTDs.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}ignore_comments/">ignore_comments</a></td>
      <td>bool</td>
      <td>Gets or sets whether to ignore comments.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}ignore_processing_instructions/">ignore_processing_instructions</a></td>
      <td>bool</td>
      <td>Gets or sets whether to ignore prcoessing instructions.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}ignore_whitespace/">ignore_whitespace</a></td>
      <td>bool</td>
      <td>Gets or sets whether to ignore whitespace.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}line_number_offset/">line_number_offset</a></td>
      <td>number</td>
      <td>Gets or sets the line number offset of the XmlReader.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}line_position_offset/">line_position_offset</a></td>
      <td>number</td>
      <td>Gets or sets the line position offset of the XmlReader.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}max_characters_from_entities/">max_characters_from_entities</a></td>
      <td>number</td>
      <td>Gets or sets the maximum allowable number of characters in a document that results from expanding entities.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}max_characters_in_document/">max_characters_in_document</a></td>
      <td>number</td>
      <td>Gets or sets the maximum allowable number of characters in an xml document. A zero means any number of characters.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}validation_flags/">validation_flags</a></td>
      <td><a href="https://docs.microsoft.com/en-us/dotnet/api/system.xml.schema.xmlschemavalidationflags?view=netframework-4.7.2">XmlSchemaValidationFlags</a> (number)</td>
      <td>Gets or sets the schema validation flags.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}validation_type/">validation_type</a></td>
      <td><a href="https://docs.microsoft.com/en-us/dotnet/api/system.xml.validationtype?view=netframework-4.7.2">ValidationType</a> (number)</td>
      <td>Get or sets whether the XmlReader will perform validation or type assignment while reading.</td>
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
      <td><a href="{{page.url}}clone">clone()</a></td>
      <td>Creates a copy of the XmlReaderSettings.</td>
    </tr>
    <tr>
      <td><a href="{{page.url}}reset">reset()</a></td>
      <td>Resets the members of the settings class to their default values.</td>
    </tr>
  </tbody>
</table>
