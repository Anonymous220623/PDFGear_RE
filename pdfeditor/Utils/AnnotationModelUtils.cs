// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.AnnotationModelUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Models.Annotations;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Utils;

public static class AnnotationModelUtils
{
  public static BaseAnnotation CreateRecord(
    PdfAnnotation annotation,
    out System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation> replies)
  {
    replies = (System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>) null;
    if ((PdfWrapper) annotation == (PdfWrapper) null || annotation.Page == null || annotation.Page.Document == null)
      return (BaseAnnotation) null;
    if (annotation.Page.Annots == null || annotation.Page.Annots.Count == 0)
      return (BaseAnnotation) null;
    if (annotation.Page.Annots.IndexOf(annotation) == -1)
      return (BaseAnnotation) null;
    PdfPage page = annotation.Page;
    switch (annotation)
    {
      case PdfPopupAnnotation _:
      case PdfMarkupAnnotation _:
        System.Collections.Generic.IReadOnlyList<BaseAnnotation> baseAnnotationList = AnnotationFactory.Create(page);
        IReadOnlyDictionary<BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>> annotationRepliesModel = CommetUtils.GetMarkupAnnotationRepliesModel(baseAnnotationList);
        int annotIdx = page.Annots.IndexOf(annotation);
        BaseAnnotation key = baseAnnotationList.FirstOrDefault<BaseAnnotation>((Func<BaseAnnotation, bool>) (c => c.AnnotIndex == annotIdx));
        annotationRepliesModel?.TryGetValue(key, out replies);
        return key;
      default:
        return AnnotationFactory.Create(annotation);
    }
  }

  public static System.Collections.Generic.IReadOnlyList<BaseAnnotation> FlattenRecord(
    BaseAnnotation record,
    System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation> replies)
  {
    if (record == null)
      return (System.Collections.Generic.IReadOnlyList<BaseAnnotation>) Array.Empty<BaseAnnotation>();
    List<BaseAnnotation> _list = new List<BaseAnnotation>();
    AddCore(record, _list);
    if (replies != null && replies.Count > 0)
    {
      foreach (BaseAnnotation reply in (IEnumerable<BaseMarkupAnnotation>) replies)
        AddCore(reply, _list);
    }
    List<BaseAnnotation> source = new List<BaseAnnotation>();
    foreach (BaseAnnotation baseAnnotation in _list)
    {
      if (baseAnnotation != null)
        source.Add(baseAnnotation);
    }
    return (System.Collections.Generic.IReadOnlyList<BaseAnnotation>) source.OrderBy<BaseAnnotation, int>((Func<BaseAnnotation, int>) (c => c.AnnotIndex)).ToList<BaseAnnotation>();

    static void AddCore(BaseAnnotation _record, List<BaseAnnotation> _list)
    {
      _list.Add(_record);
      if (!(_record is BaseMarkupAnnotation markupAnnotation) || markupAnnotation.Popup == null)
        return;
      _list.Add((BaseAnnotation) markupAnnotation.Popup);
    }
  }

