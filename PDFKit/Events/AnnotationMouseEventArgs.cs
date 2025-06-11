// Decompiled with JetBrains decompiler
// Type: PDFKit.Events.AnnotationMouseEventArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.Annotations;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace PDFKit.Events;

public class AnnotationMouseEventArgs : EventArgs
{
  private WeakReference<MouseEventArgs> weakRawEvent;

  internal AnnotationMouseEventArgs(
    PdfAnnotation pdfAnnotation,
    Rect boundsInControl,
    MouseEventArgs rawEvent,
    int pageIndex,
    Point pagePoint)
  {
    this.PdfAnnotation = pdfAnnotation;
    this.BoundsInControl = boundsInControl;
    if (rawEvent.LeftButton == MouseButtonState.Pressed)
      this.LeftButton = MouseButtonState.Pressed;
    this.weakRawEvent = new WeakReference<MouseEventArgs>(rawEvent);
    this.PageIndex = pageIndex;
    this.PagePoint = pagePoint;
  }

  public Point GetPosition(IInputElement relativeTo)
  {
    MouseEventArgs target;
    return this.weakRawEvent.TryGetTarget(out target) ? target.GetPosition(relativeTo) : new Point();
  }

  public PdfAnnotation PdfAnnotation { get; }

  public Rect BoundsInControl { get; }

  public int PageIndex { get; }

  public Point PagePoint { get; }

  public bool Handled { get; set; }

  public MouseButtonState LeftButton { get; }
}
