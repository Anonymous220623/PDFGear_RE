// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageDisposeHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace PDFKit.Utils;

public static class PageDisposeHelper
{
  private static Action<PdfPage, PdfAnnotationCollection> annotsFieldSetter;
  private static Func<PdfAnnotationCollection, PdfTypeArray> annotsTypeArrayFieldGetter;
  private static readonly IReadOnlyCollection<string> annotationSubTypes = (IReadOnlyCollection<string>) new HashSet<string>()
  {
    "Text",
    "Link",
    "FreeText",
    "Line",
    "Square",
    "Circle",
    "Polygon",
    "PolyLine",
    "Highlight",
    "Underline",
    "Squiggly",
    "StrikeOut",
    "Stamp",
    "Caret",
    "Ink",
    "Popup",
    "FileAttachment",
    "Sound",
    "Movie",
    "Screen",
    "PrinterMark",
    "TrapNet",
    "Watermark",
    "3D",
    "Redact"
  };

  public static void DisposePage(PdfPage page)
  {
    if (page == null || page.IsDisposed)
      return;
    lock (page)
    {
      try
      {
        if (PageDisposeHelper.CheckIfAnnotationTypeWrong(page, out int[] _))
        {
          GAManager.SendEvent("Exception", "CheckIfAnnotationTypeWrong", "Count", 1L);
          PageDisposeHelper.DisposeAnnotations(page);
        }
        page.Dispose();
      }
      catch (Exception ex)
      {
      }
    }
  }

  public static void TryFixResource(PdfDocument doc, int startPage, int endPage)
  {
    if (doc == null || startPage < 0 || endPage < 0)
      return;
    endPage = Math.Min(endPage, doc.Pages.Count - 1);
    if (endPage < startPage)
      return;
    PDFKit.PdfControl viewer = PDFKit.PdfControl.GetPdfControl(doc);
    int viewportStartPage = -1;
    int viewportEndPage = -1;
    if (viewer != null)
    {
      if (viewer.Dispatcher.CheckAccess())
        (viewportStartPage, viewportEndPage) = viewer.GetVisiblePageRange();
      else
        viewer.Dispatcher.Invoke((Action) (() =>
        {
          (viewportStartPage, viewportEndPage) = viewer.GetVisiblePageRange();
        }));
    }
    try
    {
      for (int index = startPage; index <= endPage; ++index)
      {
        try
        {
          PdfPage page = doc.Pages[index];
          if (!page.Dictionary.ContainsKey("Resources"))
          {
            PdfTypeBase parentResources = PageDisposeHelper.FindParentResources(page);
            if (parentResources != null)
              page.Dictionary["Resources"] = PageDisposeHelper.DeepClone(parentResources);
          }
          if (page.PageIndex < viewportStartPage || page.PageIndex > viewportEndPage)
            PageDisposeHelper.DisposePage(page);
        }
        catch
        {
        }
      }
    }
    catch
    {
    }
  }

  public static bool TryFixPageAnnotations(PdfDocument pdfDocument, int pageIndex)
  {
    return PageDisposeHelper.TryFixPageAnnotations(pdfDocument, pageIndex, false);
  }

