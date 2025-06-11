// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.ParseRdf
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public static class ParseRdf
{
  public const string DefaultPrefix = "_dflt";

  internal static XmpMeta Parse(XElement xmlRoot, ParseOptions options)
  {
    XmpMeta xmp = new XmpMeta();
    ParseRdf.Rdf_RDF(xmp, xmlRoot, options);
    return xmp;
  }

  private static void Rdf_RDF(XmpMeta xmp, XElement rdfRdfNode, ParseOptions options)
  {
    if (!rdfRdfNode.Attributes().Any<XAttribute>())
      throw new XmpException("Invalid attributes of rdf:RDF element", XmpErrorCode.BadRdf);
    ParseRdf.Rdf_NodeElementList(xmp, xmp.GetRoot(), rdfRdfNode, options);
  }

  private static void Rdf_NodeElementList(
    XmpMeta xmp,
    XmpNode xmpParent,
    XElement rdfRdfNode,
    ParseOptions options)
  {
    foreach (XNode node in rdfRdfNode.Nodes())
    {
      if (node is XElement xmlNode)
        ParseRdf.Rdf_NodeElement(xmp, xmpParent, xmlNode, true, options);
    }
  }

  private static void Rdf_NodeElement(
    XmpMeta xmp,
    XmpNode xmpParent,
    XElement xmlNode,
    bool isTopLevel,
    ParseOptions options)
  {
    RdfTerm rdfTermKind = ParseRdf.GetRdfTermKind(xmlNode);
    switch (rdfTermKind)
    {
      case RdfTerm.Other:
      case RdfTerm.Description:
        if (isTopLevel && rdfTermKind == RdfTerm.Other)
          throw new XmpException("Top level typed node not allowed", XmpErrorCode.BadXmp);
        ParseRdf.Rdf_NodeElementAttrs(xmp, xmpParent, xmlNode, isTopLevel, options);
        ParseRdf.Rdf_PropertyElementList(xmp, xmpParent, xmlNode, isTopLevel, options);
        break;
      default:
        throw new XmpException("Node element must be rdf:Description or typed node", XmpErrorCode.BadRdf);
    }
  }

  private static void Rdf_NodeElementAttrs(
    XmpMeta xmp,
    XmpNode xmpParent,
    XElement xmlNode,
    bool isTopLevel,
    ParseOptions options)
  {
    int num = 0;
    foreach (XAttribute attribute in xmlNode.Attributes())
    {
      switch (xmlNode.GetPrefixOfNamespace(attribute.Name.Namespace))
      {
        case "xmlns":
          continue;
        case null:
          if (attribute.Name == (XName) "xmlns")
            continue;
          break;
      }
      RdfTerm rdfTermKind = ParseRdf.GetRdfTermKind(attribute);
      switch (rdfTermKind)
      {
        case RdfTerm.Other:
          ParseRdf.AddChildNode(xmp, xmpParent, attribute, attribute.Value, isTopLevel);
          continue;
        case RdfTerm.Id:
        case RdfTerm.About:
        case RdfTerm.NodeId:
          if (num > 0)
            throw new XmpException("Mutally exclusive about, ID, nodeID attributes", XmpErrorCode.BadRdf);
          ++num;
          if (isTopLevel && rdfTermKind == RdfTerm.About)
          {
            if (!string.IsNullOrEmpty(xmpParent.Name))
            {
              if (attribute.Value != xmpParent.Name)
                throw new XmpException("Mismatched top level rdf:about values", XmpErrorCode.BadXmp);
              continue;
            }
            xmpParent.Name = attribute.Value;
            continue;
          }
          continue;
        default:
          throw new XmpException("Invalid nodeElement attribute", XmpErrorCode.BadRdf);
      }
    }
  }

  private static void Rdf_PropertyElementList(
    XmpMeta xmp,
    XmpNode xmpParent,
    XElement xmlParent,
    bool isTopLevel,
    ParseOptions options)
  {
    int num = 0;
    foreach (XNode node in xmlParent.Nodes())
    {
      if (!ParseRdf.IsWhitespaceNode(node) && node.NodeType != XmlNodeType.Comment)
      {
        if (node.NodeType != XmlNodeType.Element)
          throw new XmpException("Expected property element node not found", XmpErrorCode.BadRdf);
        if (xmpParent.Options.IsArrayLimited && num > xmpParent.Options.ArrayElementsLimit)
          break;
        ParseRdf.Rdf_PropertyElement(xmp, xmpParent, (XElement) node, isTopLevel, options);
        ++num;
      }
    }
  }

  private static void Rdf_PropertyElement(
    XmpMeta xmp,
    XmpNode xmpParent,
    XElement xmlNode,
    bool isTopLevel,
    ParseOptions options)
  {
    List<XAttribute> source = ParseRdf.IsPropertyElementName(ParseRdf.GetRdfTermKind(xmlNode)) ? xmlNode.Attributes().ToList<XAttribute>() : throw new XmpException("Invalid property element name", XmpErrorCode.BadRdf);
    List<string> ignoreNodes = new List<string>();
    foreach (XAttribute xattribute in source)
    {
      switch (xmlNode.GetPrefixOfNamespace(xattribute.Name.Namespace))
      {
        case "xmlns":
          ignoreNodes.Add(xattribute.Name.ToString());
          continue;
        case null:
          if (!(xattribute.Name == (XName) "xmlns"))
            continue;
          goto case "xmlns";
        default:
          continue;
      }
    }
    if (source.Count - ignoreNodes.Count > 3)
    {
      ParseRdf.Rdf_EmptyPropertyElement(xmp, xmpParent, xmlNode, isTopLevel);
    }
    else
    {
      XAttribute xattribute = source.Select(attribute => new
      {
        attribute = attribute,
        attrLocal = attribute.Name.LocalName
      }).Select(_param1 => new
      {
        \u003C\u003Eh__TransparentIdentifier0 = _param1,
        attrNs = _param1.attribute.Name.NamespaceName
      }).Where(_param1 => "xml:" + _param1.\u003C\u003Eh__TransparentIdentifier0.attrLocal != "xml:lang" && (!(_param1.\u003C\u003Eh__TransparentIdentifier0.attrLocal == "ID") || !(_param1.attrNs == "http://www.w3.org/1999/02/22-rdf-syntax-ns#")) && !ignoreNodes.Contains(_param1.\u003C\u003Eh__TransparentIdentifier0.attribute.Name.ToString())).Select(_param1 => _param1.\u003C\u003Eh__TransparentIdentifier0.attribute).FirstOrDefault<XAttribute>();
      if (xattribute != null)
      {
        string localName = xattribute.Name.LocalName;
        string namespaceName = xattribute.Name.NamespaceName;
        string str = xattribute.Value;
        if (localName == "datatype" && namespaceName == "http://www.w3.org/1999/02/22-rdf-syntax-ns#")
          ParseRdf.Rdf_LiteralPropertyElement(xmp, xmpParent, xmlNode, isTopLevel);
        else if (!(localName == "parseType") || !(namespaceName == "http://www.w3.org/1999/02/22-rdf-syntax-ns#"))
        {
          ParseRdf.Rdf_EmptyPropertyElement(xmp, xmpParent, xmlNode, isTopLevel);
        }
        else
        {
          switch (str)
          {
            case "Literal":
              ParseRdf.Rdf_ParseTypeLiteralPropertyElement();
              break;
            case "Resource":
              ParseRdf.Rdf_ParseTypeResourcePropertyElement(xmp, xmpParent, xmlNode, isTopLevel, options);
              break;
            case "Collection":
              ParseRdf.Rdf_ParseTypeCollectionPropertyElement();
              break;
            default:
              ParseRdf.Rdf_ParseTypeOtherPropertyElement();
              break;
          }
        }
      }
      else if (xmlNode.IsEmpty)
        ParseRdf.Rdf_EmptyPropertyElement(xmp, xmpParent, xmlNode, isTopLevel);
      else if (xmlNode.Nodes().FirstOrDefault<XNode>((Func<XNode, bool>) (t => t.NodeType != XmlNodeType.Text)) == null)
        ParseRdf.Rdf_LiteralPropertyElement(xmp, xmpParent, xmlNode, isTopLevel);
      else
        ParseRdf.Rdf_ResourcePropertyElement(xmp, xmpParent, xmlNode, isTopLevel, options);
    }
  }

  private static void Rdf_ResourcePropertyElement(
    XmpMeta xmp,
    XmpNode xmpParent,
    XElement xmlNode,
    bool isTopLevel,
    ParseOptions options)
  {
    if (isTopLevel && xmlNode.Name == XName.Get("changes", "iX"))
      return;
    XmpNode xmpNode = ParseRdf.AddChildNode(xmp, xmpParent, xmlNode, string.Empty, isTopLevel);
    foreach (XAttribute attribute in xmlNode.Attributes())
    {
      switch (xmlNode.GetPrefixOfNamespace(attribute.Name.Namespace))
      {
        case "xmlns":
          continue;
        case null:
          if (attribute.Name == (XName) "xmlns")
            continue;
          break;
      }
      string localName = attribute.Name.LocalName;
      string namespaceName = attribute.Name.NamespaceName;
      if ("xml:" + attribute.Name.LocalName == "xml:lang")
        ParseRdf.AddQualifierNode(xmpNode, "xml:lang", attribute.Value);
      else if (!(localName == "ID") || !(namespaceName == "http://www.w3.org/1999/02/22-rdf-syntax-ns#"))
        throw new XmpException("Invalid attribute for resource property element", XmpErrorCode.BadRdf);
    }
    bool flag1 = false;
    foreach (XNode node in xmlNode.Nodes())
    {
      if (!ParseRdf.IsWhitespaceNode(node))
      {
        if (node.NodeType != XmlNodeType.Element | flag1)
        {
          if (flag1)
            throw new XmpException("Invalid child of resource property element", XmpErrorCode.BadRdf);
          throw new XmpException("Children of resource property element must be XML elements", XmpErrorCode.BadRdf);
        }
        XElement xmlNode1 = (XElement) node;
        bool flag2 = xmlNode1.Name.NamespaceName == "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        string localName = xmlNode1.Name.LocalName;
        if (flag2 && localName == "Bag")
          xmpNode.Options.IsArray = true;
        else if (flag2 && localName == "Seq")
        {
          xmpNode.Options.IsArray = true;
          xmpNode.Options.IsArrayOrdered = true;
        }
        else if (flag2 && localName == "Alt")
        {
          xmpNode.Options.IsArray = true;
          xmpNode.Options.IsArrayOrdered = true;
          xmpNode.Options.IsArrayAlternate = true;
        }
        else
        {
          xmpNode.Options.IsStruct = true;
          if (!flag2 && localName != "Description")
          {
            string str = $"{xmlNode1.Name.NamespaceName}:{localName}";
            ParseRdf.AddQualifierNode(xmpNode, "rdf:type", str);
          }
        }
        int arrayLimit;
        if (xmpNode.Options.IsArray && options.GetXMPNodesToLimit().TryGetValue(xmpNode.Name, out arrayLimit))
          xmpNode.Options.SetArrayElementLimit(arrayLimit);
        ParseRdf.Rdf_NodeElement(xmp, xmpNode, xmlNode1, false, options);
        if (xmpNode.HasValueChild)
          ParseRdf.FixupQualifiedNode(xmpNode);
        else if (xmpNode.Options.IsArrayAlternate)
          XmpNodeUtils.DetectAltText(xmpNode);
        flag1 = true;
      }
    }
    if (!flag1)
      throw new XmpException("Missing child of resource property element", XmpErrorCode.BadRdf);
  }

  private static void Rdf_LiteralPropertyElement(
    XmpMeta xmp,
    XmpNode xmpParent,
    XElement xmlNode,
    bool isTopLevel)
  {
    XmpNode xmpParent1 = ParseRdf.AddChildNode(xmp, xmpParent, xmlNode, (string) null, isTopLevel);
    foreach (XAttribute attribute in xmlNode.Attributes())
    {
      switch (xmlNode.GetPrefixOfNamespace(attribute.Name.Namespace))
      {
        case "xmlns":
          continue;
        case null:
          if (attribute.Name == (XName) "xmlns")
            continue;
          break;
      }
      string namespaceName = attribute.Name.NamespaceName;
      string localName = attribute.Name.LocalName;
      if ("xml:" + attribute.Name.LocalName == "xml:lang")
        ParseRdf.AddQualifierNode(xmpParent1, "xml:lang", attribute.Value);
      else if (!(namespaceName == "http://www.w3.org/1999/02/22-rdf-syntax-ns#") || !(localName == "ID") && !(localName == "datatype"))
        throw new XmpException("Invalid attribute for literal property element", XmpErrorCode.BadRdf);
    }
    StringBuilder stringBuilder = new StringBuilder();
    foreach (XNode node in xmlNode.Nodes())
    {
      if (node.NodeType != XmlNodeType.Text)
        throw new XmpException("Invalid child of literal property element", XmpErrorCode.BadRdf);
      stringBuilder.Append(((XText) node).Value);
    }
    xmpParent1.Value = stringBuilder.ToString();
  }

  private static void Rdf_ParseTypeLiteralPropertyElement()
  {
    throw new XmpException("ParseTypeLiteral property element not allowed", XmpErrorCode.BadXmp);
  }

  private static void Rdf_ParseTypeResourcePropertyElement(
    XmpMeta xmp,
    XmpNode xmpParent,
    XElement xmlNode,
    bool isTopLevel,
    ParseOptions options)
  {
    XmpNode xmpParent1 = ParseRdf.AddChildNode(xmp, xmpParent, xmlNode, string.Empty, isTopLevel);
    xmpParent1.Options.IsStruct = true;
    foreach (XAttribute attribute in xmlNode.Attributes())
    {
      switch (xmlNode.GetPrefixOfNamespace(attribute.Name.Namespace))
      {
        case "xmlns":
          continue;
        case null:
          if (attribute.Name == (XName) "xmlns")
            continue;
          break;
      }
      string localName = attribute.Name.LocalName;
      string namespaceName = attribute.Name.NamespaceName;
      if ("xml:" + attribute.Name.LocalName == "xml:lang")
        ParseRdf.AddQualifierNode(xmpParent1, "xml:lang", attribute.Value);
      else if (!(namespaceName == "http://www.w3.org/1999/02/22-rdf-syntax-ns#") || !(localName == "ID") && !(localName == "parseType"))
        throw new XmpException("Invalid attribute for ParseTypeResource property element", XmpErrorCode.BadRdf);
    }
    ParseRdf.Rdf_PropertyElementList(xmp, xmpParent1, xmlNode, false, options);
    if (!xmpParent1.HasValueChild)
      return;
    ParseRdf.FixupQualifiedNode(xmpParent1);
  }

  private static void Rdf_ParseTypeCollectionPropertyElement()
  {
    throw new XmpException("ParseTypeCollection property element not allowed", XmpErrorCode.BadXmp);
  }

  private static void Rdf_ParseTypeOtherPropertyElement()
  {
    throw new XmpException("ParseTypeOther property element not allowed", XmpErrorCode.BadXmp);
  }

  private static void Rdf_EmptyPropertyElement(
    XmpMeta xmp,
    XmpNode xmpParent,
    XElement xmlNode,
    bool isTopLevel)
  {
    if (xmlNode.FirstNode != null)
      throw new XmpException("Nested content not allowed with rdf:resource or property attributes", XmpErrorCode.BadRdf);
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    XAttribute xattribute = (XAttribute) null;
    foreach (XAttribute attribute in xmlNode.Attributes())
    {
      switch (xmlNode.GetPrefixOfNamespace(attribute.Name.Namespace))
      {
        case "xmlns":
          continue;
        case null:
          if (attribute.Name == (XName) "xmlns")
            continue;
          break;
      }
      switch (ParseRdf.GetRdfTermKind(attribute))
      {
        case RdfTerm.Other:
          if (attribute.Name.LocalName == "value" && attribute.Name.NamespaceName == "http://www.w3.org/1999/02/22-rdf-syntax-ns#")
          {
            if (flag2)
              throw new XmpException("Empty property element can't have both rdf:value and rdf:resource", XmpErrorCode.BadXmp);
            flag4 = true;
            xattribute = attribute;
            continue;
          }
          if ("xml:" + attribute.Name.LocalName != "xml:lang")
          {
            flag1 = true;
            continue;
          }
          continue;
        case RdfTerm.Id:
          continue;
        case RdfTerm.Resource:
          if (flag3)
            throw new XmpException("Empty property element can't have both rdf:resource and rdf:nodeID", XmpErrorCode.BadRdf);
          if (flag4)
            throw new XmpException("Empty property element can't have both rdf:value and rdf:resource", XmpErrorCode.BadXmp);
          flag2 = true;
          xattribute = attribute;
          continue;
        case RdfTerm.NodeId:
          if (flag2)
            throw new XmpException("Empty property element can't have both rdf:resource and rdf:nodeID", XmpErrorCode.BadRdf);
          flag3 = true;
          continue;
        default:
          throw new XmpException("Unrecognized attribute of empty property element", XmpErrorCode.BadRdf);
      }
    }
    XmpNode xmpParent1 = ParseRdf.AddChildNode(xmp, xmpParent, xmlNode, string.Empty, isTopLevel);
    bool flag5 = false;
    if (flag4 | flag2)
    {
      xmpParent1.Value = xattribute != null ? xattribute.Value : string.Empty;
      if (!flag4)
        xmpParent1.Options.IsUri = true;
    }
    else if (flag1)
    {
      xmpParent1.Options.IsStruct = true;
      flag5 = true;
    }
    foreach (XAttribute attribute in xmlNode.Attributes())
    {
      string prefixOfNamespace = xmlNode.GetPrefixOfNamespace(attribute.Name.Namespace);
      if (attribute != xattribute)
      {
        switch (prefixOfNamespace)
        {
          case "xmlns":
            continue;
          case null:
            if (attribute.Name == (XName) "xmlns")
              continue;
            break;
        }
        switch (ParseRdf.GetRdfTermKind(attribute))
        {
          case RdfTerm.Other:
            if (!flag5)
            {
              ParseRdf.AddQualifierNode(xmpParent1, attribute.Name.LocalName, attribute.Value);
              continue;
            }
            if ("xml:" + attribute.Name.LocalName == "xml:lang")
            {
              ParseRdf.AddQualifierNode(xmpParent1, "xml:lang", attribute.Value);
              continue;
            }
            ParseRdf.AddChildNode(xmp, xmpParent1, attribute, attribute.Value, false);
            continue;
          case RdfTerm.Id:
          case RdfTerm.NodeId:
            continue;
          case RdfTerm.Resource:
            ParseRdf.AddQualifierNode(xmpParent1, "rdf:resource", attribute.Value);
            continue;
          default:
            throw new XmpException("Unrecognized attribute of empty property element", XmpErrorCode.BadRdf);
        }
      }
    }
  }

  private static XmpNode AddChildNode(
    XmpMeta xmp,
    XmpNode xmpParent,
    XElement xmlNode,
    string value,
    bool isTopLevel)
  {
    return ParseRdf.AddChildNode(xmp, xmpParent, xmlNode.Name, xmlNode.GetPrefixOfNamespace(xmlNode.Name.Namespace), value, isTopLevel);
  }

  private static XmpNode AddChildNode(
    XmpMeta xmp,
    XmpNode xmpParent,
    XAttribute xmlNode,
    string value,
    bool isTopLevel)
  {
    return ParseRdf.AddChildNode(xmp, xmpParent, xmlNode.Name, xmlNode.Parent.GetPrefixOfNamespace(xmlNode.Name.Namespace), value, isTopLevel);
  }

  private static XmpNode AddChildNode(
    XmpMeta xmp,
    XmpNode xmpParent,
    XName nodeName,
    string nodeNamespacePrefix,
    string value,
    bool isTopLevel)
  {
    IXmpSchemaRegistry schemaRegistry = XmpMetaFactory.SchemaRegistry;
    string namespaceUri = nodeName.NamespaceName;
    if (namespaceUri == string.Empty)
      throw new XmpException("XML namespace required for all elements and attributes", XmpErrorCode.BadRdf);
    if (namespaceUri == "http://purl.org/dc/1.1/")
      namespaceUri = "http://purl.org/dc/elements/1.1/";
    string str1 = schemaRegistry.GetNamespacePrefix(namespaceUri);
    if (str1 == null)
    {
      string suggestedPrefix = nodeNamespacePrefix ?? "_dflt";
      str1 = schemaRegistry.RegisterNamespace(namespaceUri, suggestedPrefix);
    }
    string str2 = str1 + nodeName.LocalName;
    PropertyOptions options = new PropertyOptions();
    bool flag1 = false;
    if (isTopLevel)
    {
      XmpNode schemaNode = XmpNodeUtils.FindSchemaNode(xmp.GetRoot(), namespaceUri, "_dflt", true);
      schemaNode.IsImplicit = false;
      xmpParent = schemaNode;
      if (schemaRegistry.FindAlias(str2) != null)
      {
        flag1 = true;
        xmp.GetRoot().HasAliases = true;
        schemaNode.HasAliases = true;
      }
    }
    bool flag2 = ParseRdf.IsNumberedArrayItemName(str2);
    int num = str2 == "rdf:value" ? 1 : 0;
    XmpNode node = new XmpNode(str2, value, options)
    {
      IsAlias = flag1
    };
    if (num == 0)
      xmpParent.AddChild(node);
    else
      xmpParent.AddChild(1, node);
    if (num != 0)
    {
      if (isTopLevel || !xmpParent.Options.IsStruct)
        throw new XmpException("Misplaced rdf:value element", XmpErrorCode.BadRdf);
      xmpParent.HasValueChild = true;
    }
    bool isArray = xmpParent.Options.IsArray;
    if (isArray & flag2)
    {
      node.Name = "[]";
    }
    else
    {
      if (!isArray & flag2)
        throw new XmpException("Misplaced rdf:li element", XmpErrorCode.BadRdf);
      if (isArray && !flag2)
        throw new XmpException("Arrays cannot have arbitrary child names", XmpErrorCode.BadRdf);
    }
    return node;
  }

  private static void AddQualifierNode(XmpNode xmpParent, string name, string value)
  {
    if (name == "xml:lang")
      value = Utils.NormalizeLangValue(value);
    xmpParent.AddQualifier(new XmpNode(name, value, (PropertyOptions) null));
  }

  private static void FixupQualifiedNode(XmpNode xmpParent)
  {
    XmpNode child1 = xmpParent.GetChild(1);
    if (child1.Options.HasLanguage)
    {
      if (xmpParent.Options.HasLanguage)
        throw new XmpException("Redundant xml:lang for rdf:value element", XmpErrorCode.BadXmp);
      XmpNode qualifier = child1.GetQualifier(1);
      child1.RemoveQualifier(qualifier);
      xmpParent.AddQualifier(qualifier);
    }
    for (int index = 1; index <= child1.GetQualifierLength(); ++index)
    {
      XmpNode qualifier = child1.GetQualifier(index);
      xmpParent.AddQualifier(qualifier);
    }
    for (int index = 2; index <= xmpParent.GetChildrenLength(); ++index)
    {
      XmpNode child2 = xmpParent.GetChild(index);
      xmpParent.AddQualifier(child2);
    }
    xmpParent.HasValueChild = false;
    xmpParent.Options.IsStruct = false;
    xmpParent.Options.MergeWith(child1.Options);
    xmpParent.Value = child1.Value;
    xmpParent.RemoveChildren();
    IIterator iterator = child1.IterateChildren();
    while (iterator.HasNext())
    {
      XmpNode node = (XmpNode) iterator.Next();
      xmpParent.AddChild(node);
    }
  }

  private static bool IsWhitespaceNode(XNode node)
  {
    return node.NodeType == XmlNodeType.Text && Utils.IsNullOrWhiteSpace(((XText) node).Value);
  }

  private static bool IsPropertyElementName(RdfTerm term)
  {
    return term != RdfTerm.Description && !ParseRdf.IsOldTerm(term) && !ParseRdf.IsCoreSyntaxTerm(term);
  }

  private static bool IsOldTerm(RdfTerm term) => RdfTerm.AboutEach <= term && term <= RdfTerm.BagId;

  private static bool IsCoreSyntaxTerm(RdfTerm term)
  {
    return RdfTerm.Rdf <= term && term <= RdfTerm.Datatype;
  }

  private static RdfTerm GetRdfTermKind(XElement node)
  {
    return ParseRdf.GetRdfTermKind(node.Name, node.NodeType);
  }

  private static RdfTerm GetRdfTermKind(XAttribute node)
  {
    return ParseRdf.GetRdfTermKind(node.Name, node.NodeType, node.Parent.Name);
  }

  private static RdfTerm GetRdfTermKind(XName name, XmlNodeType nodeType, XName parentName = null)
  {
    string localName = name.LocalName;
    string str1 = name.NamespaceName;
    string str2 = parentName != (XName) null ? parentName.NamespaceName : string.Empty;
    if (str1 == string.Empty && (localName == "about" || localName == "ID") && nodeType == XmlNodeType.Attribute && str2 == "http://www.w3.org/1999/02/22-rdf-syntax-ns#")
      str1 = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
    if (str1 != "http://www.w3.org/1999/02/22-rdf-syntax-ns#" || localName == null)
      return RdfTerm.Other;
    switch (localName.Length)
    {
      case 2:
        switch (localName[0])
        {
          case 'I':
            if (localName == "ID")
              return RdfTerm.Id;
            break;
          case 'l':
            if (localName == "li")
              return RdfTerm.Li;
            break;
        }
        break;
      case 3:
        if (localName == "RDF")
          return RdfTerm.Rdf;
        break;
      case 5:
        switch (localName[0])
        {
          case 'a':
            if (localName == "about")
              return RdfTerm.About;
            break;
          case 'b':
            if (localName == "bagID")
              return RdfTerm.BagId;
            break;
        }
        break;
      case 6:
        if (localName == "nodeID")
          return RdfTerm.NodeId;
        break;
      case 8:
        switch (localName[0])
        {
          case 'd':
            if (localName == "datatype")
              return RdfTerm.Datatype;
            break;
          case 'r':
            if (localName == "resource")
              return RdfTerm.Resource;
            break;
        }
        break;
      case 9:
        switch (localName[0])
        {
          case 'a':
            if (localName == "aboutEach")
              return RdfTerm.AboutEach;
            break;
          case 'p':
            if (localName == "parseType")
              return RdfTerm.ParseType;
            break;
        }
        break;
      case 11:
        if (localName == "Description")
          return RdfTerm.Description;
        break;
      case 15:
        if (localName == "aboutEachPrefix")
          return RdfTerm.AboutEachPrefix;
        break;
    }
    return RdfTerm.Other;
  }

  private static bool IsNumberedArrayItemName(string nodeName)
  {
    bool flag = "rdf:li" == nodeName;
    if (nodeName.StartsWith("rdf:_"))
    {
      flag = true;
      for (int index = 5; index < nodeName.Length; ++index)
        flag = flag && nodeName[index] >= '0' && nodeName[index] <= '9';
    }
    return flag;
  }
}
