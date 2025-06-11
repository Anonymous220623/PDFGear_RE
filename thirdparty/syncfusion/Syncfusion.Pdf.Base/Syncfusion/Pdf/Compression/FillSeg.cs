// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.FillSeg
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class FillSeg
{
  private int m_xLeft;
  private int m_xRight;
  private int m_y;
  private int m_dy;

  internal int XLeft
  {
    get => this.m_xLeft;
    set => this.m_xLeft = value;
  }

  internal int XRight
  {
    get => this.m_xRight;
    set => this.m_xRight = value;
  }

  internal int Y
  {
    get => this.m_y;
    set => this.m_y = value;
  }

  internal int Dy
  {
    get => this.m_dy;
    set => this.m_dy = value;
  }
}
