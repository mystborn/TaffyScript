using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TaffyScript.Xml
{
    /// <summary>
    /// Specifies a set of features to support on the XmlReader created with these settings.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings?view=netframework-4.7</source>
    /// <property>
    /// <property name="check_characters" type="bool" access="both">
    ///     <summary>Determines if the XmlReader does character checking.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.checkcharacters?view=netframework-4.7</source>
    /// </property>
    /// <property name="close_input" type="bool" access="both">
    ///     <summary>Determines if the XmlReader should close the underlying device when the reader is closed.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.closeinput?view=netframework-4.7</source>
    /// </property>
    /// <property name="conformance_level" type="[ConformanceLevel](https://docs.microsoft.com/en-us/dotnet/api/system.xml.conformancelevel?view=netframework-4.7)" access="both">
    ///     <summary>Determines the level of conformance which the XmlReader will comply.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.conformancelevel?view=netframework-4.7</source>
    /// </property>
    /// <property name="dtd_processing" type="[DtdProcessing](https://docs.microsoft.com/en-us/dotnet/api/system.xml.dtdprocessing?view=netframework-4.7)" access="both">
    ///     <summary>Determines the processing of DTDs.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.dtdprocessing?view=netframework-4.7</source>
    /// </property>
    /// <property name="ignore_comments" type="bool" access="both">
    ///     <summary>Determines if the XmlReader will ignore comments.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.ignorecomments?view=netframework-4.7</source>
    /// </property>
    /// <property name="ignore_processing_instructions" type="bool" access="both">
    ///     <summary>Determines if the XmlReader will ignore processing instructions.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.ignoreprocessinginstructions?view=netframework-4.7</source>
    /// </property>
    /// <property name="ignore_whitespace" type="bool" access="both">
    ///     <summary>Determines if the XmlReader will ignore whitespace.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.ignorewhitespace?view=netframework-4.7</source>
    /// </property>
    /// <property name="line_number_offset" type="number" access="both">
    ///     <summary>Determines the line number offset of the XmlReader.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.linenumberoffset?view=netframework-4.7</source>
    /// </property>
    /// <property name="line_position_offset" type="number" access="both">
    ///     <summary>Determines the line position offset of the XmlReader.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.linepositionoffset?view=netframework-4.7</source>
    /// </property>
    /// <property name="max_characters_from_entities" type="number" access="both">
    ///     <summary>Determines the maximum allowable number of characters in a document that result from expanded entities.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.maxcharactersfromentities?view=netframework-4.7</source>
    /// </property>
    /// <property name="max_characters_in_document" type="number" access="both">
    ///     <summary>Gets the maximum allowable number characters in an XML document. A value of zero means there is no limit.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.maxcharactersindocument?view=netframework-4.7</source>
    /// </property>
    /// <property name="validation_flags" type="[XmlSchemaValidationFlags](https://docs.microsoft.com/en-us/dotnet/api/system.xml.schema.xmlschemavalidationflags?view=netframework-4.7)" access="both">
    ///     <summary>Determines the schema validation settings.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.validationflags?view=netframework-4.7</source>
    /// </property>
    /// <property name="validation_type" type="[ValidationType](https://docs.microsoft.com/en-us/dotnet/api/system.xml.validationtype?view=netframework-4.7)" access="both">
    ///     <summary>Determines if the XmlReader will perform validation or type assignment when reading.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.validationtype?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject]
    public class XmlReaderSettings : ITsInstance
    {
        public System.Xml.XmlReaderSettings Settings { get; }

        public string ObjectType => "TaffyScript.Xml.XmlReaderSettings";

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public XmlReaderSettings(TsObject[] args)
        {
            Settings = new System.Xml.XmlReaderSettings();
        }

        public XmlReaderSettings(System.Xml.XmlReaderSettings settings)
        {
            Settings = settings;
        }

        public TsObject GetMember(string name)
        {
            switch(name)
            {
                case "check_characters":
                    return Settings.CheckCharacters;
                case "close_input":
                    return Settings.CloseInput;
                case "conformance_level":
                    return (float)Settings.ConformanceLevel;
                case "dtd_processing":
                    return (float)Settings.DtdProcessing;
                case "ignore_comments":
                    return Settings.IgnoreComments;
                case "ignore_processing_instructions":
                    return Settings.IgnoreProcessingInstructions;
                case "ignore_whitespace":
                    return Settings.IgnoreWhitespace;
                case "line_number_offset":
                    return Settings.LineNumberOffset;
                case "line_number_position":
                    return Settings.LinePositionOffset;
                case "max_characters_from_entities":
                    return Settings.MaxCharactersFromEntities;
                case "max_characters_in_document":
                    return Settings.MaxCharactersInDocument;
                case "validation_flags":
                    return (float)Settings.ValidationFlags;
                case "validation_type":
                    return (float)Settings.ValidationType;
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
                case "check_characters":
                    Settings.CheckCharacters = (bool)value;
                    break;
                case "close_input":
                    Settings.CloseInput = (bool)value;
                    break;
                case "conformance_level":
                    Settings.ConformanceLevel = (ConformanceLevel)(float)value;
                    break;
                case "dtd_processing":
                    Settings.DtdProcessing = (DtdProcessing)(float)value;
                    break;
                case "ignore_comments":
                    Settings.IgnoreComments = (bool)value;
                    break;
                case "ignore_processing_instructions":
                    Settings.IgnoreProcessingInstructions = (bool)value;
                    break;
                case "ignore_whitespace":
                    Settings.IgnoreWhitespace = (bool)value;
                    break;
                case "line_number_offset":
                    Settings.LineNumberOffset = (int)value;
                    break;
                case "line_number_position":
                    Settings.LinePositionOffset = (int)value;
                    break;
                case "max_characters_from_entities":
                    Settings.MaxCharactersFromEntities = (int)value;
                    break;
                case "max_characters_in_document":
                    Settings.MaxCharactersInDocument = (int)value;
                    break;
                case "validation_flags":
                    Settings.ValidationFlags = (System.Xml.Schema.XmlSchemaValidationFlags)(float)value;
                    break;
                case "validation_type":
                    Settings.ValidationType = (ValidationType)(float)value;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            switch(delegateName)
            {
                case "clone":
                    del = new TsDelegate(clone, "clone");
                    return true;
                case "reset":
                    del = new TsDelegate(reset, "reset");
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            if (TryGetDelegate(delegateName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, delegateName);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "clone":
                    return clone(args);
                case "reset":
                    return reset(args);
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        /// <summary>
        /// Creates a copy of this instance.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.clone</source>
        /// <returns>[XmlReaderSettings]({{site.baseurl}}/docs/TaffyScript/IO/)</returns>
        public TsObject clone(TsObject[] args)
        {
            return new XmlReaderSettings(Settings.Clone());
        }

        /// <summary>
        /// Resets the members of this instance to their default values.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.reset</source>
        /// <returns>null</returns>
        public TsObject reset(TsObject[] args)
        {
            Settings.Reset();
            return TsObject.Empty;
        } 

        public static implicit operator TsObject(XmlReaderSettings settings)
        {
            return new TsInstanceWrapper(settings);
        }

        public static explicit operator XmlReaderSettings(TsObject obj)
        {
            return (XmlReaderSettings)obj.WeakValue;
        }
    }
}
