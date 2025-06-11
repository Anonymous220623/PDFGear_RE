// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfLayoutResult
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfLayoutResult
{
  private PdfPage m_page;
  private RectangleF m_bounds;
  private double m_totalPageSize;

  public PdfPage Page => this.m_page;

  public RectangleF Bounds => this.m_bounds;

  internal double TotalPageSize
  {
    get => this.m_totalPageSize;
    set => this.m_totalPageSize = value;
  }

  public PdfLayoutResult(PdfPage page, RectangleF bounds)
  {
    this.m_page = page;
    this.m_bounds = bounds;
  }
}
