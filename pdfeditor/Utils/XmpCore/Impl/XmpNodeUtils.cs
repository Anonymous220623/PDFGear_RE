// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpNodeUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using XmpCore.Impl.XPath;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public static class XmpNodeUtils
{
  internal const int CltNoValues = 0;
  internal const int CltSpecificMatch = 1;
  internal const int CltSingleGeneric = 2;
  internal const int CltMultipleGeneric = 3;
  internal const int CltXDefault = 4;
  internal const int CltFirstItem = 5;

  internal static XmpNode FindSchemaNode(XmpNode tree, string namespaceUri, bool createNodes)
  {
    return XmpNodeUtils.FindSchemaNode(tree, namespaceUri, (string) null, createNodes);
  }

  internal static XmpNode FindSchemaNode(
    XmpNode tree,
    string namespaceUri,
    string suggestedPrefix,
    bool createNodes)
  {
    XmpNode node = tree.FindChildByName(namespaceUri);
    if (node == null & createNodes)
    {
      PropertyOptions options = new PropertyOptions()
      {
        IsSchemaNode = true
      };
      node = new XmpNode(namespaceUri, options)
      {
        IsImplicit = true
      };
      string str = XmpMetaFactory.SchemaRegistry.GetNamespacePrefix(namespaceUri);
      if (str == null)
      {
        if (string.IsNullOrEmpty(suggestedPrefix))
          throw new XmpException("Unregistered schema namespace URI", XmpErrorCode.BadSchema);
        str = XmpMetaFactory.SchemaRegistry.RegisterNamespace(namespaceUri, suggestedPrefix);
      }
      node.Value = str;
      tree.AddChild(node);
    }
    return node;
  }

  internal static XmpNode FindChildNode(XmpNode parent, string childName, bool createNodes)
  {
    if (!parent.Options.IsSchemaNode && !parent.Options.IsStruct)
    {
      if (!parent.IsImplicit)
        throw new XmpException("Named children only allowed for schemas and structs", XmpErrorCode.BadXPath);
      if (parent.Options.IsArray)
        throw new XmpException("Named children not allowed for arrays", XmpErrorCode.BadXPath);
      if (createNodes)
        parent.Options.IsStruct = true;
    }
    XmpNode node = parent.FindChildByName(childName);
    if (node == null & createNodes)
    {
      PropertyOptions options = new PropertyOptions();
      node = new XmpNode(childName, options)
      {
        IsImplicit = true
      };
      parent.AddChild(node);
    }
    return node;
  }

  internal static XmpNode FindNode(
    XmpNode xmpTree,
    XmpPath xpath,
    bool createNodes,
    PropertyOptions leafOptions)
  {
    if (xpath == null || xpath.Size() == 0)
      throw new XmpException("Empty XmpPath", XmpErrorCode.BadXPath);
    XmpNode node = (XmpNode) null;
    XmpNode parentNode = XmpNodeUtils.FindSchemaNode(xmpTree, xpath.GetSegment(0).Name, createNodes);
    if (parentNode == null)
      return (XmpNode) null;
    if (parentNode.IsImplicit)
    {
      parentNode.IsImplicit = false;
      node = parentNode;
    }
    try
    {
      for (int index = 1; index < xpath.Size(); ++index)
      {
        parentNode = XmpNodeUtils.FollowXPathStep(parentNode, xpath.GetSegment(index), createNodes);
        if (parentNode == null)
        {
          if (createNodes)
            XmpNodeUtils.DeleteNode(node);
          return (XmpNode) null;
        }
        if (parentNode.IsImplicit)
        {
          parentNode.IsImplicit = false;
          if (index == 1 && xpath.GetSegment(index).IsAlias && xpath.GetSegment(index).AliasForm != 0)
            parentNode.Options.SetOption(xpath.GetSegment(index).AliasForm, true);
          else if (index < xpath.Size() - 1 && xpath.GetSegment(index).Kind == XmpPathStepType.StructFieldStep && !parentNode.Options.IsCompositeProperty)
            parentNode.Options.IsStruct = true;
          if (node == null)
            node = parentNode;
        }
      }
    }
    catch (XmpException ex)
    {
      if (node != null)
        XmpNodeUtils.DeleteNode(node);
      throw;
    }
    if (node != null)
    {
      parentNode.Options.MergeWith(leafOptions);
      parentNode.Options = parentNode.Options;
    }
    return parentNode;
  }

  internal static void DeleteNode(XmpNode node)
  {
    XmpNode parent = node.Parent;
    if (node.Options.IsQualifier)
      parent.RemoveQualifier(node);
    else
      parent.RemoveChild(node);
    if (parent.HasChildren || !parent.Options.IsSchemaNode)
      return;
    parent.Parent.RemoveChild(parent);
  }

  internal static void SetNodeValue(XmpNode node, object value)
  {
    string str = XmpNodeUtils.SerializeNodeValue(value);
    node.Value = !node.Options.IsQualifier || !(node.Name == "xml:lang") ? str : Utils.NormalizeLangValue(str);
  }

  internal static PropertyOptions VerifySetOptions(PropertyOptions options, object itemValue)
  {
    if (options == null)
      options = new PropertyOptions();
    if (options.IsArrayAltText)
      options.IsArrayAlternate = true;
    if (options.IsArrayAlternate)
      options.IsArrayOrdered = true;
    if (options.IsArrayOrdered)
      options.IsArray = true;
    if (options.IsCompositeProperty && itemValue != null && itemValue.ToString().Length > 0)
      throw new XmpException("Structs and arrays can't have values", XmpErrorCode.BadOptions);
    options.AssertConsistency(options.GetOptions());
    return options;
  }

  private static string SerializeNodeValue(object value)
  {
    string str1;
    string str2;
    switch (value)
    {
      case null:
        return (string) null;
      case bool flag:
        return XmpCore.XmpUtils.ConvertFromBoolean(flag);
      case int num1:
        return XmpCore.XmpUtils.ConvertFromInteger(num1);
      case long num2:
        return XmpCore.XmpUtils.ConvertFromLong(num2);
      case double num3:
        return XmpCore.XmpUtils.ConvertFromDouble(num3);
      case IXmpDateTime xmpDateTime:
        str1 = XmpCore.XmpUtils.ConvertFromDate(xmpDateTime);
        goto label_11;
      case GregorianCalendar gregorianCalendar:
        str1 = XmpCore.XmpUtils.ConvertFromDate(XmpDateTimeFactory.CreateFromCalendar((Calendar) gregorianCalendar));
        goto label_11;
      case byte[] buffer:
        str2 = XmpCore.XmpUtils.EncodeBase64(buffer);
        break;
      default:
        str2 = value.ToString();
        break;
    }
    str1 = str2;
label_11:
    return str1 == null ? (string) null : Utils.RemoveControlChars(str1);
  }

  private static XmpNode FollowXPathStep(
    XmpNode parentNode,
    XmpPathSegment nextStep,
    bool createNodes)
  {
    XmpNode xmpNode = (XmpNode) null;
    XmpPathStepType kind = nextStep.Kind;
    switch (kind)
    {
      case XmpPathStepType.StructFieldStep:
        xmpNode = XmpNodeUtils.FindChildNode(parentNode, nextStep.Name, createNodes);
        break;
      case XmpPathStepType.QualifierStep:
        xmpNode = XmpNodeUtils.FindQualifierNode(parentNode, nextStep.Name.Substring(1), createNodes);
        break;
      default:
        if (!parentNode.Options.IsArray)
          throw new XmpException("Indexing applied to non-array", XmpErrorCode.BadXPath);
        int index;
        switch (kind)
        {
          case XmpPathStepType.ArrayIndexStep:
            index = XmpNodeUtils.FindIndexedItem(parentNode, nextStep.Name, createNodes);
            break;
          case XmpPathStepType.ArrayLastStep:
            index = parentNode.GetChildrenLength();
            break;
          case XmpPathStepType.QualSelectorStep:
            string name1;
            string qualValue;
            Utils.SplitNameAndValue(nextStep.Name, out name1, out qualValue);
            index = XmpNodeUtils.LookupQualSelector(parentNode, name1, qualValue, nextStep.AliasForm);
            break;
          case XmpPathStepType.FieldSelectorStep:
            string name2;
            string fieldValue;
            Utils.SplitNameAndValue(nextStep.Name, out name2, out fieldValue);
            index = XmpNodeUtils.LookupFieldSelector(parentNode, name2, fieldValue);
            break;
          default:
            throw new XmpException("Unknown array indexing step in FollowXPathStep", XmpErrorCode.InternalFailure);
        }
        if (1 <= index && index <= parentNode.GetChildrenLength())
        {
          xmpNode = parentNode.GetChild(index);
          break;
        }
        break;
    }
    return xmpNode;
  }

  private static XmpNode FindQualifierNode(XmpNode parent, string qualName, bool createNodes)
  {
    XmpNode qualNode = parent.FindQualifierByName(qualName);
    if (qualNode == null & createNodes)
    {
      qualNode = new XmpNode(qualName, (PropertyOptions) null)
      {
        IsImplicit = true
      };
      parent.AddQualifier(qualNode);
    }
    return qualNode;
  }

  private static int FindIndexedItem(XmpNode arrayNode, string segment, bool createNodes)
  {
    int result;
    if (!int.TryParse(segment.Substring(1, segment.Length - 1 - 1), out result))
      throw new XmpException("Array index not digits.", XmpErrorCode.BadXPath);
    if (createNodes && result == arrayNode.GetChildrenLength() + 1)
    {
      XmpNode node = new XmpNode("[]", (PropertyOptions) null)
      {
        IsImplicit = true
      };
      arrayNode.AddChild(node);
    }
    return result;
  }

  private static int LookupFieldSelector(XmpNode arrayNode, string fieldName, string fieldValue)
  {
    int num = -1;
    for (int index1 = 1; index1 <= arrayNode.GetChildrenLength() && num < 0; ++index1)
    {
      XmpNode child1 = arrayNode.GetChild(index1);
      if (!child1.Options.IsStruct)
        throw new XmpException("Field selector must be used on array of struct", XmpErrorCode.BadXPath);
      for (int index2 = 1; index2 <= child1.GetChildrenLength(); ++index2)
      {
        XmpNode child2 = child1.GetChild(index2);
        if (!(child2.Name != fieldName) && !(child2.Value != fieldValue))
        {
          num = index1;
          break;
        }
      }
    }
    return num;
  }

  private static int LookupQualSelector(
    XmpNode arrayNode,
    string qualName,
    string qualValue,
    int aliasForm)
  {
    if (qualName == "xml:lang")
    {
      qualValue = Utils.NormalizeLangValue(qualValue);
      int num = XmpNodeUtils.LookupLanguageItem(arrayNode, qualValue);
      if (num >= 0 || (aliasForm & 4096 /*0x1000*/) <= 0)
        return num;
      XmpNode node = new XmpNode("[]", (PropertyOptions) null);
      XmpNode qualNode = new XmpNode("xml:lang", "x-default", (PropertyOptions) null);
      node.AddQualifier(qualNode);
      arrayNode.AddChild(1, node);
      return 1;
    }
    for (int index = 1; index < arrayNode.GetChildrenLength(); ++index)
    {
      IIterator iterator = arrayNode.GetChild(index).IterateQualifier();
      while (iterator.HasNext())
      {
        XmpNode xmpNode = (XmpNode) iterator.Next();
        if (xmpNode.Name == qualName && xmpNode.Value == qualValue)
          return index;
      }
    }
    return -1;
  }

  internal static void NormalizeLangArray(XmpNode arrayNode)
  {
    if (!arrayNode.Options.IsArrayAltText)
      return;
    for (int index = 2; index <= arrayNode.GetChildrenLength(); ++index)
    {
      XmpNode child = arrayNode.GetChild(index);
      if (child.HasQualifier)
      {
        if (!(child.GetQualifier(1).Value != "x-default"))
        {
          try
          {
            arrayNode.RemoveChild(index);
            arrayNode.AddChild(1, child);
          }
          catch (XmpException ex)
          {
          }
          if (index != 2)
            break;
          arrayNode.GetChild(2).Value = child.Value;
          break;
        }
      }
    }
  }

  internal static void DetectAltText(XmpNode arrayNode)
  {
    if (!arrayNode.Options.IsArrayAlternate || !arrayNode.HasChildren)
      return;
    bool flag = false;
    IIterator iterator = arrayNode.IterateChildren();
    while (iterator.HasNext())
    {
      if (((XmpNode) iterator.Next()).Options.HasLanguage)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    arrayNode.Options.IsArrayAltText = true;
    XmpNodeUtils.NormalizeLangArray(arrayNode);
  }

  internal static void AppendLangItem(XmpNode arrayNode, string itemLang, string itemValue)
  {
    XmpNode node = new XmpNode("[]", itemValue, (PropertyOptions) null);
    XmpNode qualNode = new XmpNode("xml:lang", itemLang, (PropertyOptions) null);
    node.AddQualifier(qualNode);
    if (qualNode.Value != "x-default")
      arrayNode.AddChild(node);
    else
      arrayNode.AddChild(1, node);
  }

  internal static object[] ChooseLocalizedText(
    XmpNode arrayNode,
    string genericLang,
    string specificLang)
  {
    if (!arrayNode.Options.IsArrayAltText)
      throw new XmpException("Localized text array is not alt-text", XmpErrorCode.BadXPath);
    if (!arrayNode.HasChildren)
      return new object[2]{ (object) 0, null };
    int num = 0;
    XmpNode xmpNode1 = (XmpNode) null;
    XmpNode xmpNode2 = (XmpNode) null;
    IIterator iterator = arrayNode.IterateChildren();
    while (iterator.HasNext())
    {
      XmpNode xmpNode3 = (XmpNode) iterator.Next();
      if (xmpNode3.Options.IsCompositeProperty)
        throw new XmpException("Alt-text array item is not simple", XmpErrorCode.BadXPath);
      if (!xmpNode3.HasQualifier || xmpNode3.GetQualifier(1).Name != "xml:lang")
        throw new XmpException("Alt-text array item has no language qualifier", XmpErrorCode.BadXPath);
      string str = xmpNode3.GetQualifier(1).Value;
      if (str == specificLang)
        return new object[2]
        {
          (object) 1,
          (object) xmpNode3
        };
      if (genericLang != null && str.StartsWith(genericLang))
      {
        if (xmpNode1 == null)
          xmpNode1 = xmpNode3;
        ++num;
      }
      else if (str == "x-default")
        xmpNode2 = xmpNode3;
    }
    return num == 1 ? new object[2]
    {
      (object) 2,
      (object) xmpNode1
    } : (num > 1 ? new object[2]
    {
      (object) 3,
      (object) xmpNode1
    } : (xmpNode2 != null ? new object[2]
    {
      (object) 4,
      (object) xmpNode2
    } : new object[2]
    {
      (object) 5,
      (object) arrayNode.GetChild(1)
    }));
  }

  internal static int LookupLanguageItem(XmpNode arrayNode, string language)
  {
    if (!arrayNode.Options.IsArray)
      throw new XmpException("Language item must be used on array", XmpErrorCode.BadXPath);
    for (int index = 1; index <= arrayNode.GetChildrenLength(); ++index)
    {
      XmpNode child = arrayNode.GetChild(index);
      if (child.HasQualifier && !(child.GetQualifier(1).Name != "xml:lang") && child.GetQualifier(1).Value == language)
        return index;
    }
    return -1;
  }
}
