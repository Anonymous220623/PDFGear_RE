// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfBorders
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfBorders
{
  private PdfPen m_left;
  private PdfPen m_right;
  private PdfPen m_top;
  private PdfPen m_bottom;

  public PdfPen Left
  {
    get => this.m_left;
    set => this.m_left = value;
  }

  public PdfPen Right
  {
    get => this.m_right;
    set => this.m_right = value;
  }

  public PdfPen Top
  {
    get => this.m_top;
    set => this.m_top = value;
  }

  public PdfPen Bottom
  {
    get => this.m_bottom;
    set => this.m_bottom = value;
  }

  public PdfPen All
  {
    set => this.m_left = this.m_right = this.m_top = this.m_bottom = value;
  }

  internal bool IsAll
  {
    get => this.m_left == this.m_right && this.m_left == this.m_top && this.m_left == this.m_bottom;
  }

  public static PdfBorders Default => new PdfBorders();

  public PdfBorders()
  {
    PdfPen pdfPen1 = new PdfPen(new PdfColor((byte) 0, (byte) 0, (byte) 0));
    pdfPen1.DashStyle = PdfDashStyle.Solid;
    PdfPen pdfPen2 = new PdfPen(new PdfColor((byte) 0, (byte) 0, (byte) 0));
    pdfPen2.DashStyle = PdfDashStyle.Solid;
    PdfPen pdfPen3 = new PdfPen(new PdfColor((byte) 0, (byte) 0, (byte) 0));
    pdfPen3.DashStyle = PdfDashStyle.Solid;
    PdfPen pdfPen4 = new PdfPen(new PdfColor((byte) 0, (byte) 0, (byte) 0));
    pdfPen4.DashStyle = PdfDashStyle.Solid;
    this.m_left = pdfPen1;
    this.m_right = pdfPen2;
    this.m_top = pdfPen3;
    this.m_bottom = pdfPen4;
  }
}
