// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.AnnotationOperationManagerExtensions
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
using System.Threading.Tasks;

#nullable disable
namespace pdfeditor.Utils;

public static class AnnotationOperationManagerExtensions
{
  public static AnnotationChangedTraceContext TraceAnnotationChange(
    this OperationManager manager,
    PdfDocument pdfDocument,
    string tag = "")
  {
    if (manager == null)
      throw new ArgumentNullException(nameof (manager));
    return new AnnotationChangedTraceContext(manager, pdfDocument, tag);
  }

  public static IDisposable TraceAnnotationChange(
    this OperationManager manager,
    PdfPage page,
    string tag = "")
  {
    if (manager == null)
      throw new ArgumentNullException(nameof (manager));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (page.Document == null)
      throw new ArgumentNullException("document");
    AnnotationChangedTraceContext changedTraceContext = new AnnotationChangedTraceContext(manager, page.Document, tag);
    changedTraceContext.TryAddPage(page.PageIndex);
    return (IDisposable) changedTraceContext;
  }

  public static async Task TraceAnnotationInsertAsync(
    this OperationManager manager,
    PdfAnnotation annotation,
    string tag = "")
  {
    await manager.TraceAnnotationInsertAsync((System.Collections.Generic.IReadOnlyList<PdfAnnotation>) new PdfAnnotation[1]
    {
      annotation
    }, tag).ConfigureAwait(false);
  }

  public static async Task TraceAnnotationInsertAsync(
    this OperationManager manager,
    System.Collections.Generic.IReadOnlyList<PdfAnnotation> annotations,
    string tag = "")
  {
    if (annotations == null || annotations.Count == 0 || annotations.Any<PdfAnnotation>((Func<PdfAnnotation, bool>) (c => c?.Page?.Document == null)))
      throw new ArgumentException(nameof (annotations));
    (BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)[] records = annotations.Select<PdfAnnotation, (BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)>((Func<PdfAnnotation, (BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)>) (c => AnnotationModelUtils.CreateFlattenedRecord(c))).OrderBy<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>), int>((Func<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>), int>) (c => c.target.AnnotIndex)).ToArray<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)>();
    await manager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
    {
      AnnotationOperationManagerExtensions.TryShowAnnotations(doc);
      HashSet<int> source = new HashSet<int>();
      foreach ((BaseAnnotation target, System.Collections.Generic.IReadOnlyList<BaseAnnotation> flattenedRecord) in ((IEnumerable<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)>) records).Reverse<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)>())
      {
        AnnotationModelUtils.RemoveRecord(doc, target, flattenedRecord);
        source.Add(target.PageIndex);
      }
      foreach (int index in (IEnumerable<int>) source.OrderBy<int, int>((Func<int, int>) (c => c)))
        await doc.Pages[index].TryRedrawPageAsync();
    }), (Func<PdfDocument, Task>) (async doc =>
    {
      AnnotationOperationManagerExtensions.TryShowAnnotations(doc);
      HashSet<int> source = new HashSet<int>();
      foreach ((BaseAnnotation target, System.Collections.Generic.IReadOnlyList<BaseAnnotation> flattenedRecord) in records)
      {
        AnnotationModelUtils.InsertRecord(doc, target, flattenedRecord);
        source.Add(target.PageIndex);
      }
      foreach (int index in (IEnumerable<int>) source.OrderBy<int, int>((Func<int, int>) (c => c)))
        await doc.Pages[index].TryRedrawPageAsync();
    }), tag).ConfigureAwait(false);
  }

  public static async Task TraceAnnotationRemoveAsync(
    this OperationManager manager,
    PdfAnnotation annotation,
    string tag = "")
  {
    await manager.TraceAnnotationRemoveAsync((System.Collections.Generic.IReadOnlyList<PdfAnnotation>) new PdfAnnotation[1]
    {
      annotation
    }, tag).ConfigureAwait(false);
  }

  public static async Task TraceAnnotationRemoveAsync(
    this OperationManager manager,
    System.Collections.Generic.IReadOnlyList<PdfAnnotation> annotations,
    string tag = "")
  {
    if (annotations == null || annotations.Count == 0 || annotations.Any<PdfAnnotation>((Func<PdfAnnotation, bool>) (c => c?.Page?.Document == null)))
      throw new ArgumentException(nameof (annotations));
    (BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)[] records = annotations.Select<PdfAnnotation, (BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)>((Func<PdfAnnotation, (BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)>) (c => AnnotationModelUtils.CreateFlattenedRecord(c))).OrderBy<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>), int>((Func<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>), int>) (c => c.target.AnnotIndex)).ToArray<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)>();
    await manager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
    {
      AnnotationOperationManagerExtensions.TryShowAnnotations(doc);
      HashSet<int> source = new HashSet<int>();
      foreach ((BaseAnnotation target, System.Collections.Generic.IReadOnlyList<BaseAnnotation> flattenedRecord) in records)
      {
        AnnotationModelUtils.InsertRecord(doc, target, flattenedRecord);
        source.Add(target.PageIndex);
      }
      foreach (int index in (IEnumerable<int>) source.OrderBy<int, int>((Func<int, int>) (c => c)))
        await doc.Pages[index].TryRedrawPageAsync();
    }), (Func<PdfDocument, Task>) (async doc =>
    {
      AnnotationOperationManagerExtensions.TryShowAnnotations(doc);
      HashSet<int> source = new HashSet<int>();
      foreach ((BaseAnnotation target, System.Collections.Generic.IReadOnlyList<BaseAnnotation> flattenedRecord) in ((IEnumerable<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)>) records).Reverse<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseAnnotation>)>())
      {
        AnnotationModelUtils.RemoveRecord(doc, target, flattenedRecord);
        source.Add(target.PageIndex);
      }
      foreach (int index in (IEnumerable<int>) source.OrderBy<int, int>((Func<int, int>) (c => c)))
        await doc.Pages[index].TryRedrawPageAsync();
    }), tag).ConfigureAwait(false);
  }

  private static void TryShowAnnotations(PdfDocument doc)
  {
    if (doc == null || !(PDFKit.PdfControl.GetPdfControl(doc)?.DataContext is MainViewModel dataContext) || dataContext.IsAnnotationVisible)
      return;
    dataContext.IsAnnotationVisible = true;
  }
}
