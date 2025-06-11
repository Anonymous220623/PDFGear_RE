// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfDocumentUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PDFKit.Utils;

public static class PdfDocumentUtils
{
  private static List<PdfDocumentUtils.PdfPageExtensionData> pageExtensionDataList = new List<PdfDocumentUtils.PdfPageExtensionData>();

  public static bool RemoveUnusedObjects(PdfDocument document)
  {
    if (document == null)
      return false;
    PdfIndirectList pdfIndirectList = PdfIndirectList.FromPdfDocument(document);
    PdfCrossReferenceTable crossReferenceTable = PdfCrossReferenceTable.FromPdfDocument(document);
    List<PdfTypeBase> pages = new List<PdfTypeBase>();
    List<PdfTypeBase> pdfTypeBaseList = new List<PdfTypeBase>();
    foreach (PdfTypeBase pdfTypeBase1 in pdfIndirectList)
    {
      if (pdfTypeBase1.Is<PdfTypeDictionary>())
      {
        PdfTypeBase pdfTypeBase2;
        string str1;
        int num1;
        if (pdfTypeBase1.As<PdfTypeDictionary>().TryGetValue("Type", out pdfTypeBase2) && pdfTypeBase2.Is<PdfTypeName>())
        {
          str1 = pdfTypeBase2.As<PdfTypeName>().Value;
          num1 = str1 != null ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          switch (str1)
          {
            case "Page":
              pages.Add(pdfTypeBase1);
              break;
            case "Annot":
              PdfTypeBase pdfTypeBase3;
              string str2;
              int num2;
              if (pdfTypeBase1.As<PdfTypeDictionary>().TryGetValue("Subtype", out pdfTypeBase3) && pdfTypeBase3.Is<PdfTypeName>())
              {
                str2 = pdfTypeBase3.As<PdfTypeName>().Value;
                num2 = str2 != null ? 1 : 0;
              }
              else
                num2 = 0;
              if (num2 != 0 && str2 == "FreeText")
                pdfTypeBaseList.Add(pdfTypeBase1);
              break;
          }
        }
      }
    }
    for (int i = 0; i < pages.Count; i++)
    {
      if (document.Pages.All<PdfPage>((Func<PdfPage, bool>) (c => c.Dictionary.Handle != pages[i].Handle)))
        pages[i].As<PdfTypeDictionary>().Clear();
    }
    for (int index = 0; index < pdfTypeBaseList.Count; ++index)
    {
      PdfTypeBase pdfTypeBase;
      if (pdfTypeBaseList[index].Is<PdfTypeDictionary>() && pdfTypeBaseList[index].As<PdfTypeDictionary>().TryGetValue("RD", out pdfTypeBase) && (!pdfTypeBase.Is<PdfTypeArray>() || pdfTypeBase.As<PdfTypeArray>().Count != 4))
        pdfTypeBaseList[index].As<PdfTypeDictionary>().Remove("RD");
    }
    bool flag = false;
    List<int> intList1 = new List<int>();
    HashSet<int> intSet = (HashSet<int>) null;
    using (PdfRefObjectsCollection source = PdfRefObjectsCollection.FromPdfDocument(document))
      intSet = new HashSet<int>(source.Select<REFOBJ, int>((Func<REFOBJ, int>) (c => c.ObjectNumber)).Distinct<int>());
    foreach (PdfTypeBase pdfTypeBase in pdfIndirectList)
    {
      if (!intSet.Contains(pdfTypeBase.ObjectNumber))
      {
        flag = true;
        intList1.Add(pdfTypeBase.ObjectNumber);
      }
    }
    foreach (int objectNumber in intList1)
      pdfIndirectList.Remove(objectNumber);
    List<int> intList2 = new List<int>();
    foreach (PdfCrossRefItem pdfCrossRefItem in crossReferenceTable)
    {
      if (!intSet.Contains(pdfCrossRefItem.ObjectNumber))
        intList2.Add(pdfCrossRefItem.ObjectNumber);
    }
    foreach (int objectNumber in intList2)
    {
      flag = true;
      crossReferenceTable.Remove(objectNumber);
    }
    return flag;
  }

  public static void SetExtensionData(this PdfPage page, string key, object value)
  {
    if (page == null)
      return;
    PdfDocumentUtils.SetPageExtensionData(page.Document, page.PageIndex, key, value);
  }

  public static object GetExtensionData(this PdfPage page, string key)
  {
    return page == null ? (object) null : PdfDocumentUtils.GetPageExtensionData(page.Document, page.PageIndex, key);
  }

