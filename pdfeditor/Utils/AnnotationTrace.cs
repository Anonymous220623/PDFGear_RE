// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.AnnotationTrace
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Models.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Utils;

public class AnnotationTrace
{
  public const int MAX_TRACE_PAGE_COUNT = 20;
  private PdfPage[] pages;

  public AnnotationTrace(PdfDocument document)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (document.Pages.Count > 0)
      this.pages = document.Pages.Take<PdfPage>(20).ToArray<PdfPage>();
    else
      this.pages = Array.Empty<PdfPage>();
  }

  public object[] GetPageAnnotationModels(int pageIndex, bool onlyType)
  {
    if (this.pages.Length == 0)
      return Array.Empty<object>();
    if (pageIndex < 0 || pageIndex >= this.pages.Length)
      throw new ArgumentException(nameof (pageIndex));
    IEnumerable<IGrouping<string, BaseAnnotation>> source = this.pages[pageIndex].Annots.Where<PdfAnnotation>((Func<PdfAnnotation, bool>) (c => c.NormalAppearance != null || c is PdfPopupAnnotation)).Select<PdfAnnotation, BaseAnnotation>((Func<PdfAnnotation, BaseAnnotation>) (c =>
    {
      try
      {
        return AnnotationFactory.Create(c);
      }
      catch
      {
      }
      return (BaseAnnotation) null;
    })).Where<BaseAnnotation>((Func<BaseAnnotation, bool>) (c => c != null)).GroupBy<BaseAnnotation, string>((Func<BaseAnnotation, string>) (c => c.AnnotationType));
    return onlyType ? (object[]) source.Select(c => new
    {
      type = c.Key,
      count = c.Count<BaseAnnotation>()
    }).ToArray() : (object[]) source.Select(c =>
    {
      BaseAnnotation[] array = c.ToArray<BaseAnnotation>();
      return new
      {
        type = c.Key,
        count = array.Length,
        items = array
      };
    }).ToArray();
  }

  public object[] GetAnnotationModelTraceObject()
  {
    return this.pages.Length == 0 ? Array.Empty<object>() : (object[]) ((IEnumerable<PdfPage>) this.pages).Select(c => new
    {
      pageIndex = c.PageIndex,
      annots = this.GetPageAnnotationModels(c.PageIndex, false)
    }).ToArray();
  }

  public object[] GetAnnotationTypeTraceObject()
  {
    return this.pages.Length == 0 ? Array.Empty<object>() : (object[]) ((IEnumerable<PdfPage>) this.pages).Select(c => new
    {
      pageIndex = c.PageIndex,
      annots = this.GetPageAnnotationModels(c.PageIndex, true)
    }).ToArray();
  }

  public (string type, int count)[] GetAnnotationTypeTraceObjectAllPages()
  {
    if (this.pages.Length == 0)
      return Array.Empty<(string, int)>();
    IEnumerable<PdfAnnotation> source1 = ((IEnumerable<PdfPage>) this.pages).Where<PdfPage>((Func<PdfPage, bool>) (a => a.Annots != null)).SelectMany<PdfPage, PdfAnnotation>((Func<PdfPage, IEnumerable<PdfAnnotation>>) (p => (IEnumerable<PdfAnnotation>) p.Annots));
    if (source1 == null || source1 != null && source1.Count<PdfAnnotation>() <= 0)
      return Array.Empty<(string, int)>();
    IEnumerable<IGrouping<string, BaseAnnotation>> source2 = source1.Select<PdfAnnotation, BaseAnnotation>((Func<PdfAnnotation, BaseAnnotation>) (c =>
    {
      try
      {
        return AnnotationFactory.Create(c);
      }
      catch
      {
      }
      return (BaseAnnotation) null;
    })).Where<BaseAnnotation>((Func<BaseAnnotation, bool>) (c => c != null)).GroupBy<BaseAnnotation, string>((Func<BaseAnnotation, string>) (c => c.AnnotationType));
    return source2 == null || source2 != null && source2.Count<IGrouping<string, BaseAnnotation>>() <= 0 ? Array.Empty<(string, int)>() : source2.Select<IGrouping<string, BaseAnnotation>, (string, int)>((Func<IGrouping<string, BaseAnnotation>, (string, int)>) (c => (c.Key, c.Count<BaseAnnotation>()))).ToArray<(string, int)>();
  }
}
