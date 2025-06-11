// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Viewer.DragOperationModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using pdfeditor.Utils;
using PDFKit;
using PDFKit.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Models.Viewer;

internal class DragOperationModel : HoverOperationModel
{
  private Rectangle rectangle;
  private int startPage = -1;
  private FS_POINTF startPoint;

  protected override bool RotatePreviewElementWithPage => true;

  public DragOperationModel(PdfViewer viewer)
    : base(viewer)
  {
    Rectangle rectangle = new Rectangle();
    rectangle.Stroke = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 24, (byte) 146, byte.MaxValue));
    rectangle.StrokeThickness = 2.0;
    rectangle.Fill = (Brush) new SolidColorBrush(Color.FromArgb((byte) 127 /*0x7F*/, byte.MaxValue, byte.MaxValue, byte.MaxValue));
    rectangle.IsHitTestVisible = false;
    this.rectangle = rectangle;
    this.PreviewElement = (UIElement) this.rectangle;
    viewer.OverrideCursor = Cursors.Cross;
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    base.OnMouseDown(e);
    if (this.ElementContainer == null || this.startPage != -1)
      return;
    this.startPage = this.CurrentPage;
    this.startPoint = this.PositionFromDocument;
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
  }

  protected override void UpdatePagePosition()
  {
    Point? nullable = new Point?();
    base.UpdatePagePosition();
    Border elementContainer = this.ElementContainer;
    Point clientPoint1;
    if (this.CurrentPage != -1 && elementContainer != null && this.startPage != -1 && this.Viewer.TryGetClientPoint(this.startPage, this.startPoint.ToPoint(), out clientPoint1))
    {
      this.CurrentPage = -1;
      Point positionFromViewer = this.MousePositionFromViewer;
      Point clientPoint2 = new Point(Math.Min(clientPoint1.X, positionFromViewer.X), Math.Min(clientPoint1.Y, positionFromViewer.Y));
      FS_RECTF pageRect;
      if (this.Viewer.TryGetPagePoint(this.startPage, clientPoint2, out Point _) && this.Viewer.TryGetPageRect(this.startPage, new Rect(clientPoint1, positionFromViewer), out pageRect))
      {
        this.CurrentPage = this.startPage;
        this.PositionFromDocument = new FS_POINTF(pageRect.left, pageRect.top);
        this.SizeInDocument = new FS_SIZEF(pageRect.Width, pageRect.Height);
        nullable = new Point?(clientPoint2);
      }
    }
    if (!nullable.HasValue)
      return;
    Canvas.SetLeft((UIElement) this.ElementContainer, nullable.Value.X);
    Canvas.SetTop((UIElement) this.ElementContainer, nullable.Value.Y);
  }

  protected override void UpdatePreview()
  {
    if (this.IsDisposed)
      throw new ObjectDisposedException("DataOperationModel");
    if (this.IsCompleted)
      return;
    Point positionFromViewer1 = this.MousePositionFromViewer;
    int currentPage = this.CurrentPage;
    Border elementContainer1 = this.ElementContainer;
    Point? nullable = new Point?();
    Point clientPoint1;
    if (elementContainer1 != null && this.startPage != -1 && this.Viewer.TryGetClientPoint(this.startPage, this.startPoint.ToPoint(), out clientPoint1))
    {
      Point positionFromViewer2 = this.MousePositionFromViewer;
      Point clientPoint2 = new Point(Math.Min(clientPoint1.X, positionFromViewer2.X), Math.Min(clientPoint1.Y, positionFromViewer2.Y));
      FS_RECTF pageRect;
      if (this.Viewer.TryGetPagePoint(this.startPage, clientPoint2, out Point _) && this.Viewer.TryGetPageRect(this.startPage, new Rect(clientPoint1, positionFromViewer2), out pageRect))
      {
        this.PositionFromDocument = new FS_POINTF(pageRect.left, pageRect.top);
        this.SizeInDocument = new FS_SIZEF(pageRect.Width, pageRect.Height);
        nullable = new Point?(clientPoint2);
        Size previewSize = this.GetPreviewSize(currentPage);
        elementContainer1.Width = previewSize.Width;
        elementContainer1.Height = previewSize.Height;
        if (this.PreviewElement is FrameworkElement previewElement)
        {
          previewElement.Width = previewSize.Width;
          previewElement.Height = previewSize.Height;
        }
      }
    }
    if (!nullable.HasValue)
      return;
    Border elementContainer2 = this.ElementContainer;
    Point point = nullable.Value;
    double x = point.X;
    Canvas.SetLeft((UIElement) elementContainer2, x);
    Border elementContainer3 = this.ElementContainer;
    point = nullable.Value;
    double y = point.Y;
    Canvas.SetTop((UIElement) elementContainer3, y);
  }

  protected override void OnCompleted(bool success, bool result)
  {
    if (success)
      this.UpdatePagePosition();
    base.OnCompleted(success, result);
  }
}
