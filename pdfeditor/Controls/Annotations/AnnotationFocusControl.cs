// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.AnnotationFocusControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Utils;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public class AnnotationFocusControl : Control
{
  private static Color DefaultStrokeColor = Color.FromArgb((byte) 204, (byte) 0, (byte) 0, (byte) 0);
  private readonly AnnotationCanvas annotationCanvas;
  private Rect bounds;
  private Pen pen;
  public static readonly DependencyProperty AnnotationProperty = DependencyProperty.Register(nameof (Annotation), typeof (PdfAnnotation), typeof (AnnotationFocusControl), new PropertyMetadata((object) null, new PropertyChangedCallback(AnnotationFocusControl.OnAnnotationPropertyChanged)));
  public static readonly DependencyProperty IsTextMarkupFocusVisibleProperty = DependencyProperty.Register(nameof (IsTextMarkupFocusVisible), typeof (bool), typeof (AnnotationFocusControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

  public AnnotationFocusControl(AnnotationCanvas annotationCanvas)
  {
    this.annotationCanvas = annotationCanvas != null ? annotationCanvas : throw new ArgumentNullException(nameof (annotationCanvas));
    this.IsHitTestVisible = false;
    this.Foreground = (Brush) new SolidColorBrush(AnnotationFocusControl.DefaultStrokeColor);
  }

  public PdfAnnotation Annotation
  {
    get => (PdfAnnotation) this.GetValue(AnnotationFocusControl.AnnotationProperty);
    set => this.SetValue(AnnotationFocusControl.AnnotationProperty, (object) value);
  }

  private static void OnAnnotationPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is AnnotationFocusControl annotationFocusControl))
      return;
    annotationFocusControl.InvalidateMeasure();
  }

  public bool IsTextMarkupFocusVisible
  {
    get => (bool) this.GetValue(AnnotationFocusControl.IsTextMarkupFocusVisibleProperty);
    set => this.SetValue(AnnotationFocusControl.IsTextMarkupFocusVisibleProperty, (object) value);
  }

  protected override Size MeasureOverride(Size constraint)
  {
    this.bounds = this.GetAnnotationClientBounds(this.Annotation);
    if (this.bounds.IsEmpty)
      this.bounds = new Rect(0.0, 0.0, 0.0, 0.0);
    Canvas.SetLeft((UIElement) this, this.bounds.Left);
    Canvas.SetTop((UIElement) this, this.bounds.Top);
    return new Size(this.bounds.Width, this.bounds.Height);
  }

  protected override Size ArrangeOverride(Size arrangeBounds)
  {
    if (this.bounds.IsEmpty)
      this.bounds = new Rect(0.0, 0.0, 0.0, 0.0);
    return base.ArrangeOverride(arrangeBounds);
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    base.OnRender(drawingContext);
    PdfAnnotation annotation = this.Annotation;
    Rect bounds = this.bounds;
    if (bounds.IsEmpty || bounds.Width == 0.0 || bounds.Height == 0.0)
      return;
    Pen pen = this.GetPen();
    switch (annotation)
    {
      case PdfTextMarkupAnnotation markupAnnotation:
        if (!this.IsTextMarkupFocusVisible || markupAnnotation.QuadPoints == null || markupAnnotation.QuadPoints.Count <= 0)
          break;
        if (pen.Brush is SolidColorBrush brush)
        {
          SolidColorBrush solidColorBrush = new SolidColorBrush(brush.Color);
          solidColorBrush.Opacity = 0.6;
          pen = new Pen((Brush) solidColorBrush, 1.0);
          pen.Freeze();
        }
        using (IEnumerator<FS_QUADPOINTSF> enumerator = markupAnnotation.QuadPoints.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            FS_RECTF pdfRect = enumerator.Current.ToPdfRect();
            Rect clientRect;
            if (this.annotationCanvas.PdfViewer.TryGetClientRect(markupAnnotation.Page.PageIndex, pdfRect, out clientRect))
              drawingContext.DrawRectangle((Brush) null, pen, new Rect(clientRect.Left - bounds.Left - 2.0, clientRect.Top - bounds.Top - 2.0, clientRect.Width + 4.0, clientRect.Height + 4.0));
          }
          break;
        }
      case PdfLineAnnotation pdfLineAnnotation when pdfLineAnnotation.Line != null && pdfLineAnnotation.Line.Count > 1:
        FS_POINTF point1 = pdfLineAnnotation.Line[0];
        FS_POINTF point2 = pdfLineAnnotation.Line[1];
        PdfBorderStyle lineStyle = pdfLineAnnotation.LineStyle;
        float x = lineStyle != null ? lineStyle.Width : 1f;
        double num1 = 1.0;
        Point clientPoint1;
        Point clientPoint2;
        if (this.annotationCanvas.PdfViewer.TryGetClientPoint(pdfLineAnnotation.Page.PageIndex, new Point((double) x, 0.0), out clientPoint1) && this.annotationCanvas.PdfViewer.TryGetClientPoint(pdfLineAnnotation.Page.PageIndex, new Point(0.0, 0.0), out clientPoint2))
          num1 = clientPoint1.X - clientPoint2.X;
        Point clientPoint3;
        Point clientPoint4;
        if (!this.annotationCanvas.PdfViewer.TryGetClientPoint(pdfLineAnnotation.Page.PageIndex, point1.ToPoint(), out clientPoint3) || !this.annotationCanvas.PdfViewer.TryGetClientPoint(pdfLineAnnotation.Page.PageIndex, point2.ToPoint(), out clientPoint4))
          break;
        double num2 = Math.Sqrt((clientPoint4.X - clientPoint3.X) * (clientPoint4.X - clientPoint3.X) + (clientPoint4.Y - clientPoint3.Y) * (clientPoint4.Y - clientPoint3.Y));
        double num3 = Math.Atan2(clientPoint4.Y - clientPoint3.Y, clientPoint4.X - clientPoint3.X);
        Rect rect;
        if (!AnnotationFocusControl.TryCreateRect(-num1 - 2.0, -num1 / 2.0 - 4.0, num2 + num1 * 2.0 + 4.0, num1 + 8.0, out rect))
          break;
        clientPoint3 = new Point(clientPoint3.X - bounds.Left, clientPoint3.Y - bounds.Top);
        clientPoint4 = new Point(clientPoint4.X - bounds.Left, clientPoint4.Y - bounds.Top);
        double num4 = Math.Min(num2 / 2.0 + num1 + 2.0, num1 / 2.0 + 4.0);
        RectangleGeometry rectangleGeometry = new RectangleGeometry(rect, num4, num4);
        Matrix identity = Matrix.Identity;
        identity.Rotate(num3 * 180.0 / Math.PI);
        identity.Translate(clientPoint3.X, clientPoint3.Y);
        rectangleGeometry.Transform = (Transform) new MatrixTransform(identity);
        drawingContext.DrawGeometry((Brush) null, pen, (Geometry) rectangleGeometry);
        break;
      case PdfMarkupAnnotation _:
        drawingContext.DrawRoundedRectangle((Brush) null, pen, new Rect(0.0, 0.0, bounds.Width, bounds.Height), 2.0, 2.0);
        break;
    }
  }

  private Rect GetAnnotationClientBounds(PdfAnnotation annot)
  {
    Rect annotationClientBounds = new Rect();
    if ((PdfWrapper) annot != (PdfWrapper) null && this.annotationCanvas.PdfViewer != null && this.annotationCanvas.PdfViewer.Document != null && annot.Page != null && annot.Page.PageIndex != -1)
    {
      switch (annot)
      {
        case PdfTextMarkupAnnotation markupAnnotation:
          if (markupAnnotation.QuadPoints != null && markupAnnotation.QuadPoints.Count > 0)
          {
            double num1 = double.MaxValue;
            double num2 = double.MinValue;
            double num3 = double.MinValue;
            double num4 = double.MaxValue;
            foreach (FS_QUADPOINTSF quadPoint in markupAnnotation.QuadPoints)
            {
              FS_RECTF pdfRect = quadPoint.ToPdfRect();
              num1 = Math.Min((double) pdfRect.left, num1);
              num2 = Math.Max((double) pdfRect.right, num2);
              num3 = Math.Max((double) pdfRect.top, num3);
              num4 = Math.Min((double) pdfRect.bottom, num4);
            }
            Rect clientRect;
            if (num2 > num1 && num3 > num4 && this.annotationCanvas.PdfViewer.TryGetClientRect(markupAnnotation.Page.PageIndex, new FS_RECTF(num1, num3, num2, num4), out clientRect))
            {
              annotationClientBounds = new Rect(clientRect.Left - 2.0, clientRect.Top - 2.0, clientRect.Width + 4.0, clientRect.Height + 4.0);
              break;
            }
            break;
          }
          break;
        case PdfLineAnnotation annotation1:
          Rect deviceBounds = annotation1.GetDeviceBounds();
          annotationClientBounds = new Rect(deviceBounds.Left - 10.0, deviceBounds.Top - 10.0, deviceBounds.Width + 20.0, deviceBounds.Height + 20.0);
          break;
        case PdfInkAnnotation pdfInkAnnotation:
          if (pdfInkAnnotation.InkList.Count > 0)
          {
            double num5 = double.MaxValue;
            double num6 = double.MinValue;
            double num7 = double.MinValue;
            double num8 = double.MaxValue;
            foreach (PdfLinePointCollection<PdfInkAnnotation> ink in pdfInkAnnotation.InkList)
            {
              foreach (FS_POINTF fsPointf in ink)
              {
                num5 = Math.Min((double) fsPointf.X, num5);
                num6 = Math.Max((double) fsPointf.X, num6);
                num7 = Math.Max((double) fsPointf.Y, num7);
                num8 = Math.Min((double) fsPointf.Y, num8);
              }
            }
            Rect clientRect;
            if (num6 > num5 && num7 > num8 && this.annotationCanvas.PdfViewer.TryGetClientRect(pdfInkAnnotation.Page.PageIndex, new FS_RECTF(num5, num7, num6, num8), out clientRect))
            {
              annotationClientBounds = new Rect(clientRect.Left - 3.0, clientRect.Top - 3.0, clientRect.Width + 6.0, clientRect.Height + 6.0);
              break;
            }
            break;
          }
          break;
        case PdfMarkupAnnotation annotation2:
          annotationClientBounds = annotation2.GetDeviceBounds();
          break;
      }
    }
    if (annotationClientBounds.IsEmpty)
      annotationClientBounds = new Rect(0.0, 0.0, 0.0, 0.0);
    return annotationClientBounds;
  }

  private Pen GetPen()
  {
    Brush foreground = this.Foreground;
    if (this.pen == null)
    {
      this.pen = new Pen();
      this.pen.Thickness = 1.0;
    }
    if (foreground == null)
    {
      if (!(this.pen.Brush is SolidColorBrush))
        this.pen.Brush = (Brush) new SolidColorBrush(AnnotationFocusControl.DefaultStrokeColor);
    }
    else if (this.pen.Brush != this.Foreground)
      this.pen.Brush = this.Foreground;
    return this.pen;
  }

  private static bool TryCreateRect(
    double x,
    double y,
    double width,
    double height,
    out Rect rect)
  {
    rect = Rect.Empty;
    if (width < 0.0 || height < 0.0)
      return false;
    rect = new Rect(x, y, width, height);
    return true;
  }
}
