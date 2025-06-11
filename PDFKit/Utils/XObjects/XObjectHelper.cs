// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.XObjects.XObjectHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace PDFKit.Utils.XObjects;

public static class XObjectHelper
{
  public static PdfTypeDictionary GetXObjectDict(PdfPage page)
  {
    if (page == null || page.PageObjects == null || page.PageObjects.Count == 0)
      return (PdfTypeDictionary) null;
    PdfTypeDictionary directValue = XObjectHelper.GetDirectValue<PdfTypeDictionary>(page.Dictionary, "Resources");
    return directValue == null ? (PdfTypeDictionary) null : XObjectHelper.GetDirectValue<PdfTypeDictionary>(directValue, "XObject");
  }

  public static string GetDocSettingString(PdfPage page, PdfTypeStream xObject)
  {
    if (page == null || xObject == null)
      return string.Empty;
    PdfTypeDictionary dictionary = xObject.Dictionary;
    if (dictionary == null)
      return string.Empty;
    PdfTypeDictionary directValue1 = XObjectHelper.GetDirectValue<PdfTypeDictionary>(dictionary, "PieceInfo");
    if (directValue1 == null)
      return string.Empty;
    PdfTypeDictionary directValue2 = XObjectHelper.GetDirectValue<PdfTypeDictionary>(directValue1, "ADBE_CompoundType");
    if (directValue2 == null)
      return string.Empty;
    PdfTypeStream directValue3 = XObjectHelper.GetDirectValue<PdfTypeStream>(directValue2, "DocSettings");
    if (directValue3 == null)
      return string.Empty;
    string decodedText = directValue3.DecodedText;
    try
    {
      if (decodedText.ToLowerInvariant().Contains("<HeaderFooterSettings versions=\"8.0\"></HeaderFooterSettings>".ToLowerInvariant()))
        return string.Empty;
    }
    catch
    {
    }
    if (directValue3.DecodedData != null && directValue3.DecodedData.Length != 0 && decodedText != null && decodedText.ToLowerInvariant().Contains("utf-8"))
    {
      try
      {
        decodedText = Encoding.UTF8.GetString(directValue3.DecodedData);
      }
      catch
      {
      }
    }
    return decodedText;
  }

  public static IEnumerable<string> GetAllDocSettingsStrings(PdfPage page)
  {
    if (page != null)
    {
      PdfTypeDictionary resources = XObjectHelper.GetDirectValue<PdfTypeDictionary>(page.Dictionary, "Resources");
      if (resources != null)
      {
        PdfTypeDictionary xObject = XObjectHelper.GetDirectValue<PdfTypeDictionary>(resources, "XObject");
        if (xObject != null)
        {
          HashSet<string> hash = new HashSet<string>();
          foreach (string key1 in (IEnumerable<string>) xObject.Keys)
          {
            string key = key1;
            PdfTypeDictionary itemDict = XObjectHelper.GetDirectValue<PdfTypeStream>(xObject, key)?.Dictionary;
            if (itemDict != null)
            {
              PdfTypeDictionary pieceInfo = XObjectHelper.GetDirectValue<PdfTypeDictionary>(itemDict, "PieceInfo");
              if (pieceInfo != null)
              {
                PdfTypeDictionary ADBE_CompoundType = XObjectHelper.GetDirectValue<PdfTypeDictionary>(pieceInfo, "ADBE_CompoundType");
                if (ADBE_CompoundType != null)
                {
                  PdfTypeStream docSettings = XObjectHelper.GetDirectValue<PdfTypeStream>(ADBE_CompoundType, "DocSettings");
                  if (docSettings != null)
                  {
                    if (hash.Add(docSettings.DecodedText))
                      yield return docSettings.DecodedText;
                    itemDict = (PdfTypeDictionary) null;
                    pieceInfo = (PdfTypeDictionary) null;
                    ADBE_CompoundType = (PdfTypeDictionary) null;
                    docSettings = (PdfTypeStream) null;
                    key = (string) null;
                  }
                }
              }
            }
          }
        }
      }
    }
  }

  internal static T GetDirectValue<T>(PdfTypeDictionary dict, string key) where T : PdfTypeBase
  {
    PdfTypeBase pdfTypeBase;
    if (dict == null || string.IsNullOrEmpty(key) || !dict.TryGetValue(key, out pdfTypeBase))
    {
      pdfTypeBase = (PdfTypeBase) null;
      return default (T);
    }
    return pdfTypeBase.Is<T>() ? pdfTypeBase.As<T>() : default (T);
  }

