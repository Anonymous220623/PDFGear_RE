// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfViewerAutoScrollStateChangedEventArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;

#nullable disable
namespace PDFKit.Utils;

public class PdfViewerAutoScrollStateChangedEventArgs : EventArgs
{
  public PdfViewerAutoScrollStateChangedEventArgs(
    PdfViewerAutoScrollState oldState,
    PdfViewerAutoScrollState newState)
  {
    this.OldState = oldState;
    this.NewState = newState;
  }

  public PdfViewerAutoScrollState OldState { get; }

  public PdfViewerAutoScrollState NewState { get; }
}
