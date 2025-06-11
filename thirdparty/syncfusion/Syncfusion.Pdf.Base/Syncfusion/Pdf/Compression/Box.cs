// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.Box
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class Box
{
  private int m_x;
  private int m_y;
  private int m_w;
  private int m_h;
  private uint m_refCount;

  internal int X
  {
    get => this.m_x;
    set => this.m_x = value;
  }

  internal int Y
  {
    get => this.m_y;
    set => this.m_y = value;
  }

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

  internal uint RefCount
  {
    get => this.m_refCount;
    set => this.m_refCount = value;
  }

  internal Box()
  {
  }

  internal Box(int x, int y, int w, int h)
  {
    this.X = x;
    this.Y = y;
    this.W = w;
    this.H = h;
  }
}
