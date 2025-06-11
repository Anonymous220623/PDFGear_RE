// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.InkAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Models.Menus;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class InkAnnotation : BaseMarkupAnnotation
{
  public PdfBorderStyleModel LineStyle { get; protected set; }

  public IReadOnlyList<IReadOnlyList<FS_POINTF>> InkList { get; internal set; }

  protected override void Init(PdfAnnotation pdfAnnotation)
  {
    base.Init(pdfAnnotation);
    PdfInkAnnotation annot = pdfAnnotation as PdfInkAnnotation;
    if (annot == null)
      return;
    this.LineStyle = BaseAnnotation.ReturnValueOrDefault<PdfBorderStyleModel>((Func<PdfBorderStyleModel>) (() => !((PdfWrapper) annot.LineStyle != (PdfWrapper) null) ? (PdfBorderStyleModel) null : new PdfBorderStyleModel(annot.LineStyle.Width, annot.LineStyle.Style, annot.LineStyle.DashPattern)));
    this.InkList = (IReadOnlyList<IReadOnlyList<FS_POINTF>>) BaseAnnotation.ReturnArrayOrEmpty<FS_POINTF[]>((Func<FS_POINTF[][]>) (() =>
    {
      PdfInkPointCollection inkList = annot.InkList;
      return inkList == null ? (FS_POINTF[][]) null : inkList.Select<PdfLinePointCollection<PdfInkAnnotation>, FS_POINTF[]>((Func<PdfLinePointCollection<PdfInkAnnotation>, FS_POINTF[]>) (c => BaseAnnotation.ReturnArrayOrEmpty<FS_POINTF>((Func<FS_POINTF[]>) (() => c.ToArray<FS_POINTF>())))).ToArray<FS_POINTF[]>();
    }));
  }

  protected override void ApplyCore(PdfAnnotation annot)
  {
    base.ApplyCore(annot);
    if (!(annot is PdfInkAnnotation pdfInkAnnotation))
      return;
    if (this.LineStyle == null)
    {
      pdfInkAnnotation.LineStyle = (PdfBorderStyle) null;
    }
    else
    {
      if ((PdfWrapper) pdfInkAnnotation.LineStyle == (PdfWrapper) null)
        pdfInkAnnotation.LineStyle = new PdfBorderStyle();
      pdfInkAnnotation.LineStyle.Width = this.LineStyle.Width;
      pdfInkAnnotation.LineStyle.DashPattern = this.LineStyle.DashPattern.ToArray<float>();
      pdfInkAnnotation.LineStyle.Style = this.LineStyle.Style;
    }
    if (pdfInkAnnotation.InkList == null)
      pdfInkAnnotation.InkList = new PdfInkPointCollection();
    else
      pdfInkAnnotation.InkList.Clear();
    if (this.InkList == null)
      return;
    foreach (IReadOnlyList<FS_POINTF> ink in (IEnumerable<IReadOnlyList<FS_POINTF>>) this.InkList)
    {
      PdfLinePointCollection<PdfInkAnnotation> linePointCollection = new PdfLinePointCollection<PdfInkAnnotation>();
      pdfInkAnnotation.InkList.Add(linePointCollection);
      foreach (FS_POINTF fsPointf in (IEnumerable<FS_POINTF>) ink)
        linePointCollection.Add(fsPointf);
    }
  }

  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is InkAnnotation inkAnnotation && EqualityComparer<PdfBorderStyleModel>.Default.Equals(inkAnnotation.LineStyle, this.LineStyle) && BaseAnnotation.CollectionEqual<IReadOnlyList<FS_POINTF>, FS_POINTF>(inkAnnotation.InkList, this.InkList);
  }

  public override object GetValue(AnnotationMode mode, ContextMenuItemType type)
  {
    if (mode != AnnotationMode.Ink)
      return base.GetValue(mode, type);
    switch (type)
    {
      case ContextMenuItemType.StrokeColor:
        return (object) this.Color;
      case ContextMenuItemType.StrokeThickness:
        PdfBorderStyleModel lineStyle = this.LineStyle;
        return (object) (float) (lineStyle != null ? (double) lineStyle.Width : 1.0);
      default:
        return (object) null;
    }
  }
}