  private static bool TryFixPageAnnotations(PdfDocument pdfDocument, int pageIndex, bool validOnly)
  {
    if (pdfDocument == null || pageIndex < 0 || pageIndex >= pdfDocument.Pages.Count)
      return false;
    if (PdfDocumentUtils.GetPageExtensionData(pdfDocument, pageIndex, "FIXANNOTSTATE") is string pageExtensionData)
    {
      if (pageExtensionData == "2" & validOnly)
        return true;
      if (pageExtensionData == "1")
        return false;
    }
    bool flag = false;
    PdfTypeDictionary pdfTypeDictionary = PdfTypeDictionary.Create(Pdfium.FPDF_GetPageDictionary(pdfDocument.Handle, pageIndex));
    if (pdfTypeDictionary != null && pdfTypeDictionary.ContainsKey("Annots"))
    {
      if (pdfTypeDictionary["Annots"].Is<PdfTypeArray>())
      {
        PdfTypeArray annotArray = pdfTypeDictionary["Annots"].As<PdfTypeArray>();
        for (int annotationIndex = annotArray.Count - 1; annotationIndex >= 0; --annotationIndex)
        {
          PageDisposeHelper.FixAnnotationResult annotationResult = PageDisposeHelper.TryFixAnnotation(annotArray, annotationIndex, validOnly);
          if (annotationResult != PageDisposeHelper.FixAnnotationResult.Valid && annotationResult != PageDisposeHelper.FixAnnotationResult.NotExist)
            flag = true;
        }
        if (flag)
        {
          if (validOnly)
            PdfDocumentUtils.SetPageExtensionData(pdfDocument, pageIndex, "FIXANNOTSTATE", (object) "2");
          else
            PdfDocumentUtils.SetPageExtensionData(pdfDocument, pageIndex, "FIXANNOTSTATE", (object) "1");
        }
        else
          PdfDocumentUtils.SetPageExtensionData(pdfDocument, pageIndex, "FIXANNOTSTATE", (object) "1");
      }
      else
        pdfTypeDictionary.Remove("Annots");
    }
    return flag;
  }

  private static PageDisposeHelper.FixAnnotationResult TryFixAnnotation(
    PdfTypeArray annotArray,
    int annotationIndex,
    bool validOnly)
  {
    if (annotArray == null || annotationIndex < 0 || annotationIndex >= annotArray.Count)
      return PageDisposeHelper.FixAnnotationResult.NotExist;
    if (annotArray[annotationIndex].Is<PdfTypeDictionary>())
    {
      PdfTypeDictionary pdfTypeDictionary = annotArray[annotationIndex].As<PdfTypeDictionary>();
      PageDisposeHelper.FixAnnotationResult annotationResult = PageDisposeHelper.FixAnnotationResult.Valid;
      if (pdfTypeDictionary.ContainsKey("Subtype"))
      {
        if (pdfTypeDictionary["Subtype"].Is<PdfTypeString>())
        {
          PdfTypeString pdfTypeString = pdfTypeDictionary["Subtype"].As<PdfTypeString>();
          if (PageDisposeHelper.annotationSubTypes.Contains<string>(pdfTypeString.UnicodeString))
          {
            pdfTypeDictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create(pdfTypeString.UnicodeString);
            annotationResult = PageDisposeHelper.FixAnnotationResult.InvalidAndFixed;
          }
          else
            annotationResult = PageDisposeHelper.FixAnnotationResult.Invalid;
        }
        else if (!pdfTypeDictionary["Subtype"].Is<PdfTypeName>())
          annotationResult = PageDisposeHelper.FixAnnotationResult.Invalid;
      }
      if (annotationResult != PageDisposeHelper.FixAnnotationResult.Invalid && !pdfTypeDictionary.ContainsKey("Subtype"))
      {
        annotationResult = PageDisposeHelper.FixAnnotationResult.Invalid;
        PdfTypeBase pdfTypeBase;
        PdfTypeName pdfTypeName;
        int num;
        if (pdfTypeDictionary.TryGetValue("FT", out pdfTypeBase))
        {
          pdfTypeName = pdfTypeBase as PdfTypeName;
          num = pdfTypeName != null ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          if (pdfTypeName.Value == "Btn" || pdfTypeName.Value == "Tx" || pdfTypeName.Value == "Ch" || pdfTypeName.Value == "Sig")
          {
            pdfTypeDictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Widget");
            annotationResult = PageDisposeHelper.FixAnnotationResult.InvalidAndFixed;
          }
        }
        else if (pdfTypeDictionary.ContainsKey("Subj") && pdfTypeDictionary["Subj"].Is<PdfTypeString>() && pdfTypeDictionary["Subj"].As<PdfTypeString>().UnicodeString == "Rectangle")
        {
          pdfTypeDictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Square");
          annotationResult = PageDisposeHelper.FixAnnotationResult.InvalidAndFixed;
        }
      }
      if (annotationResult != PageDisposeHelper.FixAnnotationResult.Invalid && (!pdfTypeDictionary.ContainsKey("Type") || !pdfTypeDictionary["Type"].Is<PdfTypeName>() || pdfTypeDictionary["Type"].As<PdfTypeName>().Value != "Annot"))
      {
        annotationResult = PageDisposeHelper.FixAnnotationResult.Invalid;
        PdfTypeBase pdfTypeBase;
        if (pdfTypeDictionary.TryGetValue("Subtype", out pdfTypeBase) && pdfTypeBase is PdfTypeName pdfTypeName && PageDisposeHelper.annotationSubTypes.Contains<string>(pdfTypeName.Value))
        {
          pdfTypeDictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Annot");
          annotationResult = PageDisposeHelper.FixAnnotationResult.InvalidAndFixed;
        }
      }
      return annotationResult;
    }
    if (validOnly)
      return PageDisposeHelper.FixAnnotationResult.Invalid;
    annotArray.RemoveAt(annotationIndex);
    return PageDisposeHelper.FixAnnotationResult.InvalidAndFixed;
  }

