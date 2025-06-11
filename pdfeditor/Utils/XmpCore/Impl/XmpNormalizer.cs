// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpNormalizer
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System.Collections;
using System.Collections.Generic;
using XmpCore.Impl.XPath;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public static class XmpNormalizer
{
  private static readonly IDictionary _dcArrayForms;

  static XmpNormalizer()
  {
    PropertyOptions propertyOptions1 = new PropertyOptions()
    {
      IsArray = true
    };
    PropertyOptions propertyOptions2 = new PropertyOptions()
    {
      IsArray = true,
      IsArrayOrdered = true
    };
    PropertyOptions propertyOptions3 = new PropertyOptions()
    {
      IsArray = true,
      IsArrayOrdered = true,
      IsArrayAlternate = true,
      IsArrayAltText = true
    };
    XmpNormalizer._dcArrayForms = (IDictionary) new Dictionary<string, PropertyOptions>()
    {
      ["dc:contributor"] = propertyOptions1,
      ["dc:language"] = propertyOptions1,
      ["dc:publisher"] = propertyOptions1,
      ["dc:relation"] = propertyOptions1,
      ["dc:subject"] = propertyOptions1,
      ["dc:type"] = propertyOptions1,
      ["dc:creator"] = propertyOptions2,
      ["dc:date"] = propertyOptions2,
      ["dc:description"] = propertyOptions3,
      ["dc:rights"] = propertyOptions3,
      ["dc:title"] = propertyOptions3
    };
  }

  internal static IXmpMeta Process(XmpMeta xmp, ParseOptions options)
  {
    XmpNode root = xmp.GetRoot();
    XmpNormalizer.TouchUpDataModel(xmp);
    XmpNormalizer.MoveExplicitAliases(root, options);
    XmpNormalizer.TweakOldXmp(root);
    XmpNormalizer.DeleteEmptySchemas(root);
    return (IXmpMeta) xmp;
  }

  private static void TweakOldXmp(XmpNode tree)
  {
    if (tree.Name == null || tree.Name.Length < 36)
      return;
    string uuid = tree.Name.ToLower();
    if (uuid.StartsWith("uuid:"))
      uuid = uuid.Substring(5);
    if (!Utils.CheckUuidFormat(uuid))
      return;
    XmpPath xpath = XmpPathParser.ExpandXPath("http://ns.adobe.com/xap/1.0/mm/", "InstanceID");
    XmpNode node = XmpNodeUtils.FindNode(tree, xpath, true, (PropertyOptions) null);
    if (node == null)
      throw new XmpException("Failure creating xmpMM:InstanceID", XmpErrorCode.InternalFailure);
    node.Options = (PropertyOptions) null;
    node.Value = "uuid:" + uuid;
    node.RemoveChildren();
    node.RemoveQualifiers();
    tree.Name = (string) null;
  }

  private static void TouchUpDataModel(XmpMeta xmp)
  {
    XmpNodeUtils.FindSchemaNode(xmp.GetRoot(), "http://purl.org/dc/elements/1.1/", true);
    IIterator iterator = xmp.GetRoot().IterateChildren();
    while (iterator.HasNext())
    {
      XmpNode xmpNode = (XmpNode) iterator.Next();
      switch (xmpNode.Name)
      {
        case "http://purl.org/dc/elements/1.1/":
          XmpNormalizer.NormalizeDcArrays(xmpNode);
          continue;
        case "http://ns.adobe.com/exif/1.0/":
          XmpNormalizer.FixGpsTimeStamp(xmpNode);
          XmpNode childNode1 = XmpNodeUtils.FindChildNode(xmpNode, "exif:UserComment", false);
          if (childNode1 != null)
          {
            if (childNode1.Options.IsSimple)
            {
              XmpNode node = new XmpNode("[]", childNode1.Value, childNode1.Options);
              node.Parent = childNode1;
              for (int qualifierLength = childNode1.GetQualifierLength(); qualifierLength > 0; --qualifierLength)
                node.AddQualifier(childNode1.GetQualifier(childNode1.GetQualifierLength() - qualifierLength));
              childNode1.RemoveQualifiers();
              if (!node.Options.HasLanguage)
              {
                PropertyOptions options = new PropertyOptions();
                options.SetOption(16 /*0x10*/, true);
                XmpNode qualNode = new XmpNode("xml:lang", "x-default", options);
                node.AddQualifier(qualNode);
                node.Options.SetOption(16 /*0x10*/, true);
                node.Options.SetOption(64 /*0x40*/, true);
              }
              childNode1.AddChild(node);
              childNode1.Options = new PropertyOptions(7680);
              childNode1.Value = "";
            }
            XmpNormalizer.RepairAltText(childNode1);
            continue;
          }
          continue;
        case "http://ns.adobe.com/xmp/1.0/DynamicMedia/":
          XmpNode childNode2 = XmpNodeUtils.FindChildNode(xmpNode, "xmpDM:copyright", false);
          if (childNode2 != null)
          {
            XmpNormalizer.MigrateAudioCopyright((IXmpMeta) xmp, childNode2);
            continue;
          }
          continue;
        case "http://ns.adobe.com/xap/1.0/rights/":
          XmpNode childNode3 = XmpNodeUtils.FindChildNode(xmpNode, "xmpRights:UsageTerms", false);
          if (childNode3 != null)
          {
            XmpNormalizer.RepairAltText(childNode3);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  private static void NormalizeDcArrays(XmpNode dcSchema)
  {
    for (int index = 1; index <= dcSchema.GetChildrenLength(); ++index)
    {
      XmpNode child = dcSchema.GetChild(index);
      PropertyOptions dcArrayForm = (PropertyOptions) XmpNormalizer._dcArrayForms[(object) child.Name];
      if (dcArrayForm != null)
      {
        if (child.Options.IsSimple)
        {
          XmpNode node = new XmpNode(child.Name, dcArrayForm);
          child.Name = "[]";
          node.AddChild(child);
          dcSchema.ReplaceChild(index, node);
          if (dcArrayForm.IsArrayAltText && !child.Options.HasLanguage)
          {
            XmpNode qualNode = new XmpNode("xml:lang", "x-default", (PropertyOptions) null);
            child.AddQualifier(qualNode);
          }
        }
        else
        {
          child.Options.SetOption(7680, false);
          child.Options.MergeWith(dcArrayForm);
          if (dcArrayForm.IsArrayAltText)
            XmpNormalizer.RepairAltText(child);
        }
      }
    }
  }

  private static void RepairAltText(XmpNode arrayNode)
  {
    if (arrayNode == null || !arrayNode.Options.IsArray)
      return;
    arrayNode.Options.IsArrayOrdered = true;
    arrayNode.Options.IsArrayAlternate = true;
    arrayNode.Options.IsArrayAltText = true;
    IIterator iterator = arrayNode.IterateChildren();
    while (iterator.HasNext())
    {
      XmpNode xmpNode = (XmpNode) iterator.Next();
      if (xmpNode.Options.IsCompositeProperty)
        iterator.Remove();
      else if (!xmpNode.Options.HasLanguage)
      {
        if (string.IsNullOrEmpty(xmpNode.Value))
        {
          iterator.Remove();
        }
        else
        {
          XmpNode qualNode = new XmpNode("xml:lang", "x-repair", (PropertyOptions) null);
          xmpNode.AddQualifier(qualNode);
        }
      }
    }
  }

  private static void MoveExplicitAliases(XmpNode tree, ParseOptions options)
  {
    if (!tree.HasAliases)
      return;
    tree.HasAliases = false;
    bool strictAliasing = options.StrictAliasing;
    Iterator<object> iterator = tree.GetUnmodifiableChildren().Iterator<object>();
    while (iterator.HasNext())
    {
      XmpNode xmpNode1 = (XmpNode) iterator.Next();
      if (xmpNode1.HasAliases)
      {
        IIterator propertyIt = xmpNode1.IterateChildren();
        while (propertyIt.HasNext())
        {
          XmpNode xmpNode2 = (XmpNode) propertyIt.Next();
          if (xmpNode2.IsAlias)
          {
            xmpNode2.IsAlias = false;
            IXmpAliasInfo alias = XmpMetaFactory.SchemaRegistry.FindAlias(xmpNode2.Name);
            if (alias != null)
            {
              XmpNode schemaNode = XmpNodeUtils.FindSchemaNode(tree, alias.Namespace, (string) null, true);
              schemaNode.IsImplicit = false;
              XmpNode childNode = XmpNodeUtils.FindChildNode(schemaNode, alias.Prefix + alias.PropName, false);
              if (childNode == null)
              {
                if (alias.AliasForm.IsSimple())
                {
                  string str = alias.Prefix + alias.PropName;
                  xmpNode2.Name = str;
                  schemaNode.AddChild(xmpNode2);
                  propertyIt.Remove();
                }
                else
                {
                  XmpNode xmpNode3 = new XmpNode(alias.Prefix + alias.PropName, alias.AliasForm.ToPropertyOptions());
                  schemaNode.AddChild(xmpNode3);
                  XmpNormalizer.TransplantArrayItemAlias(propertyIt, xmpNode2, xmpNode3);
                }
              }
              else if (alias.AliasForm.IsSimple())
              {
                if (strictAliasing)
                  XmpNormalizer.CompareAliasedSubtrees(xmpNode2, childNode, true);
                propertyIt.Remove();
              }
              else
              {
                XmpNode baseNode = (XmpNode) null;
                if (alias.AliasForm.IsArrayAltText)
                {
                  int index = XmpNodeUtils.LookupLanguageItem(childNode, "x-default");
                  if (index != -1)
                    baseNode = childNode.GetChild(index);
                }
                else if (childNode.HasChildren)
                  baseNode = childNode.GetChild(1);
                if (baseNode == null)
                  XmpNormalizer.TransplantArrayItemAlias(propertyIt, xmpNode2, childNode);
                else if (strictAliasing)
                  XmpNormalizer.CompareAliasedSubtrees(xmpNode2, baseNode, true);
                propertyIt.Remove();
              }
            }
          }
        }
        xmpNode1.HasAliases = false;
      }
    }
  }

  private static void TransplantArrayItemAlias(
    IIterator propertyIt,
    XmpNode childNode,
    XmpNode baseArray)
  {
    if (baseArray.Options.IsArrayAltText)
    {
      if (childNode.Options.HasLanguage)
        throw new XmpException("Alias to x-default already has a language qualifier", XmpErrorCode.BadXmp);
      XmpNode qualNode = new XmpNode("xml:lang", "x-default", (PropertyOptions) null);
      childNode.AddQualifier(qualNode);
    }
    propertyIt.Remove();
    childNode.Name = "[]";
    baseArray.AddChild(childNode);
  }

  private static void FixGpsTimeStamp(XmpNode exifSchema)
  {
    XmpNode childNode = XmpNodeUtils.FindChildNode(exifSchema, "exif:GPSTimeStamp", false);
    if (childNode == null)
      return;
    try
    {
      IXmpDateTime date1 = XmpCore.XmpUtils.ConvertToDate(childNode.Value);
      if (date1.Year != 0 || date1.Month != 0 || date1.Day != 0)
        return;
      XmpNode xmpNode = XmpNodeUtils.FindChildNode(exifSchema, "exif:DateTimeOriginal", false) ?? XmpNodeUtils.FindChildNode(exifSchema, "exif:DateTimeDigitized", false);
      if (xmpNode == null)
        return;
      IXmpDateTime date2 = XmpCore.XmpUtils.ConvertToDate(xmpNode.Value);
      Calendar calendar = date1.Calendar;
      calendar.Set(CalendarEnum.Year, date2.Year);
      calendar.Set(CalendarEnum.Month, date2.Month);
      calendar.Set(CalendarEnum.DayOfMonth, date2.Day);
      IXmpDateTime xmpDateTime = (IXmpDateTime) new XmpDateTime(calendar);
      childNode.Value = XmpCore.XmpUtils.ConvertFromDate(xmpDateTime);
    }
    catch (XmpException ex)
    {
    }
  }

  private static void DeleteEmptySchemas(XmpNode tree)
  {
    IIterator iterator = tree.IterateChildren();
    while (iterator.HasNext())
    {
      if (!((XmpNode) iterator.Next()).HasChildren)
        iterator.Remove();
    }
  }

  private static void CompareAliasedSubtrees(XmpNode aliasNode, XmpNode baseNode, bool outerCall)
  {
    if (baseNode.Value != aliasNode.Value || aliasNode.GetChildrenLength() != baseNode.GetChildrenLength())
      throw new XmpException("Mismatch between alias and base nodes", XmpErrorCode.BadXmp);
    if (!outerCall && (baseNode.Name != aliasNode.Name || !aliasNode.Options.Equals((object) baseNode.Options) || aliasNode.GetQualifierLength() != baseNode.GetQualifierLength()))
      throw new XmpException("Mismatch between alias and base nodes", XmpErrorCode.BadXmp);
    IIterator iterator1 = aliasNode.IterateChildren();
    IIterator iterator2 = baseNode.IterateChildren();
    while (iterator1.HasNext() && iterator2.HasNext())
      XmpNormalizer.CompareAliasedSubtrees((XmpNode) iterator1.Next(), (XmpNode) iterator2.Next(), false);
    IIterator iterator3 = aliasNode.IterateQualifier();
    IIterator iterator4 = baseNode.IterateQualifier();
    while (iterator3.HasNext() && iterator4.HasNext())
      XmpNormalizer.CompareAliasedSubtrees((XmpNode) iterator3.Next(), (XmpNode) iterator4.Next(), false);
  }

  private static void MigrateAudioCopyright(IXmpMeta xmp, XmpNode dmCopyright)
  {
    try
    {
      XmpNode schemaNode = XmpNodeUtils.FindSchemaNode(((XmpMeta) xmp).GetRoot(), "http://purl.org/dc/elements/1.1/", true);
      string str1 = dmCopyright.Value;
      XmpNode childNode = XmpNodeUtils.FindChildNode(schemaNode, "dc:rights", false);
      if (childNode == null || !childNode.HasChildren)
      {
        string itemValue = "\n\n" + str1;
        xmp.SetLocalizedText("http://purl.org/dc/elements/1.1/", "rights", string.Empty, "x-default", itemValue, (PropertyOptions) null);
      }
      else
      {
        int index = XmpNodeUtils.LookupLanguageItem(childNode, "x-default");
        if (index < 0)
        {
          string itemValue = childNode.GetChild(1).Value;
          xmp.SetLocalizedText("http://purl.org/dc/elements/1.1/", "rights", string.Empty, "x-default", itemValue, (PropertyOptions) null);
          index = XmpNodeUtils.LookupLanguageItem(childNode, "x-default");
        }
        XmpNode child = childNode.GetChild(index);
        string str2 = child.Value;
        int num = str2.IndexOf("\n\n");
        if (num < 0)
        {
          if (str2 != str1)
            child.Value = $"{str2}\n\n{str1}";
        }
        else if (str1 != str2.Substring(num + 2))
          child.Value = str2.Substring(0, num + 2) + str1;
      }
      dmCopyright.Parent.RemoveChild(dmCopyright);
    }
    catch (XmpException ex)
    {
    }
  }
}
