// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfViewerDecorators.PdfLinkAnnotationDecorator
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net.Annotations;
using PDFKit.Utils;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.PdfViewerDecorators;

internal class PdfLinkAnnotationDecorator : IPdfViewerDecorator
{
  private Pen borderPen;
  private SolidColorBrush fillBrush;

  public bool IsEnabled { get; set; }

  public Color BorderColor
  {
    get => !(this.borderPen?.Brush is SolidColorBrush brush) ? Colors.Transparent : brush.Color;
    set
    {
      if (!(this.BorderColor != value))
        return;
      Pen pen = new Pen((Brush) new SolidColorBrush(value), this.BorderThickness);
      pen.Freeze();
      this.borderPen = pen;
    }
  }

  public double BorderThickness
  {
    get
    {
      Pen borderPen = this.borderPen;
      return borderPen == null ? 0.0 : borderPen.Thickness;
    }
    set
    {
      if (this.BorderThickness == value)
        return;
      Pen pen = new Pen((Brush) new SolidColorBrush(this.BorderColor), value);
      pen.Freeze();
      this.borderPen = pen;
    }
  }

  public Color FillColor
  {
    get
    {
      SolidColorBrush fillBrush = this.fillBrush;
      return fillBrush == null ? Colors.Transparent : fillBrush.Color;
    }
    set
    {
      if (!(this.FillColor != value))
        return;
      SolidColorBrush solidColorBrush = new SolidColorBrush(value);
      solidColorBrush.Freeze();
      this.fillBrush = solidColorBrush;
    }
  }

  public bool CanDrawPdfBitmap(PdfViewerDecoratorDrawingArgs args) => false;

  public bool CanDrawVisual(PdfViewerDecoratorDrawingArgs args)
  {
    return args.DrawingContext != null && args.Viewer != null && this.IsEnabled && (this.borderPen != null || this.fillBrush != null) && args.PdfPage.Annots != null && args.PdfPage.Annots.Count != 0;
  }

  public void DrawPdfBitmap(PdfViewerDecoratorDrawingArgs args)
  {
    throw new NotImplementedException();
  }

  public void DrawVisual(PdfViewerDecoratorDrawingArgs args)
  {
    try
    {
      foreach (PdfAnnotation annotation in args.PdfPage.Annots.OfType<PdfLinkAnnotation>())
      {
        FS_RECTF rect = annotation.GetRECT();
        Rect clientRect = args.Viewer.PageToClientRect(rect, args.PdfPage.PageIndex);
        args.DrawingContext.DrawRectangle((Brush) this.fillBrush, this.borderPen, clientRect);
      }
    }
    catch
    {
    }
  }
}
