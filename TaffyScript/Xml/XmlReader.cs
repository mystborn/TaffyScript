using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TaffyScript.IO;
using InternalReader = System.Xml.XmlReader;

namespace TaffyScript.Xml
{
    /// <summary>
    /// Represents a reader that provides fast, noncached, forward-only access to XML data.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader?view=netframework-4.7</source>
    /// <property name="attribute_count" type="number" access="get">
    ///     <summary>Gets number of attributes on the current node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.attributecount?view=netframework-4.7</source>
    /// </property>
    /// <property name="base_uri" type="string" access="get">
    ///     <summary>Gets the base URI of the current node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.baseuri?view=netframework-4.7</source>
    /// </property>
    /// <property name="can_read_binary_content" type="bool" access="get">
    ///     <summary>Determines if this instance implements the binary content read methods.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.canreadbinarycontent?view=netframework-4.7</source>
    /// </property>
    /// <property name="can_read_value_chunk" type="bool" access="get">
    ///     <summary>Determines if this instance can read a chunk of values.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.canreadvaluechunk?view=netframework-4.7</source>
    /// </property>
    /// <property name="can_resolve_entity" type="bool" access="get">
    ///     <summary>Determines if this instance can parse and resolve entities.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.canresolveentity?view=netframework-4.7</source>
    /// </property>
    /// <property name="depth" type="number" access="get">
    ///     <summary>Gets the depth of the current node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.depth?view=netframework-4.7</source>
    /// </property>
    /// <property name="eof" type="bool" access="get">
    ///     <summary>Determines if the reader is positioned at the end of the stream.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.eof?view=netframework-4.7</source>
    /// </property>
    /// <property name="has_attributes" type="bool" access="get">
    ///     <summary>Determines if the current node has any attributes.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.hasattributes?view=netframework-4.7</source>
    /// </property>
    /// <property name="has_value" type="bool" access="get">
    ///     <summary>Determines if the current node can have a value.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.hasvalue?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_default" type="bool" access="get">
    ///     <summary>Determines if the current node is an attribute that was generated from the default value defined in the DTD or schema.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.isdefault?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_empty_element" type="bool" access="get">
    ///     <summary>Determines if the current node is an empty element.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.isemptyelement?view=netframework-4.7</source>
    /// </property>
    /// <property name="local_name" type="string" access="get">
    ///     <summary>Gets the local name of the current node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.localname?view=netframework-4.7</source>
    /// </property>
    /// <property name="name" type="string" access="get">
    ///     <summary>Gets the qualified name of the current node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.name?view=netframework-4.7</source>
    /// </property>
    /// <property name="namespace_uri" type="string" access="get">
    ///     <summary>Gets the namespace URI of the current node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.namespaceuri?view=netframework-4.7</source>
    /// </property>
    /// <property name="node_type" type="[XmlNodeType](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlnodetype?view=netframework-4.7)" access="get">
    ///     <summary>Gets the type of the current node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.nodetype?view=netframework-4.7</source>
    /// </property>
    /// <property name="prefix" type="string" access="get">
    ///     <summary>Gets the namespace prefix associated with the current node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.prefix?view=netframework-4.7</source>
    /// </property>
    /// <property name="quote_char" type="string" access="get">
    ///     <summary>Gets the quotation mark character used to enclose the value of an attribute node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.quotechar?view=netframework-4.7</source>
    /// </property>
    /// <property name="read_state" type="[ReadState](https://docs.microsoft.com/en-us/dotnet/api/system.xml.readstate?view=netframework-4.7)" access="get">
    ///     <summary>Gets the state of the reader.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readstate?view=netframework-4.7</source>
    /// </property>
    /// <property name="settings" type="[XmlReaderSettings]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReaderSettings)" access="get">
    ///     <summary>Gets the XmlReaderSettings used to create this instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.settings?view=netframework-4.7</source>
    /// </property>
    /// <property name="value" type="string" access="get">
    ///     <summary>Gets the text value of the current node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.value?view=netframework-4.7</source>
    /// </property>
    /// <property name="value_type" type="string" access="get">
    ///     <summary>Gets the CLR type of the current node.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.valuetype?view=netframework-4.7</source>
    /// </property>
    /// <property name="xml_lang" type="string" access="get">
    ///     <summary>Gets the current xml:lang scope.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.xmllang?view=netframework-4.7</source>
    /// </property>
    /// <property name="xml_space" type="[XmlSpace](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlspace?view=netframework-4.7)" access="get">
    ///     <summary>Gets the current xml:space scope.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.xmlspace?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject]
    public class XmlReader : ITsInstance
    {
        public InternalReader Source { get; }

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.Xml.XmlReader";