  public static void InsertRecord(
    PdfDocument doc,
    BaseAnnotation target,
    System.Collections.Generic.IReadOnlyList<BaseAnnotation> flattenedRecord)
  {
    PdfPage page = doc.Pages[target.PageIndex];
    if (page.Annots == null)
      page.CreateAnnotations();
    foreach (BaseAnnotation annotation in (IEnumerable<BaseAnnotation>) flattenedRecord)
    {
      PdfAnnotation pdfAnnotation = AnnotationFactory.Create(page, annotation);
      page.Annots.Insert(annotation.AnnotIndex, pdfAnnotation);
    }
    int? nullable;
    if (target is PopupAnnotation popupAnnotation && popupAnnotation.ParentAnnotationIndex.HasValue)
    {
      PdfAnnotationCollection annots1 = page.Annots;
      nullable = popupAnnotation.ParentAnnotationIndex;
      int index1 = nullable.Value;
      ((PdfMarkupAnnotation) annots1[index1]).Popup = (PdfPopupAnnotation) page.Annots[popupAnnotation.AnnotIndex];
      PdfPopupAnnotation annot = (PdfPopupAnnotation) page.Annots[popupAnnotation.AnnotIndex];
      PdfAnnotationCollection annots2 = page.Annots;
      nullable = popupAnnotation.ParentAnnotationIndex;
      int index2 = nullable.Value;
      PdfMarkupAnnotation markupAnnotation = (PdfMarkupAnnotation) annots2[index2];
      annot.Parent = (PdfAnnotation) markupAnnotation;
    }
    foreach (BaseAnnotation baseAnnotation in (IEnumerable<BaseAnnotation>) flattenedRecord)
    {
      if (baseAnnotation is BaseMarkupAnnotation markupAnnotation)
      {
        nullable = markupAnnotation.PopupAnnotationIndex;
        if (nullable.HasValue)
        {
          PdfMarkupAnnotation annot = (PdfMarkupAnnotation) page.Annots[markupAnnotation.AnnotIndex];
          PdfAnnotationCollection annots3 = page.Annots;
          nullable = markupAnnotation.PopupAnnotationIndex;
          int index3 = nullable.Value;
          PdfPopupAnnotation pdfPopupAnnotation = (PdfPopupAnnotation) annots3[index3];
          annot.Popup = pdfPopupAnnotation;
          PdfAnnotationCollection annots4 = page.Annots;
          nullable = markupAnnotation.PopupAnnotationIndex;
          int index4 = nullable.Value;
          ((PdfPopupAnnotation) annots4[index4]).Parent = page.Annots[markupAnnotation.AnnotIndex];
        }
        if (markupAnnotation.RelationshipAnnotation != null && markupAnnotation.RelationshipAnnotation.AnnotIndex != -1)
          ((PdfMarkupAnnotation) page.Annots[markupAnnotation.AnnotIndex]).RelationshipAnnotation = page.Annots[markupAnnotation.RelationshipAnnotation.AnnotIndex];
      }
      if ((baseAnnotation is FreeTextAnnotation || baseAnnotation is StampAnnotation) && page.Annots[baseAnnotation.AnnotIndex] is PdfMarkupAnnotation annot1)
        annot1.TryRedrawAnnotation();
      baseAnnotation.Apply(page.Annots[baseAnnotation.AnnotIndex]);
    }
    if (!(PDFKit.PdfControl.GetPdfControl(doc)?.DataContext is MainViewModel dataContext))
      return;
    dataContext.PageEditors?.NotifyPageAnnotationChanged(page.PageIndex);
  }

  public static void RemoveRecord(
    PdfDocument doc,
    BaseAnnotation target,
    System.Collections.Generic.IReadOnlyList<BaseAnnotation> flattenedRecord)
  {
    PdfPage page = doc.Pages[target.PageIndex];
    if (page.Annots == null)
      throw new ArgumentException("Annots is null");
    PdfAnnotation annot = page.Annots[target.AnnotIndex];
    if (target is PopupAnnotation)
    {
      PdfMarkupAnnotation parent = ((PdfPopupAnnotation) annot).Parent as PdfMarkupAnnotation;
      if ((PdfWrapper) parent != (PdfWrapper) null)
        parent.Popup = (PdfPopupAnnotation) null;
    }
    foreach (BaseAnnotation baseAnnotation in flattenedRecord.Reverse<BaseAnnotation>())
      page.Annots.RemoveAt(baseAnnotation.AnnotIndex);
    if (!(PDFKit.PdfControl.GetPdfControl(doc)?.DataContext is MainViewModel dataContext))
      return;
    dataContext.PageEditors?.NotifyPageAnnotationChanged(page.PageIndex);
  }

  public static (BaseAnnotation target, System.Collections.Generic.IReadOnlyList<BaseAnnotation>) CreateFlattenedRecord(
    PdfAnnotation annot)
  {
    System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation> replies;
    BaseAnnotation record = annot != null ? AnnotationModelUtils.CreateRecord(annot, out replies) : throw new ArgumentNullException(nameof (annot));
    return (record, AnnotationModelUtils.FlattenRecord(record, replies));
  }

  public static BaseAnnotation CloneRecord(
    PdfAnnotation annotation,
    out System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation> replies)
  {
    BaseAnnotation record = annotation != null ? AnnotationModelUtils.CreateRecord(annotation, out replies) : throw new ArgumentNullException(nameof (annotation));
    System.Collections.Generic.IReadOnlyList<BaseAnnotation> source = AnnotationModelUtils.FlattenRecord(record, replies);
    BaseAnnotation[] array = source.OrderBy<BaseAnnotation, int>((Func<BaseAnnotation, int>) (c => c.AnnotIndex)).ToArray<BaseAnnotation>();
    if (array.Length == 0)
      return (BaseAnnotation) null;
    int num = annotation.Page.Annots.Count - array[0].AnnotIndex;
    foreach (BaseAnnotation baseAnnotation in (IEnumerable<BaseAnnotation>) source.OrderBy<BaseAnnotation, int>((Func<BaseAnnotation, int>) (c => c.AnnotIndex)))
      baseAnnotation.AnnotIndex += num;
    return record;
  }
}
