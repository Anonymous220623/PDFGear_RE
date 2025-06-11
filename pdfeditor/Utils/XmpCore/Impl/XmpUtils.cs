// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmpCore.Impl.XPath;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public static class XmpUtils
{
  private const string Spaces = " 　〿";
  private const string Commas = ",，､﹐﹑、،՝";
  private const string Semicola = ";；﹔؛;";
  private const string Quotes = "\"«»〝〞〟―‹›";
  private const string Controls = "\u2028\u2029";

  public static string CatenateArrayItems(
    IXmpMeta xmp,
    string schemaNs,
    string arrayName,
    string separator,
    string quotes,
    bool allowCommas)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertArrayName(arrayName);
    ParameterAsserts.AssertImplementation(xmp);
    if (string.IsNullOrEmpty(separator))
      separator = "; ";
    if (string.IsNullOrEmpty(quotes))
      quotes = "\"";
    XmpMeta xmpMeta = (XmpMeta) xmp;
    XmpPath xpath = XmpPathParser.ExpandXPath(schemaNs, arrayName);
    XmpNode node = XmpNodeUtils.FindNode(xmpMeta.GetRoot(), xpath, false, (PropertyOptions) null);
    if (node == null)
      return string.Empty;
    if (!node.Options.IsArray || node.Options.IsArrayAlternate)
      throw new XmpException("Named property must be non-alternate array", XmpErrorCode.BadParam);
    XmpUtils.CheckSeparator(separator);
    char quote = quotes[0];
    char closeQuote = XmpUtils.CheckQuotes(quotes, quote);
    StringBuilder stringBuilder = new StringBuilder();
    IIterator iterator = node.IterateChildren();
    while (iterator.HasNext())
    {
      XmpNode xmpNode = (XmpNode) iterator.Next();
      if (xmpNode.Options.IsCompositeProperty)
        throw new XmpException("Array items must be simple", XmpErrorCode.BadParam);
      string str = XmpUtils.ApplyQuotes(xmpNode.Value, quote, closeQuote, allowCommas);
      stringBuilder.Append(str);
      if (iterator.HasNext())
        stringBuilder.Append(separator);
    }
    return stringBuilder.ToString();
  }

  public static void SeparateArrayItems(
    IXmpMeta xmp,
    string schemaNs,
    string arrayName,
    string catedStr,
    PropertyOptions arrayOptions,
    bool preserveCommas)
  {
    ParameterAsserts.AssertSchemaNs(schemaNs);
    ParameterAsserts.AssertArrayName(arrayName);
    if (catedStr == null)
      throw new XmpException("Parameter must not be null", XmpErrorCode.BadParam);
    ParameterAsserts.AssertImplementation(xmp);
    XmpMeta xmp1 = (XmpMeta) xmp;
    XmpNode createArray = XmpUtils.SeparateFindCreateArray(schemaNs, arrayName, arrayOptions, xmp1);
    int num1 = int.MaxValue;
    if (arrayOptions != null)
    {
      num1 = arrayOptions.ArrayElementsLimit;
      if (num1 == -1)
        num1 = int.MaxValue;
    }
    XmpUtils.UnicodeKind unicodeKind1 = XmpUtils.UnicodeKind.Normal;
    char minValue = char.MinValue;
    int index1 = 0;
    int length = catedStr.Length;
    while (index1 < length && createArray.GetChildrenLength() < num1)
    {
      int num2;
      for (num2 = index1; num2 < length; ++num2)
      {
        minValue = catedStr[num2];
        unicodeKind1 = XmpUtils.ClassifyCharacter(minValue);
        switch (unicodeKind1)
        {
          case XmpUtils.UnicodeKind.Normal:
          case XmpUtils.UnicodeKind.Quote:
            goto label_10;
          default:
            continue;
        }
      }
label_10:
      if (num2 >= length)
        break;
      string str;
      if (unicodeKind1 != XmpUtils.UnicodeKind.Quote)
      {
        for (index1 = num2; index1 < length; ++index1)
        {
          minValue = catedStr[index1];
          unicodeKind1 = XmpUtils.ClassifyCharacter(minValue);
          switch (unicodeKind1)
          {
            case XmpUtils.UnicodeKind.Normal:
            case XmpUtils.UnicodeKind.Quote:
              continue;
            default:
              if (!(unicodeKind1 == XmpUtils.UnicodeKind.Comma & preserveCommas))
              {
                if (unicodeKind1 == XmpUtils.UnicodeKind.Space && index1 + 1 < length)
                {
                  minValue = catedStr[index1 + 1];
                  XmpUtils.UnicodeKind unicodeKind2 = XmpUtils.ClassifyCharacter(minValue);
                  switch (unicodeKind2)
                  {
                    case XmpUtils.UnicodeKind.Normal:
                    case XmpUtils.UnicodeKind.Quote:
                      continue;
                    default:
                      if (!(unicodeKind2 == XmpUtils.UnicodeKind.Comma & preserveCommas))
                        goto label_20;
                      continue;
                  }
                }
                else
                  goto label_20;
              }
              else
                continue;
          }
        }
label_20:
        str = catedStr.Substring(num2, index1 - num2);
      }
      else
      {
        char openQuote = minValue;
        char closingQuote = XmpUtils.GetClosingQuote(openQuote);
        int num3 = num2 + 1;
        StringBuilder stringBuilder = new StringBuilder();
        for (index1 = num3; index1 < length; ++index1)
        {
          minValue = catedStr[index1];
          unicodeKind1 = XmpUtils.ClassifyCharacter(minValue);
          if (unicodeKind1 != XmpUtils.UnicodeKind.Quote || !XmpUtils.IsSurroundingQuote(minValue, openQuote, closingQuote))
          {
            stringBuilder.Append(minValue);
          }
          else
          {
            char ch = index1 + 1 >= length ? ';' : catedStr[index1 + 1];
            if ((int) minValue == (int) ch)
            {
              stringBuilder.Append(minValue);
              ++index1;
            }
            else if (!XmpUtils.IsClosingQuote(minValue, openQuote, closingQuote))
            {
              stringBuilder.Append(minValue);
            }
            else
            {
              ++index1;
              break;
            }
          }
        }
        str = stringBuilder.ToString();
      }
      int num4 = -1;
      for (int index2 = 1; index2 <= createArray.GetChildrenLength(); ++index2)
      {
        if (createArray.GetChild(index2).Value == str)
        {
          num4 = index2;
          break;
        }
      }
      if (num4 < 0)
        createArray.AddChild(new XmpNode("[]", str, (PropertyOptions) null));
    }
  }

  private static XmpNode SeparateFindCreateArray(
    string schemaNs,
    string arrayName,
    PropertyOptions arrayOptions,
    XmpMeta xmp)
  {
    arrayOptions = XmpNodeUtils.VerifySetOptions(arrayOptions, (object) null);
    if (!arrayOptions.IsOnlyArrayOptions)
      throw new XmpException("Options can only provide array form", XmpErrorCode.BadOptions);
    XmpPath xpath = XmpPathParser.ExpandXPath(schemaNs, arrayName);
    XmpNode node = XmpNodeUtils.FindNode(xmp.GetRoot(), xpath, false, (PropertyOptions) null);
    if (node != null)
    {
      PropertyOptions options = node.Options;
      if (!options.IsArray || options.IsArrayAlternate)
        throw new XmpException("Named property must be non-alternate array", XmpErrorCode.BadXPath);
      if (arrayOptions.EqualArrayTypes(options))
        throw new XmpException("Mismatch of specified and existing array form", XmpErrorCode.BadXPath);
    }
    else
    {
      arrayOptions.IsArray = true;
      node = XmpNodeUtils.FindNode(xmp.GetRoot(), xpath, true, arrayOptions);
      if (node == null)
        throw new XmpException("Failed to create named array", XmpErrorCode.BadXPath);
    }
    return node;
  }

  public static void RemoveProperties(
    IXmpMeta xmp,
    string schemaNs,
    string propName,
    bool doAllProperties,
    bool includeAliases)
  {
    ParameterAsserts.AssertImplementation(xmp);
    XmpMeta xmpMeta = (XmpMeta) xmp;
    if (!string.IsNullOrEmpty(propName))
    {
      XmpPath xpath = !string.IsNullOrEmpty(schemaNs) ? XmpPathParser.ExpandXPath(schemaNs, propName) : throw new XmpException("Property name requires schema namespace", XmpErrorCode.BadParam);
      XmpNode node = XmpNodeUtils.FindNode(xmpMeta.GetRoot(), xpath, false, (PropertyOptions) null);
      if (node == null || !doAllProperties && Utils.IsInternalProperty(xpath.GetSegment(0).Name, xpath.GetSegment(1).Name))
        return;
      XmpNode parent = node.Parent;
      parent.RemoveChild(node);
      if (!parent.Options.IsSchemaNode || parent.HasChildren)
        return;
      parent.Parent.RemoveChild(parent);
    }
    else if (!string.IsNullOrEmpty(schemaNs))
    {
      XmpNode schemaNode = XmpNodeUtils.FindSchemaNode(xmpMeta.GetRoot(), schemaNs, false);
      if (schemaNode != null && XmpUtils.RemoveSchemaChildren(schemaNode, doAllProperties))
        xmpMeta.GetRoot().RemoveChild(schemaNode);
      if (!includeAliases)
        return;
      foreach (IXmpAliasInfo alias in XmpMetaFactory.SchemaRegistry.FindAliases(schemaNs))
      {
        XmpPath xpath = XmpPathParser.ExpandXPath(alias.Namespace, alias.PropName);
        XmpNode node = XmpNodeUtils.FindNode(xmpMeta.GetRoot(), xpath, false, (PropertyOptions) null);
        node?.Parent.RemoveChild(node);
      }
    }
    else
    {
      IIterator iterator = xmpMeta.GetRoot().IterateChildren();
      while (iterator.HasNext())
      {
        if (XmpUtils.RemoveSchemaChildren((XmpNode) iterator.Next(), doAllProperties))
          iterator.Remove();
      }
    }
  }

  public static void AppendProperties(
    IXmpMeta source,
    IXmpMeta destination,
    bool doAllProperties,
    bool replaceOldValues,
    bool deleteEmptyValues)
  {
    ParameterAsserts.AssertImplementation(source);
    ParameterAsserts.AssertImplementation(destination);
    XmpMeta xmpMeta = (XmpMeta) source;
    XmpMeta destXmp = (XmpMeta) destination;
    IIterator iterator1 = xmpMeta.GetRoot().IterateChildren();
    while (iterator1.HasNext())
    {
      XmpNode xmpNode1 = (XmpNode) iterator1.Next();
      XmpNode xmpNode2 = XmpNodeUtils.FindSchemaNode(destXmp.GetRoot(), xmpNode1.Name, false);
      bool flag = false;
      if (xmpNode2 == null)
      {
        xmpNode2 = new XmpNode(xmpNode1.Name, xmpNode1.Value, new PropertyOptions()
        {
          IsSchemaNode = true
        });
        destXmp.GetRoot().AddChild(xmpNode2);
        flag = true;
      }
      IIterator iterator2 = xmpNode1.IterateChildren();
      while (iterator2.HasNext())
      {
        XmpNode sourceNode = (XmpNode) iterator2.Next();
        if (doAllProperties || !Utils.IsInternalProperty(xmpNode1.Name, sourceNode.Name))
          XmpUtils.AppendSubtree(destXmp, sourceNode, xmpNode2, false, replaceOldValues, deleteEmptyValues);
      }
      if (!xmpNode2.HasChildren && flag | deleteEmptyValues)
        destXmp.GetRoot().RemoveChild(xmpNode2);
    }
  }

  private static bool RemoveSchemaChildren(XmpNode schemaNode, bool doAllProperties)
  {
    IIterator iterator = schemaNode.IterateChildren();
    while (iterator.HasNext())
    {
      XmpNode xmpNode = (XmpNode) iterator.Next();
      if (doAllProperties || !Utils.IsInternalProperty(schemaNode.Name, xmpNode.Name))
        iterator.Remove();
    }
    return !schemaNode.HasChildren;
  }

  private static void AppendSubtree(
    XmpMeta destXmp,
    XmpNode sourceNode,
    XmpNode destParent,
    bool mergeCompound,
    bool replaceOldValues,
    bool deleteEmptyValues)
  {
    XmpNode childNode = XmpNodeUtils.FindChildNode(destParent, sourceNode.Name, false);
    if ((sourceNode.Options.IsSimple ? (string.IsNullOrEmpty(sourceNode.Value) ? 1 : 0) : (!sourceNode.HasChildren ? 1 : 0)) != 0)
    {
      if (!deleteEmptyValues || childNode == null)
        return;
      destParent.RemoveChild(childNode);
    }
    else if (childNode == null)
    {
      XmpNode node = (XmpNode) sourceNode.Clone(true);
      if (node == null)
        return;
      destParent.AddChild(node);
    }
    else
    {
      PropertyOptions options1 = sourceNode.Options;
      bool flag1 = replaceOldValues;
      if (mergeCompound && !options1.IsSimple)
        flag1 = false;
      if (flag1)
      {
        destParent.RemoveChild(childNode);
        XmpNode node = (XmpNode) sourceNode.Clone(true);
        if (node == null)
          return;
        destParent.AddChild(node);
      }
      else
      {
        PropertyOptions options2 = childNode.Options;
        if (options1.GetOptions() != options2.GetOptions() || options1.IsSimple)
          return;
        if (options1.IsStruct)
        {
          IIterator iterator = sourceNode.IterateChildren();
          while (iterator.HasNext())
          {
            XmpNode sourceNode1 = (XmpNode) iterator.Next();
            XmpUtils.AppendSubtree(destXmp, sourceNode1, childNode, mergeCompound, replaceOldValues, deleteEmptyValues);
            if (deleteEmptyValues && !childNode.HasChildren)
              destParent.RemoveChild(childNode);
          }
        }
        else if (options1.IsArrayAltText)
        {
          IIterator iterator = sourceNode.IterateChildren();
          while (iterator.HasNext())
          {
            XmpNode xmpNode1 = (XmpNode) iterator.Next();
            if (xmpNode1.HasQualifier && !(xmpNode1.GetQualifier(1).Name != "xml:lang"))
            {
              int num = XmpNodeUtils.LookupLanguageItem(childNode, xmpNode1.GetQualifier(1).Value);
              if (string.IsNullOrEmpty(xmpNode1.Value))
              {
                if (deleteEmptyValues && num != -1)
                {
                  childNode.RemoveChild(num);
                  if (!childNode.HasChildren)
                    destParent.RemoveChild(childNode);
                }
              }
              else if (num == -1)
              {
                if (xmpNode1.GetQualifier(1).Value != "x-default" || !childNode.HasChildren)
                {
                  XmpNode node = (XmpNode) xmpNode1.Clone(true);
                  if (node != null)
                    childNode.AddChild(node);
                }
                else
                {
                  XmpNode xmpNode2 = new XmpNode(xmpNode1.Name, xmpNode1.Value, xmpNode1.Options);
                  xmpNode1.CloneSubtree(xmpNode2, true);
                  childNode.AddChild(1, xmpNode2);
                }
              }
              else if (replaceOldValues)
                childNode.GetChild(num).Value = xmpNode1.Value;
            }
          }
        }
        else
        {
          if (!options1.IsArray)
            return;
          IIterator iterator1 = sourceNode.IterateChildren();
          while (iterator1.HasNext())
          {
            XmpNode leftNode = (XmpNode) iterator1.Next();
            bool flag2 = false;
            IIterator iterator2 = childNode.IterateChildren();
            while (iterator2.HasNext())
            {
              XmpNode rightNode = (XmpNode) iterator2.Next();
              if (XmpUtils.ItemValuesMatch(leftNode, rightNode))
              {
                flag2 = true;
                break;
              }
            }
            if (!flag2)
            {
              XmpNode node = (XmpNode) leftNode.Clone(true);
              if (node != null)
                childNode.AddChild(node);
            }
          }
        }
      }
    }
  }

  private static bool ItemValuesMatch(XmpNode leftNode, XmpNode rightNode)
  {
    PropertyOptions options1 = leftNode.Options;
    PropertyOptions options2 = rightNode.Options;
    if (!options1.Equals((object) options2))
      return false;
    if (options1.IsSimple)
    {
      if (leftNode.Value != rightNode.Value || leftNode.Options.HasLanguage != rightNode.Options.HasLanguage || leftNode.Options.HasLanguage && leftNode.GetQualifier(1).Value != rightNode.GetQualifier(1).Value)
        return false;
    }
    else if (options1.IsStruct)
    {
      if (leftNode.GetChildrenLength() != rightNode.GetChildrenLength())
        return false;
      IIterator iterator = leftNode.IterateChildren();
      while (iterator.HasNext())
      {
        XmpNode leftNode1 = (XmpNode) iterator.Next();
        XmpNode childNode = XmpNodeUtils.FindChildNode(rightNode, leftNode1.Name, false);
        if (childNode == null || !XmpUtils.ItemValuesMatch(leftNode1, childNode))
          return false;
      }
    }
    else
    {
      IIterator iterator1 = leftNode.IterateChildren();
      while (iterator1.HasNext())
      {
        XmpNode leftNode2 = (XmpNode) iterator1.Next();
        bool flag = false;
        IIterator iterator2 = rightNode.IterateChildren();
        while (iterator2.HasNext())
        {
          XmpNode rightNode1 = (XmpNode) iterator2.Next();
          if (XmpUtils.ItemValuesMatch(leftNode2, rightNode1))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
    }
    return true;
  }

  public static void DuplicateSubtree(
    IXmpMeta source,
    IXmpMeta dest,
    string sourceNS,
    string sourceRoot,
    string destNS,
    string destRoot,
    PropertyOptions options)
  {
    bool flag1 = false;
    bool flag2 = false;
    ParameterAsserts.AssertNotNull((object) source);
    ParameterAsserts.AssertSchemaNs(sourceNS);
    ParameterAsserts.AssertSchemaNs(sourceRoot);
    ParameterAsserts.AssertNotNull((object) dest);
    ParameterAsserts.AssertNotNull((object) destNS);
    ParameterAsserts.AssertNotNull((object) destRoot);
    if (destNS.Length == 0)
      destNS = sourceNS;
    if (destRoot.Length == 0)
      destRoot = sourceRoot;
    if (sourceNS == "*")
      flag1 = true;
    if (destNS == "*")
      flag2 = true;
    if (source == dest && flag1 | flag2)
      throw new XmpException("Can't duplicate tree onto itself", XmpErrorCode.BadParam);
    if (flag1 & flag2)
      throw new XmpException("Use Clone for full tree to full tree", XmpErrorCode.BadParam);
    if (flag1)
    {
      XmpPath xpath = XmpPathParser.ExpandXPath(destNS, destRoot);
      XmpNode node = XmpNodeUtils.FindNode(((XmpMeta) dest).GetRoot(), xpath, false, (PropertyOptions) null);
      if (node == null || !node.Options.IsStruct)
        throw new XmpException("Destination must be an existing struct", XmpErrorCode.BadXPath);
      if (node.HasChildren)
      {
        if (options == null || (options.GetOptions() & 536870912 /*0x20000000*/) == 0)
          throw new XmpException("Destination must be an empty struct", XmpErrorCode.BadXPath);
        node.RemoveChildren();
      }
      XmpMeta xmpMeta = (XmpMeta) source;
      int index1 = 1;
      for (int childrenLength1 = xmpMeta.GetRoot().GetChildrenLength(); index1 <= childrenLength1; ++index1)
      {
        XmpNode child1 = xmpMeta.GetRoot().GetChild(index1);
        int index2 = 1;
        for (int childrenLength2 = child1.GetChildrenLength(); index2 <= childrenLength2; ++index2)
        {
          XmpNode child2 = child1.GetChild(index2);
          node.AddChild((XmpNode) child2.Clone(false));
        }
      }
    }
    else if (flag2)
    {
      XmpMeta xmpMeta1 = (XmpMeta) source;
      XmpMeta xmpMeta2 = (XmpMeta) dest;
      XmpPath xpath = XmpPathParser.ExpandXPath(sourceNS, sourceRoot);
      XmpNode node = XmpNodeUtils.FindNode(xmpMeta1.GetRoot(), xpath, false, (PropertyOptions) null);
      if (node == null || !node.Options.IsStruct)
        throw new XmpException("Source must be an existing struct", XmpErrorCode.BadXPath);
      XmpNode root = xmpMeta2.GetRoot();
      if (root.HasChildren)
      {
        if (options == null || (options.GetOptions() & 536870912 /*0x20000000*/) == 0)
          throw new XmpException("Source must be an existing struct", XmpErrorCode.BadXPath);
        root.RemoveChildren();
      }
      int index = 1;
      for (int childrenLength = node.GetChildrenLength(); index <= childrenLength; ++index)
      {
        XmpNode child = node.GetChild(index);
        int num = child.Name.IndexOf(':');
        if (num != -1)
          (XmpNodeUtils.FindSchemaNode(xmpMeta2.GetRoot(), XmpMetaFactory.SchemaRegistry.GetNamespaceUri(child.Name.Substring(0, num + 1)) ?? throw new XmpException("Source field namespace is not global", XmpErrorCode.BadSchema), true) ?? throw new XmpException("Failed to find destination schema", XmpErrorCode.BadSchema)).AddChild((XmpNode) child.Clone(false));
      }
    }
    else
    {
      XmpPath xpath1 = XmpPathParser.ExpandXPath(sourceNS, sourceRoot);
      XmpPath xpath2 = XmpPathParser.ExpandXPath(destNS, destRoot);
      XmpMeta xmpMeta3 = (XmpMeta) source;
      XmpMeta xmpMeta4 = (XmpMeta) dest;
      XmpNode node = XmpNodeUtils.FindNode(xmpMeta3.GetRoot(), xpath1, false, (PropertyOptions) null);
      if (node == null)
        throw new XmpException("Can't find source subtree", XmpErrorCode.BadXPath);
      XmpNode destination = XmpNodeUtils.FindNode(xmpMeta4.GetRoot(), xpath2, false, (PropertyOptions) null) == null ? XmpNodeUtils.FindNode(xmpMeta4.GetRoot(), xpath2, true, (PropertyOptions) null) : throw new XmpException("Destination subtree must not exist", XmpErrorCode.BadXPath);
      if (destination == null)
        throw new XmpException("Can't create destination root node", XmpErrorCode.BadXPath);
      if (source == dest)
      {
        for (XmpNode xmpNode = destination; xmpNode != null; xmpNode = xmpNode.Parent)
        {
          if (xmpNode == node)
            throw new XmpException("Destination subtree is within the source subtree", XmpErrorCode.BadXPath);
        }
      }
      destination.Value = node.Value;
      destination.Options = node.Options;
      node.CloneSubtree(destination, false);
    }
  }

  private static void CheckSeparator(string separator)
  {
    bool flag = false;
    foreach (char ch in separator)
    {
      switch (XmpUtils.ClassifyCharacter(ch))
      {
        case XmpUtils.UnicodeKind.Space:
          continue;
        case XmpUtils.UnicodeKind.Semicolon:
          flag = !flag ? true : throw new XmpException("Separator can have only one semicolon", XmpErrorCode.BadParam);
          continue;
        default:
          throw new XmpException("Separator can have only spaces and one semicolon", XmpErrorCode.BadParam);
      }
    }
    if (!flag)
      throw new XmpException("Separator must have one semicolon", XmpErrorCode.BadParam);
  }

  private static char CheckQuotes(string quotes, char openQuote)
  {
    if (XmpUtils.ClassifyCharacter(openQuote) != XmpUtils.UnicodeKind.Quote)
      throw new XmpException("Invalid quoting character", XmpErrorCode.BadParam);
    char ch;
    if (quotes.Length == 1)
    {
      ch = openQuote;
    }
    else
    {
      ch = quotes[1];
      if (XmpUtils.ClassifyCharacter(ch) != XmpUtils.UnicodeKind.Quote)
        throw new XmpException("Invalid quoting character", XmpErrorCode.BadParam);
    }
    return (int) ch == (int) XmpUtils.GetClosingQuote(openQuote) ? ch : throw new XmpException("Mismatched quote pair", XmpErrorCode.BadParam);
  }

  private static XmpUtils.UnicodeKind ClassifyCharacter(char ch)
  {
    if (" 　〿".IndexOf(ch) >= 0 || ' ' <= ch && ch <= '\u200B')
      return XmpUtils.UnicodeKind.Space;
    if (",，､﹐﹑、،՝".IndexOf(ch) >= 0)
      return XmpUtils.UnicodeKind.Comma;
    if (";；﹔؛;".IndexOf(ch) >= 0)
      return XmpUtils.UnicodeKind.Semicolon;
    if ("\"«»〝〞〟―‹›".IndexOf(ch) >= 0 || '〈' <= ch && ch <= '』' || '‘' <= ch && ch <= '‟')
      return XmpUtils.UnicodeKind.Quote;
    return ch < ' ' || "\u2028\u2029".IndexOf(ch) >= 0 ? XmpUtils.UnicodeKind.Control : XmpUtils.UnicodeKind.Normal;
  }

  private static char GetClosingQuote(char openQuote)
  {
    switch (openQuote)
    {
      case '"':
        return '"';
      case '«':
        return '»';
      case '»':
        return '«';
      case '―':
        return '―';
      case '‘':
        return '’';
      case '‚':
        return '‛';
      case '“':
        return '”';
      case '„':
        return '‟';
      case '‹':
        return '›';
      case '›':
        return '‹';
      case '〈':
        return '〉';
      case '《':
        return '》';
      case '「':
        return '」';
      case '『':
        return '』';
      case '〝':
        return '〟';
      default:
        return char.MinValue;
    }
  }

  private static string ApplyQuotes(
    string item,
    char openQuote,
    char closeQuote,
    bool allowCommas)
  {
    if (item == null)
      item = string.Empty;
    bool flag = false;
    int index1;
    for (index1 = 0; index1 < item.Length; ++index1)
    {
      XmpUtils.UnicodeKind unicodeKind = XmpUtils.ClassifyCharacter(item[index1]);
      if (index1 != 0 || unicodeKind != XmpUtils.UnicodeKind.Quote)
      {
        if (unicodeKind == XmpUtils.UnicodeKind.Space)
        {
          if (!flag)
            flag = true;
          else
            break;
        }
        else
        {
          flag = false;
          if (unicodeKind == XmpUtils.UnicodeKind.Semicolon || unicodeKind == XmpUtils.UnicodeKind.Control || unicodeKind == XmpUtils.UnicodeKind.Comma && !allowCommas)
            break;
        }
      }
      else
        break;
    }
    if (index1 < item.Length)
    {
      StringBuilder stringBuilder = new StringBuilder(item.Length + 2);
      int length = 0;
      while (length <= index1 && XmpUtils.ClassifyCharacter(item[index1]) != XmpUtils.UnicodeKind.Quote)
        ++length;
      stringBuilder.Append(openQuote).Append(item.Substring(0, length));
      for (int index2 = length; index2 < item.Length; ++index2)
      {
        stringBuilder.Append(item[index2]);
        if (XmpUtils.ClassifyCharacter(item[index2]) == XmpUtils.UnicodeKind.Quote && XmpUtils.IsSurroundingQuote(item[index2], openQuote, closeQuote))
          stringBuilder.Append(item[index2]);
      }
      stringBuilder.Append(closeQuote);
      item = stringBuilder.ToString();
    }
    return item;
  }

  private static bool IsSurroundingQuote(char ch, char openQuote, char closeQuote)
  {
    return (int) ch == (int) openQuote || XmpUtils.IsClosingQuote(ch, openQuote, closeQuote);
  }

  private static bool IsClosingQuote(char ch, char openQuote, char closeQuote)
  {
    return (int) ch == (int) closeQuote || openQuote == '〝' && ch == '〞' || ch == '〟';
  }

  private static bool MoveOneProperty(
    XmpMeta stdXMP,
    XmpMeta extXMP,
    string schemaURI,
    string propName)
  {
    XmpNode node = (XmpNode) null;
    XmpNode schemaNode1 = XmpNodeUtils.FindSchemaNode(stdXMP.GetRoot(), schemaURI, false);
    if (schemaNode1 != null)
      node = XmpNodeUtils.FindChildNode(schemaNode1, propName, false);
    if (node == null)
      return false;
    XmpNode schemaNode2 = XmpNodeUtils.FindSchemaNode(extXMP.GetRoot(), schemaURI, true);
    node.Parent = schemaNode2;
    schemaNode2.IsImplicit = false;
    schemaNode2.AddChild(node);
    schemaNode1.RemoveChild(node);
    if (!schemaNode1.HasChildren)
      schemaNode1.Parent.RemoveChild(schemaNode1);
    return true;
  }

  private static int EstimateSizeForJPEG(XmpNode xmpNode)
  {
    int num1 = 0;
    int length = xmpNode.Name.Length;
    bool flag = !xmpNode.Options.IsArray;
    int num2;
    if (xmpNode.Options.IsSimple)
    {
      if (flag)
        num1 += length + 3;
      num2 = num1 + xmpNode.Value.Length;
    }
    else if (xmpNode.Options.IsArray)
    {
      if (flag)
        num1 += 2 * length + 5;
      int childrenLength = xmpNode.GetChildrenLength();
      num2 = num1 + 19 + childrenLength * 17;
      for (int index = 1; index <= childrenLength; ++index)
        num2 += XmpUtils.EstimateSizeForJPEG(xmpNode.GetChild(index));
    }
    else
    {
      if (flag)
        num1 += 2 * length + 5;
      num2 = num1 + 25;
      int childrenLength = xmpNode.GetChildrenLength();
      for (int index = 1; index <= childrenLength; ++index)
        num2 += XmpUtils.EstimateSizeForJPEG(xmpNode.GetChild(index));
    }
    return num2;
  }

  private static void PutObjectsInMultiMap(
    SortedDictionary<int, List<List<string>>> multiMap,
    int key,
    List<string> stringPair)
  {
    if (multiMap == null)
      return;
    List<List<string>> stringListList;
    if (!multiMap.TryGetValue(key, out stringListList))
    {
      stringListList = new List<List<string>>();
      multiMap[key] = stringListList;
    }
    stringListList.Add(stringPair);
  }

  private static List<string> GetBiggestEntryInMultiMap(
    SortedDictionary<int, List<List<string>>> multiMap)
  {
    if (multiMap == null || multiMap.Count == 0)
      return (List<string>) null;
    List<List<string>> multi = multiMap[multiMap.Keys.Last<int>()];
    List<string> biggestEntryInMultiMap = multi[0];
    multi.RemoveAt(0);
    if (multi.Count == 0)
      multiMap.Remove(multiMap.Keys.Last<int>());
    return biggestEntryInMultiMap;
  }

  private static void CreateEstimatedSizeMap(
    XmpMeta stdXMP,
    SortedDictionary<int, List<List<string>>> propSizes)
  {
    for (int childrenLength1 = stdXMP.GetRoot().GetChildrenLength(); childrenLength1 > 0; --childrenLength1)
    {
      XmpNode child1 = stdXMP.GetRoot().GetChild(childrenLength1);
      for (int childrenLength2 = child1.GetChildrenLength(); childrenLength2 > 0; --childrenLength2)
      {
        XmpNode child2 = child1.GetChild(childrenLength2);
        if (!child1.Name.Equals("http://ns.adobe.com/xmp/note/") || !child2.Name.Equals("xmpNote:HasExtendedXMP"))
        {
          int key = XmpUtils.EstimateSizeForJPEG(child2);
          List<string> stringPair = new List<string>()
          {
            child1.Name,
            child2.Name
          };
          XmpUtils.PutObjectsInMultiMap(propSizes, key, stringPair);
        }
      }
    }
  }

  private static int MoveLargestProperty(
    XmpMeta stdXMP,
    XmpMeta extXMP,
    SortedDictionary<int, List<List<string>>> propSizes)
  {
    int num = propSizes.Keys.Last<int>();
    List<string> biggestEntryInMultiMap = XmpUtils.GetBiggestEntryInMultiMap(propSizes);
    XmpUtils.MoveOneProperty(stdXMP, extXMP, biggestEntryInMultiMap[0], biggestEntryInMultiMap[1]);
    return num;
  }

  public static void PackageForJPEG(
    IXmpMeta origXMPImpl,
    StringBuilder stdStr,
    StringBuilder extStr,
    StringBuilder digestStr)
  {
    XmpMeta xmp = (XmpMeta) origXMPImpl;
    int length1 = "<?xpacket end=\"w\"?>".Length;
    XmpMeta xmpMeta1 = new XmpMeta();
    XmpMeta xmpMeta2 = new XmpMeta();
    SerializeOptions options = new SerializeOptions(64 /*0x40*/)
    {
      Padding = 0,
      Indent = "",
      BaseIndent = 0,
      Newline = " "
    };
    string str1 = XmpMetaFactory.SerializeToString((IXmpMeta) xmp, options);
    if (str1.Length > 65000)
    {
      xmpMeta1.GetRoot().Options = xmp.GetRoot().Options;
      xmpMeta1.GetRoot().Name = xmp.GetRoot().Name;
      xmpMeta1.GetRoot().Value = xmp.GetRoot().Value;
      xmp.GetRoot().CloneSubtree(xmpMeta1.GetRoot(), false);
      if (xmpMeta1.DoesPropertyExist("http://ns.adobe.com/xap/1.0/", "Thumbnails"))
      {
        xmpMeta1.DeleteProperty("http://ns.adobe.com/xap/1.0/", "Thumbnails");
        str1 = XmpMetaFactory.SerializeToString((IXmpMeta) xmpMeta1, options);
      }
    }
    if (str1.Length > 65000)
    {
      xmpMeta1.SetProperty("http://ns.adobe.com/xmp/note/", "HasExtendedXMP", (object) "123456789-123456789-123456789-12", new PropertyOptions(0));
      XmpNode schemaNode = XmpNodeUtils.FindSchemaNode(xmpMeta1.GetRoot(), "http://ns.adobe.com/camera-raw-settings/1.0/", false);
      if (schemaNode != null)
      {
        schemaNode.Parent = xmpMeta2.GetRoot();
        xmpMeta2.GetRoot().AddChild(schemaNode);
        xmpMeta1.GetRoot().RemoveChild(schemaNode);
        str1 = XmpMetaFactory.SerializeToString((IXmpMeta) xmpMeta1, options);
      }
    }
    if (str1.Length > 65000 && XmpUtils.MoveOneProperty(xmpMeta1, xmpMeta2, "http://ns.adobe.com/photoshop/1.0/", "photoshop:History"))
      str1 = XmpMetaFactory.SerializeToString((IXmpMeta) xmpMeta1, options);
    if (str1.Length > 65000)
    {
      SortedDictionary<int, List<List<string>>> propSizes = new SortedDictionary<int, List<List<string>>>();
      XmpUtils.CreateEstimatedSizeMap(xmpMeta1, propSizes);
      int num;
      for (; str1.Length > 65000 && propSizes.Count != 0; str1 = XmpMetaFactory.SerializeToString((IXmpMeta) xmpMeta1, options))
      {
        for (int length2 = str1.Length; length2 > 65000 && propSizes.Count != 0; length2 -= num)
        {
          num = XmpUtils.MoveLargestProperty(xmpMeta1, xmpMeta2, propSizes);
          if (num > length2)
            num = length2;
        }
      }
    }
    if (str1.Length > 65000)
      throw new XmpException("Can't reduce XMP enough for JPEG file", XmpErrorCode.InternalFailure);
    if (xmpMeta2.GetRoot().GetChildrenLength() == 0)
    {
      stdStr.Append(str1);
    }
    else
    {
      string str2 = XmpMetaFactory.SerializeToString((IXmpMeta) xmpMeta2, new SerializeOptions(80 /*0x50*/));
      extStr.Append(str2);
      xmpMeta1.SetProperty("http://ns.adobe.com/xmp/note/", "HasExtendedXMP", (object) digestStr.ToString(), new PropertyOptions(0));
      string str3 = XmpMetaFactory.SerializeToString((IXmpMeta) xmpMeta1, options);
      stdStr.Append(str3);
    }
    int repeatCount = 65000 - stdStr.Length;
    if (repeatCount > 2047 /*0x07FF*/)
      repeatCount = 2047 /*0x07FF*/;
    int startIndex = stdStr.ToString().IndexOf("<?xpacket end=\"w\"?>");
    int length3 = stdStr.Length - startIndex;
    stdStr.Remove(startIndex, length3);
    stdStr.Append(' ', repeatCount);
    stdStr.Append("<?xpacket end=\"w\"?>").ToString();
  }

  public static void MergeFromJPEG(IXmpMeta fullXMP, IXmpMeta extendedXMP)
  {
    TemplateOptions actions = new TemplateOptions(48 /*0x30*/);
    XmpUtils.ApplyTemplate(fullXMP, extendedXMP, actions);
    fullXMP.DeleteProperty("http://ns.adobe.com/xmp/note/", "HasExtendedXMP");
  }

  public static void ApplyTemplate(IXmpMeta origXMP, IXmpMeta tempXMP, TemplateOptions actions)
  {
    XmpMeta destXmp = (XmpMeta) origXMP;
    XmpMeta xmpMeta = (XmpMeta) tempXMP;
    bool flag1 = (actions.GetOptions() & 2) != 0;
    bool mergeCompound = (actions.GetOptions() & 64 /*0x40*/) != 0;
    bool flag2 = (actions.GetOptions() & 16 /*0x10*/) != 0;
    bool flag3 = (actions.GetOptions() & 128 /*0x80*/) != 0;
    bool replaceOldValues = flag2 | flag3;
    bool deleteEmptyValues = flag3 & !flag1;
    bool flag4 = (actions.GetOptions() & 32 /*0x20*/) != 0;
    if (flag1)
    {
      for (int childrenLength1 = destXmp.GetRoot().GetChildrenLength(); childrenLength1 > 0; --childrenLength1)
      {
        XmpNode child1 = destXmp.GetRoot().GetChild(childrenLength1);
        XmpNode schemaNode = XmpNodeUtils.FindSchemaNode(xmpMeta.GetRoot(), child1.Name, false);
        if (schemaNode == null)
        {
          if (flag4)
          {
            child1.RemoveChildren();
          }
          else
          {
            for (int childrenLength2 = child1.GetChildrenLength(); childrenLength2 > 0; --childrenLength2)
            {
              XmpNode child2 = child1.GetChild(childrenLength2);
              if (!Utils.IsInternalProperty(child1.Name, child2.Name))
                child1.RemoveChild(childrenLength2);
            }
          }
        }
        else
        {
          for (int childrenLength3 = child1.GetChildrenLength(); childrenLength3 > 0; --childrenLength3)
          {
            XmpNode child3 = child1.GetChild(childrenLength3);
            if ((flag4 || !Utils.IsInternalProperty(child1.Name, child3.Name)) && XmpNodeUtils.FindChildNode(schemaNode, child3.Name, false) == null)
              child1.RemoveChild(childrenLength3);
          }
        }
        if (!child1.HasChildren)
          destXmp.GetRoot().RemoveChild(childrenLength1);
      }
    }
    if (!(mergeCompound | replaceOldValues))
      return;
    int num = 0;
    for (int childrenLength4 = xmpMeta.GetRoot().GetChildrenLength(); num < childrenLength4; ++num)
    {
      XmpNode child4 = xmpMeta.GetRoot().GetChild(num + 1);
      XmpNode xmpNode = XmpNodeUtils.FindSchemaNode(destXmp.GetRoot(), child4.Name, false);
      if (xmpNode == null)
      {
        xmpNode = new XmpNode(child4.Name, child4.Value, new PropertyOptions(int.MinValue));
        destXmp.GetRoot().AddChild(xmpNode);
        xmpNode.Parent = destXmp.GetRoot();
      }
      int index = 1;
      for (int childrenLength5 = child4.GetChildrenLength(); index <= childrenLength5; ++index)
      {
        XmpNode child5 = child4.GetChild(index);
        if (flag4 || !Utils.IsInternalProperty(child4.Name, child5.Name))
          XmpUtils.AppendSubtree(destXmp, child5, xmpNode, mergeCompound, replaceOldValues, deleteEmptyValues);
      }
      if (!xmpNode.HasChildren)
        destXmp.GetRoot().RemoveChild(xmpNode);
    }
  }

  private enum UnicodeKind
  {
    Normal,
    Space,
    Comma,
    Semicolon,
    Quote,
    Control,
  }
}
