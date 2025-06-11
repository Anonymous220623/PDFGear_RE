// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.AnnotationChangedTraceContext
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Models.Annotations;
using pdfeditor.Models.Operations;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Utils;

public class AnnotationChangedTraceContext : IDisposable
{
  private bool completed;
  private readonly OperationManager manager;
  private readonly PdfDocument pdfDocument;
  private readonly string tag;
  private Dictionary<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>> startAnnotations;

  public AnnotationChangedTraceContext(
    OperationManager manager,
    PdfDocument pdfDocument,
    string tag)
  {
    this.manager = manager;
    this.pdfDocument = pdfDocument;
    this.tag = tag;
    this.startAnnotations = new Dictionary<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>>();
  }

  public void TryAddPage(int pageIndex)
  {
    if (this.completed)
      throw new ObjectDisposedException(nameof (AnnotationChangedTraceContext));
    if (pageIndex < 0 || this.startAnnotations.ContainsKey(pageIndex))
      return;
    this.startAnnotations[pageIndex] = AnnotationFactory.Create(this.pdfDocument.Pages[pageIndex]);
  }

  public void Dispose()
  {
    this.completed = !this.completed ? true : throw new ObjectDisposedException(nameof (AnnotationChangedTraceContext));
    Dictionary<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>> endAnnotations = this.startAnnotations.ToDictionary<KeyValuePair<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>>, int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>>((Func<KeyValuePair<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>>, int>) (c => c.Key), (Func<KeyValuePair<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>>, System.Collections.Generic.IReadOnlyList<BaseAnnotation>>) (c => AnnotationFactory.Create(this.pdfDocument.Pages[c.Key])));
    if (this.startAnnotations.Count != endAnnotations.Count)
      throw new ArgumentException("Not supported insert or delete annotation");
    foreach (KeyValuePair<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>> pair in this.startAnnotations.ToList<KeyValuePair<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>>>())
    {
      int key1;
      pair.Deconstruct<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>>(out key1, out System.Collections.Generic.IReadOnlyList<BaseAnnotation> _);
      int key2 = key1;
      if (this.startAnnotations[key2].Count != endAnnotations[key2].Count)
        throw new ArgumentException("Not supported insert or delete annotation");
      if (BaseAnnotation.CollectionEqual<BaseAnnotation>(this.startAnnotations[key2], endAnnotations[key2]))
      {
        this.startAnnotations.Remove(key2);
        endAnnotations.Remove(key2);
      }
      else
      {
        List<BaseAnnotation> baseAnnotationList1 = new List<BaseAnnotation>();
        List<BaseAnnotation> baseAnnotationList2 = new List<BaseAnnotation>();
        for (int index = 0; index < this.startAnnotations[key2].Count; ++index)
        {
          if (!this.startAnnotations[key2][index].Equals(endAnnotations[key2][index]))
          {
            baseAnnotationList1.Add(this.startAnnotations[key2][index]);
            baseAnnotationList2.Add(endAnnotations[key2][index]);
          }
        }
        this.startAnnotations[key2] = (System.Collections.Generic.IReadOnlyList<BaseAnnotation>) baseAnnotationList1;
        endAnnotations[key2] = (System.Collections.Generic.IReadOnlyList<BaseAnnotation>) baseAnnotationList2;
      }
    }
    if (this.startAnnotations.Count == 0)
      return;
    this.manager.AddOperationAsync((Action<PdfDocument>) (doc =>
    {
      if (PDFKit.PdfControl.GetPdfControl(doc)?.DataContext is MainViewModel dataContext2 && !dataContext2.IsAnnotationVisible)
        dataContext2.IsAnnotationVisible = true;
      foreach (KeyValuePair<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>> startAnnotation in this.startAnnotations)
      {
        int key;
        System.Collections.Generic.IReadOnlyList<BaseAnnotation> baseAnnotationList3;
        startAnnotation.Deconstruct<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>>(out key, out baseAnnotationList3);
        int num = key;
        System.Collections.Generic.IReadOnlyList<BaseAnnotation> baseAnnotationList4 = baseAnnotationList3;
        PdfPage page = doc.Pages[num];
        if (page.Annots == null)
          throw new ArgumentException("Annots is null");
        for (int index = 0; index < baseAnnotationList4.Count; ++index)
        {
          baseAnnotationList4[index].Apply(page.Annots[baseAnnotationList4[index].AnnotIndex]);
          if (page.Annots[baseAnnotationList4[index].AnnotIndex] is PdfMarkupAnnotation annot2)
          {
            if (annot2 is PdfStampAnnotation)
            {
              PdfPageObjectsCollection normalAppearance = annot2.NormalAppearance;
              if ((normalAppearance != null ? normalAppearance.OfType<PdfImageObject>().FirstOrDefault<PdfImageObject>() : (PdfImageObject) null) != null)
              {
                page.TryRedrawPageAsync();
                continue;
              }
            }
            annot2.TryRedrawAnnotation();
          }
          else if (page.Annots[baseAnnotationList4[index].AnnotIndex] is PdfLinkAnnotation)
            page.TryRedrawPageAsync();
        }
        dataContext2?.PageEditors?.NotifyPageAnnotationChanged(num);
      }
    }), (Action<PdfDocument>) (doc =>
    {
      if (PDFKit.PdfControl.GetPdfControl(doc)?.DataContext is MainViewModel dataContext4 && !dataContext4.IsAnnotationVisible)
        dataContext4.IsAnnotationVisible = true;
      foreach (KeyValuePair<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>> pair in endAnnotations)
      {
        int key;
        System.Collections.Generic.IReadOnlyList<BaseAnnotation> baseAnnotationList5;
        pair.Deconstruct<int, System.Collections.Generic.IReadOnlyList<BaseAnnotation>>(out key, out baseAnnotationList5);
        int num = key;
        System.Collections.Generic.IReadOnlyList<BaseAnnotation> baseAnnotationList6 = baseAnnotationList5;
        PdfPage page = doc.Pages[num];
        if (page.Annots == null)
          throw new ArgumentException("Annots is null");
        for (int index = 0; index < baseAnnotationList6.Count; ++index)
        {
          baseAnnotationList6[index].Apply(page.Annots[baseAnnotationList6[index].AnnotIndex]);
          if (page.Annots[baseAnnotationList6[index].AnnotIndex] is PdfMarkupAnnotation annot4)
          {
            if (annot4 is PdfStampAnnotation)
            {
              PdfPageObjectsCollection normalAppearance = annot4.NormalAppearance;
              if ((normalAppearance != null ? normalAppearance.OfType<PdfImageObject>().FirstOrDefault<PdfImageObject>() : (PdfImageObject) null) != null)
              {
                page.TryRedrawPageAsync();
                continue;
              }
            }
            annot4.TryRedrawAnnotation();
          }
          else if (page.Annots[baseAnnotationList6[index].AnnotIndex] is PdfLinkAnnotation)
            page.TryRedrawPageAsync();
        }
        dataContext4?.PageEditors?.NotifyPageAnnotationChanged(num);
      }
    }), this.tag).Wait();
  }
}
