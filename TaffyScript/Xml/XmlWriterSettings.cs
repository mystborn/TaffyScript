using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TaffyScript.Xml
{
    /// <summary>
    /// Specifies a set of features to support on the XmlWriter created with these settings.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings?view=netframework-4.7</source>
    /// <property name="async" type="bool" access="both">
    ///     <summary>Determines if asynchronous methods can be used on a particular SmlWriter instance.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.async?view=netframework-4.7</source>
    /// </property>
    /// <property name="check_characters" type="bool" access="both">
    ///     <summary>Determines if the XmlWriter should ensure that all characters conform to the XML specification.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.checkcharacters?view=netframework-4.7</source>
    /// </property>
    /// <property name="close_output" type="bool" access="both">
    ///     <summary>Determines if the XmlWriter should also close the underlying device when it's closed.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.closeoutput?view=netframework-4.7</source>
    /// </property>
    /// <property name="conformance_level" type="" access="both">
    ///     <summary>Determines the level of conformance the XmlWriter checks the output for.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.conformancelevel?view=netframework-4.7</source>
    /// </property>
    /// <property name="do_not_excape_uri_attributes" type="bool" access="both">
    ///     <summary>Determines whether the XmlWriter does not escape URI attributes.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.donotescapeuriattributes?view=netframework-4.7</source>
    /// </property>
    /// <property name="encoding" type="string" access="both">
    ///     <summary>Determines the type of text encoding to use.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.encoding?view=netframework-4.7</source>
    /// </property>
    /// <property name="indent" type="bool" access="both">
    ///     <summary>Determines whether to indent elements.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.indent?view=netframework-4.7</source>
    /// </property>
    /// <property name="indent_chars" type="string" access="both">
    ///     <summary>Determines the string to be used when indenting.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.indentchars?view=netframework-4.7</source>
    /// </property>
    /// <property name="namespace_handling" type="" access="both">
    ///     <summary>Determines if the XmlWriter should remove duplicate namespace declarations when writing Xml content.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.namespacehandling?view=netframework-4.7</source>
    /// </property>
    /// <property name="new_line_chars" type="string" access="both">
    ///     <summary>Determines the string to be used for line breaks.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.newlinechars?view=netframework-4.7</source>
    /// </property>
    /// <property name="new_line_handling" type="" access="both">
    ///     <summary>Determines whether to normalize line breaks in the output.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.newlinehandling?view=netframework-4.7</source>
    /// </property>
    /// <property name="new_line_on_attributes" type="bool" access="both">
    ///     <summary>Determines whether to write attributes on a new line.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.newlineonattributes?view=netframework-4.7</source>
    /// </property>
    /// <property name="omit_xml_declaration" type="bool" access="both">
    ///     <summary>Determines whether to omit an XML declaration.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.omitxmldeclaration?view=netframework-4.7</source>
    /// </property>
    /// <property name="output_method" type="" access="get">
    ///     <summary>Gets the method used to serialize XmlWriter output.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.outputmethod?view=netframework-4.7</source>
    /// </property>
    /// <property name="write_end_document_on_close" type="bool" access="both">
    ///     <summary>Determines whether the XmlWriter will add closing tags to all unclosed elements when closed.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.writeenddocumentonclose?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject]
    public class XmlWriterSettings : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public System.Xml.XmlWriterSettings Source { get; }

        public string ObjectType => "TaffyScript.Xml.XmlWriterSettings";

        public XmlWriterSettings(System.Xml.XmlWriterSettings source)
        {
            Source = source;
        }

        public XmlWriterSettings(TsObject[] args)
        {
            Source = new System.Xml.XmlWriterSettings();
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "clone":
                    return clone(args);
                case "reset":
                    return reset(args);
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
                case "async":
                    return Source.Async;
                case "check_chracters":
                    return Source.CheckCharacters;
                case "close_output":
                    return Source.CloseOutput;
                case "conformance_level":
                    return (float)Source.ConformanceLevel;
                case "do_not_escape_uri_attributes":
                    return Source.DoNotEscapeUriAttributes;
                case "encoding":
                    return Source.Encoding.EncodingName;
                case "indent":
                    return Source.Indent;
                case "indent_chars":
                    return Source.IndentChars;
                case "namespace_handling":
                    return (float)Source.NamespaceHandling;
                case "new_line_chars":
                    return Source.NewLineChars;
                case "new_line_handling":
                    return (float)Source.NewLineHandling;
                case "new_line_on_attributes":
                    return Source.NewLineOnAttributes;
                case "omit_xml_declaration":
                    return Source.OmitXmlDeclaration;
                case "output_method":
                    return (float)Source.OutputMethod;
                case "write_end_document_on_close":
                    return Source.WriteEndDocumentOnClose;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            switch (name)
            {
                case "async":
                    Source.Async = (bool)value;
                    break;
                case "check_chracters":
                    Source.CheckCharacters = (bool)value;
                    break;
                case "close_output":
                    Source.CloseOutput = (bool)value;
                    break;
                case "conformance_level":
                    Source.ConformanceLevel = (ConformanceLevel)value.GetNumber();
                    break;
                case "do_not_escape_uri_attributes":
                    Source.DoNotEscapeUriAttributes = (bool)value;
                    break;
                case "encoding":
                    Source.Encoding = Encoding.GetEncoding((string)value);
                    break;
                case "indent":
                    Source.Indent = (bool)value;
                    break;
                case "indent_chars":
                    Source.IndentChars = (string)value;
                    break;
                case "namespace_handling":
                    Source.NamespaceHandling = (NamespaceHandling)value.GetNumber();
                    break;
                case "new_line_chars":
                    Source.NewLineChars = (string)value;
                    break;
                case "new_line_handling":
                    Source.NewLineHandling = (NewLineHandling)value.GetNumber();
                    break;
                case "new_line_on_attributes":
                    Source.NewLineOnAttributes = (bool)value;
                    break;
                case "omit_xml_declaration":
                    Source.OmitXmlDeclaration = (bool)value;
                    break;
                case "write_end_document_on_close":
                    Source.WriteEndDocumentOnClose = (bool)value;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "clone":
                    del = new TsDelegate(clone, scriptName);
                    break;
                case "reset":
                    del = new TsDelegate(reset, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a copy of this instance.
        /// </summary>
        /// <returns>[XmlWriterSettings]({{site.baseurl}}/docs/TaffyScript/Xml/XmlWriterSettings)</returns>
        public TsObject clone(TsObject[] args)
        {
            return new XmlWriterSettings(Source.Clone());
        }

        /// <summary>
        /// Resets the settings to their default values.
        /// </summary>
        /// <returns>null</returns>
        public TsObject reset(TsObject[] args)
        {
            Source.Reset();
            return TsObject.Empty;
        }

        public override string ToString()
        {
            return Source.ToString();
        }

        public static implicit operator TsObject(XmlWriterSettings settings)
        {
            return new TsInstanceWrapper(settings);
        }

        public static explicit operator XmlWriterSettings(TsObject obj)
        {
            return (XmlWriterSettings)obj.WeakValue;
        }
    }
}