  public static void SetPageExtensionData(
    PdfDocument doc,
    int pageIndex,
    string key,
    object value)
  {
    if (doc == null)
      return;
    lock (PdfDocumentUtils.pageExtensionDataList)
    {
      PdfDocumentUtils.RemoveInvalidExtensionData();
      for (int index = 0; index < PdfDocumentUtils.pageExtensionDataList.Count; ++index)
      {
        if (PdfDocumentUtils.pageExtensionDataList[index].PdfDocument == doc)
        {
          PdfDocumentUtils.pageExtensionDataList[index].SetValue(pageIndex, key, value);
          return;
        }
      }
      PdfDocumentUtils.PdfPageExtensionData pageExtensionData = new PdfDocumentUtils.PdfPageExtensionData(doc);
      pageExtensionData.SetValue(pageIndex, key, value);
      PdfDocumentUtils.pageExtensionDataList.Add(pageExtensionData);
    }
  }

  public static object GetPageExtensionData(PdfDocument doc, int pageIndex, string key)
  {
    if (doc == null)
      return (object) null;
    lock (PdfDocumentUtils.pageExtensionDataList)
    {
      PdfDocumentUtils.RemoveInvalidExtensionData();
      for (int index = 0; index < PdfDocumentUtils.pageExtensionDataList.Count; ++index)
      {
        if (PdfDocumentUtils.pageExtensionDataList[index].PdfDocument == doc)
          return PdfDocumentUtils.pageExtensionDataList[index].GetValue(pageIndex, key);
      }
    }
    return (object) null;
  }

  private static void RemoveInvalidExtensionData()
  {
    lock (PdfDocumentUtils.pageExtensionDataList)
    {
      for (int index = PdfDocumentUtils.pageExtensionDataList.Count - 1; index >= 0; --index)
      {
        if (PdfDocumentUtils.pageExtensionDataList[index].PdfDocument == null)
          PdfDocumentUtils.pageExtensionDataList.RemoveAt(index);
      }
    }
  }

  private class PdfPageExtensionData
  {
    private WeakReference<PdfDocument> weakDoc;
    private Dictionary<IntPtr, Dictionary<string, object>> data;

    internal PdfPageExtensionData(PdfDocument document)
    {
      this.weakDoc = new WeakReference<PdfDocument>(document);
      this.data = new Dictionary<IntPtr, Dictionary<string, object>>();
    }

    internal PdfDocument PdfDocument
    {
      get
      {
        PdfDocument target;
        if (this.weakDoc == null || !this.weakDoc.TryGetTarget(out target))
          return (PdfDocument) null;
        if (!target.IsDisposed)
          return target;
        this.weakDoc = (WeakReference<PdfDocument>) null;
        return (PdfDocument) null;
      }
    }

    internal object GetValue(int pageIndex, string key)
    {
      if (string.IsNullOrWhiteSpace(key))
        return (object) null;
      string upperInvariant = key.Trim().ToUpperInvariant();
      lock (this.data)
      {
        IntPtr key1 = IntPtr.Zero;
        PdfDocument pdfDocument = this.PdfDocument;
        if (pdfDocument == null)
          return (object) null;
        if (pageIndex >= 0 && pageIndex < Pdfium.FPDF_GetPageCount(pdfDocument.Handle))
        {
          key1 = Pdfium.FPDF_GetPageDictionary(pdfDocument.Handle, pageIndex);
          if (key1 == IntPtr.Zero)
            return (object) null;
        }
        Dictionary<string, object> dictionary;
        if (this.data.TryGetValue(key1, out dictionary))
        {
          object obj;
          if (dictionary.TryGetValue(upperInvariant, out obj))
            return obj;
        }
      }
      return (object) null;
    }

    internal void SetValue(int pageIndex, string key, object value)
    {
      if (string.IsNullOrWhiteSpace(key))
        return;
      string upperInvariant = key.Trim().ToUpperInvariant();
      lock (this.data)
      {
        IntPtr key1 = IntPtr.Zero;
        PdfDocument pdfDocument = this.PdfDocument;
        if (pdfDocument == null)
          return;
        if (pageIndex >= 0 && pageIndex < Pdfium.FPDF_GetPageCount(pdfDocument.Handle))
        {
          key1 = Pdfium.FPDF_GetPageDictionary(pdfDocument.Handle, pageIndex);
          if (key1 == IntPtr.Zero)
            return;
        }
        Dictionary<string, object> dictionary;
        if (this.data.TryGetValue(key1, out dictionary))
        {
          if (value == null)
            dictionary.Remove(upperInvariant);
          else
            dictionary[upperInvariant] = value;
          if (dictionary.Count != 0)
            return;
          this.data.Remove(key1);
        }
        else if (value != null)
        {
          dictionary = new Dictionary<string, object>()
          {
            [upperInvariant] = value
          };
          this.data[key1] = dictionary;
        }
      }
    }
  }
}
