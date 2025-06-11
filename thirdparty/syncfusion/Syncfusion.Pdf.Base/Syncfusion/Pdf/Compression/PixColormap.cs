// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.PixColormap
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class PixColormap
{
  private RGBA_Quad[] m_array;
  private int m_depth;
  private int m_nalloc;
  private int m_n;

  internal RGBA_Quad[] Array
  {
    get => this.m_array;
    set => this.m_array = value;
  }

  internal int Depth
  {
    get => this.m_depth;
    set => this.m_depth = value;
  }

  internal int Nalloc
  {
    get => this.m_nalloc;
    set => this.m_nalloc = value;
  }

  internal int N
  {
    get => this.m_n;
    set => this.m_n = value;
  }
}
