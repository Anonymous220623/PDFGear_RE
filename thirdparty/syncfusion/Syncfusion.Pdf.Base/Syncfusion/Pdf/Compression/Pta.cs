// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.Pta
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class Pta
{
  private int m_n;
  private int m_nalloc;
  private int m_refCount;
  private List<float> m_x;
  private List<float> m_y;

  internal int N
  {
    get => this.m_n;
    set => this.m_n = value;
  }

  internal int Nalloc
  {
    get => this.m_nalloc;
    set => this.m_nalloc = value;
  }

  internal int RefCount
  {
    get => this.m_refCount;
    set => this.m_refCount = value;
  }

  internal List<float> X
  {
    get => this.m_x;
    set => this.m_x = value;
  }

  internal List<float> Y
  {
    get => this.m_y;
    set => this.m_y = value;
  }

  internal Pta(int n)
  {
    this.Nalloc = n;
    this.N = 0;
    this.X = new List<float>();
    this.Y = new List<float>();
  }
}
