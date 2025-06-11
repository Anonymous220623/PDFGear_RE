// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.BaseTextMarkupAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using System;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public abstract class BaseTextMarkupAnnotation : BaseMarkupAnnotation
{
  public System.Collections.Generic.IReadOnlyList<FS_QUADPOINTSF> QuadPoints { get; protected set; }

  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is BaseTextMarkupAnnotation markupAnnotation && BaseAnnotation.CollectionEqual<FS_QUADPOINTSF>(this.QuadPoints, markupAnnotation.QuadPoints);
  }

  protected override void ApplyCore(PdfAnnotation annot)
  {
    base.ApplyCore(annot);
    if (!(annot is PdfTextMarkupAnnotation markupAnnotation))
      return;
    if (markupAnnotation.QuadPoints == null)
      markupAnnotation.QuadPoints = new PdfQuadPointsCollection();
    else
      markupAnnotation.QuadPoints.Clear();
    if (this.QuadPoints == null || this.QuadPoints.Count <= 0)
      return;
    for (int index = 0; index < this.QuadPoints.Count; ++index)
      markupAnnotation.QuadPoints.Add(this.QuadPoints[index]);
  }

  protected override void Init(PdfAnnotation pdfAnnotation)
  {
    base.Init(pdfAnnotation);
    PdfTextMarkupAnnotation textMarkup = pdfAnnotation as PdfTextMarkupAnnotation;
    if (textMarkup == null)
      return;
    this.QuadPoints = (System.Collections.Generic.IReadOnlyList<FS_QUADPOINTSF>) BaseAnnotation.ReturnArrayOrEmpty<FS_QUADPOINTSF>((Func<FS_QUADPOINTSF[]>) (() =>
    {
      PdfQuadPointsCollection quadPoints = textMarkup.QuadPoints;
      return quadPoints == null ? (FS_QUADPOINTSF[]) null : quadPoints.ToArray<FS_QUADPOINTSF>();
    }));
  }
}
