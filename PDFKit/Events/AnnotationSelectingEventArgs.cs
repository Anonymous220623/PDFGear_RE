// Decompiled with JetBrains decompiler
// Type: PDFKit.Events.AnnotationSelectingEventArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.Annotations;
using System;
using System.Windows;

#nullable disable
namespace PDFKit.Events;

public class AnnotationSelectingEventArgs : EventArgs
{
  internal AnnotationSelectingEventArgs(PdfAnnotation pdfAnnotation, Rect boundsInControl)
  {
    this.PdfAnnotation = pdfAnnotation;
    this.BoundsInControl = boundsInControl;
  }

  public bool Canceled { get; set; }

  public PdfAnnotation PdfAnnotation { get; }

  public Rect BoundsInControl { get; }
}
