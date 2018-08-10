using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TaffyScript.Xml
{
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
            throw new NotImplementedException();
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

        public TsObject clone(TsObject[] args)
        {
            return new XmlReaderSettings(Settings.Clone());
        }

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
