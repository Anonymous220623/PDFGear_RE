// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageContents.PageFormObjectUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using PDFKit.Utils.PageHeaderFooters;
using PDFKit.Utils.XObjects;
using System;
using System.Collections.Generic;

#nullable disable
namespace PDFKit.Utils.PageContents;

internal static class PageFormObjectUtils
{
  internal static Dictionary<int, PageFormObjectUtils.InternalHeaderFooterData> GetHeaderFooterDataForGenerateContent(
    PdfPage page)
  {
    if (page == null)
      return (Dictionary<int, PageFormObjectUtils.InternalHeaderFooterData>) null;
    System.Collections.Generic.IReadOnlyList<HeaderFooterData> headerFooterSettings = PageHeaderFooterUtils.GetPageHeaderFooterSettings(page);
    if (headerFooterSettings == null || headerFooterSettings.Count == 0)
      return (Dictionary<int, PageFormObjectUtils.InternalHeaderFooterData>) null;
    Dictionary<int, PageFormObjectUtils.InternalHeaderFooterData> forGenerateContent = new Dictionary<int, PageFormObjectUtils.InternalHeaderFooterData>();
    int count = page.PageObjects.Count;
    foreach (HeaderFooterData headerFooterData1 in (IEnumerable<HeaderFooterData>) headerFooterSettings)
    {
      System.Collections.Generic.IReadOnlyList<int> intList;
      if (headerFooterData1.FormObjects != null && headerFooterData1.FormObjects.TryGetValue(page.PageIndex, out intList))
      {
        foreach (int num in (IEnumerable<int>) intList)
        {
          if (num >= 0 && num < count && !forGenerateContent.ContainsKey(num) && page.PageObjects[num] is PdfFormObject pageObject)
          {
            PageFormObjectUtils.InternalHeaderFooterData headerFooterData2 = new PageFormObjectUtils.InternalHeaderFooterData()
            {
              OcgsIndirectNum = PageFormObjectUtils.GetOCDictionaryIndex(pageObject),
              DocSettingsIndirectNum = PageFormObjectUtils.GetDocSettingsIndirectNum(pageObject),
              MarkedContentStr = PageFormObjectUtils.GetMarkedContentStr(pageObject)
            };
            forGenerateContent[num] = headerFooterData2;
          }
        }
      }
    }
    return forGenerateContent;
  }

  internal static void ApplyHeaderFooterDataFromGenerateContent(
    PdfPage page,
    Dictionary<int, PageFormObjectUtils.InternalHeaderFooterData> dict)
  {
    if (page == null || dict == null || dict.Count == 0)
      return;
    int count = page.PageObjects.Count;
    string modificationDateString = PdfAttributeUtils.ConvertToModificationDateString(DateTimeOffset.Now);
    foreach (KeyValuePair<int, PageFormObjectUtils.InternalHeaderFooterData> keyValuePair in dict)
    {
      int key = keyValuePair.Key;
      PageFormObjectUtils.InternalHeaderFooterData headerFooterData = keyValuePair.Value;
      if (key >= 0 && key < count && page.PageObjects[key] is PdfFormObject pageObject)
      {
        PageHeaderFooterUtils.WritePageHeaderFooterSettings(page, pageObject.Stream, modificationDateString, headerFooterData.OcgsIndirectNum, headerFooterData.DocSettingsIndirectNum);
        PdfTypeDictionary parameters = PdfTypeDictionary.Create();
        parameters["Type"] = (PdfTypeBase) PdfTypeName.Create("Pagination");
        parameters["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Header");
        parameters["Contents"] = (PdfTypeBase) PdfTypeString.Create(headerFooterData.MarkedContentStr ?? "", true);
        PdfMarkedContent pdfMarkedContent = new PdfMarkedContent("Artifact", false, PropertyListTypes.DirectDict, parameters);
        pageObject.MarkedContent.Add(pdfMarkedContent);
      }
    }
  }

  private static int GetOCDictionaryIndex(PdfFormObject formObj)
  {
    PdfTypeBase pdfTypeBase1;
    if (formObj?.Stream?.Dictionary == null || !formObj.Stream.Dictionary.TryGetValue("OC", out pdfTypeBase1) || !pdfTypeBase1.Is<PdfTypeDictionary>())
      return -1;
    PdfTypeBase pdfTypeBase2;
    PdfTypeIndirect pdfTypeIndirect;
    int num;
    if (pdfTypeBase1.As<PdfTypeDictionary>().TryGetValue("OCGs", out pdfTypeBase2) && pdfTypeBase2.Is<PdfTypeDictionary>())
    {
      pdfTypeIndirect = pdfTypeBase2 as PdfTypeIndirect;
      num = pdfTypeIndirect != null ? 1 : 0;
    }
    else
      num = 0;
    return num != 0 ? pdfTypeIndirect.Number : -1;
  }

  private static int GetDocSettingsIndirectNum(PdfFormObject formObj)
  {
    if (formObj?.Stream?.Dictionary == null)
      return -1;
    PdfTypeDictionary directValue1 = XObjectHelper.GetDirectValue<PdfTypeDictionary>(formObj?.Stream?.Dictionary, "PieceInfo");
    if (directValue1 == null)
      return -1;
    PdfTypeDictionary directValue2 = XObjectHelper.GetDirectValue<PdfTypeDictionary>(directValue1, "ADBE_CompoundType");
    if (directValue2 == null)
      return -1;
    PdfTypeBase pdfTypeBase;
    PdfTypeIndirect pdfTypeIndirect;
    int num;
    if (directValue2.TryGetValue("DocSettings", out pdfTypeBase) && pdfTypeBase.Is<PdfTypeStream>())
    {
      pdfTypeIndirect = pdfTypeBase as PdfTypeIndirect;
      num = pdfTypeIndirect != null ? 1 : 0;
    }
    else
      num = 0;
    return num != 0 ? pdfTypeIndirect.Number : -1;
  }

  private static string GetMarkedContentStr(PdfFormObject formObject)
  {
    if (formObject?.MarkedContent != null && formObject.MarkedContent.Count > 0)
    {
      foreach (PdfMarkedContent pdfMarkedContent in formObject.MarkedContent)
      {
        if (pdfMarkedContent.Tag == "Artifact")
        {
          PdfTypeDictionary parameters = pdfMarkedContent.Parameters;
          PdfTypeBase pdfTypeBase;
          if (parameters != null && parameters.TryGetValue("Contents", out pdfTypeBase) && pdfTypeBase.Is<PdfTypeName>())
            return pdfTypeBase.As<PdfTypeName>().Value ?? "";
        }
      }
    }
    return "";
  }

  internal class InternalHeaderFooterData
  {
    public int OcgsIndirectNum { get; set; }

    public int DocSettingsIndirectNum { get; set; }

    public string MarkedContentStr { get; set; }
  }
}