        /// <summary>
        /// Creates a new XmlReader from a Stream, TextReader, or path.
        /// </summary>
        /// <arg name="source" type="[Stream]({{site.baseurl}}/docs/TaffyScript/IO/Stream), [TextReader]({{site.baseurl}}/docs/TaffyScript/IO/TextReader), or string">The Stream, TextReader, or path used to initialize this XmlReader.</arg>
        /// <arg name="[settings] type="[XmlReaderSettings]({{site.baseurl}}/docs/TaffyScript/Xml/XmlReaderSettings)">The settings used to initialize this XmlReader.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.-ctor?view=netframework-4.7</source>
        public XmlReader(TsObject[] args)
        {
            switch (args[0].WeakValue)
            {
                case TsStream stream:
                    switch (args.Length)
                    {
                        case 1:
                            Source = InternalReader.Create(stream.Stream);
                            break;
                        case 2:
                            Source = InternalReader.Create(stream.Stream, ((XmlReaderSettings)args[1]).Settings);
                            break;
                        default:
                            throw new ArgumentException($"Invalid number of arguments passed to the constructor of {ObjectType}");
                    }
                    break;
                case TextReader textReader:
                    switch (args.Length)
                    {
                        case 1:
                            Source = InternalReader.Create(textReader.Reader);
                            break;
                        case 2:
                            Source = InternalReader.Create(textReader.Reader, ((XmlReaderSettings)args[1]).Settings);
                            break;
                        default:
                            throw new ArgumentException($"Invalid number of arguments passed to the constructor of {ObjectType}");
                    }
                    break;
                case string str:
                    switch (args.Length)
                    {
                        case 1:
                            Source = InternalReader.Create(str);
                            break;
                        case 2:
                            Source = InternalReader.Create(str, ((XmlReaderSettings)args[1]).Settings);
                            break;
                        default:
                            throw new ArgumentException($"Invalid number of arguments passed to the constructor of {ObjectType}");
                    }
                    break;
                default:
                    throw new ArgumentException($"The first argument passed to the constructor of {ObjectType} must be one of the following: TaffyScript.IO.Stream, TaffyScript.IO.TextReader, or string");
            }
        }

