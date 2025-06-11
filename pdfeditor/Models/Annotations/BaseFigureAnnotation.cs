// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.BaseFigureAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class BaseFigureAnnotation : BaseMarkupAnnotation
{
  public PdfBorderEffectModel BorderEffect { get; protected set; }

  public FS_COLOR InteriorColor { get; protected set; }

  public PdfBorderStyleModel BorderStyle { get; protected set; }

  public IReadOnlyList<float> InnerRectangle { get; protected set; }

  protected override void Init(PdfAnnotation pdfAnnotation)
  {
    base.Init(pdfAnnotation);
    PdfFigureAnnotation annot = pdfAnnotation as PdfFigureAnnotation;
    if (annot == null)
      return;
    this.BorderEffect = BaseAnnotation.ReturnValueOrDefault<PdfBorderEffectModel>((Func<PdfBorderEffectModel>) (() => !((PdfWrapper) annot.BorderEffect != (PdfWrapper) null) ? (PdfBorderEffectModel) null : new PdfBorderEffectModel(annot.BorderEffect.Effect, annot.BorderEffect.Intensity)));
    this.BorderStyle = BaseAnnotation.ReturnValueOrDefault<PdfBorderStyleModel>((Func<PdfBorderStyleModel>) (() => !((PdfWrapper) annot.BorderStyle != (PdfWrapper) null) ? (PdfBorderStyleModel) null : new PdfBorderStyleModel(annot.BorderStyle.Width, annot.BorderStyle.Style, annot.BorderStyle.DashPattern)));
    this.InteriorColor = BaseAnnotation.ReturnValueOrDefault<FS_COLOR>((Func<FS_COLOR>) (() => annot.InteriorColor));
    this.InnerRectangle = (IReadOnlyList<float>) BaseAnnotation.ReturnArrayOrEmpty<float>((Func<float[]>) (() =>
    {
      float[] innerRectangle = annot.InnerRectangle;
      return innerRectangle == null ? (float[]) null : ((IEnumerable<float>) innerRectangle).ToArray<float>();
    }));
  }

  protected override void ApplyCore(PdfAnnotation annot)
  {
    base.ApplyCore(annot);
    if (!(annot is PdfFigureAnnotation figureAnnotation1))
      return;
    if (this.BorderEffect == null)
    {
      figureAnnotation1.BorderEffect = (PdfBorderEffect) null;
    }
    else
    {
      if ((PdfWrapper) figureAnnotation1.BorderEffect == (PdfWrapper) null)
        figureAnnotation1.BorderEffect = new PdfBorderEffect();
      figureAnnotation1.BorderEffect.Effect = this.BorderEffect.Effect;
      figureAnnotation1.BorderEffect.Intensity = this.BorderEffect.Intensity;
    }
    if (this.BorderStyle == null)
    {
      figureAnnotation1.BorderStyle = (PdfBorderStyle) null;
    }
    else
    {
      if ((PdfWrapper) figureAnnotation1.BorderStyle == (PdfWrapper) null)
        figureAnnotation1.BorderStyle = new PdfBorderStyle();
      figureAnnotation1.BorderStyle.Width = this.BorderStyle.Width;
      figureAnnotation1.BorderStyle.DashPattern = this.BorderStyle.DashPattern.ToArray<float>();
      figureAnnotation1.BorderStyle.Style = this.BorderStyle.Style;
    }
    figureAnnotation1.InteriorColor = this.InteriorColor;
    PdfFigureAnnotation figureAnnotation2 = figureAnnotation1;
    IReadOnlyList<float> innerRectangle = this.InnerRectangle;
    float[] array = innerRectangle != null ? innerRectangle.ToArray<float>() : (float[]) null;
    figureAnnotation2.InnerRectangle = array;
    figureAnnotation1.Rectangle = this.Rectangle;
  }

  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is BaseFigureAnnotation figureAnnotation && figureAnnotation.InteriorColor == this.InteriorColor && figureAnnotation.Rectangle == this.Rectangle && EqualityComparer<PdfBorderEffectModel>.Default.Equals(figureAnnotation.BorderEffect, this.BorderEffect) && EqualityComparer<PdfBorderStyleModel>.Default.Equals(figureAnnotation.BorderStyle, this.BorderStyle) && BaseAnnotation.CollectionEqual<float>(figureAnnotation.InnerRectangle, this.InnerRectangle);
  }
}
