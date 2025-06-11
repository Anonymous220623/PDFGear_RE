// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.RGBA_Quad
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class RGBA_Quad
{
  private int m_blue;
  private int m_green;
  private int m_red;

  internal int Blue
  {
    get => this.m_blue;
    set => this.m_blue = value;
  }

  internal int Green
  {
    get => this.m_green;
    set => this.m_green = value;
  }

  internal int Red
  {
    get => this.m_red;
    set => this.m_red = value;
  }
}