  public static HeaderFooterSettings ConvertToHFSettings(string docSettings)
  {
    if (string.IsNullOrEmpty(docSettings))
      return (HeaderFooterSettings) null;
    if (!docSettings.Contains("HeaderFooterSettings"))
      return (HeaderFooterSettings) null;
    try
    {
      docSettings = docSettings.Trim();
      int startIndex = docSettings.IndexOf("<?xml", StringComparison.InvariantCultureIgnoreCase);
      if (startIndex == -1)
        return (HeaderFooterSettings) null;
      if (startIndex > 0)
        docSettings = docSettings.Substring(startIndex);
      XDocument xdocument = (XDocument) null;
      using (StringReader stringReader = new StringReader(docSettings))
        xdocument = XDocument.Load((TextReader) stringReader, LoadOptions.PreserveWhitespace);
      XElement xelement1 = xdocument.Element((XName) "HeaderFooterSettings");
      if (xelement1 == null)
        return (HeaderFooterSettings) null;
      HeaderFooterSettings hfSettings = new HeaderFooterSettings();
      hfSettings.Version = xelement1.Attribute((XName) "version")?.Value;
      XElement xelement2 = xelement1.Element((XName) "Font");
      if (xelement2 != null)
      {
        hfSettings.Font.Name = xelement2.Attribute((XName) "name")?.Value;
        hfSettings.Font.Size = ToDouble(xelement2.Attribute((XName) "size")?.Value);
      }
      XElement xelement3 = xelement1.Element((XName) "Color");
      if (xelement3 != null)
      {
        hfSettings.Color.B = ToDouble(xelement3.Attribute((XName) "b")?.Value);
        hfSettings.Color.R = ToDouble(xelement3.Attribute((XName) "r")?.Value);
        hfSettings.Color.G = ToDouble(xelement3.Attribute((XName) "g")?.Value);
      }
      XElement xelement4 = xelement1.Element((XName) "Margin");
      if (xelement4 != null)
      {
        hfSettings.Margin.Top = ToDouble(xelement4.Attribute((XName) "top")?.Value);
        hfSettings.Margin.Left = ToDouble(xelement4.Attribute((XName) "left")?.Value);
        hfSettings.Margin.Right = ToDouble(xelement4.Attribute((XName) "right")?.Value);
        hfSettings.Margin.Bottom = ToDouble(xelement4.Attribute((XName) "bottom")?.Value);
      }
      XElement xelement5 = xelement1.Element((XName) "Appearance");
      if (xelement5 != null)
      {
        hfSettings.Appearance.Shrink = ToBoolean(xelement5.Attribute((XName) "shrink")?.Value);
        hfSettings.Appearance.FixedPrint = ToBoolean(xelement5.Attribute((XName) "fixedprint")?.Value);
      }
      XElement xelement6 = xelement1.Element((XName) "PageRange");
      if (xelement6 != null)
      {
        hfSettings.PageRange.Start = ToInt(xelement6.Attribute((XName) "start")?.Value, -1);
        hfSettings.PageRange.End = ToInt(xelement6.Attribute((XName) "end")?.Value, -1);
        hfSettings.PageRange.Odd = ToBoolean(xelement6.Attribute((XName) "odd")?.Value);
        hfSettings.PageRange.Even = ToBoolean(xelement6.Attribute((XName) "even")?.Value);
      }
      XElement element1 = xelement1.Element((XName) "Page");
      if (element1 != null)
        SetPageValue(element1, hfSettings.Page);
      XElement element2 = xelement1.Element((XName) "Date");
      if (element2 != null)
        SetDateValue(element2, hfSettings.Date);
      XElement node1 = xelement1.Element((XName) "Header");
      if (node1 != null)
        SetHFValue(node1, hfSettings.Header);
      XElement node2 = xelement1.Element((XName) "Footer");
      if (node2 != null)
        SetHFValue(node2, hfSettings.Footer);
      return hfSettings;
    }
    catch
    {
    }
    return (HeaderFooterSettings) null;

    static double ToDouble(string value, double defaultValue = 0.0)
    {
      double result;
      return double.TryParse(value, out result) ? result : defaultValue;
    }

    static bool ToBoolean(string value, bool defaultValue = false)
    {
      bool result1;
      if (bool.TryParse(value, out result1))
        return result1;
      int result2;
      if (int.TryParse(value, out result2))
        return result2 != 0;
      double result3;
      return double.TryParse(value, out result3) && result3 != 0.0;
    }

    static int ToInt(string value, int defaultValue = 0)
    {
      int result;
      return int.TryParse(value, out result) ? result : defaultValue;
    }

    static void SetHFValue(XElement node, HeaderFooterSettings.HeaderFooterModel hf)
    {
      XElement element1 = node.Element((XName) "Left");
      if (element1 != null)
        SetLocationValue(element1, hf.Left);
      XElement element2 = node.Element((XName) "Center");
      if (element2 != null)
        SetLocationValue(element2, hf.Center);
      XElement element3 = node.Element((XName) "Right");
      if (element3 == null)
        return;
      SetLocationValue(element3, hf.Right);
    }

    static void SetLocationValue(XElement element, HeaderFooterSettings.LocationModel location)
    {
      foreach (XNode node in element.Nodes())
      {
        int num;
        switch (node)
        {
          case XText xtext1:
            location.Add((object) xtext1.Value);
            goto label_10;
          case XElement element3:
            num = element3.Name == (XName) "Page" ? 1 : 0;
            break;
          default:
            num = 0;
            break;
        }
        if (num != 0)
        {
          HeaderFooterSettings.PageModel page = new HeaderFooterSettings.PageModel();
          SetPageValue(element3, page);
          location.Add((object) page);
        }
        else if (node is XElement element4 && element4.Name == (XName) "Date")
        {
          HeaderFooterSettings.DateModel date = new HeaderFooterSettings.DateModel();
          SetDateValue(element4, date);
          location.Add((object) date);
        }
label_10:;
      }
    }

    static void SetPageValue(XElement element, HeaderFooterSettings.PageModel page)
    {
      page.Offset = ToInt(element.Attribute((XName) "offset")?.Value, -1);
      foreach (XNode node in element.Nodes())
      {
        if (node is XText xtext2)
          page.Add((object) xtext2.Value);
        else if (node is XElement xelement7 && (xelement7.Name.LocalName == "PageIndex" || xelement7.Name.LocalName == "PageTotalNum"))
          page.Add((object) new HeaderFooterSettings.VariableModel(xelement7.Name.LocalName)
          {
            Format = xelement7.Attribute((XName) "format")?.Value
          });
      }
    }

    static void SetDateValue(XElement element, HeaderFooterSettings.DateModel date)
    {
      foreach (XNode node in element.Nodes())
      {
        if (node is XText xtext3)
          date.Add((object) xtext3.Value);
        else if (node is XElement xelement8 && (xelement8.Name == (XName) "Month" || xelement8.Name == (XName) "Day" || xelement8.Name == (XName) "Year"))
        {
          string str = xelement8.Attribute((XName) "format")?.Value;
          if (string.IsNullOrEmpty(str))
            str = xelement8.Name == (XName) "Year" ? "0" : "1";
          date.Add((object) new HeaderFooterSettings.VariableModel(xelement8.Name.LocalName)
          {
            Format = str
          });
        }
      }
    }
  }

