// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.CommetUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Models.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Utils;

public static class CommetUtils
{
  public static IReadOnlyDictionary<PdfAnnotation, System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation>> GetMarkupAnnotationReplies(
    PdfPage page)
  {
    return page == null || page.Annots == null || page.Annots.Count == 0 ? (IReadOnlyDictionary<PdfAnnotation, System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation>>) null : (IReadOnlyDictionary<PdfAnnotation, System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation>>) CommetUtils.GetMarkupAnnotationRepliesModel(AnnotationFactory.Create(page)).ToDictionary<KeyValuePair<BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>>, PdfAnnotation, System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation>>((Func<KeyValuePair<BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>>, PdfAnnotation>) (c => page.Annots[c.Key.AnnotIndex]), (Func<KeyValuePair<BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>>, System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation>>) (c => (System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation>) c.Value.Select<BaseMarkupAnnotation, PdfAnnotation>((Func<BaseMarkupAnnotation, PdfAnnotation>) (x => page.Annots[x.AnnotIndex])).OfType<PdfMarkupAnnotation>().ToArray<PdfMarkupAnnotation>()), (IEqualityComparer<PdfAnnotation>) new CommetUtils.PdfAnnotationCompare());
  }

  public static IReadOnlyDictionary<BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>> GetMarkupAnnotationRepliesModel(
    System.Collections.Generic.IReadOnlyList<BaseAnnotation> annots)
  {
    return annots == null || annots.Count == 0 ? (IReadOnlyDictionary<BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>>) null : (IReadOnlyDictionary<BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>>) annots.OfType<BaseMarkupAnnotation>().Where<BaseMarkupAnnotation>((Func<BaseMarkupAnnotation, bool>) (c => c.Relationship == RelationTypes.Reply && c.RelationshipAnnotation != null)).GroupBy<BaseMarkupAnnotation, BaseAnnotation, (BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>)>((Func<BaseMarkupAnnotation, BaseAnnotation>) (c => CommetUtils.GetRelationshipRoot(c)), (Func<BaseAnnotation, IEnumerable<BaseMarkupAnnotation>, (BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>)>) ((c, s) => (c, (System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>) s.ToArray<BaseMarkupAnnotation>()))).Where<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>)>((Func<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>), bool>) (c => c.Value.Count > 0)).ToDictionary<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>), BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>>((Func<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>), BaseAnnotation>) (c => c.Key), (Func<(BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>), System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>>) (c => c.Value));
  }

  private static BaseAnnotation GetRelationshipRoot(BaseMarkupAnnotation annot)
  {
    if (annot.Relationship != RelationTypes.Reply)
      return (BaseAnnotation) null;
    if (annot.RelationshipAnnotation == null)
      return (BaseAnnotation) null;
    return annot.RelationshipAnnotation is BaseMarkupAnnotation relationshipAnnotation ? CommetUtils.GetRelationshipRoot(relationshipAnnotation) ?? (BaseAnnotation) relationshipAnnotation : annot.RelationshipAnnotation;
  }

  private class PdfAnnotationCompare : IEqualityComparer<PdfAnnotation>
  {
    public bool Equals(PdfAnnotation x, PdfAnnotation y)
    {
      if ((PdfWrapper) x == (PdfWrapper) y)
        return true;
      return x != null && x.Equals((PdfWrapper) y);
    }

    public int GetHashCode(PdfAnnotation obj)
    {
      return (PdfWrapper) obj == (PdfWrapper) null ? int.MinValue : (int) (long) obj.Dictionary.Handle;
    }
  }
}
