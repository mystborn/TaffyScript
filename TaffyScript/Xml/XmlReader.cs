using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using InternalReader = System.Xml.XmlReader;

namespace TaffyScript.Xml
{
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

        public XmlReader(TsObject[] args)
        {
            Source = args.Length == 1 ? InternalReader.Create((string)args[0]) : InternalReader.Create((string)args[0], ((XmlReaderSettings)args[1]).Settings);
        }

        public XmlReader(System.Xml.XmlReader reader)
        {
            Source = reader;
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "close":
                    return close(args);
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
                case "read_content_as_number":
                    return read_content_as_number(args);
                case "read_content_as_string":
                    return read_content_as_string(args);
                case "read_element_content_as_bool":
                    return read_element_content_as_bool(args);
                case "read_element_content_as_number":
                    return read_element_content_as_number(args);
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
                    throw new MissingMemberException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string delegateName)
        {
            throw new NotImplementedException();
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

        public bool TryGetDelegate(string delegateName, out TsDelegate del)
        {
            switch (delegateName)
            {
                case "close":
                    del = new TsDelegate(close, "close");
                    return true;
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
                    del = new TsDelegate(move_to_first_attribute, "move_to_first_attribute");
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
                case "read_content_as_number":
                    del = new TsDelegate(read_content_as_number, "read_content_as_number");
                    return true;
                case "read_content_as_string":
                    del = new TsDelegate(read_content_as_string, "read_content_as_string");
                    return true;
                case "read_element_content_as_bool":
                    del = new TsDelegate(read_element_content_as_bool, "read_element_content_as_bool");
                    return true;
                case "read_element_content_as_number":
                    del = new TsDelegate(read_element_content_as_number, "read_element_content_as_number");
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

        public TsObject get(TsObject[] args)
        {
            return args[0].Type == VariableType.Real ? Source.GetAttribute((int)args[0]) : Source.GetAttribute((string)args[0]);
        }

        public TsObject get_attribute(TsObject[] args)
        {
            return args[0].Type == VariableType.Real ? Source.GetAttribute((int)args[0]) : Source.GetAttribute((string)args[0]);
        }

        public TsObject is_start_element(TsObject[] args)
        {
            return args is null || args.Length == 0 ? Source.IsStartElement() : Source.IsStartElement((string)args[0]);
        }

        public TsObject lookup_namespace(TsObject[] args)
        {
            return Source.LookupNamespace((string)args[0]);
        }

        public TsObject move_to_attribute(TsObject[] args)
        {
            if (args[0].Type == VariableType.Real)
                Source.MoveToAttribute((int)args[0]);
            else
                return Source.MoveToAttribute((string)args[0]);

            return true;
        }

        public TsObject move_to_content(TsObject[] args)
        {
            return (float)Source.MoveToContent();
        }

        public TsObject move_to_element(TsObject[] args)
        {
            return Source.MoveToElement();
        }

        public TsObject move_to_first_attribute(TsObject[] args)
        {
            return Source.MoveToFirstAttribute();
        }

        public TsObject move_to_next_attribute(TsObject[] args)
        {
            return Source.MoveToNextAttribute();
        }

        public TsObject read(TsObject[] args)
        {
            return Source.Read();
        }

        public TsObject read_attribute_value(TsObject[] args)
        {
            return Source.ReadAttributeValue();
        }

        public TsObject read_content_as_bool(TsObject[] args)
        {
            return Source.ReadContentAsBoolean();
        }

        public TsObject read_content_as_number(TsObject[] args)
        {
            return Source.ReadContentAsFloat();
        }

        public TsObject read_content_as_string(TsObject[] args)
        {
            return Source.ReadContentAsString();
        }

        public TsObject read_element_content_as_bool(TsObject[] args)
        {
            return Source.ReadElementContentAsBoolean();
        }

        public TsObject read_element_content_as_number(TsObject[] args)
        {
            return Source.ReadElementContentAsFloat();
        }

        public TsObject read_element_content_as_string(TsObject[] args)
        {
            return Source.ReadElementContentAsString();
        }

        public TsObject read_end_element(TsObject[] args)
        {
            Source.ReadEndElement();
            return TsObject.Empty;
        }

        public TsObject read_inner_xml(TsObject[] args)
        {
            return Source.ReadInnerXml();
        }

        public TsObject read_outer_xml(TsObject[] args)
        {
            return Source.ReadOuterXml();
        }

        public TsObject read_start_element(TsObject[] args)
        {
            Source.ReadStartElement();
            return TsObject.Empty;
        }

        public TsObject read_subtree(TsObject[] args)
        {
            return new XmlReader(Source.ReadSubtree());
        }

        public TsObject read_to_descendant(TsObject[] args)
        {
            return args.Length == 1 ? Source.ReadToDescendant((string)args[0]) : Source.ReadToDescendant((string)args[0], (string)args[1]);
        }

        public TsObject read_to_following(TsObject[] args)
        {
            return args.Length == 1 ? Source.ReadToFollowing((string)args[0]) : Source.ReadToFollowing((string)args[0], (string)args[1]);
        }

        public TsObject read_to_next_sibling(TsObject[] args)
        {
            return args.Length == 1 ? Source.ReadToNextSibling((string)args[0]) : Source.ReadToNextSibling((string)args[0], (string)args[1]);
        }

        public TsObject resolve_entity(TsObject[] args)
        {
            Source.ResolveEntity();
            return TsObject.Empty;
        }

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