        public XmlReader(System.Xml.XmlReader reader)
        {
            Source = reader;
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "dispose":
                    return dispose(args);
                case "get":
                    return get(args);
                case "get_attribute":
                    return get_attribute(args);
                case "is_start_element":
                    return is_start_element(args);
                case "lookup_namespace":
                    return lookup_namespace(args);
                case "move_to_attribute":
                    return move_to_attribute(args);
                case "move_to_content":
                    return move_to_content(args);
                case "move_to_element":
                    return move_to_element(args);
                case "move_to_first_attribute":
                    return move_to_first_attribute(args);
                case "move_to_next_attribute":
                    return move_to_next_attribute(args);
                case "read":
                    return read(args);
                case "read_attribute_value":
                    return read_attribute_value(args);
                case "read_content_as_bool":
                    return read_content_as_bool(args);
                case "read_content_as_date_time":
                    return read_content_as_date_time(args);
                case "read_content_as_double":
                    return read_content_as_double(args);
                case "read_content_as_float":
                    return read_content_as_float(args);
                case "read_content_as_int":
                    return read_content_as_int(args);
                case "read_content_as_long":
                    return read_content_as_long(args);
                case "read_content_as_string":
                    return read_content_as_string(args);
                case "read_element_content_as_bool":
                    return read_element_content_as_bool(args);
                case "read_element_content_as_date_time":
                    return read_element_content_as_date_time(args);
                case "read_element_content_as_double":
                    return read_element_content_as_double(args);
                case "read_element_content_as_float":
                    return read_element_content_as_float(args);
                case "read_element_content_as_int":
                    return read_element_content_as_int(args);
                case "read_element_content_as_long":
                    return read_element_content_as_long(args);
                case "read_element_content_as_string":
                    return read_element_content_as_string(args);
                case "read_end_element":
                    return read_end_element(args);
                case "read_inner_xml":
                    return read_inner_xml(args);
                case "read_outer_xml":
                    return read_outer_xml(args);
                case "read_start_element":
                    return read_start_element(args);
                case "read_subtree":
                    return read_subtree(args);
                case "read_to_descendant":
                    return read_to_descendant(args);
                case "read_to_following":
                    return read_to_following(args);
                case "read_to_next_sibling":
                    return read_to_next_sibling(args);
                case "resolve_entity":
                    return resolve_entity(args);
                case "skip":
                    return skip(args);
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
            switch (name)
            {
                case "attribute_count":
                    return Source.AttributeCount;
                case "base_uri":
                    return Source.BaseURI;
                case "can_read_binary_content":
                    return Source.CanReadBinaryContent;
                case "can_read_value_chunk":
                    return Source.CanReadValueChunk;
                case "can_resolve_entity":
                    return Source.CanResolveEntity;
                case "depth":
                    return Source.Depth;
                case "eof":
                    return Source.EOF;
                case "has_attributes":
                    return Source.HasAttributes;
                case "has_value":
                    return Source.HasValue;
                case "is_default":
                    return Source.IsDefault;
                case "is_empty_element":
                    return Source.IsEmptyElement;
                case "local_name":
                    return Source.LocalName;
                case "name":
                    return Source.Name;
                case "namespace_uri":
                    return Source.NamespaceURI;
                case "node_type":
                    return (float)Source.NodeType;
                case "prefix":
                    return Source.Prefix;
                case "quote_char":
                    return Source.QuoteChar;
                case "read_state":
                    return (float)Source.ReadState;
                case "settings":
                    return new XmlReaderSettings(Source.Settings);
                case "value":
                    return Source.Value;
                case "value_type":
                    return Source.ValueType.FullName;
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
            switch (scriptName)
            {
                case "dispose":
                    del = new TsDelegate(dispose, "dispose");
                    return true;
                case "get":
                    del = new TsDelegate(get, "get");
                    return true;
                case "get_attribute":
                    del = new TsDelegate(get_attribute, "get_attribute");
                    return true;
                case "is_start_element":
                    del = new TsDelegate(is_start_element, "is_start_element");
                    return true;
                case "lookup_namespace":
                    del = new TsDelegate(lookup_namespace, "lookup_namespace");
                    return true;
                case "move_to_attribute":
                    del = new TsDelegate(move_to_attribute, "move_to_attribute");
                    return true;
                case "move_to_content":
                    del = new TsDelegate(move_to_content, "move_to_content");
                    return true;
                case "move_to_element":
                    del = new TsDelegate(move_to_element, "move_to_element");
                    return true;
                case "move_to_first_attribute":
                    del = new TsDelegate(move_to_first_attribute, scriptName);
                    return true;
                case "move_to_next_attribute":
                    del = new TsDelegate(move_to_next_attribute, "move_to_next_attribute");
                    return true;
                case "read":
                    del = new TsDelegate(read, "read");
                    return true;
                case "read_attribute_value":
                    del = new TsDelegate(read_attribute_value, "read_attribute_value");
                    return true;
                case "read_content_as_bool":
                    del = new TsDelegate(read_content_as_bool, "read_content_as_bool");
                    return true;
                case "read_content_as_date_time":
                    del = new TsDelegate(read_content_as_date_time, scriptName);
                    return true;
                case "read_content_as_double":
                    del = new TsDelegate(read_content_as_double, scriptName);
                    return true;
                case "read_content_as_float":
                    del = new TsDelegate(read_content_as_float, scriptName);
                    return true;
                case "read_content_as_int":
                    del = new TsDelegate(read_content_as_int, scriptName);
                    return true;
                case "read_content_as_long":
                    del = new TsDelegate(read_content_as_long, scriptName);
                    return true;
                case "read_content_as_string":
                    del = new TsDelegate(read_content_as_string, "read_content_as_string");
                    return true;
                case "read_element_content_as_bool":
                    del = new TsDelegate(read_element_content_as_bool, "read_element_content_as_bool");
                    return true;
                case "read_element_content_as_date_time":
                    del = new TsDelegate(read_element_content_as_date_time, scriptName);
                    return true;
                case "read_element_content_as_double":
                    del = new TsDelegate(read_element_content_as_double, scriptName);
                    return true;
                case "read_element_content_as_float":
                    del = new TsDelegate(read_element_content_as_float, scriptName);
                    return true;
                case "read_element_content_as_int":
                    del = new TsDelegate(read_element_content_as_int, scriptName);
                    return true;
                case "read_element_content_as_long":
                    del = new TsDelegate(read_element_content_as_long, scriptName);
                    return true;
                case "read_element_content_as_string":
                    del = new TsDelegate(read_element_content_as_string, "read_element_content_as_string");
                    return true;
                case "read_end_element":
                    del = new TsDelegate(read_end_element, "read_end_element");
                    return true;
                case "read_inner_xml":
                    del = new TsDelegate(read_inner_xml, "read_inner_xml");
                    return true;
                case "read_outer_xml":
                    del = new TsDelegate(read_outer_xml, "read_outer_xml");
                    return true;
                case "read_start_element":
                    del = new TsDelegate(read_start_element, "read_start_element");
                    return true;
                case "read_subtree":
                    del = new TsDelegate(read_subtree, "read_subtree");
                    return true;
                case "read_to_descendant":
                    del = new TsDelegate(read_to_descendant, "read_to_descendant");
                    return true;
                case "read_to_following":
                    del = new TsDelegate(read_to_following, "read_to_following");
                    return true;
                case "read_to_next_sibling":
                    del = new TsDelegate(read_to_next_sibling, "read_to_next_sibling");
                    return true;
                case "resolve_entity":
                    del = new TsDelegate(resolve_entity, "resolve_entity");
                    return true;
                case "skip":
                    del = new TsDelegate(skip, "skip");
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        /// <summary>
        /// Releases all dynamic resources used by this instance.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.dispose?view=netframework-4.7.2</source>
        /// <returns>null</returns>
        public TsObject dispose(TsObject[] args)
        {
            Source.Dispose();
            return TsObject.Empty;
        }

        /// <summary>
        /// Gets the value of an attibute.
        /// </summary>
        /// <arg name="name_or_index" type="string or number">The name or index of the attribute.</arg>
        /// <arg name="[ns]" type="string">The namespace URI of the attribute.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.item?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject get(TsObject[] args)
        {
            switch (args[0].Type)
            {
                case VariableType.Real:
                    return Source.GetAttribute((int)args[0]) ?? TsObject.Empty;
                case VariableType.String:
                    switch (args.Length)
                    {
                        case 1:
                            return Source.GetAttribute((string)args[0]) ?? TsObject.Empty;
                        default:
                            return Source.GetAttribute((string)args[0], (string)args[1]) ?? TsObject.Empty;
                    }
                default:
                    throw new ArgumentException($"The first argument to {ObjectType}.{nameof(get)} should be a number or a string.");
            }
        }

        /// <summary>
        /// Gets the value of an attibute.
        /// </summary>
        /// <arg name="name_or_index" type="string or number">The name or index of the attribute.</arg>
        /// <arg name="[ns]" type="string">The namespace URI of the attribute.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.getattribute?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject get_attribute(TsObject[] args)
        {
            switch (args[0].Type)
            {
                case VariableType.Real:
                    return Source.GetAttribute((int)args[0]) ?? TsObject.Empty;
                case VariableType.String:
                    switch (args.Length)
                    {
                        case 1:
                            return Source.GetAttribute((string)args[0]) ?? TsObject.Empty;
                        default:
                            return Source.GetAttribute((string)args[0], (string)args[1]) ?? TsObject.Empty;
                    }
                default:
                    throw new ArgumentException($"The first argument to {ObjectType}.{nameof(get_attribute)} should be a number or a string.");
            }
        }

        /// <summary>
        /// Determines if the specified string is a valid XML name.
        /// </summary>
        /// <arg name="str" type="string">The name to validate.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.isname?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public static TsObject is_name(TsObject[] args)
        {
            return InternalReader.IsName((string)args[0]);
        }

        /// <summary>
        /// Determines if the specified string is a valid XML name token.
        /// </summary>
        /// <arg name="str" type="string">The name token to validate.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.isnametoken?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public static TsObject is_name_token(TsObject[] args)
        {
            return InternalReader.IsNameToken((string)args[1]);
        }

        /// <summary>
        /// Tests if the current node is a start tag, optionally testing if the name matches the specified string.
        /// </summary>
        /// <arg name="[name]" type="string">The string to match against the LocalName property of the element.</arg>
        /// <arg name="[ns]" type="string">The string to match against the NamespaceURI property of the element.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.isstartelement?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject is_start_element(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    return Source.IsStartElement();
                case 1:
                    return Source.IsStartElement((string)args[0]);
                case 2:
                    return Source.IsStartElement((string)args[0], (string)args[1]);
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(is_start_element)}");
            }
        }

