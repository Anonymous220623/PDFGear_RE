// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfEdges
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

public class PdfEdges
{
  private int m_left;
  private int m_right;
  private int m_top;
  private int m_bottom;

  public int Left
  {
    get => this.m_left;
    set => this.m_left = value;
  }

  public int Right
  {
    get => this.m_right;
    set => this.m_right = value;
  }

  public int Top
  {
    get => this.m_top;
    set => this.m_top = value;
  }

  public int Bottom
  {
    get => this.m_bottom;
    set => this.m_bottom = value;
  }

  public int All
  {
    set => this.m_left = this.m_right = this.m_top = this.m_bottom = value;
  }

  internal bool IsAll
  {
    get => this.m_left == this.m_right && this.m_left == this.m_top && this.m_left == this.m_bottom;
  }

  public PdfEdges()
  {
  }

  public PdfEdges(int left, int right, int top, int bottom)
  {
    this.m_left = left;
    this.m_right = right;
    this.m_top = top;
    this.m_bottom = bottom;
  }
}
