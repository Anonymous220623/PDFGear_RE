// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HtmlToPdf.HtmlHyperLink
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.HtmlToPdf;

internal class HtmlHyperLink
{
  private RectangleF m_bounds;
  private string m_href;
  private string m_name;
  private string m_hash;

  public HtmlHyperLink()
  {
  }

  public HtmlHyperLink(RectangleF Bounds, string Href)
  {
    this.m_bounds = Bounds;
    this.m_href = Href;
    this.ConvertBoundsToPoint();
  }

  public RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  internal string Hash
  {
    get => this.m_hash;
    set => this.m_hash = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public string Href
  {
    get => this.m_href;
    set => this.m_href = value;
  }

  internal void ConvertBoundsToPoint()
  {
    this.m_bounds = new PdfUnitConvertor().ConvertFromPixels(this.m_bounds, PdfGraphicsUnit.Point);
  }
}