        /// <summary>
        /// Resolves a namespace prefix in the current scope.
        /// </summary>
        /// <arg name="prefix" type="string">The prefix whose namespace URI is to be resolved. To match the default namespace, pass an empty string.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.lookupnamespace?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject lookup_namespace(TsObject[] args)
        {
            return Source.LookupNamespace((string)args[0]);
        }

        /// <summary>
        /// Moves to the specified attribute.
        /// </summary>
        /// <arg name="name_or_index" type="string or number">The name or index of the attribute.</arg>
        /// <arg name="[ns]" type="string">The namespace URI of the attribute.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.movetoattribute?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject move_to_attribute(TsObject[] args)
        {
            if (args[0].Type == VariableType.Real)
            {
                var index = (int)args[0];
                if (index >= Source.AttributeCount)
                    return false;
                Source.MoveToAttribute((int)args[0]);
            }
            else
            {
                switch (args.Length)
                {
                    case 1:
                        return Source.MoveToAttribute((string)args[0]);
                    default:
                        return Source.MoveToAttribute((string)args[0], (string)args[1]);
                }
            }

            return true;
        }

        /// <summary>
        /// If the current element is not a content node, skips ahead to the next conent node or end of file.
        /// </summary>
        /// <source></source>
        /// <returns>[XmlNodeType](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlnodetype?view=netframework-4.7.2)</returns>
        public TsObject move_to_content(TsObject[] args)
        {
            return (float)Source.MoveToContent();
        }

