// Decompiled with JetBrains decompiler
// Type: XmpCore.XmpPathFactory
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using XmpCore.Impl;
using XmpCore.Impl.XPath;

#nullable disable
namespace XmpCore;

public static class XmpPathFactory
{
  public static string ComposeArrayItemPath(string arrayName, int itemIndex)
  {
    if (itemIndex > 0)
      return $"{arrayName}[{itemIndex}]";
    if (itemIndex == -1)
      return arrayName + "[last()]";
    throw new XmpException("Array index must be larger than zero", XmpErrorCode.BadIndex);
  }

  public static string ComposeStructFieldPath(string fieldNs, string fieldName)
  {
    XmpPathFactory.AssertFieldNs(fieldNs);
    XmpPathFactory.AssertFieldName(fieldName);
    XmpPath xmpPath = XmpPathParser.ExpandXPath(fieldNs, fieldName);
    if (xmpPath.Size() != 2)
      throw new XmpException("The field name must be simple", XmpErrorCode.BadXPath);
    return "/" + xmpPath.GetSegment(1).Name;
  }

  public static string ComposeQualifierPath(string qualNs, string qualName)
  {
    XmpPathFactory.AssertQualNs(qualNs);
    XmpPathFactory.AssertQualName(qualName);
    XmpPath xmpPath = XmpPathParser.ExpandXPath(qualNs, qualName);
    if (xmpPath.Size() != 2)
      throw new XmpException("The qualifier name must be simple", XmpErrorCode.BadXPath);
    return "/?" + xmpPath.GetSegment(1).Name;
  }

  public static string ComposeLangSelector(string arrayName, string langName)
  {
    return $"{arrayName}[?xml:lang=\"{Utils.NormalizeLangValue(langName)}\"]";
  }

  public static string ComposeFieldSelector(
    string arrayName,
    string fieldNs,
    string fieldName,
    string fieldValue)
  {
    XmpPath xmpPath = XmpPathParser.ExpandXPath(fieldNs, fieldName);
    if (xmpPath.Size() != 2)
      throw new XmpException("The fieldName name must be simple", XmpErrorCode.BadXPath);
    return $"{arrayName}[{xmpPath.GetSegment(1).Name}=\"{fieldValue}\"]";
  }

  private static void AssertQualNs(string qualNs)
  {
    if (string.IsNullOrEmpty(qualNs))
      throw new XmpException("Empty qualifier namespace URI", XmpErrorCode.BadSchema);
  }

  private static void AssertQualName(string qualName)
  {
    if (string.IsNullOrEmpty(qualName))
      throw new XmpException("Empty qualifier name", XmpErrorCode.BadXPath);
  }

  private static void AssertFieldNs(string fieldNs)
  {
    if (string.IsNullOrEmpty(fieldNs))
      throw new XmpException("Empty field namespace URI", XmpErrorCode.BadSchema);
  }

  private static void AssertFieldName(string fieldName)
  {
    if (string.IsNullOrEmpty(fieldName))
      throw new XmpException("Empty f name", XmpErrorCode.BadXPath);
  }
}
