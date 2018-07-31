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
                    return close(null, args);
                case "dispose":
                    return dispose(null, args);
                case "get":
                    return get(null, args);
                case "get_attribute":
                    return get_attribute(null, args);
                case "is_start_element":
                    return is_start_element(null, args);
                case "lookup_namespace":
                    return lookup_namespace(null, args);
                case "move_to_attribute":
                    return move_to_attribute(null, args);
                case "move_to_content":
                    return move_to_content(null, args);
                case "move_to_element":
                    return move_to_element(null, args);
                case "move_to_first_attribute":
                    return move_to_first_attribute(null, args);
                case "move_to_next_attribute":
                    return move_to_next_attribute(null, args);
                case "read":
                    return read(null, args);
                case "read_attribute_value":
                    return read_attribute_value(null, args);
                case "read_content_as_bool":
                    return read_content_as_bool(null, args);
                case "read_content_as_number":
                    return read_content_as_number(null, args);
                case "read_content_as_string":
                    return read_content_as_string(null, args);
                case "read_element_content_as_bool":
                    return read_element_content_as_bool(null, args);
                case "read_element_content_as_number":
                    return read_element_content_as_number(null, args);
                case "read_element_content_as_string":
                    return read_element_content_as_string(null, args);
                case "read_end_element":
                    return read_end_element(null, args);
                case "read_inner_xml":
                    return read_inner_xml(null, args);
                case "read_outer_xml":
                    return read_outer_xml(null, args);
                case "read_start_element":
                    return read_start_element(null, args);
                case "read_subtree":
                    return read_subtree(null, args);
                case "read_to_descendant":
                    return read_to_descendant(null, args);
                case "read_to_following":
                    return read_to_following(null, args);
                case "read_to_next_sibling":
                    return read_to_next_sibling(null, args);
                case "resolve_entity":
                    return resolve_entity(null, args);
                case "skip":
                    return skip(null, args);
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
                    del = new TsDelegate(close, "close", this);
                    return true;
                case "dispose":
                    del = new TsDelegate(dispose, "dispose", this);
                    return true;
                case "get":
                    del = new TsDelegate(get, "get", this);
                    return true;
                case "get_attribute":
                    del = new TsDelegate(get_attribute, "get_attribute", this);
                    return true;
                case "is_start_element":
                    del = new TsDelegate(is_start_element, "is_start_element", this);
                    return true;
                case "lookup_namespace":
                    del = new TsDelegate(lookup_namespace, "lookup_namespace", this);
                    return true;
                case "move_to_attribute":
                    del = new TsDelegate(move_to_attribute, "move_to_attribute", this);
                    return true;
                case "move_to_content":
                    del = new TsDelegate(move_to_content, "move_to_content", this);
                    return true;
                case "move_to_element":
                    del = new TsDelegate(move_to_element, "move_to_element", this);
                    return true;
                case "move_to_first_attribute":
                    del = new TsDelegate(move_to_first_attribute, "move_to_first_attribute", this);
                    return true;
                case "move_to_next_attribute":
                    del = new TsDelegate(move_to_next_attribute, "move_to_next_attribute", this);
                    return true;
                case "read":
                    del = new TsDelegate(read, "read", this);
                    return true;
                case "read_attribute_value":
                    del = new TsDelegate(read_attribute_value, "read_attribute_value", this);
                    return true;
                case "read_content_as_bool":
                    del = new TsDelegate(read_content_as_bool, "read_content_as_bool", this);
                    return true;
                case "read_content_as_number":
                    del = new TsDelegate(read_content_as_number, "read_content_as_number", this);
                    return true;
                case "read_content_as_string":
                    del = new TsDelegate(read_content_as_string, "read_content_as_string", this);
                    return true;
                case "read_element_content_as_bool":
                    del = new TsDelegate(read_element_content_as_bool, "read_element_content_as_bool", this);
                    return true;
                case "read_element_content_as_number":
                    del = new TsDelegate(read_element_content_as_number, "read_element_content_as_number", this);
                    return true;
                case "read_element_content_as_string":
                    del = new TsDelegate(read_element_content_as_string, "read_element_content_as_string", this);
                    return true;
                case "read_end_element":
                    del = new TsDelegate(read_end_element, "read_end_element", this);
                    return true;
                case "read_inner_xml":
                    del = new TsDelegate(read_inner_xml, "read_inner_xml", this);
                    return true;
                case "read_outer_xml":
                    del = new TsDelegate(read_outer_xml, "read_outer_xml", this);
                    return true;
                case "read_start_element":
                    del = new TsDelegate(read_start_element, "read_start_element", this);
                    return true;
                case "read_subtree":
                    del = new TsDelegate(read_subtree, "read_subtree", this);
                    return true;
                case "read_to_descendant":
                    del = new TsDelegate(read_to_descendant, "read_to_descendant", this);
                    return true;
                case "read_to_following":
                    del = new TsDelegate(read_to_following, "read_to_following", this);
                    return true;
                case "read_to_next_sibling":
                    del = new TsDelegate(read_to_next_sibling, "read_to_next_sibling", this);
                    return true;
                case "resolve_entity":
                    del = new TsDelegate(resolve_entity, "resolve_entity", this);
                    return true;
                case "skip":
                    del = new TsDelegate(skip, "skip", this);
                    return true;
                default:
                    del = null;
                    return false;
            }
        }

        public TsObject close(ITsInstance inst, TsObject[] args)
        {
            Source.Close();
            return TsObject.Empty;
        }

        public TsObject dispose(ITsInstance inst, TsObject[] args)
        {
            Source.Dispose();
            return TsObject.Empty;
        }

        public TsObject get(ITsInstance inst, TsObject[] args)
        {
            return args[0].Type == VariableType.Real ? Source.GetAttribute((int)args[0]) : Source.GetAttribute((string)args[0]);
        }

        public TsObject get_attribute(ITsInstance inst, TsObject[] args)
        {
            return args[0].Type == VariableType.Real ? Source.GetAttribute((int)args[0]) : Source.GetAttribute((string)args[0]);
        }

        public TsObject is_start_element(ITsInstance inst, TsObject[] args)
        {
            return args is null || args.Length == 0 ? Source.IsStartElement() : Source.IsStartElement((string)args[0]);
        }

        public TsObject lookup_namespace(ITsInstance inst, TsObject[] args)
        {
            return Source.LookupNamespace((string)args[0]);
        }

        public TsObject move_to_attribute(ITsInstance inst, TsObject[] args)
        {
            if (args[0].Type == VariableType.Real)
                Source.MoveToAttribute((int)args[0]);
            else
                return Source.MoveToAttribute((string)args[0]);

            return true;
        }

        public TsObject move_to_content(ITsInstance inst, TsObject[] args)
        {
            return (float)Source.MoveToContent();
        }

        public TsObject move_to_element(ITsInstance inst, TsObject[] args)
        {
            return Source.MoveToElement();
        }

        public TsObject move_to_first_attribute(ITsInstance inst, TsObject[] args)
        {
            return Source.MoveToFirstAttribute();
        }

        public TsObject move_to_next_attribute(ITsInstance inst, TsObject[] args)
        {
            return Source.MoveToNextAttribute();
        }

        public TsObject read(ITsInstance inst, TsObject[] args)
        {
            return Source.Read();
        }

        public TsObject read_attribute_value(ITsInstance inst, TsObject[] args)
        {
            return Source.ReadAttributeValue();
        }

        public TsObject read_content_as_bool(ITsInstance inst, TsObject[] args)
        {
            return Source.ReadContentAsBoolean();
        }

        public TsObject read_content_as_number(ITsInstance inst, TsObject[] args)
        {
            return Source.ReadContentAsFloat();
        }

        public TsObject read_content_as_string(ITsInstance inst, TsObject[] args)
        {
            return Source.ReadContentAsString();
        }

        public TsObject read_element_content_as_bool(ITsInstance inst, TsObject[] args)
        {
            return Source.ReadElementContentAsBoolean();
        }

        public TsObject read_element_content_as_number(ITsInstance inst, TsObject[] args)
        {
            return Source.ReadElementContentAsFloat();
        }

        public TsObject read_element_content_as_string(ITsInstance inst, TsObject[] args)
        {
            return Source.ReadElementContentAsString();
        }

        public TsObject read_end_element(ITsInstance inst, TsObject[] args)
        {
            Source.ReadEndElement();
            return TsObject.Empty;
        }

        public TsObject read_inner_xml(ITsInstance inst, TsObject[] args)
        {
            return Source.ReadInnerXml();
        }

        public TsObject read_outer_xml(ITsInstance inst, TsObject[] args)
        {
            return Source.ReadOuterXml();
        }

        public TsObject read_start_element(ITsInstance inst, TsObject[] args)
        {
            Source.ReadStartElement();
            return TsObject.Empty;
        }

        public TsObject read_subtree(ITsInstance inst, TsObject[] args)
        {
            return new XmlReader(Source.ReadSubtree());
        }

        public TsObject read_to_descendant(ITsInstance inst, TsObject[] args)
        {
            return args.Length == 1 ? Source.ReadToDescendant((string)args[0]) : Source.ReadToDescendant((string)args[0], (string)args[1]);
        }

        public TsObject read_to_following(ITsInstance inst, TsObject[] args)
        {
            return args.Length == 1 ? Source.ReadToFollowing((string)args[0]) : Source.ReadToFollowing((string)args[0], (string)args[1]);
        }

        public TsObject read_to_next_sibling(ITsInstance inst, TsObject[] args)
        {
            return args.Length == 1 ? Source.ReadToNextSibling((string)args[0]) : Source.ReadToNextSibling((string)args[0], (string)args[1]);
        }

        public TsObject resolve_entity(ITsInstance inst, TsObject[] args)
        {
            Source.ResolveEntity();
            return TsObject.Empty;
        }

        public TsObject skip(ITsInstance inst, TsObject[] args)
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