        /// <summary>
        /// Moves to the element that contains the current attribute node.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.movetocontent?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject move_to_element(TsObject[] args)
        {
            return Source.MoveToElement();
        }

        /// <summary>
        /// Moves to the first attribute.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.movetofirstattribute?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject move_to_first_attribute(TsObject[] args)
        {
            return Source.MoveToFirstAttribute();
        }

        /// <summary>
        /// Moves to the next attribute.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.movetonextattribute?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject move_to_next_attribute(TsObject[] args)
        {
            return Source.MoveToNextAttribute();
        }

        /// <summary>
        /// Reads the next node from the stream.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.read?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject read(TsObject[] args)
        {
            return Source.Read();
        }

        /// <summary>
        /// Parses the attribute value into a node.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readattributevalue?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject read_attribute_value(TsObject[] args)
        {
            return Source.ReadAttributeValue();
        }

        /// <summary>
        /// Reads the text at the current position as a bool.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readcontentasboolean?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject read_content_as_bool(TsObject[] args)
        {
            return Source.ReadContentAsBoolean();
        }

        /// <summary>
        /// Reads the text at the current position as a DateTime.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readcontentasdatetime?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject read_content_as_date_time(TsObject[] args)
        {
            return new TsDateTime(Source.ReadContentAsDateTime());
        }

        /// <summary>
        /// Reads the text at the current position as a double.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readcontentasdouble?view=netframework-4.7</source>
        /// <returns>number</returns>
        public TsObject read_content_as_double(TsObject[] args)
        {
            return Source.ReadContentAsDouble();
        }

        /// <summary>
        /// Reads the text at the current position as a float.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readcontentasfloat?view=netframework-4.7</source>
        /// <returns>number</returns>
        public TsObject read_content_as_float(TsObject[] args)
        {
            return Source.ReadContentAsFloat();
        }

        /// <summary>
        /// Reads the text at the current position as an int.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readcontentasint?view=netframework-4.7</source>
        /// <returns>number</returns>
        public TsObject read_content_as_int(TsObject[] args)
        {
            return Source.ReadContentAsInt();
        }

        /// <summary>
        /// Reads the text at the current position as a long.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readcontentaslong?view=netframework-4.7</source>
        /// <returns>number</returns>
        public TsObject read_content_as_long(TsObject[] args)
        {
            return Source.ReadContentAsLong();
        }

        /// <summary>
        /// Reads the text at the current position as a string.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readcontentasstring?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject read_content_as_string(TsObject[] args)
        {
            return Source.ReadContentAsString();
        }

        /// <summary>
        /// Reads the current element as a bool.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readelementcontentasboolean?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject read_element_content_as_bool(TsObject[] args)
        {
            return Source.ReadElementContentAsBoolean();
        }

        /// <summary>
        /// Reads the current element as a DateTime.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readelementcontentasdatetime?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public TsObject read_element_content_as_date_time(TsObject[] args)
        {
            return new TsDateTime(Source.ReadElementContentAsDateTime());
        }

