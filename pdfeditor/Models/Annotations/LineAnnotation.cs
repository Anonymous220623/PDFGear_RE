// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.LineAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Models.Menus;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class LineAnnotation : BaseMarkupAnnotation
{
  public IReadOnlyList<FS_POINTF> Line { get; internal set; }

  public IReadOnlyList<LineEndingStyles> LineEnding { get; protected set; }

  public PdfBorderStyleModel LineStyle { get; protected set; }

  public FS_COLOR InteriorColor { get; protected set; }

  public AnnotationIntent Intent { get; protected set; }

  public float LeaderLineLenght { get; protected set; }

  public float LeaderLineExtension { get; protected set; }

  public bool Cap { get; protected set; }

  public float LeaderLineOffset { get; protected set; }

  public CaptionPositions CaptionPosition { get; protected set; }

  public FS_SIZEF CaptionOffset { get; protected set; }

  protected override void Init(PdfAnnotation pdfAnnotation)
  {
    base.Init(pdfAnnotation);
    PdfLineAnnotation annot = pdfAnnotation as PdfLineAnnotation;
    if (annot == null)
      return;
    this.Line = (IReadOnlyList<FS_POINTF>) BaseAnnotation.ReturnArrayOrEmpty<FS_POINTF>((Func<FS_POINTF[]>) (() =>
    {
      PdfLinePointCollection<PdfLineAnnotation> line = annot.Line;
      return line == null ? (FS_POINTF[]) null : line.ToArray<FS_POINTF>();
    }));
    this.LineEnding = (IReadOnlyList<LineEndingStyles>) BaseAnnotation.ReturnArrayOrEmpty<LineEndingStyles>((Func<LineEndingStyles[]>) (() =>
    {
      PdfLineEndingCollection lineEnding = annot.LineEnding;
      return lineEnding == null ? (LineEndingStyles[]) null : lineEnding.ToArray<LineEndingStyles>();
    }));
    this.LineStyle = BaseAnnotation.ReturnValueOrDefault<PdfBorderStyleModel>((Func<PdfBorderStyleModel>) (() => !((PdfWrapper) annot.LineStyle != (PdfWrapper) null) ? (PdfBorderStyleModel) null : new PdfBorderStyleModel(annot.LineStyle.Width, annot.LineStyle.Style, annot.LineStyle.DashPattern)));
    this.InteriorColor = BaseAnnotation.ReturnValueOrDefault<FS_COLOR>((Func<FS_COLOR>) (() => annot.InteriorColor));
    this.Intent = BaseAnnotation.ReturnValueOrDefault<AnnotationIntent>((Func<AnnotationIntent>) (() => annot.Intent));
    this.LeaderLineLenght = BaseAnnotation.ReturnValueOrDefault<float>((Func<float>) (() => annot.LeaderLineLenght));
    this.LeaderLineExtension = BaseAnnotation.ReturnValueOrDefault<float>((Func<float>) (() => annot.LeaderLineExtension));
    this.Cap = BaseAnnotation.ReturnValueOrDefault<bool>((Func<bool>) (() => annot.Cap));
    this.LeaderLineOffset = BaseAnnotation.ReturnValueOrDefault<float>((Func<float>) (() => annot.LeaderLineOffset));
    this.CaptionPosition = BaseAnnotation.ReturnValueOrDefault<CaptionPositions>((Func<CaptionPositions>) (() => annot.CaptionPosition));
    this.CaptionOffset = BaseAnnotation.ReturnValueOrDefault<FS_SIZEF>((Func<FS_SIZEF>) (() => annot.CaptionOffset));
  }

  protected override void ApplyCore(PdfAnnotation annot)
  {
    base.ApplyCore(annot);
    if (!(annot is PdfLineAnnotation pdfLineAnnotation))
      return;
    if (pdfLineAnnotation.Line == null)
      pdfLineAnnotation.Line = new PdfLinePointCollection<PdfLineAnnotation>();
    else
      pdfLineAnnotation.Line.Clear();
    foreach (FS_POINTF fsPointf in (IEnumerable<FS_POINTF>) this.Line)
      pdfLineAnnotation.Line.Add(fsPointf);
    if (pdfLineAnnotation.LineEnding == null)
    {
      pdfLineAnnotation.LineEnding = new PdfLineEndingCollection(LineEndingStyles.None, LineEndingStyles.None);
      pdfLineAnnotation.LineEnding.Clear();
    }
    else
      pdfLineAnnotation.LineEnding.Clear();
    foreach (LineEndingStyles lineEndingStyles in (IEnumerable<LineEndingStyles>) this.LineEnding)
      pdfLineAnnotation.LineEnding.Add(lineEndingStyles);
    if (this.LineStyle == null)
    {
      pdfLineAnnotation.LineStyle = (PdfBorderStyle) null;
    }
    else
    {
      if ((PdfWrapper) pdfLineAnnotation.LineStyle == (PdfWrapper) null)
        pdfLineAnnotation.LineStyle = new PdfBorderStyle();
      pdfLineAnnotation.LineStyle.Width = this.LineStyle.Width;
      pdfLineAnnotation.LineStyle.DashPattern = this.LineStyle.DashPattern.ToArray<float>();
      pdfLineAnnotation.LineStyle.Style = this.LineStyle.Style;
    }
    pdfLineAnnotation.InteriorColor = this.InteriorColor;
    pdfLineAnnotation.Intent = this.Intent;
    pdfLineAnnotation.LeaderLineLenght = this.LeaderLineLenght;
    pdfLineAnnotation.LeaderLineExtension = this.LeaderLineExtension;
    pdfLineAnnotation.Cap = this.Cap;
    pdfLineAnnotation.LeaderLineOffset = this.LeaderLineOffset;
    pdfLineAnnotation.CaptionPosition = this.CaptionPosition;
    pdfLineAnnotation.CaptionOffset = this.CaptionOffset;
  }

  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is LineAnnotation lineAnnotation && lineAnnotation.InteriorColor == this.InteriorColor && lineAnnotation.Intent == this.Intent && (double) lineAnnotation.LeaderLineLenght == (double) this.LeaderLineLenght && (double) lineAnnotation.LeaderLineExtension == (double) this.LeaderLineExtension && lineAnnotation.Cap == this.Cap && (double) lineAnnotation.LeaderLineOffset == (double) this.LeaderLineOffset && lineAnnotation.CaptionPosition == this.CaptionPosition && lineAnnotation.CaptionOffset == this.CaptionOffset && EqualityComparer<PdfBorderStyleModel>.Default.Equals(lineAnnotation.LineStyle, this.LineStyle) && BaseAnnotation.CollectionEqual<FS_POINTF>(lineAnnotation.Line, this.Line) && BaseAnnotation.CollectionEqual<LineEndingStyles>(lineAnnotation.LineEnding, this.LineEnding);
  }

  public override object GetValue(AnnotationMode mode, ContextMenuItemType type)
  {
    if (mode == AnnotationMode.Line)
    {
      switch (type)
      {
        case ContextMenuItemType.StrokeColor:
          return (object) this.Color;
        case ContextMenuItemType.StrokeThickness:
          PdfBorderStyleModel lineStyle = this.LineStyle;
          return (object) (float) (lineStyle != null ? (double) lineStyle.Width : 1.0);
      }
    }
    return base.GetValue(mode, type);
  }
}
