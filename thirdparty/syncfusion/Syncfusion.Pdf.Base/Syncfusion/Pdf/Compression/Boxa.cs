// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.Boxa
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class Boxa
{
  private int m_n;
  private int m_nalloc;
  private uint m_refCount;
  private List<Syncfusion.Pdf.Compression.Box> m_box;

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

  internal uint RefCount
  {
    get => this.m_refCount;
    set => this.m_refCount = value;
  }

  internal List<Syncfusion.Pdf.Compression.Box> Box
  {
    get => this.m_box;
    set => this.m_box = value;
  }

  internal Boxa(int n)
  {
    this.Nalloc = n;
    this.N = 0;
    this.RefCount = 1U;
    this.Box = new List<Syncfusion.Pdf.Compression.Box>();
  }
}
