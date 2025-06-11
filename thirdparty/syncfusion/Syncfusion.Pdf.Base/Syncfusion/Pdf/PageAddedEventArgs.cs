// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PageAddedEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

public class PageAddedEventArgs : EventArgs
{
  private PdfPage m_page;

  public PdfPage Page => this.m_page;

  private PageAddedEventArgs()
  {
  }

  public PageAddedEventArgs(PdfPage page) => this.m_page = page;
}
