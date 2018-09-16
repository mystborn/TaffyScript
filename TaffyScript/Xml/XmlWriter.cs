using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TaffyScript.Xml
{
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
                case "close":
                    return close(args);
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
                case "write_value":
                    return write_value(args);
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
                case "close":
                    del = new TsDelegate(close, scriptName);
                    break;
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
                case "write_value":
                    del = new TsDelegate(write_value, scriptName);
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

        public TsObject close(TsObject[] args)
        {
            Source.Close();
            return TsObject.Empty;
        }

        public TsObject dispose(TsObject[] args)
        {
            Source.Dispose();
            return TsObject.Empty;
        }

        public TsObject flush(TsObject[] args)
        {
            Source.Flush();
            return TsObject.Empty;
        }

        public TsObject lookup_prefix(TsObject[] args)
        {
            return Source.LookupPrefix((string)args[0]);
        }

        public TsObject write_attributes(TsObject[] args)
        {
            Source.WriteAttributes(((XmlReader)args[0]).Source, (bool)args[1]);
            return TsObject.Empty;
        }

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

        public TsObject write_cdata(TsObject[] args)
        {
            Source.WriteCData((string)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_comment(TsObject[] args)
        {
            Source.WriteComment((string)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_doc_type(TsObject[] args)
        {
            Source.WriteDocType((string)args[0], args[1].GetStringOrNull(), args[2].GetStringOrNull(), args[3].GetStringOrNull());
            return TsObject.Empty;
        }

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

        public TsObject write_end_attribute(TsObject[] args)
        {
            Source.WriteEndAttribute();
            return TsObject.Empty;
        }

        public TsObject write_end_document(TsObject[] args)
        {
            Source.WriteEndDocument();
            return TsObject.Empty;
        }

        public TsObject write_end_element(TsObject[] args)
        {
            Source.WriteEndElement();
            return TsObject.Empty;
        }

        public TsObject write_entity_ref(TsObject[] args)
        {
            Source.WriteEntityRef((string)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_full_end_element(TsObject[] args)
        {
            Source.WriteFullEndElement();
            return TsObject.Empty;
        }

        public TsObject write_name(TsObject[] args)
        {
            Source.WriteName((string)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_nm_token(TsObject[] args)
        {
            Source.WriteNmToken((string)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_node(TsObject[] args)
        {
            Source.WriteNode(((XmlReader)args[0]).Source, (bool)args[1]);
            return TsObject.Empty;
        }

        public TsObject write_processing_instruction(TsObject[] args)
        {
            Source.WriteProcessingInstruction((string)args[0], args[1].GetStringOrNull());
            return TsObject.Empty;
        }

        public TsObject write_qualified_name(TsObject[] args)
        {
            Source.WriteQualifiedName((string)args[0], args[1].GetStringOrNull());
            return TsObject.Empty;
        }

        public TsObject write_raw(TsObject[] args)
        {
            Source.WriteRaw((string)args[0]);
            return TsObject.Empty;
        }

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

        public TsObject write_string(TsObject[] args)
        {
            Source.WriteString(args[0].GetStringOrNull());
            return TsObject.Empty;
        }

        public TsObject write_value(TsObject[] args)
        {
            Source.WriteValue(args[0].WeakValue);
            return TsObject.Empty;
        }

        public TsObject write_value_bool(TsObject[] args)
        {
            Source.WriteValue((bool)args[0]);
            return TsObject.Empty;
        }

        // Todo: XmlWriter.write_value_date_time
        public TsObject write_value_date_time(TsObject[] args)
        {
            throw new NotImplementedException();
        }

        // Todo: XmlWriter.write_value_date_time_offset
        public TsObject write_value_date_time_offset(TsObject[] args)
        {
            throw new NotImplementedException();
        }

        public TsObject write_value_decimal(TsObject[] args)
        {
            Source.WriteValue((decimal)args[0].GetNumber());
            return TsObject.Empty;
        }

        public TsObject write_value_double(TsObject[] args)
        {
            Source.WriteValue((double)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_value_float(TsObject[] args)
        {
            Source.WriteValue((float)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_value_int(TsObject[] args)
        {
            Source.WriteValue((int)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_value_long(TsObject[] args)
        {
            Source.WriteValue((long)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_value_string(TsObject[] args)
        {
            Source.WriteValue((string)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_whitespace(TsObject[] args)
        {
            Source.WriteWhitespace((string)args[0]);
            return TsObject.Empty;
        }

        public static implicit operator TsObject(XmlWriter writer) => new TsInstanceWrapper(writer);
        public static explicit operator XmlWriter(TsObject obj) => (XmlWriter)obj.WeakValue;
    }
}