        /// <summary>
        /// Reads the current element as a double.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readelementcontentasdouble?view=netframework-4.7</source>
        /// <returns>number</returns>
        public TsObject read_element_content_as_double(TsObject[] args)
        {
            return Source.ReadElementContentAsDouble();
        }

        /// <summary>
        /// Reads the current element as a float.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readelementcontentasfloat?view=netframework-4.7</source>
        /// <returns>number</returns>
        public TsObject read_element_content_as_float(TsObject[] args)
        {
            return Source.ReadElementContentAsFloat();
        }

        /// <summary>
        /// Reads the current element as an int.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readelementcontentasint?view=netframework-4.7</source>
        /// <returns>number</returns>
        public TsObject read_element_content_as_int(TsObject[] args)
        {
            return Source.ReadElementContentAsInt();
        }

        /// <summary>
        /// Reads the current element as a long.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readelementcontentaslong?view=netframework-4.7</source>
        /// <returns>number</returns>
        public TsObject read_element_content_as_long(TsObject[] args)
        {
            return Source.ReadElementContentAsLong();
        }

        /// <summary>
        /// Reads the current element as a string.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readelementcontentasstring?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject read_element_content_as_string(TsObject[] args)
        {
            return Source.ReadElementContentAsString();
        }

        /// <summary>
        /// Reads an end tag.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readendelement?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject read_end_element(TsObject[] args)
        {
            Source.ReadEndElement();
            return TsObject.Empty;
        }

        /// <summary>
        /// Reads all of the content, including markup, representing this nodes children.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readinnerxml?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject read_inner_xml(TsObject[] args)
        {
            return Source.ReadInnerXml();
        }

        /// <summary>
        /// Reads all of the content, including markup, representing this node and its children.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readouterxml?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject read_outer_xml(TsObject[] args)
        {
            return Source.ReadOuterXml();
        }

        /// <summary>
        /// Reads an element.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readstartelement?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject read_start_element(TsObject[] args)
        {
            Source.ReadStartElement();
            return TsObject.Empty;
        }

        /// <summary>
        /// Returns a new XmlReader that can be used to read the current node and its descendants.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readsubtree?view=netframework-4.7</source>
        /// <returns>[XmlReader]({{site.baseurl/docs/TaffyScript/Xml/XmlReader)</returns>
        public TsObject read_subtree(TsObject[] args)
        {
            return new XmlReader(Source.ReadSubtree());
        }

        /// <summary>
        /// Advances the reader to the next matching descendant element.
        /// </summary>
        /// <arg name="name" type="string">The name of the element to move to.</arg>
        /// <arg name="[ns]" type="string">The namespace URI of the element to move to.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readtodescendant?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject read_to_descendant(TsObject[] args)
        {
            return args.Length == 1 ? Source.ReadToDescendant((string)args[0]) : Source.ReadToDescendant((string)args[0], (string)args[1]);
        }

        /// <summary>
        /// Reads until the named element is found.
        /// </summary>
        /// <arg name="name" type="string">The name of the element to find.</arg>
        /// <arg name="[ns]" type="string">The namespace URI of the element to find.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readtofollowing?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject read_to_following(TsObject[] args)
        {
            return args.Length == 1 ? Source.ReadToFollowing((string)args[0]) : Source.ReadToFollowing((string)args[0], (string)args[1]);
        }

        /// <summary>
        /// Advances the reader to the next matching sibling element.
        /// </summary>
        /// <arg name="name" type="string">The name of the element to move to.</arg>
        /// <arg name="[ns]" type="string">The namespace URI of the element to move to.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.readtonextsibling?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public TsObject read_to_next_sibling(TsObject[] args)
        {
            return args.Length == 1 ? Source.ReadToNextSibling((string)args[0]) : Source.ReadToNextSibling((string)args[0], (string)args[1]);
        }

        /// <summary>
        /// Resolves the entity reference for EntityReference nodes.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.resolveentity?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject resolve_entity(TsObject[] args)
        {
            Source.ResolveEntity();
            return TsObject.Empty;
        }

        /// <summary>
        /// Skips the children of the current node.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.skip?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject skip(TsObject[] args)
        {
            Source.Skip();
            return TsObject.Empty;
        }

        public static implicit operator TsObject(XmlReader reader)
        {
            return new TsInstanceWrapper(reader);
        }

        public static explicit operator XmlReader(TsObject obj)
        {
            return (XmlReader)obj.WeakValue;
        }
    }
}
