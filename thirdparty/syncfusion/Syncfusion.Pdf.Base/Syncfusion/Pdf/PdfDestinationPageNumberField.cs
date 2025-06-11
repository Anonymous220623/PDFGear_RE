// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDestinationPageNumberField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfDestinationPageNumberField : PdfPageNumberField
{
  private PdfPage m_page;
  private PdfLoadedPage m_loadedPage;

  public PdfDestinationPageNumberField()
  {
  }

  public PdfDestinationPageNumberField(PdfFont font)
    : base(font)
  {
  }

  public PdfDestinationPageNumberField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfDestinationPageNumberField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  public PdfLoadedPage LoadedPage
  {
    get => this.m_loadedPage;
    set => this.m_loadedPage = value != null ? value : throw new ArgumentNullException("Page");
  }

  public PdfPage Page
  {
    get => this.m_page;
    set => this.m_page = value != null ? value : throw new ArgumentNullException(nameof (Page));
  }

  protected internal override string GetValue(PdfGraphics graphics)
  {
    return this.m_loadedPage != null ? this.InternalLoadedGetValue(this.m_loadedPage) : this.InternalGetValue(this.m_page);
  }
}
