// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfAttached
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

public class PdfAttached
{
  private bool m_top;
  private bool m_left;
  private bool m_bottom;
  private bool m_right;

  public PdfAttached(PdfEdge pageEdge)
  {
    this.SetEdge(new PdfEdge[1]{ pageEdge });
  }

  public PdfAttached(PdfEdge edge1, PdfEdge edge2)
  {
    this.SetEdge(new PdfEdge[2]{ edge1, edge2 });
  }

  public PdfAttached(PdfEdge edge1, PdfEdge edge2, PdfEdge edge3, PdfEdge edge4)
  {
    this.SetEdge(new PdfEdge[4]
    {
      edge1,
      edge2,
      edge3,
      edge4
    });
  }

  internal bool Top => this.m_top;

  internal bool Left => this.m_left;

  internal bool Bottom => this.m_bottom;

  internal bool Right => this.m_right;

  public void SetEdge(PdfEdge[] edges)
  {
    foreach (int edge in edges)
    {
      switch (edge)
      {
        case 0:
          this.m_top = true;
          break;
        case 1:
          this.m_bottom = true;
          break;
        case 2:
          this.m_left = true;
          break;
        case 3:
          this.m_right = true;
          break;
      }
    }
  }
}
