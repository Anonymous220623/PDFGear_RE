// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.ImeReceiverImeRequestedEventArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;

#nullable disable
namespace PDFKit.Contents;

public class ImeReceiverImeRequestedEventArgs : EventArgs
{
  public string FontFamily { get; set; }

  public double FontSize { get; set; }

  public int CaretPointLeftInPixel { get; set; }

  public int CaretPointTopInPixel { get; set; }
}
