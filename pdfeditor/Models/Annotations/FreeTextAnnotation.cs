// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.FreeTextAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Models.Menus;
using pdfeditor.ViewModels;
using PDFKit.Utils.PdfRichTextStrings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class FreeTextAnnotation : BaseMarkupAnnotation
{
  public string DefaultAppearance { get; protected set; }

  public JustifyTypes TextAlignment { get; protected set; }

  public string DefaultStyle { get; protected set; }

  public IReadOnlyList<FS_POINTF> CalloutLine { get; protected set; }

  public IReadOnlyList<LineEndingStyles> LineEnding { get; protected set; }

  public AnnotationIntent Intent { get; protected set; }

  public PdfBorderEffectModel BorderEffect { get; protected set; }

  public PdfBorderStyleModel BorderStyle { get; protected set; }

  public IReadOnlyList<float> InnerRectangle { get; protected set; }

  protected override void Init(PdfAnnotation pdfAnnotation)
  {
    base.Init(pdfAnnotation);
    PdfFreeTextAnnotation annot = pdfAnnotation as PdfFreeTextAnnotation;
    if (annot == null)
      return;
    this.CalloutLine = (IReadOnlyList<FS_POINTF>) BaseAnnotation.ReturnArrayOrEmpty<FS_POINTF>((Func<FS_POINTF[]>) (() =>
    {
      PdfLinePointCollection<PdfFreeTextAnnotation> calloutLine = annot.CalloutLine;
      return calloutLine == null ? (FS_POINTF[]) null : calloutLine.ToArray<FS_POINTF>();
    }));
    this.LineEnding = (IReadOnlyList<LineEndingStyles>) BaseAnnotation.ReturnArrayOrEmpty<LineEndingStyles>((Func<LineEndingStyles[]>) (() =>
    {
      PdfLineEndingCollection lineEnding = annot.LineEnding;
      return lineEnding == null ? (LineEndingStyles[]) null : lineEnding.ToArray<LineEndingStyles>();
    }));
    this.BorderEffect = BaseAnnotation.ReturnValueOrDefault<PdfBorderEffectModel>((Func<PdfBorderEffectModel>) (() => !((PdfWrapper) annot.BorderEffect != (PdfWrapper) null) ? (PdfBorderEffectModel) null : new PdfBorderEffectModel(annot.BorderEffect.Effect, annot.BorderEffect.Intensity)));
    this.BorderStyle = BaseAnnotation.ReturnValueOrDefault<PdfBorderStyleModel>((Func<PdfBorderStyleModel>) (() => !((PdfWrapper) annot.BorderStyle != (PdfWrapper) null) ? (PdfBorderStyleModel) null : new PdfBorderStyleModel(annot.BorderStyle.Width, annot.BorderStyle.Style, annot.BorderStyle.DashPattern)));
    this.DefaultAppearance = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => annot.DefaultAppearance));
    this.TextAlignment = BaseAnnotation.ReturnValueOrDefault<JustifyTypes>((Func<JustifyTypes>) (() => annot.TextAlignment));
    this.DefaultStyle = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => annot.DefaultStyle));
    this.Intent = BaseAnnotation.ReturnValueOrDefault<AnnotationIntent>((Func<AnnotationIntent>) (() => annot.Intent));
    this.InnerRectangle = (IReadOnlyList<float>) BaseAnnotation.ReturnArrayOrEmpty<float>((Func<float[]>) (() =>
    {
      float[] innerRectangle = annot.InnerRectangle;
      return innerRectangle == null ? (float[]) null : ((IEnumerable<float>) innerRectangle).ToArray<float>();
    }));
  }

  protected override void ApplyCore(PdfAnnotation annot)
  {
    base.ApplyCore(annot);
    if (!(annot is PdfFreeTextAnnotation freeTextAnnotation1))
      return;
    if (freeTextAnnotation1.CalloutLine == null)
      freeTextAnnotation1.CalloutLine = new PdfLinePointCollection<PdfFreeTextAnnotation>();
    else
      freeTextAnnotation1.CalloutLine.Clear();
    foreach (FS_POINTF fsPointf in (IEnumerable<FS_POINTF>) this.CalloutLine)
      freeTextAnnotation1.CalloutLine.Add(fsPointf);
    if (freeTextAnnotation1.LineEnding == null)
    {
      freeTextAnnotation1.LineEnding = new PdfLineEndingCollection(LineEndingStyles.None, LineEndingStyles.None);
      freeTextAnnotation1.LineEnding.Clear();
    }
    else
      freeTextAnnotation1.LineEnding.Clear();
    foreach (LineEndingStyles lineEndingStyles in (IEnumerable<LineEndingStyles>) this.LineEnding)
      freeTextAnnotation1.LineEnding.Add(lineEndingStyles);
    if (this.BorderEffect == null)
    {
      freeTextAnnotation1.BorderEffect = (PdfBorderEffect) null;
    }
    else
    {
      if ((PdfWrapper) freeTextAnnotation1.BorderEffect == (PdfWrapper) null)
        freeTextAnnotation1.BorderEffect = new PdfBorderEffect();
      freeTextAnnotation1.BorderEffect.Effect = this.BorderEffect.Effect;
      freeTextAnnotation1.BorderEffect.Intensity = this.BorderEffect.Intensity;
    }
    if (this.BorderStyle == null)
    {
      freeTextAnnotation1.BorderStyle = (PdfBorderStyle) null;
    }
    else
    {
      if ((PdfWrapper) freeTextAnnotation1.BorderStyle == (PdfWrapper) null)
        freeTextAnnotation1.BorderStyle = new PdfBorderStyle();
      freeTextAnnotation1.BorderStyle.Width = this.BorderStyle.Width;
      freeTextAnnotation1.BorderStyle.DashPattern = this.BorderStyle.DashPattern.ToArray<float>();
      freeTextAnnotation1.BorderStyle.Style = this.BorderStyle.Style;
    }
    freeTextAnnotation1.DefaultAppearance = this.DefaultAppearance;
    freeTextAnnotation1.TextAlignment = this.TextAlignment;
    freeTextAnnotation1.DefaultStyle = this.DefaultStyle;
    freeTextAnnotation1.Intent = this.Intent;
    PdfFreeTextAnnotation freeTextAnnotation2 = freeTextAnnotation1;
    IReadOnlyList<float> innerRectangle = this.InnerRectangle;
    float[] array = innerRectangle != null ? innerRectangle.ToArray<float>() : (float[]) null;
    freeTextAnnotation2.InnerRectangle = array;
    freeTextAnnotation1.Rectangle = this.Rectangle;
  }

  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is FreeTextAnnotation freeTextAnnotation && freeTextAnnotation.DefaultAppearance == this.DefaultAppearance && freeTextAnnotation.TextAlignment == this.TextAlignment && freeTextAnnotation.DefaultStyle == this.DefaultStyle && freeTextAnnotation.Intent == this.Intent && freeTextAnnotation.Rectangle == this.Rectangle && EqualityComparer<PdfBorderEffectModel>.Default.Equals(freeTextAnnotation.BorderEffect, this.BorderEffect) && EqualityComparer<PdfBorderStyleModel>.Default.Equals(freeTextAnnotation.BorderStyle, this.BorderStyle) && BaseAnnotation.CollectionEqual<FS_POINTF>(freeTextAnnotation.CalloutLine, this.CalloutLine) && BaseAnnotation.CollectionEqual<LineEndingStyles>(freeTextAnnotation.LineEnding, this.LineEnding) && BaseAnnotation.CollectionEqual<float>(freeTextAnnotation.InnerRectangle, this.InnerRectangle);
  }

  public override object GetValue(AnnotationMode mode, ContextMenuItemType type)
  {
    if (mode == AnnotationMode.TextBox || mode == AnnotationMode.Text)
    {
      switch (type)
      {
        case ContextMenuItemType.StrokeColor:
          PdfDefaultAppearance pdfFontStyle1;
          if (PdfDefaultAppearance.TryParse(this.DefaultAppearance, out pdfFontStyle1))
            return (object) pdfFontStyle1.StrokeColor;
          break;
        case ContextMenuItemType.FillColor:
          return (object) this.Color;
        case ContextMenuItemType.StrokeThickness:
          PdfBorderStyleModel borderStyle = this.BorderStyle;
          return (object) (float) (borderStyle != null ? (double) borderStyle.Width : 1.0);
        case ContextMenuItemType.FontSize:
          PdfDefaultAppearance pdfFontStyle2;
          if (PdfDefaultAppearance.TryParse(this.DefaultAppearance, out pdfFontStyle2))
            return (object) pdfFontStyle2.FontSize;
          break;
        case ContextMenuItemType.FontColor:
          FS_COLOR? nullable = new FS_COLOR?();
          PdfDefaultAppearance pdfFontStyle3;
          if (PdfDefaultAppearance.TryParse(this.DefaultAppearance, out pdfFontStyle3))
            nullable = new FS_COLOR?(pdfFontStyle3.FillColor);
          if (nullable.HasValue)
          {
            if (nullable.Value.A < 250)
              return (object) new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            int r = nullable.Value.R;
            FS_COLOR fsColor = nullable.Value;
            int g = fsColor.G;
            fsColor = nullable.Value;
            int b = fsColor.B;
            return (object) new FS_COLOR((int) byte.MaxValue, r, g, b);
          }
          break;
        default:
          return (object) null;
      }
    }
    return base.GetValue(mode, type);
  }
}