  private static PdfTypeBase FindParentResources(PdfPage page)
  {
    if (page?.Dictionary == null)
      return (PdfTypeBase) null;
    for (PdfTypeDictionary parentPagesNode = GetParentPagesNode(page.Dictionary); parentPagesNode != null; parentPagesNode = GetParentPagesNode(parentPagesNode))
    {
      PdfTypeBase resources = GetResources((PdfTypeBase) parentPagesNode);
      if (resources != null)
        return resources;
    }
    return (PdfTypeBase) null;

    static PdfTypeDictionary GetParentPagesNode(PdfTypeDictionary _dict)
    {
      PdfTypeBase pdfTypeBase;
      return _dict.TryGetValue("Parent", out pdfTypeBase) && pdfTypeBase.Is<PdfTypeDictionary>() ? pdfTypeBase.As<PdfTypeDictionary>() : (PdfTypeDictionary) null;
    }

    static PdfTypeBase GetResources(PdfTypeBase _pagesNode)
    {
      PdfTypeBase pdfTypeBase;
      return _pagesNode.Is<PdfTypeDictionary>() && _pagesNode.As<PdfTypeDictionary>().TryGetValue("Resources", out pdfTypeBase) && pdfTypeBase.Is<PdfTypeDictionary>() ? pdfTypeBase : (PdfTypeBase) null;
    }
  }

  public static PdfTypeBase DeepClone(PdfTypeBase obj)
  {
    if (obj == null)
      return (PdfTypeBase) null;
    if (obj is PdfTypeIndirect pdfTypeIndirect)
      return pdfTypeIndirect.Clone();
    if (obj.Is<PdfTypeBoolean>() || obj.Is<PdfTypeName>() || obj.Is<PdfTypeNull>() || obj.Is<PdfTypeNumber>() || obj.Is<PdfTypeString>() || obj.Is<PdfTypeUnknown>() || obj.Is<PdfTypeStream>())
      return obj.Clone();
    if (obj.Is<PdfTypeArray>())
    {
      PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
      foreach (PdfTypeBase a in obj.As<PdfTypeArray>())
        pdfTypeArray.Add(PageDisposeHelper.DeepClone(a));
      return (PdfTypeBase) pdfTypeArray;
    }
    if (!obj.Is<PdfTypeDictionary>())
      return (PdfTypeBase) null;
    PdfTypeDictionary pdfTypeDictionary = PdfTypeDictionary.Create();
    foreach (KeyValuePair<string, PdfTypeBase> a in obj.As<PdfTypeDictionary>())
      pdfTypeDictionary[a.Key] = PageDisposeHelper.DeepClone(a.Value);
    return (PdfTypeBase) pdfTypeDictionary;
  }

  private static void DisposeAnnotations(PdfPage page)
  {
    if (page == null || page.IsDisposed || page.Annots == null || page.Annots.IsDisposed || page.Annots.Count <= 0)
      return;
    PdfAnnotationCollection annots = page.Annots;
    for (int index = annots.Count - 1; index >= 0; --index)
    {
      try
      {
        PdfAnnotation pdfAnnotation = annots[index];
        if ((PdfWrapper) pdfAnnotation != (PdfWrapper) null)
          pdfAnnotation.Dispose();
      }
      catch
      {
      }
    }
    PageDisposeHelper.GetAnnotsTypeArrayField(annots)?.Dispose();
    GC.SuppressFinalize((object) annots);
    PageDisposeHelper.SetAnnotsField(page, (PdfAnnotationCollection) null);
  }

