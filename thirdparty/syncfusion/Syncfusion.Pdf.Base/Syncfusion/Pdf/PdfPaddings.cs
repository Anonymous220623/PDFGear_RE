// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPaddings
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPaddings
{
  private float m_left;
  private float m_right;
  private float m_top;
  private float m_bottom;

  public float Left
  {
    get => this.m_left;
    set => this.m_left = value;
  }

  public float Right
  {
    get => this.m_right;
    set => this.m_right = value;
  }

  public float Top
  {
    get => this.m_top;
    set => this.m_top = value;
  }

  public float Bottom
  {
    get => this.m_bottom;
    set => this.m_bottom = value;
  }

  public float All
  {
    set => this.m_left = this.m_right = this.m_top = this.m_bottom = value;
  }

  public PdfPaddings() => this.m_left = this.m_right = this.m_top = this.m_bottom = 0.5f;

  public PdfPaddings(float left, float right, float top, float bottom)
  {
    this.m_left = left;
    this.m_right = right;
    this.m_top = top;
    this.m_bottom = bottom;
  }
}