  public static PdfTypeArray ConvertContentsToArray(PdfPage page)
  {
    PdfTypeDictionary dictionary = page.Dictionary;
    PdfIndirectList list = PdfIndirectList.FromPdfDocument(page.Document);
    if (!dictionary.ContainsKey("Contents"))
    {
      PdfTypeArray array = PdfTypeArray.Create();
      list.Add((PdfTypeBase) array);
      dictionary.SetIndirectAt("Contents", list, (PdfTypeBase) array);
      return array;
    }
    PdfTypeBase array1 = dictionary["Contents"];
    switch (array1)
    {
      case PdfTypeArray _:
        return array1 as PdfTypeArray;
      case PdfTypeIndirect _:
        if ((array1 as PdfTypeIndirect).Direct is PdfTypeArray)
          return (array1 as PdfTypeIndirect).Direct as PdfTypeArray;
        if (!((array1 as PdfTypeIndirect).Direct is PdfTypeStream))
          throw new Exception("Unexpected content type");
        PdfTypeArray array2 = PdfTypeArray.Create();
        array2.AddIndirect(list, (array1 as PdfTypeIndirect).Direct);
        list.Add((PdfTypeBase) array2);
        dictionary.SetIndirectAt("Contents", list, (PdfTypeBase) array2);
        return array2;
      case PdfTypeStream _:
        list.Add(array1);
        PdfTypeArray array3 = PdfTypeArray.Create();
        array3.AddIndirect(list, array1);
        list.Add((PdfTypeBase) array3);
        dictionary.SetIndirectAt("Contents", list, (PdfTypeBase) array3);
        return array3;
      default:
        throw new Exception("Unexpected content type");
    }
  }

  public static void SetAsModifiedRecursively(PdfTypeDictionary dictionary)
  {
    Pdfium.FPDFOBJ_SetIsModified(dictionary.Handle, true);
    foreach (PdfTypeBase pdfTypeBase in (IEnumerable<PdfTypeBase>) dictionary.Values)
    {
      if (pdfTypeBase.Is<PdfTypeDictionary>())
        XObjectHelper.SetAsModifiedRecursively(pdfTypeBase.As<PdfTypeDictionary>());
      else if (pdfTypeBase.Is<PdfTypeArray>())
        XObjectHelper.SetAsModifiedRecursively(pdfTypeBase.As<PdfTypeArray>());
      else
        Pdfium.FPDFOBJ_SetIsModified(Pdfium.FPDFOBJ_GetDirect(pdfTypeBase.Handle), true);
    }
  }

  public static void SetAsModifiedRecursively(PdfTypeArray array)
  {
    Pdfium.FPDFOBJ_SetIsModified(array.Handle, true);
    foreach (PdfTypeBase pdfTypeBase in array)
    {
      if (pdfTypeBase.Is<PdfTypeDictionary>())
        XObjectHelper.SetAsModifiedRecursively(pdfTypeBase.As<PdfTypeDictionary>());
      else if (pdfTypeBase.Is<PdfTypeArray>())
        XObjectHelper.SetAsModifiedRecursively(pdfTypeBase.As<PdfTypeArray>());
      else
        Pdfium.FPDFOBJ_SetIsModified(Pdfium.FPDFOBJ_GetDirect(pdfTypeBase.Handle), true);
    }
  }
}