  private static PdfTypeArray GetAnnotsTypeArrayField(PdfAnnotationCollection annots)
  {
    if (annots == null)
      return (PdfTypeArray) null;
    if (PageDisposeHelper.annotsTypeArrayFieldGetter == null)
    {
      lock (typeof (PageDisposeHelper))
      {
        if (PageDisposeHelper.annotsFieldSetter == null)
        {
          Type type = typeof (PdfAnnotationCollection);
          FieldInfo field = type.GetField("_annots", BindingFlags.Instance | BindingFlags.NonPublic);
          if (field != (FieldInfo) null)
          {
            ParameterExpression parameterExpression = Expression.Parameter(type, "p");
            PageDisposeHelper.annotsTypeArrayFieldGetter = Expression.Lambda<Func<PdfAnnotationCollection, PdfTypeArray>>((Expression) Expression.Field((Expression) parameterExpression, field), parameterExpression).Compile();
          }
        }
      }
    }
    if (PageDisposeHelper.annotsTypeArrayFieldGetter == null)
    {
      lock (typeof (PageDisposeHelper))
      {
        if (PageDisposeHelper.annotsTypeArrayFieldGetter == null)
          PageDisposeHelper.annotsTypeArrayFieldGetter = (Func<PdfAnnotationCollection, PdfTypeArray>) (p => (PdfTypeArray) null);
      }
    }
    try
    {
      return PageDisposeHelper.annotsTypeArrayFieldGetter(annots);
    }
    catch
    {
    }
    return (PdfTypeArray) null;
  }

  private static void SetAnnotsField(PdfPage page, PdfAnnotationCollection annots)
  {
    if (page == null)
      return;
    if (PageDisposeHelper.annotsFieldSetter == null)
    {
      lock (typeof (PageDisposeHelper))
      {
        if (PageDisposeHelper.annotsFieldSetter == null)
        {
          Type type1 = typeof (PdfPage);
          Type type2 = typeof (PdfAnnotationCollection);
          FieldInfo field = type1.GetField("_annots", BindingFlags.Instance | BindingFlags.NonPublic);
          if (field != (FieldInfo) null)
          {
            ParameterExpression parameterExpression = Expression.Parameter(type1, "p");
            ParameterExpression right = Expression.Parameter(type2, "c");
            PageDisposeHelper.annotsFieldSetter = Expression.Lambda<Action<PdfPage, PdfAnnotationCollection>>((Expression) Expression.Assign((Expression) Expression.Field((Expression) parameterExpression, field), (Expression) right), parameterExpression, right).Compile();
          }
        }
      }
    }
    if (PageDisposeHelper.annotsFieldSetter == null)
    {
      lock (typeof (PageDisposeHelper))
      {
        if (PageDisposeHelper.annotsFieldSetter == null)
          PageDisposeHelper.annotsFieldSetter = (Action<PdfPage, PdfAnnotationCollection>) ((p, c) => { });
      }
    }
    PageDisposeHelper.annotsFieldSetter(page, annots);
  }

  private static bool CheckIfAnnotationTypeWrong(PdfPage page, out int[] wrongIndexes)
  {
    wrongIndexes = (int[]) null;
    if (page == null || page.IsDisposed)
      return false;
    List<int> intList = new List<int>();
    try
    {
      if (page.Annots == null || PageDisposeHelper.TryFixPageAnnotations(page.Document, page.PageIndex, true))
        return false;
      for (int index = 0; index < page.Annots.Count; ++index)
      {
        try
        {
          PdfAnnotation annot = page.Annots[index];
        }
        catch
        {
          intList.Add(index);
        }
      }
    }
    catch
    {
    }
    if (intList.Count > 0)
      wrongIndexes = intList.ToArray();
    return intList.Count > 0;
  }

  private enum FixAnnotationResult
  {
    Valid,
    NotExist,
    Invalid,
    InvalidAndFixed,
  }

  private enum FixAnnotationState
  {
    Unknown,
    Valid,
    NotFixed,
  }
}
