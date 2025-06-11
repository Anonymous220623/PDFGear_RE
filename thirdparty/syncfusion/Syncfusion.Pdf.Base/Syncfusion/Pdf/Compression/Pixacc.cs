// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.Pixacc
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class Pixacc
{
  private int m_w;
  private int m_h;
  private uint m_offset;
  private Pix m_pix;

  internal int W
  {
    get => this.m_w;
    set => this.m_w = value;
  }

  internal int H
  {
    get => this.m_h;
    set => this.m_h = value;
  }

  internal uint Offset
  {
    get => this.m_offset;
    set => this.m_offset = value;
  }

  internal Pix Pix
  {
    get => this.m_pix;
    set => this.m_pix = value;
  }
}
