using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TaffyScript.Xml
{
    /// <summary>
    /// Represents a writer that provides a fast, non-cached, forward-only way to generate streams or files that contain XML data.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter?view=netframework-4.7</source>
    /// <property name="settings" type="[XmlWriterSettings]({{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings)" access="get">
    ///     <summary>Gets the settings used to create this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.settings?view=netframework-4.7</source>
    /// </property>
    /// <property name="write_state" type="[WriteState](https://docs.microsoft.com/en-us/dotnet/api/system.xml.writestate?view=netframework-4.7)" access="get">
    ///     <summary>Gets the state of this writer.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writestate?view=netframework-4.7</source>
    /// </property>
    /// <property name="xml_lang" type="string" access="get">
    ///     <summary>Gets the current xmk:lang scope.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.xmllang?view=netframework-4.7</source>
    /// </property>
    /// <property name="xml_space" type="[XmlSpace](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlspace?view=netframework-4.7)" access="get">
    ///     <summary>Gets the current xml:space scope.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.xmlspace?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject]
    public class XmlWriter : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public System.Xml.XmlWriter Source { get; }

        public string ObjectType => "TaffyScript.Xml.XmlWriter";

        public XmlWriter(TsObject[] args)
        {
            Source = args.Length > 1 ? System.Xml.XmlWriter.Create((string)args[0], ((XmlWriterSettings)args[1]).Source) :
                                       System.Xml.XmlWriter.Create((string)args[0]);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "dispose":
                    return dispose(args);
                case "flush":
                    return flush(args);
                case "lookup_prefix":
                    return lookup_prefix(args);
                case "write_attributes":
                    return write_attributes(args);
                case "write_cdata":
                    return write_cdata(args);
                case "write_comment":
                    return write_comment(args);
                case "write_doc_type":
                    return write_doc_type(args);
                case "write_element_string":
                    return write_element_string(args);
                case "write_end_attribute":
                    return write_end_attribute(args);
                case "write_end_document":
                    return write_end_document(args);
                case "write_end_element":
                    return write_end_element(args);
                case "write_entity_ref":
                    return write_entity_ref(args);
                case "write_full_end_element":
                    return write_full_end_element(args);
                case "write_name":
                    return write_name(args);
                case "write_nm_token":
                    return write_nm_token(args);
                case "write_node":
                    return write_node(args);
                case "write_processing_instruction":
                    return write_processing_instruction(args);
                case "write_qualified_name":
                    return write_qualified_name(args);
                case "write_raw":
                    return write_raw(args);
                case "write_start_attribute":
                    return write_start_attribute(args);
                case "write_start_document":
                    return write_start_document(args);
                case "write_start_element":
                    return write_start_element(args);
                case "write_string":
                    return write_string(args);
                case "write_value_bool":
                    return write_value_bool(args);
                case "write_value_date_time":
                    return write_value_date_time(args);
                case "write_value_date_time_offset":
                    return write_value_date_time_offset(args);
                case "write_value_decimal":
                    return write_value_decimal(args);
                case "write_value_double":
                    return write_value_double(args);
                case "write_value_float":
                    return write_value_float(args);
                case "write_value_int":
                    return write_value_int(args);
                case "write_value_long":
                    return write_value_long(args);
                case "write_value_string":
                    return write_value_string(args);
                case "write_whitespace":
                    return write_whitespace(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public TsObject GetMember(string name)
        {
            switch(name)
            {
                case "settings":
                    return new XmlWriterSettings(Source.Settings);
                case "write_state":
                    return (float)Source.WriteState;
                case "xml_lang":
                    return Source.XmlLang;
                case "xml_space":
                    return (float)Source.XmlSpace;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName)
            {
                case "dispose":
                    del = new TsDelegate(dispose, scriptName);
                    break;
                case "flush":
                    del = new TsDelegate(flush, scriptName);
                    break;
                case "lookup_prefix":
                    del = new TsDelegate(lookup_prefix, scriptName);
                    break;
                case "write_attributes":
                    del = new TsDelegate(write_attributes, scriptName);
                    break;
                case "write_cdata":
                    del = new TsDelegate(write_cdata, scriptName);
                    break;
                case "write_comment":
                    del = new TsDelegate(write_comment, scriptName);
                    break;
                case "write_doc_type":
                    del = new TsDelegate(write_doc_type, scriptName);
                    break;
                case "write_element_string":
                    del = new TsDelegate(write_element_string, scriptName);
                    break;
                case "write_end_attribute":
                    del = new TsDelegate(write_end_attribute, scriptName);
                    break;
                case "write_end_document":
                    del = new TsDelegate(write_end_document, scriptName);
                    break;
                case "write_end_element":
                    del = new TsDelegate(write_end_element, scriptName);
                    break;
                case "write_entity_ref":
                    del = new TsDelegate(write_entity_ref, scriptName);
                    break;
                case "write_full_end_element":
                    del = new TsDelegate(write_full_end_element, scriptName);
                    break;
                case "write_name":
                    del = new TsDelegate(write_name, scriptName);
                    break;
                case "write_nm_token":
                    del = new TsDelegate(write_nm_token, scriptName);
                    break;
                case "write_node":
                    del = new TsDelegate(write_node, scriptName);
                    break;
                case "write_processing_instruction":
                    del = new TsDelegate(write_processing_instruction, scriptName);
                    break;
                case "write_qualified_name":
                    del = new TsDelegate(write_qualified_name, scriptName);
                    break;
                case "write_raw":
                    del = new TsDelegate(write_raw, scriptName);
                    break;
                case "write_start_attribute":
                    del = new TsDelegate(write_start_attribute, scriptName);
                    break;
                case "write_start_document":
                    del = new TsDelegate(write_start_document, scriptName);
                    break;
                case "write_start_element":
                    del = new TsDelegate(write_start_element, scriptName);
                    break;
                case "write_string":
                    del = new TsDelegate(write_string, scriptName);
                    break;
                case "write_value_bool":
                    del = new TsDelegate(write_value_bool, scriptName);
                    break;
                case "write_value_date_time":
                    del = new TsDelegate(write_value_date_time, scriptName);
                    break;
                case "write_value_date_time_offset":
                    del = new TsDelegate(write_value_date_time_offset, scriptName);
                    break;
                case "write_value_decimal":
                    del = new TsDelegate(write_value_decimal, scriptName);
                    break;
                case "write_value_double":
                    del = new TsDelegate(write_value_double, scriptName);
                    break;
                case "write_value_float":
                    del = new TsDelegate(write_value_float, scriptName);
                    break;
                case "write_value_int":
                    del = new TsDelegate(write_value_int, scriptName);
                    break;
                case "write_value_long":
                    del = new TsDelegate(write_value_long, scriptName);
                    break;
                case "write_value_string":
                    del = new TsDelegate(write_value_string, scriptName);
                    break;
                case "write_whitespace":
                    del = new TsDelegate(write_whitespace, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Releases all dynamic resources used by this instance.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.dispose?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject dispose(TsObject[] args)
        {
            Source.Dispose();
            return TsObject.Empty;
        }

        /// <summary>
        /// Flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.flush?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject flush(TsObject[] args)
        {
            Source.Flush();
            return TsObject.Empty;
        }

        /// <summary>
        /// Returns the closest prefix defined in the current namespace for the namespace URI.
        /// </summary>
        /// <arg name="ns" type="string">The namespace URI whose prefix is to be found.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.lookupprefix?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject lookup_prefix(TsObject[] args)
        {
            return Source.LookupPrefix((string)args[0]);
        }

        /// <summary>
        /// Writes out all of the attributes found at the current position in the specified XmlReader.
        /// </summary>
        /// <arg name="reader" type="[XmlReader]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReader)">The reader fomr which to copt the attributes.</arg>
        /// <arg name="defattr" type="bool">Determines whether the copy the default attributes from the XmlReader.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writeattributes?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_attributes(TsObject[] args)
        {
            Source.WriteAttributes(((XmlReader)args[0]).Source, (bool)args[1]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes an attribute witht the specified value.
        /// </summary>
        /// <arg name="name" type="string">The name of the attribute.</arg>
        /// <arg name="value" type="string">The value of the attribute.</arg>
        /// <arg name="[ns]" type="string">The namespace URI of the attribute.</arg>
        /// <arg name="[prefix]" type="string">The namespace prefix of the attribute.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writeattributestring?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_attribute_string(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    Source.WriteAttributeString((string)args[0], args[1].GetStringOrNull());
                    break;
                case 3:
                    Source.WriteAttributeString((string)args[0], args[2].GetStringOrNull(), args[1].GetStringOrNull());
                    break;
                default:
                    Source.WriteAttributeString(args[3].GetString(), (string)args[0], args[2].GetStringOrNull(), args[1].GetStringOrNull());
                    break;
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes out a CDATA block containing the specified text.
        /// </summary>
        /// <arg name="text" type="string">The text to place inside of the CDATA block.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writecdata?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_cdata(TsObject[] args)
        {
            Source.WriteCData((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes out a comment containing the specified text.
        /// </summary>
        /// <arg name="text" type="string">The text to place inside the comment.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writecomment?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_comment(TsObject[] args)
        {
            Source.WriteComment((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the DOCTYPE declaration with the specified name and optional attributes.
        /// </summary>
        /// <arg name="name" type="string">The name of the DOCTYPE. Cannot be null or empty.</arg>
        /// <arg name="pubid" type="string">If non-null, writes PUBLIC "pubid" "sysid" where pubid and sysid are replaced with the value of the given arguments.</arg>
        /// <arg name="sysid" type="string">If pubid is null and this isn't, writes SYSTEM "sysid" where sysid is replaced with the value of this argument.</arg>
        /// <arg name="subset" type="string">If non-null, writes [subset] where the subset is replaced witht the value of this argument.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writedoctype?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_doc_type(TsObject[] args)
        {
            Source.WriteDocType((string)args[0], args[1].GetStringOrNull(), args[2].GetStringOrNull(), args[3].GetStringOrNull());
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes an element containing a string value.
        /// </summary>
        /// <arg name="name" type="string">The name of the element.</arg>
        /// <arg name="value" type="string">The value of the element.</arg>
        /// <arg name="[ns]" type="string">The namespace URI of the element.</arg>
        /// <arg name="[prefix]" type="string">The namespace prefix of the element.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writeelementstring?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_element_string(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    Source.WriteElementString((string)args[0], args[1].GetStringOrNull());
                    break;
                case 3:
                    Source.WriteElementString((string)args[0], args[2].GetStringOrNull(), args[1].GetStringOrNull());
                    break;
                default:
                    Source.WriteElementString(args[3].GetString(), (string)args[0], args[2].GetStringOrNull(), args[1].GetStringOrNull());
                    break;
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Closes the previous write_start_attribute call.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writeendattribute?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_end_attribute(TsObject[] args)
        {
            Source.WriteEndAttribute();
            return TsObject.Empty;
        }

        /// <summary>
        /// Closes any open elements and attributes and puts the writer back in the Start state.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writeenddocument?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_end_document(TsObject[] args)
        {
            Source.WriteEndDocument();
            return TsObject.Empty;
        }

        /// <summary>
        /// Closes one element andpops the corresponding namespace scope.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writeendelement?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_end_element(TsObject[] args)
        {
            Source.WriteEndElement();
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes out an entity reference.
        /// </summary>
        /// <arg name="name" type="string">The name of the entity reference.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writeentityref?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_entity_ref(TsObject[] args)
        {
            Source.WriteEntityRef((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Closes one element and pops the corresponding namespace scope.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writefullendelement?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_full_end_element(TsObject[] args)
        {
            Source.WriteFullEndElement();
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes out the specified name, ensuring is is valid.
        /// </summary>
        /// <arg name="name" type="string">The name to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writename?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_name(TsObject[] args)
        {
            Source.WriteName((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes out the specified name token, ensuring it is valid.
        /// </summary>
        /// <arg name="name_token" type="string">The name token to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writenmtoken?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_nm_token(TsObject[] args)
        {
            Source.WriteNmToken((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Copies everything from the specified XmlReader to this and moves the reader to the start of the next sibling.
        /// </summary>
        /// <arg name="reader" type=[XmlReader]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReader)">The XmlReader to read from.</arg>
        /// <arg name="defattr" type="bool">Determines whether to copy the default attributes from the XmlReader.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writenode?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_node(TsObject[] args)
        {
            Source.WriteNode(((XmlReader)args[0]).Source, (bool)args[1]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes out a processing instruction.
        /// </summary>
        /// <arg name="name" type="string">The name of the processing instruction.</arg>
        /// <arg name="text" type="string">The text to include with the processing instruction.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writeprocessinginstruction?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_processing_instruction(TsObject[] args)
        {
            Source.WriteProcessingInstruction((string)args[0], args[1].GetStringOrNull());
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes out the namespace qualified name.
        /// </summary>
        /// <arg name="name" type="string">The local name to write.</arg>
        /// <arg name="ns" type="string">The namespace URI for the name.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writequalifiedname?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_qualified_name(TsObject[] args)
        {
            Source.WriteQualifiedName((string)args[0], args[1].GetStringOrNull());
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes raw markup manually.
        /// </summary>
        /// <arg name="data" type="string">String containing the text to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writeraw?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_raw(TsObject[] args)
        {
            Source.WriteRaw((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the start of an attribute.
        /// </summary>
        /// <arg name="name" type="string">The name of the attribute.</arg>
        /// <arg name="[ns]" type="string">The namespace URI for the attribute.</arg>
        /// <arg name="[prefix]" type="string">The namespace prefix of the attribute.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writestartattribute?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_start_attribute(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    Source.WriteStartAttribute((string)args[0]);
                    break;
                case 2:
                    Source.WriteStartAttribute((string)args[0], args[1].GetStringOrNull());
                    break;
                case 3:
                    Source.WriteStartAttribute(args[2].GetStringOrNull(), (string)args[0], args[1].GetStringOrNull());
                    break;
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the XML declaration.
        /// </summary>
        /// <arg name="[standalone]" type="bool">If given, writes "standalone=yes" if true or "standalone=no" if false.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writestartdocument?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_start_document(TsObject[] args)
        {
            if (args is null)
                Source.WriteStartDocument();

            switch(args.Length)
            {
                case 0:
                    Source.WriteStartDocument();
                    break;
                case 1:
                    Source.WriteStartDocument((bool)args[0]);
                    break;
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the specified start tag.
        /// </summary>
        /// <arg name="name" type="string">The name of the element.</arg>
        /// <arg name="[ns]" type="string">The namespace URI to associate with the element.</arg>
        /// <arg name="[prefix]" type="string">The namespace prefix of the element.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writestartelement?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_start_element(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    Source.WriteStartElement((string)args[0]);
                    break;
                case 2:
                    Source.WriteStartElement((string)args[0], args[1].GetStringOrNull());
                    break;
                case 3:
                    Source.WriteStartElement(args[2].GetStringOrNull(), (string)args[0], args[1].GetStringOrNull());
                    break;
            }
            return TsObject.Empty;
        }


        /// <summary>
        /// Writes the given text content.
        /// </summary>
        /// <arg name="text" type="string">The text to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writestring?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_string(TsObject[] args)
        {
            Source.WriteString(args[0].GetStringOrNull());
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes a bool value.
        /// </summary>
        /// <arg name="value" type="bool">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writevalue?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_value_bool(TsObject[] args)
        {
            Source.WriteValue((bool)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes a DateTime value.
        /// </summary>
        /// <arg name="value" type="[{{site.baseurl}}/docs/TaffyScript/DateTime]">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writevalue?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_value_date_time(TsObject[] args)
        {
            Source.WriteValue(((TsDateTime)args[0]).Source);
            return TsObject.Empty;
        }

        // Todo: XmlWriter.write_value_date_time_offset
        public TsObject write_value_date_time_offset(TsObject[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a decimal value.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writevalue?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_value_decimal(TsObject[] args)
        {
            Source.WriteValue((decimal)args[0].GetNumber());
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes a double value.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writevalue?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_value_double(TsObject[] args)
        {
            Source.WriteValue((double)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes a float value.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writevalue?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_value_float(TsObject[] args)
        {
            Source.WriteValue((float)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes an int value.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writevalue?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_value_int(TsObject[] args)
        {
            Source.WriteValue((int)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes a long value.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writevalue?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_value_long(TsObject[] args)
        {
            Source.WriteValue((long)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes a string value.
        /// </summary>
        /// <arg name="value" type="string">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writevalue?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_value_string(TsObject[] args)
        {
            Source.WriteValue((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the given whitespace.
        /// </summary>
        /// <arg name="ws" type="string">The string of whitespace characters.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter.writewhitespace?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_whitespace(TsObject[] args)
        {
            Source.WriteWhitespace((string)args[0]);
            return TsObject.Empty;
        }

        public static implicit operator TsObject(XmlWriter writer) => new TsInstanceWrapper(writer);
        public static explicit operator XmlWriter(TsObject obj) => (XmlWriter)obj.WeakValue;
    }
}
