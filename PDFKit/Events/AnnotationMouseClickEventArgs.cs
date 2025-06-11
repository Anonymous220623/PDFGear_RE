// Decompiled with JetBrains decompiler
// Type: PDFKit.Events.AnnotationMouseClickEventArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.Annotations;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace PDFKit.Events;

public class AnnotationMouseClickEventArgs : EventArgs
{
  private WeakReference<MouseButtonEventArgs> weakRawEvent;

  internal AnnotationMouseClickEventArgs(
    PdfAnnotation pdfAnnotation,
    Rect boundsInControl,
    MouseButtonEventArgs rawEvent,
    int pageIndex,
    Point pagePoint)
  {
    this.PdfAnnotation = pdfAnnotation;
    this.BoundsInControl = boundsInControl;
    this.ChangeButton = rawEvent.ChangedButton;
    this.LeftButton = rawEvent.LeftButton;
    this.RightButton = rawEvent.RightButton;
    this.weakRawEvent = new WeakReference<MouseButtonEventArgs>(rawEvent);
    this.PageIndex = pageIndex;
    this.PagePoint = pagePoint;
  }

  public Point GetPosition(IInputElement relativeTo)
  {
    MouseButtonEventArgs target;
    return this.weakRawEvent.TryGetTarget(out target) ? target.GetPosition(relativeTo) : new Point();
  }

  public PdfAnnotation PdfAnnotation { get; }

  public Rect BoundsInControl { get; }

  public MouseButton ChangeButton { get; }

  public MouseButtonState LeftButton { get; }

  public MouseButtonState RightButton { get; }

  public int PageIndex { get; }

  public Point PagePoint { get; }

  public bool Handled { get; set; }
}
