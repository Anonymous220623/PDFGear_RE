// Decompiled with JetBrains decompiler
// Type: PDFKit.PagePrintedEventArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;

#nullable disable
namespace PDFKit;

public class PagePrintedEventArgs : EventArgs
{
  public int PageNumber { get; private set; }

  public int TotalToPrint { get; set; }

  public bool Cancel { get; set; }

  public PagePrintedEventArgs(int PageNumber, int TotalToPrint)
  {
    this.PageNumber = PageNumber;
    this.TotalToPrint = TotalToPrint;
    this.Cancel = false;
  }
}
